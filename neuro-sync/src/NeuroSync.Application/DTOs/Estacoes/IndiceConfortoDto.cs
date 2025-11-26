using System;

namespace NeuroSync.Application.DTOs.Estacoes
{
    public class IndiceConfortoDto
    {
        public int EstacaoId { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public decimal IndiceConforto { get; set; }
    }
}
