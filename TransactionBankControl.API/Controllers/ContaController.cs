using Microsoft.AspNetCore.Mvc;
using TransactionBankControl.Application.DTOs;
using TransactionBankControl.Application.Interfaces;

namespace TransactionBankControl.API.Controllers
{
    [ApiController]
    [Route("api/contas")]
    public class ContaController(IContaService service) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ContaRequest request)
        {
            await service.CadastrarConta(request);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? nome, [FromQuery] string? documento)
        {
            var contas = await service.ConsultarContas(nome, documento);
            return Ok(contas);
        }

        [HttpPatch("inativar/{documento}")]
        public async Task<IActionResult> Inativar(string documento)
        {
            await service.InativarConta(documento);
            return Ok();
        }

        [HttpPatch("ativar/{documento}")]
        public async Task<IActionResult> Ativar(string documento)
        {
            await service.AtivarConta(documento);
            return Ok();
        }

        [HttpPost("transferir")]
        public async Task<IActionResult> Transferir([FromBody] TransferenciaRequest request)
        {
            await service.Transferir(request);
            return Ok();
        }
    }
}