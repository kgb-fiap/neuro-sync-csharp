using System;

namespace NeuroSync.Domain.Entities
{
    public class UsuarioPerfil : BaseEntity
    {
        public int UsuarioId { get; private set; }
        public int PerfilId { get; private set; }
        public DateTime DataAtribuicao { get; private set; } = DateTime.UtcNow;
        public string? UsuarioResponsavel { get; private set; }

        public Usuario? Usuario { get; private set; }
        public Perfil? Perfil { get; private set; }

        protected UsuarioPerfil() { }

        public UsuarioPerfil(int usuarioId, int perfilId, string? responsavel = null)
        {
            UsuarioId = usuarioId;
            PerfilId = perfilId;
            UsuarioResponsavel = responsavel;
        }
    }
}
