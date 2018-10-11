namespace NetBoletoSantander
{
    public class Boleto
    {
        public Convenio Convenio { get; }
        public Pagador Pagador { get; }
        public Titulo Titulo { get; }

        public Boleto(Convenio convenio, Pagador pagador, Titulo titulo)
        {
            Convenio = convenio;
            Pagador = pagador;
            Titulo = titulo;
        }
    }
}