using System;
using Xunit;

namespace NetBoletoSantander.Test
{
    public class InstrucoesDoTituloTest
    {
        private readonly dynamic _instrucoesDoTitulo;
            
        public InstrucoesDoTituloTest()
        {
            _instrucoesDoTitulo = new
            {
                Multa = 5,
                MultaApos = 5,
                Juros = 5,
                TipoDesconto = TipoDesconto.PercentualAteAData,
                ValorDesconto = 45,
                DataLimiteDesconto = DateTime.Today,
                ValorAbatimento = 5,
                TipoProtesto = TipoProtesto.NaoProtestar,
                ProtestarApos = 6,
                BaixarApos = 6,
                TipoPagamento = TipoPagamento.ConformeRegistro,
                QuantidadePagamentosPossiveis = 1,
                TipoValor = TipoValor.Valor,
                PercentualMinimo = 100,
                PercentualMaximo = 100,
            };
        }

        [Theory]
        [InlineData(-1)]
        public void MultaInvalido(double multa)
        {
            var e = Assert.Throws<ArgumentException>(() => new InstrucoesDoTitulo(multa,
                _instrucoesDoTitulo.MultaApos, _instrucoesDoTitulo.Juros, _instrucoesDoTitulo.TipoDesconto,
                _instrucoesDoTitulo.ValorDesconto,
                _instrucoesDoTitulo.DataLimiteDesconto, _instrucoesDoTitulo.ValorAbatimento,
                _instrucoesDoTitulo.TipoProtesto, _instrucoesDoTitulo.ProtestarApos,
                _instrucoesDoTitulo.BaixarApos, _instrucoesDoTitulo.TipoPagamento,
                _instrucoesDoTitulo.QuantidadePagamentosPossiveis,
                _instrucoesDoTitulo.TipoValor, _instrucoesDoTitulo.PercentualMinimo,
                _instrucoesDoTitulo.PercentualMaximo));
            
            Assert.Equal("Multa", e.ParamName);
        }

