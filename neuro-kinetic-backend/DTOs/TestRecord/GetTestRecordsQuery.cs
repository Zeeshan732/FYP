namespace neuro_kinetic_backend.DTOs.TestRecord
{
    public class GetTestRecordsQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "testDate";
        public string SortOrder { get; set; } = "desc";
        public int? UserId { get; set; }
        public string? Status { get; set; }
        public string? TestResult { get; set; }
    }
}

