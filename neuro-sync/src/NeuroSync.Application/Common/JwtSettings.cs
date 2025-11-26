namespace NeuroSync.Application.Common
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = "chave-secreta-segura";
        public string Issuer { get; set; } = "neuro-sync";
        public string Audience { get; set; } = "neuro-sync-clients";
        public int ExpirationMinutes { get; set; } = 60;
    }
}
