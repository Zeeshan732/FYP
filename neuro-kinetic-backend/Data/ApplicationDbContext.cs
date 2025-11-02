using Microsoft.EntityFrameworkCore;
using neuro_kinetic_backend.Models;

namespace neuro_kinetic_backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<PerformanceMetric> PerformanceMetrics { get; set; }
        public DbSet<Dataset> Datasets { get; set; }
        public DbSet<AnalysisResult> AnalysisResults { get; set; }
        public DbSet<CrossValidationResult> CrossValidationResults { get; set; }
        public DbSet<TechnicalDocument> TechnicalDocuments { get; set; }
        public DbSet<EducationalResource> EducationalResources { get; set; }
        public DbSet<CollaborationRequest> CollaborationRequests { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }
        public DbSet<UserTestRecord> UserTestRecords { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Role).HasConversion<int>();
            });
            
            // Publication configuration
            modelBuilder.Entity<Publication>(entity =>
            {
                entity.Property(e => e.Type).HasConversion<int>();
                entity.HasIndex(e => e.Type);
            });
            
            // AnalysisResult configuration
            modelBuilder.Entity<AnalysisResult>(entity =>
            {
                entity.HasIndex(e => e.SessionId);
                entity.Property(e => e.AnalysisType).HasConversion<int>();
                entity.Property(e => e.PredictedClass).HasConversion<int>();
            });
            
            // TechnicalDocument configuration
            modelBuilder.Entity<TechnicalDocument>(entity =>
            {
                entity.Property(e => e.Type).HasConversion<int>();
                entity.Property(e => e.MinimumAccessLevel).HasConversion<int>();
            });
            
            // EducationalResource configuration
            modelBuilder.Entity<EducationalResource>(entity =>
            {
                entity.Property(e => e.Type).HasConversion<int>();
            });
            
            // CollaborationRequest configuration
            modelBuilder.Entity<CollaborationRequest>(entity =>
            {
                entity.Property(e => e.Status).HasConversion<int>();
            });
            
            // UserTestRecord configuration
            modelBuilder.Entity<UserTestRecord>(entity =>
            {
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.TestDate);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.TestResult);
                entity.HasIndex(e => e.UserName);
                
                // Optional foreign key relationship
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}

