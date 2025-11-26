using System.Collections.Generic;

namespace NeuroSync.Domain.Entities
{
    public class Perfil : BaseEntity
    {
        public string NomePerfil { get; private set; } = string.Empty;
        public string? Descricao { get; private set; }
        public int? NivelAcesso { get; private set; }

        public ICollection<UsuarioPerfil> Usuarios { get; private set; } = new List<UsuarioPerfil>();

        public Perfil(string nomePerfil)
        {
            NomePerfil = nomePerfil;
        }

        protected Perfil() { }
    }
}
