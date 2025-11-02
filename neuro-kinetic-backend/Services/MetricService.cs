using Microsoft.EntityFrameworkCore;
using neuro_kinetic_backend.Data;
using neuro_kinetic_backend.DTOs.Common;
using neuro_kinetic_backend.DTOs.Metric;
using neuro_kinetic_backend.Models;
using neuro_kinetic_backend.Repositories;
using System.Linq;

namespace neuro_kinetic_backend.Services
{
    public class MetricService : IMetricService
    {
        private readonly IRepository<PerformanceMetric> _repository;
        private readonly ApplicationDbContext _context;
        
        public MetricService(IRepository<PerformanceMetric> repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }
        
        public async Task<IEnumerable<PerformanceMetricDto>> GetAllAsync()
        {
            var metrics = await _repository.GetAllAsync();
            return metrics.Select(MapToDto);
        }
        
        public async Task<PagedResult<PerformanceMetricDto>> GetPagedAsync(QueryParameters parameters)
        {
            var query = _context.PerformanceMetrics.AsQueryable();
            
            // Apply sorting
            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                query = parameters.SortBy.ToLower() switch
                {
                    "metricname" => parameters.SortOrder == "desc" 
                        ? query.OrderByDescending(m => m.MetricName) 
                        : query.OrderBy(m => m.MetricName),
                    "dataset" => parameters.SortOrder == "desc" 
                        ? query.OrderByDescending(m => m.Dataset) 
                        : query.OrderBy(m => m.Dataset),
                    "accuracy" => parameters.SortOrder == "desc" 
                        ? query.OrderByDescending(m => m.Accuracy) 
                        : query.OrderBy(m => m.Accuracy),
                    "createdat" => parameters.SortOrder == "desc" 
                        ? query.OrderByDescending(m => m.CreatedAt) 
                        : query.OrderBy(m => m.CreatedAt),
                    _ => query.OrderByDescending(m => m.CreatedAt)
                };
            }
            else
            {
                query = query.OrderByDescending(m => m.CreatedAt);
            }
            
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
            
            return new PagedResult<PerformanceMetricDto>
            {
                Items = items.Select(MapToDto),
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }
        
        public async Task<PerformanceMetricDto?> GetByIdAsync(int id)
        {
            var metric = await _repository.GetByIdAsync(id);
            return metric != null ? MapToDto(metric) : null;
        }
        
        public async Task<IEnumerable<PerformanceMetricDto>> GetByDatasetAsync(string dataset)
        {
            var metrics = await _repository.FindAsync(m => m.Dataset == dataset);
            return metrics.OrderByDescending(m => m.CreatedAt).Select(MapToDto);
        }
        
        public async Task<PagedResult<PerformanceMetricDto>> GetByDatasetPagedAsync(string dataset, QueryParameters parameters)
        {
            var query = _context.PerformanceMetrics.Where(m => m.Dataset == dataset);
            
            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                query = parameters.SortBy.ToLower() switch
                {
                    "metricname" => parameters.SortOrder == "desc" 
                        ? query.OrderByDescending(m => m.MetricName) 
                        : query.OrderBy(m => m.MetricName),
                    "accuracy" => parameters.SortOrder == "desc" 
                        ? query.OrderByDescending(m => m.Accuracy) 
                        : query.OrderBy(m => m.Accuracy),
                    "createdat" => parameters.SortOrder == "desc" 
                        ? query.OrderByDescending(m => m.CreatedAt) 
                        : query.OrderBy(m => m.CreatedAt),
                    _ => query.OrderByDescending(m => m.CreatedAt)
                };
            }
            else
            {
                query = query.OrderByDescending(m => m.CreatedAt);
            }
            
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
            
            return new PagedResult<PerformanceMetricDto>
            {
                Items = items.Select(MapToDto),
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }
        
        public async Task<PerformanceMetricDto> CreateAsync(PerformanceMetricDto dto)
        {
            var metric = new PerformanceMetric
            {
                MetricName = dto.MetricName,
                Dataset = dto.Dataset,
                Accuracy = dto.Accuracy,
                Precision = dto.Precision,
                Recall = dto.Recall,
                F1Score = dto.F1Score,
                Specificity = dto.Specificity,
                Sensitivity = dto.Sensitivity,
                DomainAdaptationDrop = dto.DomainAdaptationDrop,
                ValidationMethod = dto.ValidationMethod,
                Notes = dto.Notes,
                ModelVersion = dto.ModelVersion,
                FoldNumber = dto.FoldNumber,
                CreatedAt = DateTime.UtcNow
            };
            
            await _repository.AddAsync(metric);
            return MapToDto(metric);
        }
        
