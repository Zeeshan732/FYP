using Microsoft.EntityFrameworkCore;
using neuro_kinetic_backend.Data;
using neuro_kinetic_backend.DTOs.TestRecord;

namespace neuro_kinetic_backend.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly ApplicationDbContext _context;
        
        public AdminDashboardService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<AdminDashboardAnalyticsDto> GetAnalyticsAsync()
        {
            var records = await _context.UserTestRecords.ToListAsync();
            
            if (!records.Any())
            {
                return new AdminDashboardAnalyticsDto();
            }
            
            // Total users (unique user IDs)
            var totalUsers = records.Select(r => r.UserId).Distinct().Count();
            
            // Total tests
            var totalTests = records.Count;
            
            // Test results distribution
            var positiveCases = records.Count(r => r.TestResult == "Positive");
            var negativeCases = records.Count(r => r.TestResult == "Negative");
            var uncertainCases = records.Count(r => r.TestResult == "Uncertain");
            
            // Average accuracy (only completed tests)
            var completedTests = records.Where(r => r.Status == "Completed").ToList();
            var averageAccuracy = completedTests.Any() 
                ? completedTests.Average(r => r.Accuracy) 
                : 0.0;
            
            // Usage by day (last 30 days)
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            var usageByDay = records
                .Where(r => r.TestDate >= thirtyDaysAgo)
                .GroupBy(r => r.TestDate.Date)
                .Select(g => new TimeSeriesDataPoint
                {
                    Date = g.Key,
                    Count = g.Count(),
                    Label = g.Key.ToString("MMM d")
                })
                .OrderBy(x => x.Date)
                .ToList();
            
            // Usage by month (last 12 months)
            var twelveMonthsAgo = DateTime.UtcNow.AddMonths(-12);
            var usageByMonth = records
                .Where(r => r.TestDate >= twelveMonthsAgo)
                .GroupBy(r => new { r.TestDate.Year, r.TestDate.Month })
                .Select(g => new TimeSeriesDataPoint
                {
                    Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                    Count = g.Count(),
                    Label = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yyyy")
                })
                .OrderBy(x => x.Date)
                .ToList();
            
            // Usage by year (last 5 years)
            var fiveYearsAgo = DateTime.UtcNow.AddYears(-5);
            var usageByYear = records
                .Where(r => r.TestDate >= fiveYearsAgo)
                .GroupBy(r => r.TestDate.Year)
                .Select(g => new TimeSeriesDataPoint
                {
                    Date = new DateTime(g.Key, 1, 1),
                    Count = g.Count(),
                    Label = g.Key.ToString()
                })
                .OrderBy(x => x.Date)
                .ToList();
            
            // Recent tests (last 10)
            var recentTests = records
                .OrderByDescending(r => r.TestDate)
                .Take(10)
                .Select(r => new UserTestRecordDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    UserName = r.UserName,
                    TestDate = r.TestDate,
                    TestResult = r.TestResult,
                    Accuracy = r.Accuracy,
                    Status = r.Status,
                    VoiceRecordingUrl = r.VoiceRecordingUrl,
                    AnalysisNotes = r.AnalysisNotes,
                    CreatedAt = r.CreatedAt
                })
                .ToList();
            
            return new AdminDashboardAnalyticsDto
            {
                TotalUsers = totalUsers,
                TotalTests = totalTests,
                PositiveCases = positiveCases,
                NegativeCases = negativeCases,
                UncertainCases = uncertainCases,
                AverageAccuracy = averageAccuracy,
                UsageByDay = usageByDay,
                UsageByMonth = usageByMonth,
                UsageByYear = usageByYear,
                RecentTests = recentTests,
                TestResultsDistribution = new TestResultsDistributionDto
                {
                    Positive = positiveCases,
                    Negative = negativeCases,
                    Uncertain = uncertainCases
                }
            };
        }
    }
}



