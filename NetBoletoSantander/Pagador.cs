using System;
using System.Text.RegularExpressions;

namespace NetBoletoSantander
{
    public class Pagador
    {
        public string TipoDocumento { get; }
        public string NumeroDocumento { get; }
        public string Nome { get; }
        public string Endereco { get; }
        public string Bairro { get; }
        public string Cidade { get; }
        public string Uf { get; }
        public string Cep { get; }

        public Pagador(string numeroDocumento, string nome, string endereco, string bairro, string cidade, string uf,
            string cep)
        {
            numeroDocumento = numeroDocumento.Replace(".", "").Replace("-", "").Replace("/", "");
            
            TipoDocumento = numeroDocumento.Length == 11 ? "01" : "02"; 
            NumeroDocumento = numeroDocumento;
            Nome = nome;
            Endereco = endereco;
            Bairro = bairro;
            Cidade = cidade;
            Uf = uf;
            Cep = cep.Replace("-", "");
            
            Validar();
        }

        private void Validar()
        {
            if (NumeroDocumento.Length == 0)
                throw new ArgumentException("Não poder ser uma string vazia.", "NumeroDocumento");
            
            if (NumeroDocumento.Length != 11 && NumeroDocumento.Length != 14)
                throw new ArgumentException("O CPF ou CNPJ deve possuir 11 ou 14 caracteres respectivamente.", "NumeroDocumento");
            
            if (!Regex.IsMatch(NumeroDocumento, @"^[0-9]*$"))
                throw new ArgumentException("Deve possuir apenas números.", "NumeroDocumento");
            
            if (Nome.Length == 0)
                throw new ArgumentException("Não poder ser uma string vazia.", "Nome");
            
            if (Nome.Length > 40)
                throw new ArgumentException("Não poder ter mais que 40 caracteres.", "Nome");
            
            if (Endereco.Length == 0)
                throw new ArgumentException("Não poder ser uma string vazia.", "Endereco");
            
            if (Endereco.Length > 40)
                throw new ArgumentException("Não poder ter mais que 40 caracteres.", "Endereco");
            
            if (Bairro.Length == 0)
                throw new ArgumentException("Não poder ser uma string vazia.", "Bairro");
            
            if (Bairro.Length > 30)
                throw new ArgumentException("Não poder ter mais que 30 caracteres.", "Bairro");
            
            if (Cidade.Length == 0)
                throw new ArgumentException("Não poder ser uma string vazia.", "Cidade");
            
            if (Cidade.Length > 20)
                throw new ArgumentException("Não poder ter mais que 20 caracteres.", "Cidade");
            
            if (Uf.Length != 2)
                throw new ArgumentException("Deve possuir exatamente 2 caracteres.", "Uf");
            
            if (Cep.Length != 8)
                throw new ArgumentException("Deve possuir exatamente 8 caracteres.", "Cep");
        }
    }
}