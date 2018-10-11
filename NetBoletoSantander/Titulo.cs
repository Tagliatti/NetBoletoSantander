using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace NetBoletoSantander
{
    public class Titulo
    {
        public double Valor { get; }
        public string NossoNumero { get; }
        public string SeuNumero { get; }
        public DateTime DataVencimento { get; }
        public string Mensagem { get; }
        public DateTime DataEmissao { get; }
        public Especie Especie { get; }
        public InstrucoesDoTitulo InstrucoesDoTitulo { get; }

        public Titulo(double valor, Especie especie, string nossoNumero, string seuNumero, DateTime dataVencimento,
            DateTime dataEmissao, bool digitoVerificador = true)
        {
            Valor = valor;
            Especie = especie;
            NossoNumero = nossoNumero + (digitoVerificador ? CalcularDigitoVerificador(nossoNumero).ToString() : "");
            SeuNumero = seuNumero;
            DataVencimento = dataVencimento;
            DataEmissao = dataEmissao;
            
            Validar();
        }

        public Titulo(double valor, Especie especie, string nossoNumero, string seuNumero, DateTime dataVencimento,
            DateTime dataEmissao, string mensagem, bool digitoVerificador = true)
        {
            Valor = valor;
            Especie = especie;
            NossoNumero = nossoNumero + (digitoVerificador ? CalcularDigitoVerificador(nossoNumero).ToString() : "");
            SeuNumero = seuNumero;
            DataVencimento = dataVencimento;
            DataEmissao = dataEmissao;
            Mensagem = mensagem;
            
            Validar();
        }

        public Titulo(double valor, Especie especie, string nossoNumero, string seuNumero, DateTime dataVencimento, 
            DateTime dataEmissao, InstrucoesDoTitulo instrucoesDoTitulo, bool digitoVerificador = true)
        {
            Valor = valor;
            Especie = especie;
            NossoNumero = nossoNumero + (digitoVerificador ? CalcularDigitoVerificador(nossoNumero).ToString() : "");
            SeuNumero = seuNumero;
            DataVencimento = dataVencimento;
            DataEmissao = dataEmissao;
            InstrucoesDoTitulo = instrucoesDoTitulo;
            
            Validar();
        }

        public Titulo(double valor, Especie especie, string nossoNumero, string seuNumero, DateTime dataVencimento, 
            DateTime dataEmissao, string mensagem, InstrucoesDoTitulo instrucoesDoTitulo, bool digitoVerificador = true)
        {
            Valor = valor;
            Especie = especie;
            NossoNumero = nossoNumero + (digitoVerificador ? CalcularDigitoVerificador(nossoNumero).ToString() : "");
            SeuNumero = seuNumero;
            DataVencimento = dataVencimento;
            DataEmissao = dataEmissao;
            Mensagem = mensagem;
            InstrucoesDoTitulo = instrucoesDoTitulo;
            
            Validar();
        }
        
        private int CalcularDigitoVerificador(string nossoNumero) {
            var digito = 0;
            var multiplicador = 2;
            var total = 0;
            var nossoNumeroArray = nossoNumero.ToCharArray().Reverse();
            
            foreach (var numero in nossoNumeroArray) {
                total += multiplicador * numero;
                
                if (++multiplicador > 9) {
                    multiplicador = 2;
                }
            }
            
            var modulo = total % 11;
            
            if (modulo > 1) {
                digito = 11 - modulo;
            }
            
            return digito;
        }

        private void Validar()
        {
            if (NossoNumero.Length == 0 || NossoNumero.Equals("0"))
                throw new ArgumentException("Não poder ser uma string vazia.", "NossoNumero");
            
            if (NossoNumero.Length > 14)
                throw new ArgumentException("Não poder ter mais que 14 caracteres.", "NossoNumero");
            
            if (!Regex.IsMatch(NossoNumero, @"^[0-9]*$"))
                throw new ArgumentException("Deve possuir apenas números.", "NossoNumero");
            
            if (SeuNumero.Length == 0)
                throw new ArgumentException("Não poder ser uma string vazia.", "SeuNumero");
            
            if (SeuNumero.Length > 15)
                throw new ArgumentException("Não poder ter mais que 15 caracteres.", "SeuNumero");
        }
    }
}