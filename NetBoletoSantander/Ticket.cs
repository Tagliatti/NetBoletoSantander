using System;

namespace NetBoletoSantander
{
    public class Ticket
    {
        public int Nsu { get; private set; }
        public DateTime Data { get; private set; }
        public Ambiente Ambiente { get; private set; }
        public string Estacao { get; private set; }
        public string Autenticacao { get; private set; }

        public Ticket(Ambiente ambiente, string estacao, string autenticacao)
        {
            Ambiente = ambiente;
            Estacao = estacao;
            Autenticacao = autenticacao;
        }
    }
}