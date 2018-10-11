using System;
using Xunit;

namespace NetBoletoSantander.Test
{
    public class CovenioTest
    {
        private readonly dynamic _convenio;
        
        public CovenioTest()
        {
            _convenio = new
            {
                CodigoBanco = 33,
                CodigoConvenio = 4234234,
            };
        }

        [Theory]
        [InlineData(-123)]
        [InlineData(0)]
        [InlineData(12345)]
        public void CodigoBancoInvalido(int banco)
        {
            var e = Assert.Throws<ArgumentException>(() => new Convenio(banco, _convenio.CodigoConvenio));
            
            Assert.Equal("CodigoBanco", e.ParamName);
        }

        [Theory]
        [InlineData(-123)]
        [InlineData(0)]
        [InlineData(1234567890)]
        public void CodigoConvenioInvalido(int convenio)
        {
            var e = Assert.Throws<ArgumentException>(() => new Convenio(_convenio.CodigoBanco, convenio));
            
            Assert.Equal("CodigoConvenio", e.ParamName);
        }
    }
}