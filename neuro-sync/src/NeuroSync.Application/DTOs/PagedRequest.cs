using System.ComponentModel.DataAnnotations;

namespace NeuroSync.Application.DTOs
{
    public class PagedRequest
    {
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 20;

        public string? SortBy { get; set; }
        public string SortDirection { get; set; } = "asc";
    }
}
