namespace neuro_kinetic_backend.DTOs.Dataset
{
    public class DatasetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Source { get; set; }
        public string? Version { get; set; }
        public int? TotalSamples { get; set; }
        public int? VoiceSamples { get; set; }
        public int? GaitSamples { get; set; }
        public int? MultiModalSamples { get; set; }
        public string? Description { get; set; }
        public string? License { get; set; }
        public string? AccessLink { get; set; }
        public bool IsPublic { get; set; }
        public string? Citation { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