        public async Task<IEnumerable<PerformanceMetricDto>> GetDashboardMetricsAsync()
        {
            var metrics = await _repository.GetAllAsync();
            return metrics
                .OrderByDescending(m => m.CreatedAt)
                .Take(20)
                .Select(MapToDto);
        }
        
        public async Task<MetricsDashboardDto> GetDashboardAggregatedAsync()
        {
            var metrics = await _context.PerformanceMetrics.ToListAsync();
            
            if (!metrics.Any())
            {
                return new MetricsDashboardDto();
            }
            
            // Calculate averages
            var avgAccuracy = metrics.Average(m => m.Accuracy);
            var avgPrecision = metrics.Average(m => m.Precision);
            var avgRecall = metrics.Average(m => m.Recall);
            var avgF1Score = metrics.Average(m => m.F1Score);
            
            // Get best and worst
            var bestAccuracy = metrics.Max(m => m.Accuracy);
            var worstAccuracy = metrics.Min(m => m.Accuracy);
            
            // Group by dataset
            var metricsByDataset = metrics
                .GroupBy(m => m.Dataset)
                .Select(g => new DatasetMetricsSummaryDto
                {
                    Dataset = g.Key ?? "Unknown",
                    Count = g.Count(),
                    AverageAccuracy = g.Average(m => m.Accuracy),
                    AveragePrecision = g.Average(m => m.Precision),
                    AverageRecall = g.Average(m => m.Recall),
                    AverageF1Score = g.Average(m => m.F1Score),
                    BestAccuracy = g.Max(m => m.Accuracy)
                })
                .ToList();
            
            // Get recent metrics
            var recentMetrics = metrics
                .OrderByDescending(m => m.CreatedAt)
                .Take(10)
                .Select(MapToDto)
                .ToList();
            
            // Calculate trends (last 30 days)
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            var recentMetricsForTrend = metrics
                .Where(m => m.CreatedAt >= thirtyDaysAgo)
                .OrderBy(m => m.CreatedAt)
                .ToList();
            
            var accuracyOverTime = recentMetricsForTrend
                .GroupBy(m => m.CreatedAt.Date)
                .Select(g => new TimeSeriesDataPoint
                {
                    Date = g.Key,
                    Value = g.Average(m => m.Accuracy),
                    Label = g.Key.ToString("yyyy-MM-dd")
                })
                .ToList();
            
            // Calculate improvement
            var firstHalf = recentMetricsForTrend.Take(recentMetricsForTrend.Count / 2).ToList();
            var secondHalf = recentMetricsForTrend.Skip(recentMetricsForTrend.Count / 2).ToList();
            
            var improvement = secondHalf.Any() && firstHalf.Any()
                ? secondHalf.Average(m => m.Accuracy) - firstHalf.Average(m => m.Accuracy)
                : 0;
            
            var trendDirection = improvement > 0.01m ? "improving" 
                : improvement < -0.01m ? "declining" 
                : "stable";
            
            return new MetricsDashboardDto
            {
                TotalMetrics = metrics.Count,
                AverageAccuracy = avgAccuracy,
                AveragePrecision = avgPrecision,
                AverageRecall = avgRecall,
                AverageF1Score = avgF1Score,
                BestAccuracy = bestAccuracy,
                WorstAccuracy = worstAccuracy,
                MetricsByDataset = metricsByDataset,
                RecentMetrics = recentMetrics,
                Trends = new MetricsTrendDto
                {
                    AccuracyOverTime = accuracyOverTime,
                    Improvement = improvement,
                    TrendDirection = trendDirection
                }
            };
        }
        
        private PerformanceMetricDto MapToDto(PerformanceMetric metric)
        {
            return new PerformanceMetricDto
            {
                Id = metric.Id,
                MetricName = metric.MetricName,
                Dataset = metric.Dataset,
                Accuracy = metric.Accuracy,
                Precision = metric.Precision,
                Recall = metric.Recall,
                F1Score = metric.F1Score,
                Specificity = metric.Specificity,
                Sensitivity = metric.Sensitivity,
                DomainAdaptationDrop = metric.DomainAdaptationDrop,
                ValidationMethod = metric.ValidationMethod,
                Notes = metric.Notes,
                ModelVersion = metric.ModelVersion,
                FoldNumber = metric.FoldNumber,
                CreatedAt = metric.CreatedAt
            };
        }
    }
}

