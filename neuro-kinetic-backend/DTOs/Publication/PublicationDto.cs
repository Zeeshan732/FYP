namespace neuro_kinetic_backend.DTOs.Publication
{
    public class PublicationDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Abstract { get; set; }
        public string? Authors { get; set; }
        public string? Journal { get; set; }
        public string? Year { get; set; }
        public string? DOI { get; set; }
        public string? Link { get; set; }
        public string Type { get; set; } = string.Empty;
        public bool IsFeatured { get; set; }
        public string? Tags { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

