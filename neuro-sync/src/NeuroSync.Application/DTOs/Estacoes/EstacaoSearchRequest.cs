using System.ComponentModel.DataAnnotations;

namespace NeuroSync.Application.DTOs.Estacoes
{
    public class EstacaoSearchRequest : PagedRequest
    {
        public int? ZonaSensorialId { get; set; }

        [StringLength(20)]
        public string? Status { get; set; }

        public string? Codigo { get; set; }
    }
}
