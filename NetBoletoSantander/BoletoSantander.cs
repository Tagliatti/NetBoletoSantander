using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Linq;

namespace NetBoletoSantander
{
    public class BoletoSantander
    {
        private const string TicketEndpoint = "https://ymbdlb.santander.com.br/dl-ticket-services/TicketEndpointService";
        private const string CobrancaEndpoint = "https://ymbcash.santander.com.br/ymbsrv/CobrancaV2EndpointService";
        private readonly XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
        private readonly XNamespace impl = "http://impl.webservice.dl.app.bsbr.altec.com/";
        private readonly X509Certificate2 _certificado;
        public Ambiente Ambiente { get; private set; }
        public string Estacao { get; private set; }
        
        public BoletoSantander(X509Certificate2 certificado, Ambiente ambiente, string estacao)
        {
            _certificado = certificado;
            Ambiente = ambiente;
            Estacao = estacao;
        }

        public RetornoTitulo RegistrarBoleto(Boleto boleto, string nsu)
        {
            if (string.IsNullOrEmpty(nsu))
                throw new ArgumentException("Valor não pode ser null ou vazio.", nameof(nsu));
                
            nsu = Ambiente == Ambiente.Teste ? $"TST{nsu}" : nsu;
            
            var ticket = GerarTicketParaRegistroBoleto(boleto);

            var resposta = EnviarRequisicao(SerializarEnvelopeEnvio(true, ticket, nsu, DateTime.Today), CobrancaEndpoint);
            
            return CriarRetornoTitulo(resposta);
        }

        public (Boleto, RetornoTitulo) SondarBoleto(Convenio convenio, string nsu, DateTime dataNsu)
        {
            if (string.IsNullOrEmpty(nsu))
                throw new ArgumentException("Valor não pode ser null ou vazio.", nameof(nsu));
                
            nsu = Ambiente == Ambiente.Teste ? $"TST{nsu}" : nsu;
            
            var ticket = GerarTicketParaSondagem(convenio, nsu, dataNsu);
            
            var resposta = EnviarRequisicao(SerializarEnvelopeEnvio(false, ticket, nsu, dataNsu), CobrancaEndpoint);

            var retornoTitulo = CriarRetornoTitulo(resposta);
            var boleto = CriarRetornoBoleto(resposta);
            
            return (boleto, retornoTitulo);
        }

        private string GerarTicketParaRegistroBoleto(Boleto boleto)
        {
            var tiquet = CriarTicket(SerializarBoleto(boleto));

            var resposta = EnviarRequisicao(tiquet, TicketEndpoint);
            
            var retCode = int.Parse(resposta.Descendants("retCode").FirstOrDefault()?.Value);

            if (retCode == 0)
            {
                return resposta.Descendants("ticket")?.FirstOrDefault()?.Value;
            }
            
            throw new NetBoletoSantanderException(retCode);
        }

        private string GerarTicketParaSondagem(Convenio convenio, string nsu, DateTime dataNsu)
        {
            var tiquet = CriarTicket(SerializarSonda(convenio));

            var resposta = EnviarRequisicao(tiquet, TicketEndpoint);
            
            var retCode = int.Parse(resposta.Descendants("retCode").FirstOrDefault()?.Value);

            if (retCode == 0)
            {
                return resposta.Descendants("ticket")?.FirstOrDefault()?.Value;
            }
            
            throw new NetBoletoSantanderException(retCode);
        }

        private XDocument EnviarRequisicao(object dados, string uri)
        {
            var httpClientHandler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
                ClientCertificateOptions = ClientCertificateOption.Manual,
                SslProtocols = SslProtocols.Tls12,
            };
            
            httpClientHandler.ClientCertificates.Add(_certificado);

