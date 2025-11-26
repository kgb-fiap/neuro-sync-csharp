using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroSync.Application.DTOs.Reservas;
using NeuroSync.Application.Responses;
using NeuroSync.Application.Services;

namespace NeuroSync.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/reservas")]
    public class ReservasController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public ReservasController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ReservaEstacaoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var reserva = await _reservaService.ObterAsync(id);
            return reserva is null ? NotFound() : Ok(reserva);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(PagedResult<ReservaEstacaoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] ReservaSearchRequest request)
        {
            var result = await _reservaService.BuscarAsync(request);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ReservaEstacaoDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] CreateReservaEstacaoDto dto)
        {
            var reserva = await _reservaService.CriarAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = reserva.Id }, reserva);
        }

        [HttpPatch("{id:int}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> AtualizarStatus(int id, [FromQuery] int statusId, [FromQuery] string? motivo)
        {
            await _reservaService.AtualizarStatusAsync(id, statusId, motivo);
            return NoContent();
        }

        [HttpGet("{id:int}/compatibilidade")]
        [ProducesResponseType(typeof(CompatibilidadeReservaDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Compatibilidade(int id)
        {
            var compatibilidade = await _reservaService.VerificarCompatibilidadeAsync(id);
            return Ok(compatibilidade);
        }

        [HttpPost("{id:int}/avaliacao")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Avaliar(int id, [FromBody] AvaliacaoEstacaoDto dto)
        {
            dto.ReservaEstacaoId = id;
            await _reservaService.RegistrarAvaliacaoAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            await _reservaService.RemoverAsync(id);
            return NoContent();
        }
    }
}
