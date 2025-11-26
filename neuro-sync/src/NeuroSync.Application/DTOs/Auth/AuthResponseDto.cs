using System;
using System.Collections.Generic;

namespace NeuroSync.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiraEm { get; set; }
        public int UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IEnumerable<string> Perfis { get; set; } = Array.Empty<string>();
    }
}
