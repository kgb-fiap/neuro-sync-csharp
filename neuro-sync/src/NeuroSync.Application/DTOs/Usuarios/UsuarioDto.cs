using System.Collections.Generic;

namespace NeuroSync.Application.DTOs.Usuarios
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; } = string.Empty;
        public string EmailCorporativo { get; set; } = string.Empty;
        public string? Telefone { get; set; }
        public string? MatriculaInterna { get; set; }
        public bool Ativo { get; set; }
        public bool Neurodivergente { get; set; }
        public IEnumerable<string> Perfis { get; set; } = new List<string>();
    }
}
