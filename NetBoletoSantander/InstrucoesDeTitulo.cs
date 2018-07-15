using System;

namespace NetBoletoSantander
{
    public class InstrucoesDeTitulo
    {
        public double Multa { get; private set; }
        public int MultaApos { get; private set; }
        public double Juros { get; private set; }
        public TipoDesconto TipoDesconto { get; private set; }
        public double ValorDesconto { get; private set; }
        public DateTime DataLimiteDesconto { get; private set; }
        public double ValorAbatimento { get; private set; }
        public TipoProtesto TipoProtesto { get; private set; }
        public int ProtestarApos { get; private set; }
        public int BaixarApos { get; private set; }
        public TipoPagamento TipoPagamento { get; private set; }
        public int QuantidadePagamentosPossiveis { get; private set; }
        public double PercentualMinimo { get; private set; }
        public double PercentualMaximo { get; private set; }

        private InstrucoesDeTitulo(double multa, int multaApos, double juros, TipoDesconto tipoDesconto,
            double valorDesconto, DateTime dataLimiteDesconto, double valorAbatimento, TipoProtesto tipoProtesto,
            int protestarApos, int baixarApos, TipoPagamento tipoPagamento, int quantidadePagamentosPossiveis,
            double percentualMinimo, double percentualMaximo)
        {
            Multa = multa;
            MultaApos = multaApos;
            Juros = juros;
            TipoDesconto = tipoDesconto;
            ValorDesconto = valorDesconto;
            DataLimiteDesconto = dataLimiteDesconto;
            ValorAbatimento = valorAbatimento;
            TipoProtesto = tipoProtesto;
            ProtestarApos = protestarApos;
            BaixarApos = baixarApos;
            TipoPagamento = tipoPagamento;
            QuantidadePagamentosPossiveis = quantidadePagamentosPossiveis;
            PercentualMinimo = percentualMinimo;
            PercentualMaximo = percentualMaximo;
        }
    }
}