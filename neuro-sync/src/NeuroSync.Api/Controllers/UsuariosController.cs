using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeuroSync.Application.DTOs;
using NeuroSync.Application.DTOs.Usuarios;
using NeuroSync.Application.Responses;
using NeuroSync.Application.Services;

namespace NeuroSync.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var usuario = await _usuarioService.ObterAsync(id);
            return usuario is null ? NotFound() : Ok(usuario);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] PagedRequest request)
        {
            var result = await _usuarioService.BuscarAsync(request);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] CreateUsuarioDto dto)
        {
            var usuario = await _usuarioService.CriarAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = usuario.Id }, usuario);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateUsuarioDto dto)
        {
            dto.Id = id;
            var usuario = await _usuarioService.AtualizarAsync(dto);
            return Ok(usuario);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            await _usuarioService.RemoverAsync(id);
            return NoContent();
        }
    }
}
