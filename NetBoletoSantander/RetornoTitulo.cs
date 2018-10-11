using System;

namespace NetBoletoSantander
{
    public class RetornoTitulo
    {
        public string Situacao { get; }
        public string[] DescricaoErro { get; }
        public string CodigoBarras { get; }
        public string LinhaDigitavel { get; }
        public DateTime? DataEntrada { get; }
        public bool TituloAceito { get; }
        public string NossoNumero { get; }

        public RetornoTitulo(string situacao, string[] descricaoErro, string codigoBarras, string linhaDigitavel,
            DateTime? dataEntrada, bool tituloAceito, string nossoNumero)
        {
            Situacao = situacao;
            DescricaoErro = descricaoErro;
            CodigoBarras = codigoBarras;
            LinhaDigitavel = linhaDigitavel;
            DataEntrada = dataEntrada;
            TituloAceito = tituloAceito;
            NossoNumero = nossoNumero;
        }
    }
}