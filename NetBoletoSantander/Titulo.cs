using System;
using System.Linq;

namespace NetBoletoSantander
{
    public class Titulo
    {
        public double Valor { get; private set; }
        public string NossoNumero { get; private set; }
        public string SeuNumero { get; private set; }
        public DateTime DataVencimento { get; private set; }
        public string Mensagem { get; private set; }
        public DateTime DataEmissao { get; private set; }
        public int Especie { get; private set; }
        public InstrucoesDeTitulo InstrucoesDeTitulo { get; private set; }

        public Titulo(double valor, string nossoNumero, string seuNumero, string mensagem, int especie,
            InstrucoesDeTitulo instrucoesDeTitulo, DateTime dataVencimento = new DateTime(),
            DateTime dataEmissao = new DateTime())
        {
            Valor = valor;
            NossoNumero = nossoNumero + CalcularDigitoVerificador(nossoNumero);
            SeuNumero = seuNumero;
            DataVencimento = dataVencimento;
            Mensagem = mensagem;
            DataEmissao = dataEmissao;
            Especie = especie;
            InstrucoesDeTitulo = instrucoesDeTitulo;
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
    }
}