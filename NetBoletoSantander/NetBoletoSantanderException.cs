using System;
using System.Runtime.Serialization;

namespace NetBoletoSantander
{
    public class NetBoletoSantanderException : Exception
    {
        public NetBoletoSantanderException(int retCode) : base(TipoDeErro(retCode))
        {
            
        }

        private static string TipoDeErro(int retCode)
        {
            switch (retCode)
            {
                case 1:
                    return "Erro, dados de entrada inválidos";
                case 2:
                    return "Erro interno de criptografia";
                case 3:
                    return "Erro, Ticket já utilizado anteriormente";
                case 4:
                    return "Erro, Ticket gerado para outro sistema";
                case 5:
                    return "Erro, Ticket expirado";
                case 6:
                    return "Erro interno (dados)";
                case 7:
                    return "Erro interno (timestamp)";
                default:
                    return "Erro desconhecido";
            }
        }
    }
}