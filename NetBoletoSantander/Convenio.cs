using System;
using System.Xml.Serialization;

namespace NetBoletoSantander
{
    public class Convenio
    {
        public int CodigoBanco { get; }
        public int CodigoConvenio { get; }

        public Convenio(int codigoBanco, int codigoConvenio)
        {
            CodigoBanco = codigoBanco;
            CodigoConvenio = codigoConvenio;
            
            Validar();
        }

        private void Validar()
        {
            if (CodigoBanco > 9999)
                throw new ArgumentException("Não poder ter mais que 4 dígitos.", "CodigoBanco");
            
            if (CodigoBanco <= 0)
                throw new ArgumentException("Não poder menor ou igual a 0.", "CodigoBanco");
            
            if (CodigoConvenio > 999999999)
                throw new ArgumentException("Não poder ter mais que 9 dígitos.", "CodigoConvenio");
            
            if (CodigoConvenio <= 0)
                throw new ArgumentException("Não poder menor ou igual a 0.", "CodigoConvenio");

        }
    }
}