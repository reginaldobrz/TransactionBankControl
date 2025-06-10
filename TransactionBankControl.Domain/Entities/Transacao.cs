using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TransactionBankControl.Domain.Enums;

namespace TransactionBankControl.Domain.Entities
{
    public class Transacao
    {
        [Key]
        public Guid Id { get; private set; }
        public Guid ContaBancariaId { get; set; }
        public decimal Valor { get; private set; }
        public TipoTransacao Tipo { get; private set; }
        public DateTime Data { get; private set; } = DateTime.UtcNow;

        [JsonIgnore]
        public ContaBancaria ContaBancaria { get; set; }
        protected Transacao() { }

        public Transacao(Guid contaBancariaId, decimal valor, TipoTransacao tipo)
        {
            if (valor <= 0)
                throw new ArgumentException("Valor deve ser positivo");

            ContaBancariaId = contaBancariaId;
            Valor = valor;
            Tipo = tipo;
            Data = DateTime.UtcNow;
        }
    }
}