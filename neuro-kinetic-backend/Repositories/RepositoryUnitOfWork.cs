using neuro_kinetic_backend.Data;
using neuro_kinetic_backend.Models;

namespace neuro_kinetic_backend.Repositories
{
    public class RepositoryUnitOfWork : IRepositoryUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        
        private IUserRepository? _users;
        private IPublicationRepository? _publications;
        private IRepository<PerformanceMetric>? _performanceMetrics;
        private IRepository<Dataset>? _datasets;
        private IAnalysisResultRepository? _analysisResults;
        private IRepository<CrossValidationResult>? _crossValidationResults;
        private IRepository<TechnicalDocument>? _technicalDocuments;
        private IRepository<EducationalResource>? _educationalResources;
        private IRepository<CollaborationRequest>? _collaborationRequests;
        private IRepository<UploadedFile>? _uploadedFiles;
        
        public RepositoryUnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IUserRepository Users =>
            _users ??= new UserRepository(_context);
        
        public IPublicationRepository Publications =>
            _publications ??= new PublicationRepository(_context);
        
        public IRepository<PerformanceMetric> PerformanceMetrics =>
            _performanceMetrics ??= new Repository<PerformanceMetric>(_context);
        
        public IRepository<Dataset> Datasets =>
            _datasets ??= new Repository<Dataset>(_context);
        
        public IAnalysisResultRepository AnalysisResults =>
            _analysisResults ??= new AnalysisResultRepository(_context);
        
        public IRepository<CrossValidationResult> CrossValidationResults =>
            _crossValidationResults ??= new Repository<CrossValidationResult>(_context);
        
        public IRepository<TechnicalDocument> TechnicalDocuments =>
            _technicalDocuments ??= new Repository<TechnicalDocument>(_context);
        
        public IRepository<EducationalResource> EducationalResources =>
            _educationalResources ??= new Repository<EducationalResource>(_context);
        
        public IRepository<CollaborationRequest> CollaborationRequests =>
            _collaborationRequests ??= new Repository<CollaborationRequest>(_context);
        
        public IRepository<UploadedFile> UploadedFiles =>
            _uploadedFiles ??= new Repository<UploadedFile>(_context);
        
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

