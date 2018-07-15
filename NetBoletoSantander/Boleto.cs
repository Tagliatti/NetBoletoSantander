namespace NetBoletoSantander
{
    public class Boleto
    {
        public Convenio Convenio { get; private set; }
        public Pagador Pagador { get; private set; }
        public Titulo Titulo { get; private set; }

        public Boleto(Convenio convenio, Pagador pagador, Titulo titulo)
        {
            Convenio = convenio;
            Pagador = pagador;
            Titulo = titulo;
        }
    }
}