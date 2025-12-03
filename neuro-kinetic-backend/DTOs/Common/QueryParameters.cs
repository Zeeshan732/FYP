namespace neuro_kinetic_backend.DTOs.Common
{
    public class QueryParameters
    {
        private const int MaxPageSize = 100;
        private int _pageSize = 10;
        
        public int PageNumber { get; set; } = 1;
        
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; } = "asc"; // "asc" or "desc"
        public string? SearchTerm { get; set; }
    }
}