        [Theory]
        [InlineData(-1)]
        public void JurosInvalido(double juros)
        {
            var e = Assert.Throws<ArgumentException>(() => new InstrucoesDoTitulo(_instrucoesDoTitulo.Multa,
                _instrucoesDoTitulo.MultaApos, juros, _instrucoesDoTitulo.TipoDesconto,
                _instrucoesDoTitulo.ValorDesconto,
                _instrucoesDoTitulo.DataLimiteDesconto, _instrucoesDoTitulo.ValorAbatimento,
                _instrucoesDoTitulo.TipoProtesto, _instrucoesDoTitulo.ProtestarApos,
                _instrucoesDoTitulo.BaixarApos, _instrucoesDoTitulo.TipoPagamento,
                _instrucoesDoTitulo.QuantidadePagamentosPossiveis,
                _instrucoesDoTitulo.TipoValor, _instrucoesDoTitulo.PercentualMinimo,
                _instrucoesDoTitulo.PercentualMaximo));
            
            Assert.Equal("Juros", e.ParamName);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(100)]
        public void MultaAposInvalido(int multaApos)
        {
            var e = Assert.Throws<ArgumentException>(() => new InstrucoesDoTitulo(_instrucoesDoTitulo.Multa,
                multaApos, _instrucoesDoTitulo.Juros, _instrucoesDoTitulo.TipoDesconto,
                _instrucoesDoTitulo.ValorDesconto,
                _instrucoesDoTitulo.DataLimiteDesconto, _instrucoesDoTitulo.ValorAbatimento,
                _instrucoesDoTitulo.TipoProtesto, _instrucoesDoTitulo.ProtestarApos,
                _instrucoesDoTitulo.BaixarApos, _instrucoesDoTitulo.TipoPagamento,
                _instrucoesDoTitulo.QuantidadePagamentosPossiveis,
                _instrucoesDoTitulo.TipoValor, _instrucoesDoTitulo.PercentualMinimo,
                _instrucoesDoTitulo.PercentualMaximo));
            
            Assert.Equal("MultaApos", e.ParamName);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(100)]
        public void ProtestarAposInvalido(int protestarApos)
        {
            var e = Assert.Throws<ArgumentException>(() => new InstrucoesDoTitulo(_instrucoesDoTitulo.Multa,
                _instrucoesDoTitulo.MultaApos, _instrucoesDoTitulo.Juros, _instrucoesDoTitulo.TipoDesconto,
                _instrucoesDoTitulo.ValorDesconto,
                _instrucoesDoTitulo.DataLimiteDesconto, _instrucoesDoTitulo.ValorAbatimento,
                _instrucoesDoTitulo.TipoProtesto, protestarApos,
                _instrucoesDoTitulo.BaixarApos, _instrucoesDoTitulo.TipoPagamento,
                _instrucoesDoTitulo.QuantidadePagamentosPossiveis,
                _instrucoesDoTitulo.TipoValor, _instrucoesDoTitulo.PercentualMinimo,
                _instrucoesDoTitulo.PercentualMaximo));
            
            Assert.Equal("ProtestarApos", e.ParamName);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(100)]
        public void BaixarAposInvalido(int baixarApos)
        {
            var e = Assert.Throws<ArgumentException>(() => new InstrucoesDoTitulo(_instrucoesDoTitulo.Multa,
                _instrucoesDoTitulo.MultaApos, _instrucoesDoTitulo.Juros, _instrucoesDoTitulo.TipoDesconto,
                _instrucoesDoTitulo.ValorDesconto,
                _instrucoesDoTitulo.DataLimiteDesconto, _instrucoesDoTitulo.ValorAbatimento,
                _instrucoesDoTitulo.TipoProtesto, _instrucoesDoTitulo.ProtestarApos,
                baixarApos, _instrucoesDoTitulo.TipoPagamento,
                _instrucoesDoTitulo.QuantidadePagamentosPossiveis,
                _instrucoesDoTitulo.TipoValor, _instrucoesDoTitulo.PercentualMinimo,
                _instrucoesDoTitulo.PercentualMaximo));
            
            Assert.Equal("BaixarApos", e.ParamName);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(100)]
        public void QuantidadePagamentosPossiveisInvalido(int quantidadePagamentosPossiveis)
        {
            var e = Assert.Throws<ArgumentException>(() => new InstrucoesDoTitulo(_instrucoesDoTitulo.Multa,
                _instrucoesDoTitulo.MultaApos, _instrucoesDoTitulo.Juros, _instrucoesDoTitulo.TipoDesconto,
                _instrucoesDoTitulo.ValorDesconto,
                _instrucoesDoTitulo.DataLimiteDesconto, _instrucoesDoTitulo.ValorAbatimento,
                _instrucoesDoTitulo.TipoProtesto, _instrucoesDoTitulo.ProtestarApos,
                _instrucoesDoTitulo.BaixarApos, _instrucoesDoTitulo.TipoPagamento,
                quantidadePagamentosPossiveis,
                _instrucoesDoTitulo.TipoValor, _instrucoesDoTitulo.PercentualMinimo,
                _instrucoesDoTitulo.PercentualMaximo));
            
            Assert.Equal("QuantidadePagamentosPossiveis", e.ParamName);
        }
        
