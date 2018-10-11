using System;

namespace NetBoletoSantander
{
    public class InstrucoesDoTitulo
    {
        public double Multa { get; }
        public int MultaApos { get; }
        public double Juros { get; }
        public TipoDesconto TipoDesconto { get; }
        public double ValorDesconto { get; }
        public DateTime? DataLimiteDesconto { get; }
        public double ValorAbatimento { get; }
        public TipoProtesto TipoProtesto { get; }
        public int ProtestarApos { get; }
        public int BaixarApos { get; }
        public TipoPagamento TipoPagamento { get; }
        public int QuantidadePagamentosPossiveis { get; }
        public TipoValor TipoValor { get; }
        public double PercentualMinimo { get; }
        public double PercentualMaximo { get; }

        public InstrucoesDoTitulo(double multa, int multaApos, double juros, TipoDesconto tipoDesconto,
            double valorDesconto, DateTime? dataLimiteDesconto, double valorAbatimento, TipoProtesto tipoProtesto,
            int protestarApos, int baixarApos, TipoPagamento tipoPagamento, int quantidadePagamentosPossiveis,
            TipoValor tipoValor, double percentualMinimo, double percentualMaximo)
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
            TipoValor = tipoValor;
            PercentualMinimo = percentualMinimo;
            PercentualMaximo = percentualMaximo;
            
            Validar();
        }

        private void Validar()
        {
            if (Multa < 0)
                throw new ArgumentException("Não poder ser menor que 0.", "Multa");
            
            if (MultaApos < 0)
                throw new ArgumentException("Não poder ser menor que 0.", "MultaApos");
            
            if (MultaApos > 99)
                throw new ArgumentException("Não poder ter mais que 2 dígitos.", "MultaApos");
            
            if (Juros < 0)
                throw new ArgumentException("Não poder ser menor que 0.", "Juros");
            
            if (ValorDesconto < 0)
                throw new ArgumentException("Não poder ser menor que 0.", "ValorDesconto");
            
            if (ValorAbatimento < 0)
                throw new ArgumentException("Não poder ser menor que 0.", "ValorAbatimento");
            
            if (ProtestarApos < 0)
                throw new ArgumentException("Não poder ser menor que 0.", "ProtestarApos");
            
            if (ProtestarApos > 99)
                throw new ArgumentException("Não poder ter mais que 2 dígitos.", "ProtestarApos");
            
            if (BaixarApos < 0)
                throw new ArgumentException("Não poder ser menor que 0.", "BaixarApos");
            
            if (BaixarApos > 99)
                throw new ArgumentException("Não poder ter mais que 2 dígitos.", "BaixarApos");
            
            if (QuantidadePagamentosPossiveis < 0)
                throw new ArgumentException("Não poder ser menor que 0.", "QuantidadePagamentosPossiveis");
            
            if (QuantidadePagamentosPossiveis > 99)
                throw new ArgumentException("Não poder ter mais que 2 dígitos.", "QuantidadePagamentosPossiveis");
            
            if (PercentualMinimo < 0)
                throw new ArgumentException("Não poder ser menor que 0.", "PercentualMinimo");
            
            if (PercentualMaximo < 0)
                throw new ArgumentException("Não poder ser menor que 0.", "PercentualMaximo");
        }
    }
}