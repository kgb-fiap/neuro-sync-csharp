using System;

namespace NeuroSync.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; protected set; }
        public DateTime DataCadastro { get; protected set; } = DateTime.UtcNow;
        public bool Ativo { get; protected set; } = true;

        public void Ativar() => Ativo = true;

        public void Desativar() => Ativo = false;
    }
}
