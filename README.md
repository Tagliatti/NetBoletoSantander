# NetBoletoSantander
Lib para registro de boletos online no banco Santander desenvolvido em .NET Core, compatível com .NET Standard 2.0 ou superior.

## Instalação
Nuget:
```
PM> Install-Package NetBoletoSantander
```
.NET CLI
```
> dotnet add package Newtonsoft.Json
```

## Uso
### Registrando um boleto
``` c#
// Obtendo o certificado da store, mas você pode importar o certificado de outras maneiras.
X509Certificate2 certificado;

using (var store = new X509Store())
{
    store.Open(OpenFlags.ReadOnly);

    certificado = store.Certificates.Find(X509FindType.FindBySerialNumber, "6v45w6v456v45644d", true)[0];

    store.Close();
}

try {
    var boletoSantander = new BoletoSantander(certificado, Ambiente.Teste, "VT8E");

    var convenio = new Convenio(33, 3909220);
    var pagador = new Pagador("667.393.514-65", "Fulano da Silva", "Endereço", "Bairro", "Cidade", "MG", "69945-000");
    var instrucoes = new InstrucoesDoTitulo(0, 0, 0, TipoDesconto.Isento, 0, DateTime.Today, 0, TipoProtesto.NaoProtestar, 
        0, 0, TipoPagamento.ConformeRegistro, 1, TipoValor.Percentual, 100, 100);
    var titulo = new Titulo(40, Especie.Outros, "000000204875", "4899379", DateTime.Today, DateTime.Today, 
        String.Empty, instrucoes);
    var boleto = new Boleto(convenio, pagador, titulo);

    // Para cada registro de boleto, este NSU deverá ser único por dia e por convênio, 
    // ou seja, não se pode usar o mesmo NSU no mesmo dia para o mesmo convênio.
    var nsu = 123;

    var retornoBoleto = boletoSantander.RegistrarBoleto(boleto, nsu);
}
catch (ArgumentException e)
{
}
catch (NetBoletoSantanderException e)
{
}
catch (Exception e)
{
}
```
Os dados retornados no registro do boleto podem ser vistos na classe [RetornoTitulo](https://github.com/Tagliatti/NetBoletoSantander/blob/master/NetBoletoSantander/RetornoTitulo.cs)

### Sondagem/Consulta do boleto
> Os campos retornados podem não estar preenchidos corretante devido a falta de atualização desse endpoint por parte do santander, prefira gardar o retorno do boleto ao invés de confiar na sondagem/consulta do boleto.

``` c#
var nsu = 123; // Deve ser o mesmo de quando registrou o boleto
var convenio = new Convenio(33, 3909220);
var boletoSantander = new BoletoSantander(certificado, Ambiente.Teste, "VT8E");

// Retorna uma tupla (Boleto, RetornoTitulo)
var retorno = boletoSantander.SondarBoleto(convenio, nsu, DateTime.Today);
```
> Documentação sobre [Tuplas](https://docs.microsoft.com/pt-br/dotnet/csharp/tuples)

## Licença
NetBoletoSantander é compartilhado sob a licença do MIT. Isso significa que você pode modificá-lo e usá-lo da maneira que quiser, mesmo para uso comercial. Mas por favor, dê ⭐️ ao repositório no Github.
