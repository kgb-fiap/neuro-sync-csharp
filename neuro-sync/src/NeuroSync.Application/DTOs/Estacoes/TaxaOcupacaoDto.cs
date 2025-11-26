using System;

namespace NeuroSync.Application.DTOs.Estacoes
{
    public class TaxaOcupacaoDto
    {
        public int EstacaoId { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public decimal TaxaOcupacao { get; set; }
    }
}
