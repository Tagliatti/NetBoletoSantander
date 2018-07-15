namespace NetBoletoSantander
{
    public class Pagador
    {
        public string TipoDocumento { get; private set; }
        public string NumeroDocumento { get; private set; }
        public string Nome { get; private set; }
        public string Endereco { get; private set; }
        public string Bairro { get; set; }
        public string Cidade { get; private set; }
        public string Uf { get; private set; }
        public string Cep { get; private set; }

        public Pagador(string numeroDocumento, string nome, string endereco, string bairro, string cidade, string uf,
            string cep)
        {
            TipoDocumento = numeroDocumento.Length == 11 ? "01" : "02"; 
            NumeroDocumento = numeroDocumento;
            Nome = nome;
            Endereco = endereco;
            Bairro = bairro;
            Cidade = cidade;
            Uf = uf;
            Cep = cep;
        }
    }
}