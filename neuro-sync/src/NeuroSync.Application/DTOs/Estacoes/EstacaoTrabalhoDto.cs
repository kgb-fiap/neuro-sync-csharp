using System.Collections.Generic;
using NeuroSync.Application.Responses;

namespace NeuroSync.Application.DTOs.Estacoes
{
    public class EstacaoTrabalhoDto
    {
        public int Id { get; set; }
        public string CodigoEstacao { get; set; } = string.Empty;
        public int ZonaSensorialId { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool PermiteReserva { get; set; }
        public bool PermiteUsoEspontaneo { get; set; }
        public IEnumerable<LinkDto> Links { get; set; } = new List<LinkDto>();
    }
}
