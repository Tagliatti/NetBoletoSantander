using System;
using Xunit;

namespace NetBoletoSantander.Test
{
    public class TituloTest
    {
        private readonly dynamic _titulo;
        
        public TituloTest()
        {
            _titulo = new
            {
                Valor = 123_45,
                Especie = Especie.Outros,
                NossoNumero = "000000676",
                SeuNumero = "123",
                DataVencimento = DateTime.Today,
                DataEmissao = DateTime.Today,
            };
        }

        [Theory]
        [InlineData("")]
        [InlineData("123456789012345")]
        [InlineData("asdadadasd")]
        public void NossoNumeroInvalido(string nossoNumero)
        {
            var e = Assert.Throws<ArgumentException>(() => new Titulo(
                _titulo.Valor,
                _titulo.Especie,
                nossoNumero,
                _titulo.SeuNumero,
                _titulo.DataVencimento,
                _titulo.DataEmissao));
            
            Assert.Equal("NossoNumero", e.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData("1234567890123456")]
        public void SeuNumeroInvalido(string seuNumero)
        {
            var e = Assert.Throws<ArgumentException>(() => new Titulo(
                _titulo.Valor,
                _titulo.Especie,
                _titulo.NossoNumero,
                seuNumero,
                _titulo.DataVencimento,
                _titulo.DataEmissao));
            
            Assert.Equal("SeuNumero", e.ParamName);
        }
        
    }
}