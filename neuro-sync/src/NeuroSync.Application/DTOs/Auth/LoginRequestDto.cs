using System.ComponentModel.DataAnnotations;

namespace NeuroSync.Application.DTOs.Auth
{
    public class LoginRequestDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(200, MinimumLength = 6)]
        public string Senha { get; set; } = string.Empty;
    }
}
