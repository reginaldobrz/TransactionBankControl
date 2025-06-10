using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TransactionBankControl.Domain.Enums;

namespace TransactionBankControl.Domain.Entities
{
    public class Auditoria
    {
        [Key]
        public Guid Id { get; private set; }
        public Guid ContaBancariaId { get; private set; }
        public string Documento { get; private set; }

        public TipoAuditoria Tipo { get; private set; }
        public DateTime Data { get; private set; }

        [JsonIgnore]
        public ContaBancaria ContaBancaria { get; set; }

        public Auditoria(
            Guid contaBancariaId,
            string documento,
            TipoAuditoria tipo)
        {
            ContaBancariaId = contaBancariaId;
            Documento = documento;
            Tipo = tipo;
            Data = DateTime.Now;
        }
    }
}
