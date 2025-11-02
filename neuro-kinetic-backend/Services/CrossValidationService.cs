using Microsoft.EntityFrameworkCore;
using neuro_kinetic_backend.Data;
using neuro_kinetic_backend.DTOs.Common;
using neuro_kinetic_backend.DTOs.CrossValidation;
using neuro_kinetic_backend.Models;
using neuro_kinetic_backend.Repositories;

namespace neuro_kinetic_backend.Services
{
    public class CrossValidationService : ICrossValidationService
    {
        private readonly IRepository<CrossValidationResult> _repository;
        private readonly ApplicationDbContext _context;
        
        public CrossValidationService(IRepository<CrossValidationResult> repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }
        
        public async Task<IEnumerable<CrossValidationResultDto>> GetAllAsync()
        {
            var results = await _repository.GetAllAsync();
            return results.Select(MapToDto);
        }
        
        public async Task<PagedResult<CrossValidationResultDto>> GetPagedAsync(QueryParameters parameters)
        {
            var query = _context.CrossValidationResults.AsQueryable();
            
            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                query = parameters.SortBy.ToLower() switch
                {
                    "datasetname" => parameters.SortOrder == "desc"
                        ? query.OrderByDescending(r => r.DatasetName)
                        : query.OrderBy(r => r.DatasetName),
                    "accuracy" => parameters.SortOrder == "desc"
                        ? query.OrderByDescending(r => r.Accuracy)
                        : query.OrderBy(r => r.Accuracy),
                    "foldnumber" => parameters.SortOrder == "desc"
                        ? query.OrderByDescending(r => r.FoldNumber)
                        : query.OrderBy(r => r.FoldNumber),
                    "createdat" => parameters.SortOrder == "desc"
                        ? query.OrderByDescending(r => r.CreatedAt)
                        : query.OrderBy(r => r.CreatedAt),
                    _ => query.OrderByDescending(r => r.CreatedAt)
                };
            }
            else
            {
                query = query.OrderByDescending(r => r.CreatedAt);
            }
            
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
            
            return new PagedResult<CrossValidationResultDto>
            {
                Items = items.Select(MapToDto),
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }
        
        public async Task<CrossValidationResultDto?> GetByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            return result != null ? MapToDto(result) : null;
        }
        
        public async Task<IEnumerable<CrossValidationResultDto>> GetByDatasetAsync(string datasetName)
        {
            var results = await _repository.FindAsync(r => r.DatasetName == datasetName);
            return results.OrderBy(r => r.FoldNumber).Select(MapToDto);
        }
        
        public async Task<PagedResult<CrossValidationResultDto>> GetByDatasetPagedAsync(string datasetName, QueryParameters parameters)
        {
            var query = _context.CrossValidationResults.Where(r => r.DatasetName == datasetName);
            
            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                query = parameters.SortBy.ToLower() switch
                {
                    "accuracy" => parameters.SortOrder == "desc"
                        ? query.OrderByDescending(r => r.Accuracy)
                        : query.OrderBy(r => r.Accuracy),
                    "foldnumber" => parameters.SortOrder == "desc"
                        ? query.OrderByDescending(r => r.FoldNumber)
                        : query.OrderBy(r => r.FoldNumber),
                    "createdat" => parameters.SortOrder == "desc"
                        ? query.OrderByDescending(r => r.CreatedAt)
                        : query.OrderBy(r => r.CreatedAt),
                    _ => query.OrderBy(r => r.FoldNumber)
                };
            }
            else
            {
                query = query.OrderBy(r => r.FoldNumber);
            }
            
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
            
            return new PagedResult<CrossValidationResultDto>
            {
                Items = items.Select(MapToDto),
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }
        
