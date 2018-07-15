namespace NetBoletoSantander
{
    public class Convenio
    {
        public string CodigoBanco { get; private set; }
        public string CodigoConvenio { get; private set; }

        public Convenio(string codigoBanco, string codigoConvenio)
        {
            CodigoBanco = codigoBanco;
            CodigoConvenio = codigoConvenio;
        }
    }
}