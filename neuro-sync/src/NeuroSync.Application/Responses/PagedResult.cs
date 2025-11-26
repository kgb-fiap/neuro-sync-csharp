using System.Collections.Generic;

namespace NeuroSync.Application.Responses
{
    public class PagedResult<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public IEnumerable<T> Items { get; set; } = new List<T>();
    }
}
