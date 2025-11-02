using Microsoft.EntityFrameworkCore;
using neuro_kinetic_backend.Data;
using neuro_kinetic_backend.Models;
using System;

namespace neuro_kinetic_backend.Data.Seed
{
    public class DataSeeder
    {
        private readonly ApplicationDbContext _context;

        public DataSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Ensure database is created
            await _context.Database.MigrateAsync();

            // Seed data only if tables are empty
            if (!await _context.Users.AnyAsync())
            {
                await SeedUsersAsync();
            }

            if (!await _context.Publications.AnyAsync())
            {
                await SeedPublicationsAsync();
            }

            if (!await _context.PerformanceMetrics.AnyAsync())
            {
                await SeedMetricsAsync();
            }

            if (!await _context.Datasets.AnyAsync())
            {
                await SeedDatasetsAsync();
            }

            if (!await _context.CrossValidationResults.AnyAsync())
            {
                await SeedCrossValidationResultsAsync();
            }

            if (!await _context.EducationalResources.AnyAsync())
            {
                await SeedEducationalResourcesAsync();
            }

            if (!await _context.UserTestRecords.AnyAsync())
            {
                await SeedUserTestRecordsAsync();
            }

            await _context.SaveChangesAsync();
        }

        private async Task SeedUsersAsync()
        {
            var users = new List<User>
            {
                new User
                {
                    Email = "admin@neurokinetic.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    FirstName = "Admin",
                    LastName = "User",
                    Role = UserRole.Admin,
                    IsActive = true,
                    Institution = "Neuro-Kinetic Research Lab",
                    ResearchFocus = "System Administration",
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    Email = "researcher@neurokinetic.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Researcher123!"),
                    FirstName = "Dr. Sarah",
                    LastName = "Johnson",
                    Role = UserRole.Researcher,
                    IsActive = true,
                    Institution = "Medical Research University",
                    ResearchFocus = "Parkinson's Disease Diagnosis using AI",
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    Email = "doctor@neurokinetic.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Doctor123!"),
                    FirstName = "Dr. Michael",
                    LastName = "Chen",
                    Role = UserRole.MedicalProfessional,
                    IsActive = true,
                    Institution = "City Hospital Neurology Department",
                    ResearchFocus = "Clinical Application of AI Diagnostics",
                    CreatedAt = DateTime.UtcNow
                }
            };

            await _context.Users.AddRangeAsync(users);
        }

        private async Task SeedPublicationsAsync()
        {
            var publications = new List<Publication>
            {
                new Publication
                {
                    Title = "Multi-Modal Domain Adaptation for Parkinson's Disease Detection",
                    Abstract = "This paper presents a novel approach to Parkinson's disease detection using voice and gait analysis with domain adaptation techniques. Our Conditional Multi-Source Domain Adversarial Network (CMDAN) achieves 87% accuracy across multiple clinical sites.",
                    Authors = "Sarah Johnson, Michael Chen, Emma Davis",
                    Journal = "Journal of Medical AI",
                    Year = "2024",
                    DOI = "10.1000/xyz123",
                    Link = "https://example.com/publication1",
                    Type = PublicationType.Journal,
                    IsFeatured = true,
                    Tags = "parkinson,ai,domain-adaptation,multi-modal",
                    CreatedAt = DateTime.UtcNow
                },
                new Publication
                {
                    Title = "Cross-Domain Validation of Voice Analysis Models for Neurological Disorders",
                    Abstract = "We validate voice analysis models across multiple clinical datasets, demonstrating robustness and generalizability of our approach in real-world settings.",
                    Authors = "Robert Williams, Sarah Johnson",
                    Journal = "ICML 2024",
                    Year = "2024",
                    DOI = "10.1000/xyz456",
                    Link = "https://example.com/publication2",
                    Type = PublicationType.Conference,
                    IsFeatured = true,
                    Tags = "voice-analysis,cross-validation,neurology",
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new Publication
                {
                    Title = "Gait Pattern Analysis Using Deep Learning: A Clinical Perspective",
                    Abstract = "This work explores the application of deep learning techniques to gait pattern analysis for early Parkinson's disease detection, with clinical validation results.",
                    Authors = "Michael Chen, Lisa Anderson",
                    Journal = "Clinical Neurology Advances",
                    Year = "2023",
                    DOI = "10.1000/xyz789",
                    Link = "https://example.com/publication3",
                    Type = PublicationType.Journal,
                    IsFeatured = false,
                    Tags = "gait-analysis,deep-learning,clinical",
                    CreatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new Publication
                {
                    Title = "Real-Time Processing Pipeline for Multi-Modal Parkinson's Disease Screening",
                    Abstract = "We present an end-to-end pipeline for real-time analysis of voice and gait data, enabling rapid screening and early intervention.",
                    Authors = "Emma Davis, Robert Williams",
                    Journal = "IEEE Transactions on Biomedical Engineering",
                    Year = "2024",
                    DOI = "10.1000/xyz012",
                    Link = "https://example.com/publication4",
                    Type = PublicationType.Journal,
                    IsFeatured = true,
                    Tags = "real-time,processing-pipeline,multi-modal",
                    CreatedAt = DateTime.UtcNow.AddDays(-3)
                }
            };

            await _context.Publications.AddRangeAsync(publications);
        }

        private async Task SeedMetricsAsync()
        {
            var metrics = new List<PerformanceMetric>
            {
                new PerformanceMetric
                {
                    MetricName = "Cross-Domain Accuracy",
                    Dataset = "Multi-Site Clinical Dataset",
                    Accuracy = 0.87m,
                    Precision = 0.85m,
                    Recall = 0.89m,
                    F1Score = 0.87m,
                    Specificity = 0.91m,
                    Sensitivity = 0.89m,
                    DomainAdaptationDrop = 0.08m,
                    ValidationMethod = "5-Fold Cross Validation",
                    ModelVersion = "v2.1",
                    Notes = "Performance across 5 different clinical sites",
                    CreatedAt = DateTime.UtcNow
                },
                new PerformanceMetric
                {
                    MetricName = "Voice-Only Performance",
                    Dataset = "Voice Analysis Dataset",
                    Accuracy = 0.82m,
                    Precision = 0.80m,
                    Recall = 0.84m,
                    F1Score = 0.82m,
                    Specificity = 0.88m,
                    Sensitivity = 0.84m,
                    DomainAdaptationDrop = 0.12m,
                    ValidationMethod = "10-Fold Cross Validation",
                    ModelVersion = "v2.0",
                    Notes = "Single modality performance - voice only",
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new PerformanceMetric
                {
                    MetricName = "Gait-Only Performance",
                    Dataset = "Gait Analysis Dataset",
                    Accuracy = 0.79m,
                    Precision = 0.77m,
                    Recall = 0.81m,
                    F1Score = 0.79m,
                    Specificity = 0.85m,
                    Sensitivity = 0.81m,
                    DomainAdaptationDrop = 0.15m,
                    ValidationMethod = "5-Fold Cross Validation",
                    ModelVersion = "v2.0",
                    Notes = "Single modality performance - gait only",
                    CreatedAt = DateTime.UtcNow.AddDays(-4)
                },
                new PerformanceMetric
                {
                    MetricName = "Multi-Modal Performance",
                    Dataset = "Combined Voice and Gait Dataset",
                    Accuracy = 0.91m,
                    Precision = 0.89m,
                    Recall = 0.93m,
                    F1Score = 0.91m,
                    Specificity = 0.95m,
                    Sensitivity = 0.93m,
                    DomainAdaptationDrop = 0.06m,
                    ValidationMethod = "5-Fold Cross Validation",
                    ModelVersion = "v2.1",
                    Notes = "Best performance with both modalities",
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                }
            };

            await _context.PerformanceMetrics.AddRangeAsync(metrics);
        }

        private async Task SeedDatasetsAsync()
        {
            var datasets = new List<Dataset>
            {
                new Dataset
                {
                    Name = "Multi-Site Clinical Dataset",
                    Source = "Various Clinical Sites",
                    Version = "1.0",
                    TotalSamples = 1250,
                    VoiceSamples = 980,
                    GaitSamples = 920,
                    MultiModalSamples = 650,
                    Description = "Comprehensive dataset collected from multiple clinical sites across different regions. Includes both voice and gait data for Parkinson's disease patients and healthy controls.",
                    License = "Research Use Only",
                    AccessLink = "https://example.com/dataset1",
                    IsPublic = false,
                    Citation = "Johnson, S. et al. Multi-Site Clinical Dataset for Parkinson's Research. 2024.",
                    CreatedAt = DateTime.UtcNow
                },
                new Dataset
                {
                    Name = "Voice Analysis Benchmark",
                    Source = "Public Repository",
                    Version = "2.1",
                    TotalSamples = 850,
                    VoiceSamples = 850,
                    GaitSamples = 0,
                    MultiModalSamples = 0,
                    Description = "Publicly available voice analysis dataset for Parkinson's disease research. Contains audio recordings from multiple sources.",
                    License = "CC BY 4.0",
                    AccessLink = "https://example.com/dataset2",
                    IsPublic = true,
                    Citation = "Public Voice Analysis Dataset. Open Research Data Initiative. 2023.",
                    CreatedAt = DateTime.UtcNow.AddDays(-30)
                },
                new Dataset
                {
                    Name = "Gait Pattern Dataset",
                    Source = "Motion Capture Lab",
                    Version = "1.5",
                    TotalSamples = 620,
                    VoiceSamples = 0,
                    GaitSamples = 620,
                    MultiModalSamples = 0,
                    Description = "Motion capture data for gait pattern analysis. Includes 3D skeleton data and temporal gait parameters.",
                    License = "Research Use Only",
                    AccessLink = "https://example.com/dataset3",
                    IsPublic = false,
                    Citation = "Chen, M. Gait Pattern Dataset for Neurological Disorders. 2024.",
                    CreatedAt = DateTime.UtcNow.AddDays(-20)
                }
            };

            await _context.Datasets.AddRangeAsync(datasets);
        }

        private async Task SeedCrossValidationResultsAsync()
        {
            var results = new List<CrossValidationResult>
            {
                new CrossValidationResult
                {
                    DatasetName = "Multi-Site Clinical Dataset",
                    ValidationMethod = "5-Fold Cross Validation",
                    FoldNumber = 1,
                    Accuracy = 0.86m,
                    Precision = 0.84m,
                    Recall = 0.88m,
                    F1Score = 0.86m,
                    DomainAdaptationDrop = 0.09m,
                    SourceSite = "Site A",
                    TargetSite = "Site B",
                    ModelVersion = "v2.1",
                    Notes = "Fold 1: Site A to Site B",
                    CreatedAt = DateTime.UtcNow
                },
                new CrossValidationResult
                {
                    DatasetName = "Multi-Site Clinical Dataset",
                    ValidationMethod = "5-Fold Cross Validation",
                    FoldNumber = 2,
                    Accuracy = 0.88m,
                    Precision = 0.86m,
                    Recall = 0.90m,
                    F1Score = 0.88m,
                    DomainAdaptationDrop = 0.07m,
                    SourceSite = "Site B",
                    TargetSite = "Site C",
                    ModelVersion = "v2.1",
                    Notes = "Fold 2: Site B to Site C",
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new CrossValidationResult
                {
                    DatasetName = "Voice Analysis Benchmark",
                    ValidationMethod = "10-Fold Cross Validation",
                    FoldNumber = 1,
                    Accuracy = 0.83m,
                    Precision = 0.81m,
                    Recall = 0.85m,
                    F1Score = 0.83m,
                    DomainAdaptationDrop = 0.11m,
                    SourceSite = "Lab A",
                    TargetSite = "Lab B",
                    ModelVersion = "v2.0",
                    Notes = "Voice-only cross-validation",
                    CreatedAt = DateTime.UtcNow.AddDays(-3)
                }
            };

            await _context.CrossValidationResults.AddRangeAsync(results);
        }

        private async Task SeedEducationalResourcesAsync()
        {
            var resources = new List<EducationalResource>
            {
                new EducationalResource
                {
                    Title = "Parkinson's Disease Progression Timeline",
                    Description = "Interactive timeline showing the stages of Parkinson's disease progression, from early symptoms to advanced stages, with key milestones and symptoms at each stage.",
                    Type = ResourceType.Timeline,
                    ContentJson = "{\"stages\":[{\"stage\":\"Early\",\"duration\":\"1-2 years\",\"symptoms\":[\"Tremor\",\"Rigidity\"]},{\"stage\":\"Moderate\",\"duration\":\"3-5 years\",\"symptoms\":[\"Balance issues\",\"Speech changes\"]},{\"stage\":\"Advanced\",\"duration\":\"6+ years\",\"symptoms\":[\"Severe motor impairment\",\"Cognitive decline\"]}]}",
                    IsActive = true,
                    DisplayOrder = 1,
                    Tags = "parkinson,timeline,progression,education",
                    CreatedAt = DateTime.UtcNow
                },
                new EducationalResource
                {
                    Title = "Domain Adaptation Explained",
                    Description = "An explainer on how domain adaptation helps AI models perform well across different clinical settings, reducing the performance drop when deployed to new sites.",
                    Type = ResourceType.Explainer,
                    ContentJson = "{\"sections\":[{\"title\":\"What is Domain Adaptation?\",\"content\":\"Domain adaptation is a technique that...\"},{\"title\":\"Why It Matters\",\"content\":\"Clinical settings vary significantly...\"},{\"title\":\"Our Approach\",\"content\":\"We use Conditional Multi-Source Domain Adversarial Networks...\"}]}",
                    IsActive = true,
                    DisplayOrder = 2,
                    Tags = "domain-adaptation,ai,explainer",
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new EducationalResource
                {
                    Title = "Case Study: Early Detection Success",
                    Description = "Real-world case study demonstrating successful early detection of Parkinson's disease using our multi-modal approach, leading to timely intervention.",
                    Type = ResourceType.CaseStudy,
                    ContentJson = "{\"patient\":\"65-year-old male\",\"symptoms\":[\"Mild voice tremor\",\"Slight gait changes\"],\"outcome\":\"Early diagnosis enabled proactive treatment\",\"impact\":\"Improved quality of life outcomes\"}",
                    IsActive = true,
                    DisplayOrder = 3,
                    Tags = "case-study,early-detection,success",
                    CreatedAt = DateTime.UtcNow.AddDays(-10)
                }
            };

            await _context.EducationalResources.AddRangeAsync(resources);
        }

        private async Task SeedUserTestRecordsAsync()
        {
            // Get user IDs from seeded users
            var users = await _context.Users.ToListAsync();
            var adminUser = users.FirstOrDefault(u => u.Email == "admin@neurokinetic.com");
            var researcherUser = users.FirstOrDefault(u => u.Email == "researcher@neurokinetic.com");
            var publicUser = users.FirstOrDefault(u => u.Email == "public@neurokinetic.com");
            
            var testRecords = new List<UserTestRecord>();
            var random = new Random();
            
            // Create test records for the last 30 days
            for (int i = 0; i < 100; i++)
            {
                var daysAgo = random.Next(0, 30);
                var testDate = DateTime.UtcNow.AddDays(-daysAgo);
                var userIndex = random.Next(0, 3);
                var userId = userIndex == 0 ? adminUser?.Id ?? 1
                    : userIndex == 1 ? researcherUser?.Id ?? 2
                    : publicUser?.Id ?? 3;
                
                var testResult = random.Next(0, 100) switch
                {
                    < 30 => "Positive",
                    < 70 => "Negative",
                    _ => "Uncertain"
                };
                var accuracy = testResult == "Uncertain" 
                    ? random.Next(50, 75) + random.NextDouble()
                    : random.Next(75, 95) + random.NextDouble();
                var status = random.Next(0, 100) switch
                {
                    < 85 => "Completed",
                    < 95 => "Pending",
                    _ => "Failed"
                };
                
                var userName = userIndex == 0 ? adminUser?.Email ?? "admin@neurokinetic.com"
                    : userIndex == 1 ? researcherUser?.Email ?? "researcher@neurokinetic.com"
                    : publicUser?.Email ?? "public@neurokinetic.com";
                
                testRecords.Add(new UserTestRecord
                {
                    UserId = userId,
                    UserName = userName,
                    TestDate = testDate,
                    TestResult = testResult,
                    Accuracy = Math.Round(accuracy, 1),
                    Status = status,
                    VoiceRecordingUrl = status == "Completed" ? $"/uploads/voice/recording_{i + 1}.wav" : null,
                    AnalysisNotes = status == "Completed" 
                        ? $"Analysis completed on {testDate:yyyy-MM-dd}. Model confidence: {Math.Round(accuracy, 1)}%."
                        : status == "Pending" 
                            ? "Analysis pending. Recording uploaded successfully."
                            : "Analysis failed. Please try again.",
                    CreatedAt = testDate
                });
            }
            
            // Create some recent test records for better analytics
            for (int i = 0; i < 20; i++)
            {
                var daysAgo = random.Next(0, 7);
                var testDate = DateTime.UtcNow.AddDays(-daysAgo);
                var userIndex = random.Next(0, 3);
                var userId = userIndex == 0 ? adminUser?.Id ?? 1
                    : userIndex == 1 ? researcherUser?.Id ?? 2
                    : publicUser?.Id ?? 3;
                
                var testResult = random.Next(0, 100) switch
                {
                    < 25 => "Positive",
                    < 75 => "Negative",
                    _ => "Uncertain"
                };
                var accuracy = testResult == "Uncertain"
                    ? random.Next(50, 75) + random.NextDouble()
                    : random.Next(80, 95) + random.NextDouble();
                
                var userName = userIndex == 0 ? adminUser?.Email ?? "admin@neurokinetic.com"
                    : userIndex == 1 ? researcherUser?.Email ?? "researcher@neurokinetic.com"
                    : publicUser?.Email ?? "public@neurokinetic.com";
                
                testRecords.Add(new UserTestRecord
                {
                    UserId = userId,
                    UserName = userName,
                    TestDate = testDate,
                    TestResult = testResult,
                    Accuracy = Math.Round(accuracy, 1),
                    Status = "Completed",
                    VoiceRecordingUrl = $"/uploads/voice/recording_{100 + i + 1}.wav",
                    AnalysisNotes = $"Recent test completed. Model confidence: {Math.Round(accuracy, 1)}%.",
                    CreatedAt = testDate
                });
            }
            
            await _context.UserTestRecords.AddRangeAsync(testRecords);
        }
    }
}