            using (var httpClient = new HttpClient(httpClientHandler))
            {
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));

                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(uri),
                    Method = HttpMethod.Post,
                };

                request.Content = new StringContent(dados.ToString(), Encoding.UTF8, "text/xml");
                request.Headers.Clear();
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");

                var response = httpClient.SendAsync(request).Result;
                
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Erro ao enviar a requisição.");
                }

                return XDocument.Parse(response.Content.ReadAsStringAsync().Result);
                
            }
        }

        private XDocument CriarSoapRequest()
        {
            var soapRequest = new XDocument(
                new XDeclaration("1.0", "UTF-8", "no"),
                new XElement(soapenv + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "soapenv", soapenv),
                    new XElement(soapenv + "Header"),
                    new XElement(soapenv + "Body")
                ));

            return soapRequest;
        }

        private XDocument CriarTicket(XElement[] dados)
        {
            var soapRequest = CriarSoapRequest();
            var body = soapRequest.Root?.Element(soapenv + "Body");
            
            body?.Add(new XElement(impl + "create", 
                    new XAttribute(XNamespace.Xmlns + "impl", impl),
                    new XElement("TicketRequest", 
                        new XElement("dados", dados), 
                        new XElement("expiracao", 100), 
                        new XElement("sistema", "YMB")
                    )
                ));

            return soapRequest;
        }

        private XDocument SerializarEnvelopeEnvio(bool registrarTitulo, string ticket, string nsu, DateTime dataNsu)
        {
            var soapRequest = CriarSoapRequest();
            var body = soapRequest.Root?.Element(soapenv + "Body");
            
            body?.Add(new XElement(impl + (registrarTitulo ? "registraTitulo" : "consultaTitulo"),
                    new XAttribute(XNamespace.Xmlns + "impl", impl),
                    new XElement("dto",
                        new XElement("dtNsu", dataNsu.ToString("ddMMyyyy")),
                        new XElement("estacao", Estacao),
                        new XElement("nsu", nsu),
                        new XElement("ticket", ticket),
                        new XElement("tpAmbiente", Ambiente == Ambiente.Producao ? "P" : "T")
                    )
                ));

            return soapRequest;
        }

        private XElement[] SerializarBoleto(Boleto boleto)
        {
            var dados = new List<XElement>();

            #region Convenio
            
            dados.Add(SerializarEntry("CONVENIO.COD-BANCO", boleto.Convenio.CodigoBanco));
            dados.Add(SerializarEntry("CONVENIO.COD-CONVENIO", boleto.Convenio.CodigoConvenio));
            
            #endregion

            #region Pagador
            
            dados.Add(SerializarEntry("PAGADOR.TP-DOC", boleto.Pagador.TipoDocumento));
            dados.Add(SerializarEntry("PAGADOR.NUM-DOC", boleto.Pagador.NumeroDocumento));
            dados.Add(SerializarEntry("PAGADOR.NOME", boleto.Pagador.Nome));
            dados.Add(SerializarEntry("PAGADOR.ENDER", boleto.Pagador.Endereco));
            dados.Add(SerializarEntry("PAGADOR.BAIRRO", boleto.Pagador.Bairro));
            dados.Add(SerializarEntry("PAGADOR.CIDADE", boleto.Pagador.Cidade));
            dados.Add(SerializarEntry("PAGADOR.UF", boleto.Pagador.Uf));
            dados.Add(SerializarEntry("PAGADOR.CEP", boleto.Pagador.Cep));

            #endregion

            #region Titulo

            dados.Add(SerializarEntry("TITULO.NOSSO-NUMERO", boleto.Titulo.NossoNumero.PadLeft(13, '0')));
            dados.Add(SerializarEntry("TITULO.SEU-NUMERO", boleto.Titulo.SeuNumero.PadLeft(15, '0')));
            dados.Add(SerializarEntry("TITULO.DT-VENCTO", boleto.Titulo.DataVencimento.ToString("ddMMyyyy")));
            dados.Add(SerializarEntry("TITULO.DT-EMISSAO", boleto.Titulo.DataEmissao.ToString("ddMMyyyy")));
            dados.Add(SerializarEntry("TITULO.ESPECIE", ((int) boleto.Titulo.Especie)));
            dados.Add(SerializarEntry("TITULO.VL-NOMINAL", boleto.Titulo.Valor.ToString("F2").Replace(".", "").Replace(",", "")));
            dados.Add(SerializarEntry("TITULO.VL-MENSAGEM", boleto.Titulo.Mensagem));

            #region Instruções do título

            if (boleto.Titulo.InstrucoesDoTitulo != null)
            {
                dados.Add(SerializarEntry("TITULO.PC-MULTA", boleto.Titulo.InstrucoesDoTitulo.Multa.ToString("F2").Replace(".", "").Replace(",", "")));
                dados.Add(SerializarEntry("TITULO.QT-DIAS-MULTA", boleto.Titulo.InstrucoesDoTitulo.MultaApos));
                dados.Add(SerializarEntry("TITULO.PC-JURO", boleto.Titulo.InstrucoesDoTitulo.Juros.ToString("F2").Replace(".", "").Replace(",", "")));
                dados.Add(SerializarEntry("TITULO.TP-DESC", (int) boleto.Titulo.InstrucoesDoTitulo.TipoDesconto));
                dados.Add(SerializarEntry("TITULO.VL-DESC", boleto.Titulo.InstrucoesDoTitulo.ValorDesconto.ToString("F2").Replace(".", "").Replace(",", "")));
                dados.Add(SerializarEntry("TITULO.DT-LIMI-DESC", boleto.Titulo.InstrucoesDoTitulo.DataLimiteDesconto?.ToString("ddMMyyyy")));
                dados.Add(SerializarEntry("TITULO.VL-ABATIMENTO", boleto.Titulo.InstrucoesDoTitulo.ValorAbatimento.ToString("F2").Replace(".", "").Replace(",", "")));
                dados.Add(SerializarEntry("TITULO.TP-PROTESTO", (int) boleto.Titulo.InstrucoesDoTitulo.TipoProtesto));
                dados.Add(SerializarEntry("TITULO.QT-DIAS-PROTESTO", boleto.Titulo.InstrucoesDoTitulo.ProtestarApos));
                dados.Add(SerializarEntry("TITULO.QT-DIAS-BAIXA", boleto.Titulo.InstrucoesDoTitulo.BaixarApos));
                dados.Add(SerializarEntry("TITULO.TP-PAGAMENTO", (int) boleto.Titulo.InstrucoesDoTitulo.TipoPagamento));
                dados.Add(SerializarEntry("TITULO.QT-PARCIAIS", boleto.Titulo.InstrucoesDoTitulo.QuantidadePagamentosPossiveis));
                dados.Add(SerializarEntry("TITULO.TP-VALOR", (int) boleto.Titulo.InstrucoesDoTitulo.TipoValor));
                dados.Add(SerializarEntry("TITULO.VL-PERC-MINIMO", boleto.Titulo.InstrucoesDoTitulo.PercentualMinimo.ToString("F5").Replace(".", "").Replace(",", "")));
                dados.Add(SerializarEntry("TITULO.VL-PERC-MAXIMO", boleto.Titulo.InstrucoesDoTitulo.PercentualMaximo.ToString("F5").Replace(".", "").Replace(",", "")));
            }

            #endregion
            
            #endregion

            return dados.ToArray();
        }

        private XElement[] SerializarSonda(Convenio convenio)
        {
            var dados = new List<XElement>();
            
            dados.Add(SerializarEntry("CONVENIO.COD-BANCO", convenio.CodigoBanco));
            dados.Add(SerializarEntry("CONVENIO.COD-CONVENIO", convenio.CodigoConvenio));

            return dados.ToArray();
        }

        private XElement SerializarEntry(string key, object value)
        {
            return new XElement("entry",
                new XElement("key", key),
                new XElement("value", value)
            );
        }

        private RetornoTitulo CriarRetornoTitulo(XDocument xml)
        {
            var situacao = xml.Descendants("situacao")?.FirstOrDefault()?.Value;
            var descricaoErro = xml.Descendants("descricaoErro")?.FirstOrDefault()?.Value.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            var codigoBarras = xml.Descendants("cdBarra")?.FirstOrDefault()?.Value;
            var linhaDigitavel = xml.Descendants("linDig")?.FirstOrDefault()?.Value;
            var aceito = xml.Descendants("aceito")?.FirstOrDefault()?.Value == "S";
            var nossoNumero = xml.Descendants("nossoNumero")?.FirstOrDefault()?.Value;
            var dataEntrada = StringParaData(xml.Descendants("dtEntr")?.FirstOrDefault()?.Value);

            var descricaoErroTamanho = descricaoErro.Length;
            
            for (var i = 0; i < descricaoErroTamanho; i++)
            {
                descricaoErro[i] = FormatarDescricaoErro(descricaoErro[i]);
            }
            
            return new RetornoTitulo(situacao, descricaoErro, codigoBarras, linhaDigitavel, dataEntrada, aceito, nossoNumero);
        }
        
        private Boleto CriarRetornoBoleto(XDocument xml)
        {
            var codigoBanco = xml.Descendants("codBanco")?.FirstOrDefault()?.Value;
            var codigoConvenio = xml.Descendants("codConv")?.FirstOrDefault()?.Value;
            
            var convenio = new Convenio(int.Parse(codigoBanco), int.Parse(codigoConvenio));

            #region Pagador

            var tipoDocumento = int.Parse(xml.Descendants("tpDoc")?.FirstOrDefault()?.Value);
            var numeroDocumento = xml.Descendants("numDoc")?.FirstOrDefault()?.Value;
            var nome = xml.Descendants("nome")?.FirstOrDefault()?.Value;
            var endereco = xml.Descendants("ender")?.FirstOrDefault()?.Value;
            var bairro = xml.Descendants("bairro")?.FirstOrDefault()?.Value;
            var cidade = xml.Descendants("cidade")?.FirstOrDefault()?.Value;
            var uf = xml.Descendants("uf")?.FirstOrDefault()?.Value;
            var cep = xml.Descendants("cep")?.FirstOrDefault()?.Value;

            if (tipoDocumento == 1)
            {
                numeroDocumento = numeroDocumento?.Substring(4, 11);
            }

            #endregion

            #region Titulo

            var dataEmissao = StringParaData(xml.Descendants("dtEmissao")?.FirstOrDefault()?.Value);
            var dataLimiteDesconto = StringParaData(xml.Descendants("dtLimiDesc")?.FirstOrDefault()?.Value);
            var dataVencimento = StringParaData(xml.Descendants("dtVencto")?.FirstOrDefault()?.Value);
            var especie = (Especie) int.Parse(xml.Descendants("especie")?.FirstOrDefault()?.Value);
            var nossoNumero = xml.Descendants("nossoNumero")?.FirstOrDefault()?.Value;
            var juros = StringParaDouble(xml.Descendants("pcJuro")?.FirstOrDefault()?.Value);
            var multa = StringParaDouble(xml.Descendants("pcMulta")?.FirstOrDefault()?.Value);
            var baixarApos = int.Parse(xml.Descendants("qtDiasBaixa")?.FirstOrDefault()?.Value);
            var multaApos = int.Parse(xml.Descendants("qtDiasMulta")?.FirstOrDefault()?.Value);
            var protestarApos = int.Parse(xml.Descendants("qtDiasProtesto")?.FirstOrDefault()?.Value);
            var quantidadePagamentosPossiveis = int.Parse(xml.Descendants("qtdParciais")?.FirstOrDefault()?.Value);
            var seuNumero = xml.Descendants("seuNumero")?.FirstOrDefault()?.Value;
            var tipoPagamento = (TipoPagamento) int.Parse(xml.Descendants("tipoPagto")?.FirstOrDefault()?.Value);
            var tipoValor = (TipoValor) int.Parse(xml.Descendants("tipoValor")?.FirstOrDefault()?.Value);
            var tipoDesconto = (TipoDesconto) int.Parse(xml.Descendants("tpDesc")?.FirstOrDefault()?.Value);
            var tipoProtesto = (TipoProtesto) int.Parse(xml.Descendants("tpProtesto")?.FirstOrDefault()?.Value);
            var percentualMaximo = StringParaDouble(xml.Descendants("valorMaximo")?.FirstOrDefault()?.Value);
            var percentualMinimo = StringParaDouble(xml.Descendants("valorMinimo")?.FirstOrDefault()?.Value);
            var valorAbatimento = StringParaDouble(xml.Descendants("vlAbatimento")?.FirstOrDefault()?.Value);
            var valorDesconto = StringParaDouble(xml.Descendants("vlDesc")?.FirstOrDefault()?.Value);
            var valor = StringParaDouble(xml.Descendants("vlNominal")?.FirstOrDefault()?.Value);

            #endregion
            
            var pagador = new Pagador(numeroDocumento, nome, endereco, bairro, cidade, uf, cep);
            var instrucoes = new InstrucoesDoTitulo(multa, multaApos, juros, tipoDesconto, valorDesconto, dataLimiteDesconto, valorAbatimento, tipoProtesto, protestarApos, baixarApos, tipoPagamento, quantidadePagamentosPossiveis, tipoValor, percentualMinimo, percentualMaximo);
            var titulo = new Titulo(valor, especie, nossoNumero, seuNumero, dataVencimento.Value, dataEmissao.Value, "", instrucoes, false);
            
            return new Boleto(convenio, pagador, titulo);
        }

        private string FormatarDescricaoErro(string descricaoErro)
        {
            return descricaoErro
                .Replace("CONVENIO.COD-BANCO", "'Convenio.CodigoBanco'")
                .Replace("CONVENIO.COD-CONVENIO", "'Convenio.CodigoConvenio'")
                .Replace("PAGADOR.TP-DOC", "'Pagador.TipoDocumento'")
                .Replace("PAGADOR.NUM-DOC", "'Pagador.NumeroDocumento'")
                .Replace("PAGADOR.NOME", "'Pagador.Nome'")
                .Replace("PAGADOR.ENDER", "'Pagador.Endereco'")
                .Replace("PAGADOR.BAIRRO", "'Pagador.Bairro'")
                .Replace("PAGADOR.CIDADE", "'Pagador.Cidade'")
                .Replace("PAGADOR.UF", "'Pagador.Uf'")
                .Replace("PAGADOR.CEP", "'Pagador.Cep'")
                .Replace("TITULO.NOSSO-NUMERO", "'Titulo.NossoNumero'")
                .Replace("TITULO.SEU-NUMERO", "'Titulo.SeuNumero'")
                .Replace("TITULO.SEU-NUMERO", "'Titulo.SeuNumero'")
                .Replace("TITULO.DT-VENCTO", "'Titulo.DataVencimento'")
                .Replace("TITULO.DT-EMISSAO", "'Titulo.DataEmissao'")
                .Replace("TITULO.ESPECIE", "'Titulo.Especie'")
                .Replace("TITULO.VL-NOMINAL", "'Titulo.Valor'")
                .Replace("TITULO.MENSAGEM", "'Titulo.Mensagem'")
                .Replace("TITULO.PC-MULTA", "'Titulo.InstrucoesDeTitulo.Multa'")
                .Replace("TITULO.QT-DIAS-MULTA", "'Titulo.InstrucoesDeTitulo.MultaApos'")
                .Replace("TITULO.PC-JURO", "'Titulo.InstrucoesDeTitulo.Juros'")
                .Replace("TITULO.TP-DESC", "'Titulo.InstrucoesDeTitulo.TipoDesconto'")
                .Replace("TITULO.VL-DESC", "'Titulo.InstrucoesDeTitulo.ValorDesconto'")
                .Replace("TITULO.DT-LIMI-DESC", "'Titulo.InstrucoesDeTitulo.DataLimiteDesconto'")
                .Replace("TITULO.VL-ABATIMENTO", "'Titulo.InstrucoesDeTitulo.ValorAbatimento'")
                .Replace("TITULO.TP-PROTESTO", "'Titulo.InstrucoesDeTitulo.TipoProtesto'")
                .Replace("TITULO.QT-DIAS-PROTESTO", "'Titulo.InstrucoesDeTitulo.ProtestarApos'")
                .Replace("TITULO.QT-DIAS-BAIXA", "'Titulo.InstrucoesDeTitulo.BaixarApos'")
                .Replace("TITULO.QT-PARCIAIS", "'Titulo.InstrucoesDeTitulo.QuantidadePagamentosPossiveis'")
                .Replace("TITULO.TP-VALOR", "'Titulo.InstrucoesDeTitulo.TipoValor'")
                .Replace("TITULO.VL-PERC-MINIMO", "'Titulo.InstrucoesDeTitulo.PercentualMinimo'")
                .Replace("TITULO.VL-PERC-MAXIMO", "'Titulo.InstrucoesDeTitulo.PercentualMaximo'");
        }

        private DateTime? StringParaData(string data)
        {
            if (string.IsNullOrEmpty(data)) return null;
            
            var dataEntradaAno = int.Parse(data.Substring(4, 4)); 
            var dataEntradaMes = int.Parse(data.Substring(2, 2)); 
            var dataEntradaDia = int.Parse(data.Substring(0, 2));
                
            return new DateTime(dataEntradaAno, dataEntradaMes, dataEntradaDia);
        }

        private double StringParaDouble(string valor)
        {
            return double.Parse(valor.Insert(valor.Length - 2, ","));
        }
    }
}