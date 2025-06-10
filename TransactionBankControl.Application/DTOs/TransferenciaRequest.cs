namespace TransactionBankControl.Application.DTOs
{
    public class TransferenciaRequest
    {
        public string DocumentoOrigem { get; set; } = null!;
        public string DocumentoDestino { get; set; } = null!;
        public decimal Valor { get; set; }
    }
}