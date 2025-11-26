using System;
using System.ComponentModel.DataAnnotations;

namespace NeuroSync.Application.DTOs.Usuarios
{
    public class CreateUsuarioDto
    {
        [Required]
        public int SetorId { get; set; }

        [Required, StringLength(150)]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string EmailCorporativo { get; set; } = string.Empty;

        [StringLength(50)]
        public string? MatriculaInterna { get; set; }

        [StringLength(20)]
        public string? Telefone { get; set; }

        public DateTime? DataAdmissao { get; set; }

        public bool FlagNeurodivergente { get; set; }

        [Required, StringLength(200, MinimumLength = 6)]
        public string Senha { get; set; } = string.Empty;
    }
}