        [Theory]
        [InlineData(-1)]
        public void ValorDescontoInvalido(double valorDesconto)
        {
            var e = Assert.Throws<ArgumentException>(() => new InstrucoesDoTitulo(_instrucoesDoTitulo.Multa,
                _instrucoesDoTitulo.MultaApos, _instrucoesDoTitulo.Juros, _instrucoesDoTitulo.TipoDesconto,
                valorDesconto,
                _instrucoesDoTitulo.DataLimiteDesconto, _instrucoesDoTitulo.ValorAbatimento,
                _instrucoesDoTitulo.TipoProtesto, _instrucoesDoTitulo.ProtestarApos,
                _instrucoesDoTitulo.BaixarApos, _instrucoesDoTitulo.TipoPagamento,
                _instrucoesDoTitulo.QuantidadePagamentosPossiveis,
                _instrucoesDoTitulo.TipoValor, _instrucoesDoTitulo.PercentualMinimo,
                _instrucoesDoTitulo.PercentualMaximo));
            
            Assert.Equal("ValorDesconto", e.ParamName);
        }
        
        [Theory]
        [InlineData(-1)]
        public void ValorAbatimentoInvalido(double valorAbatimento)
        {
            var e = Assert.Throws<ArgumentException>(() => new InstrucoesDoTitulo(_instrucoesDoTitulo.Multa,
                _instrucoesDoTitulo.MultaApos, _instrucoesDoTitulo.Juros, _instrucoesDoTitulo.TipoDesconto,
                _instrucoesDoTitulo.ValorDesconto,
                _instrucoesDoTitulo.DataLimiteDesconto, valorAbatimento,
                _instrucoesDoTitulo.TipoProtesto, _instrucoesDoTitulo.ProtestarApos,
                _instrucoesDoTitulo.BaixarApos, _instrucoesDoTitulo.TipoPagamento,
                _instrucoesDoTitulo.QuantidadePagamentosPossiveis,
                _instrucoesDoTitulo.TipoValor, _instrucoesDoTitulo.PercentualMinimo,
                _instrucoesDoTitulo.PercentualMaximo));
            
            Assert.Equal("ValorAbatimento", e.ParamName);
        }
        
        [Theory]
        [InlineData(-1)]
        public void PercentualMinimoInvalido(double percentualMinimo)
        {
            var e = Assert.Throws<ArgumentException>(() => new InstrucoesDoTitulo(_instrucoesDoTitulo.Multa,
                _instrucoesDoTitulo.MultaApos, _instrucoesDoTitulo.Juros, _instrucoesDoTitulo.TipoDesconto,
                _instrucoesDoTitulo.ValorDesconto,
                _instrucoesDoTitulo.DataLimiteDesconto, _instrucoesDoTitulo.ValorAbatimento,
                _instrucoesDoTitulo.TipoProtesto, _instrucoesDoTitulo.ProtestarApos,
                _instrucoesDoTitulo.BaixarApos, _instrucoesDoTitulo.TipoPagamento,
                _instrucoesDoTitulo.QuantidadePagamentosPossiveis,
                _instrucoesDoTitulo.TipoValor, percentualMinimo,
                _instrucoesDoTitulo.PercentualMaximo));
            
            Assert.Equal("PercentualMinimo", e.ParamName);
        }
        
        [Theory]
        [InlineData(-1)]
        public void PercentualMaximoInvalido(double percentualMaximo)
        {
            var e = Assert.Throws<ArgumentException>(() => new InstrucoesDoTitulo(_instrucoesDoTitulo.Multa,
                _instrucoesDoTitulo.MultaApos, _instrucoesDoTitulo.Juros, _instrucoesDoTitulo.TipoDesconto,
                _instrucoesDoTitulo.ValorDesconto,
                _instrucoesDoTitulo.DataLimiteDesconto, _instrucoesDoTitulo.ValorAbatimento,
                _instrucoesDoTitulo.TipoProtesto, _instrucoesDoTitulo.ProtestarApos,
                _instrucoesDoTitulo.BaixarApos, _instrucoesDoTitulo.TipoPagamento,
                _instrucoesDoTitulo.QuantidadePagamentosPossiveis,
                _instrucoesDoTitulo.TipoValor, _instrucoesDoTitulo.PercentualMinimo,
                percentualMaximo));
            
            Assert.Equal("PercentualMaximo", e.ParamName);
        }
    }
}