namespace neuro_kinetic_backend.DTOs.TestRecord
{
    public class UserTestRecordDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime TestDate { get; set; }
        public string TestResult { get; set; } = "Uncertain";
        public double Accuracy { get; set; }
        public string Status { get; set; } = "Pending";
        public string? VoiceRecordingUrl { get; set; }
        public string? AnalysisNotes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}



