using Infrastructure.Interfaces;

namespace Infrastructure.Classes.SearchFilters
{
    public class SearchCategory : IFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Name { get; set; }
    }
}
