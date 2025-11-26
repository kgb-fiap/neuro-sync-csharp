using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroSync.Application.DTOs.Preferencias;
using NeuroSync.Application.Services;

namespace NeuroSync.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/preferencias")]
    public class PreferenciasController : ControllerBase
    {
        private readonly IPreferenciaSensorialService _service;

        public PreferenciasController(IPreferenciaSensorialService service)
        {
            _service = service;
        }

        [HttpPost]
        [ProducesResponseType(typeof(PreferenciaSensorialDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] CreatePreferenciaSensorialDto dto)
        {
            var preferencia = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(GetPorUsuario), new { usuarioId = preferencia.UsuarioId }, preferencia);
        }

        [HttpGet("usuario/{usuarioId:int}")]
        [ProducesResponseType(typeof(IEnumerable<PreferenciaSensorialDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPorUsuario(int usuarioId)
        {
            var lista = await _service.ObterPorUsuarioAsync(usuarioId);
            return Ok(lista);
        }
    }
}