        public async Task<CrossValidationAggregatedDto> GetAggregatedByDatasetAsync(string datasetName)
        {
            var folds = await _repository.FindAsync(r => r.DatasetName == datasetName);
            var foldsList = folds.OrderBy(r => r.FoldNumber).ToList();
            
            if (!foldsList.Any())
            {
                return new CrossValidationAggregatedDto
                {
                    DatasetName = datasetName,
                    TotalFolds = 0
                };
            }
            
            var accuracies = foldsList.Select(f => f.Accuracy).ToList();
            var precisions = foldsList.Select(f => f.Precision).ToList();
            var recalls = foldsList.Select(f => f.Recall).ToList();
            var f1Scores = foldsList.Select(f => f.F1Score).ToList();
            
            // Calculate averages
            var avgAccuracy = accuracies.Average();
            var avgPrecision = precisions.Average();
            var avgRecall = recalls.Average();
            var avgF1Score = f1Scores.Average();
            
            // Calculate standard deviations
            var stdDevAccuracy = CalculateStandardDeviation(accuracies);
            var stdDevPrecision = CalculateStandardDeviation(precisions);
            var stdDevRecall = CalculateStandardDeviation(recalls);
            var stdDevF1Score = CalculateStandardDeviation(f1Scores);
            
            // Calculate summary statistics
            accuracies.Sort();
            precisions.Sort();
            recalls.Sort();
            f1Scores.Sort();
            
            var summary = new ValidationSummaryDto
            {
                Min = accuracies.Min(),
                Max = accuracies.Max(),
                Median = GetMedian(accuracies),
                Q1 = GetPercentile(accuracies, 0.25),
                Q3 = GetPercentile(accuracies, 0.75)
            };
            
            return new CrossValidationAggregatedDto
            {
                DatasetName = datasetName,
                TotalFolds = foldsList.Count,
                AverageAccuracy = avgAccuracy,
                AveragePrecision = avgPrecision,
                AverageRecall = avgRecall,
                AverageF1Score = avgF1Score,
                StdDevAccuracy = stdDevAccuracy,
                StdDevPrecision = stdDevPrecision,
                StdDevRecall = stdDevRecall,
                StdDevF1Score = stdDevF1Score,
                Folds = foldsList.Select(MapToDto).ToList(),
                Summary = summary
            };
        }
        
        public async Task<CrossValidationResultDto> CreateAsync(CrossValidationResultDto dto)
        {
            var result = new CrossValidationResult
            {
                DatasetName = dto.DatasetName,
                ValidationMethod = dto.ValidationMethod,
                FoldNumber = dto.FoldNumber,
                Accuracy = dto.Accuracy,
                Precision = dto.Precision,
                Recall = dto.Recall,
                F1Score = dto.F1Score,
                DomainAdaptationDrop = dto.DomainAdaptationDrop,
                SourceSite = dto.SourceSite,
                TargetSite = dto.TargetSite,
                ModelVersion = dto.ModelVersion,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow
            };
            
            await _repository.AddAsync(result);
            return MapToDto(result);
        }
        
        private CrossValidationResultDto MapToDto(CrossValidationResult result)
        {
            return new CrossValidationResultDto
            {
                Id = result.Id,
                DatasetName = result.DatasetName,
                ValidationMethod = result.ValidationMethod,
                FoldNumber = result.FoldNumber,
                Accuracy = result.Accuracy,
                Precision = result.Precision,
                Recall = result.Recall,
                F1Score = result.F1Score,
                DomainAdaptationDrop = result.DomainAdaptationDrop,
                SourceSite = result.SourceSite,
                TargetSite = result.TargetSite,
                ModelVersion = result.ModelVersion,
                Notes = result.Notes,
                CreatedAt = result.CreatedAt
            };
        }
        
        private static decimal CalculateStandardDeviation(List<decimal> values)
        {
            if (!values.Any()) return 0;
            
            var avg = values.Average();
            var sumOfSquares = values.Sum(x => (x - avg) * (x - avg));
            return (decimal)Math.Sqrt((double)(sumOfSquares / values.Count));
        }
        
        private static decimal GetMedian(List<decimal> values)
        {
            if (!values.Any()) return 0;
            
            var count = values.Count;
            if (count % 2 == 0)
            {
                return (values[count / 2 - 1] + values[count / 2]) / 2;
            }
            return values[count / 2];
        }
        
        private static decimal GetPercentile(List<decimal> values, double percentile)
        {
            if (!values.Any()) return 0;
            
            var index = (int)Math.Ceiling(values.Count * percentile) - 1;
            if (index < 0) index = 0;
            if (index >= values.Count) index = values.Count - 1;
            
            return values[index];
        }
    }
}

