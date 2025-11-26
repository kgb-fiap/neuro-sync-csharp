using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroSync.Application.DTOs.Estacoes;
using NeuroSync.Application.Responses;
using NeuroSync.Application.Services;

namespace NeuroSync.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/estacoes")]
    public class EstacoesController : ControllerBase
    {
        private readonly IEstacaoService _estacaoService;

        public EstacoesController(IEstacaoService estacaoService)
        {
            _estacaoService = estacaoService;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(EstacaoTrabalhoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var estacao = await _estacaoService.ObterAsync(id);
            return estacao is null ? NotFound() : Ok(estacao);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(PagedResult<EstacaoTrabalhoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] EstacaoSearchRequest request)
        {
            var result = await _estacaoService.BuscarAsync(request);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(EstacaoTrabalhoDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] CreateEstacaoTrabalhoDto dto)
        {
            var estacao = await _estacaoService.CriarAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = estacao.Id }, estacao);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Put(int id, [FromBody] CreateEstacaoTrabalhoDto dto)
        {
            await _estacaoService.AtualizarAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            await _estacaoService.RemoverAsync(id);
            return NoContent();
        }

        [HttpGet("{id:int}/indice-conforto")]
        [ProducesResponseType(typeof(IndiceConfortoDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> IndiceConforto(int id, [FromQuery] DateTime inicio, [FromQuery] DateTime fim)
        {
            var indice = await _estacaoService.CalcularIndiceConfortoAsync(id, inicio, fim);
            return Ok(indice);
        }

        [HttpGet("{id:int}/taxa-ocupacao")]
        [ProducesResponseType(typeof(TaxaOcupacaoDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> TaxaOcupacao(int id, [FromQuery] DateTime inicio, [FromQuery] DateTime fim)
        {
            var taxa = await _estacaoService.CalcularTaxaOcupacaoAsync(id, inicio, fim);
            return Ok(taxa);
        }
    }
}
