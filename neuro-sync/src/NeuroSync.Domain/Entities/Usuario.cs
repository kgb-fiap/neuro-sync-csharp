using System;
using System.Collections.Generic;

namespace NeuroSync.Domain.Entities
{
    public class Usuario : BaseEntity
    {
        public int SetorId { get; set; }
        public string NomeCompleto { get; set; } = string.Empty;
        public string EmailCorporativo { get; set; } = string.Empty;
        public string? MatriculaInterna { get; set; }
        public string? Telefone { get; set; }
        public DateTime? DataAdmissao { get; set; }
        public bool FlagNeurodivergente { get; set; }
        public string? ObservacoesSuporte { get; set; }
        public string SenhaHash { get; set; } = string.Empty;
        public DateTime? DataUltimoLogin { get; set; }
        public int QuantidadeTentativasLogin { get; private set; }
        public bool MudarSenhaProxLogin { get; private set; }

        public Setor? Setor { get; private set; }
        public ICollection<UsuarioPerfil> Perfis { get; private set; } = new List<UsuarioPerfil>();
        public ICollection<PreferenciaSensorial> Preferencias { get; private set; } = new List<PreferenciaSensorial>();
        public ICollection<ReservaEstacao> Reservas { get; private set; } = new List<ReservaEstacao>();

        protected Usuario() { }

        public Usuario(int setorId, string nomeCompleto, string emailCorporativo, string senhaHash)
        {
            SetorId = setorId;
            NomeCompleto = nomeCompleto;
            EmailCorporativo = emailCorporativo;
            SenhaHash = senhaHash;
        }

        public void RegistrarTentativaFalha()
        {
            QuantidadeTentativasLogin++;
        }

        public void RegistrarLoginSucesso()
        {
            QuantidadeTentativasLogin = 0;
            DataUltimoLogin = DateTime.UtcNow;
        }

        public void DefinirMudancaSenha(bool mudar) => MudarSenhaProxLogin = mudar;
    }
}
