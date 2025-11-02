using neuro_kinetic_backend.Models;

namespace neuro_kinetic_backend.Repositories
{
    public interface IRepositoryUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IPublicationRepository Publications { get; }
        IRepository<PerformanceMetric> PerformanceMetrics { get; }
        IRepository<Dataset> Datasets { get; }
        IAnalysisResultRepository AnalysisResults { get; }
        IRepository<CrossValidationResult> CrossValidationResults { get; }
        IRepository<TechnicalDocument> TechnicalDocuments { get; }
        IRepository<EducationalResource> EducationalResources { get; }
        IRepository<CollaborationRequest> CollaborationRequests { get; }
        IRepository<UploadedFile> UploadedFiles { get; }
        
        Task<int> SaveChangesAsync();
    }
}

