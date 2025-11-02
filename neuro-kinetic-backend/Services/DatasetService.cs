using Microsoft.EntityFrameworkCore;
using neuro_kinetic_backend.Data;
using neuro_kinetic_backend.DTOs.Common;
using neuro_kinetic_backend.DTOs.Dataset;
using neuro_kinetic_backend.Models;
using neuro_kinetic_backend.Repositories;

namespace neuro_kinetic_backend.Services
{
    public class DatasetService : IDatasetService
    {
        private readonly IRepository<Dataset> _repository;
        private readonly ApplicationDbContext _context;
        
        public DatasetService(IRepository<Dataset> repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }
        
        public async Task<IEnumerable<DatasetDto>> GetAllAsync()
        {
            var datasets = await _repository.GetAllAsync();
            return datasets.Select(MapToDto);
        }
        
        public async Task<PagedResult<DatasetDto>> GetPagedAsync(QueryParameters parameters)
        {
            var query = _context.Datasets.AsQueryable();
            
            // Apply sorting
            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                query = parameters.SortBy.ToLower() switch
                {
                    "name" => parameters.SortOrder == "desc" 
                        ? query.OrderByDescending(d => d.Name) 
                        : query.OrderBy(d => d.Name),
                    "source" => parameters.SortOrder == "desc" 
                        ? query.OrderByDescending(d => d.Source) 
                        : query.OrderBy(d => d.Source),
                    "createdat" => parameters.SortOrder == "desc" 
                        ? query.OrderByDescending(d => d.CreatedAt) 
                        : query.OrderBy(d => d.CreatedAt),
                    _ => query.OrderByDescending(d => d.CreatedAt)
                };
            }
            else
            {
                query = query.OrderByDescending(d => d.CreatedAt);
            }
            
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
            
            return new PagedResult<DatasetDto>
            {
                Items = items.Select(MapToDto),
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }
        
        public async Task<IEnumerable<DatasetDto>> GetPublicAsync()
        {
            var datasets = await _repository.FindAsync(d => d.IsPublic);
            return datasets.OrderByDescending(d => d.CreatedAt).Select(MapToDto);
        }
        
        public async Task<PagedResult<DatasetDto>> GetPublicPagedAsync(QueryParameters parameters)
        {
            var query = _context.Datasets.Where(d => d.IsPublic);
            
            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                query = parameters.SortBy.ToLower() switch
                {
                    "name" => parameters.SortOrder == "desc" 
                        ? query.OrderByDescending(d => d.Name) 
                        : query.OrderBy(d => d.Name),
                    "source" => parameters.SortOrder == "desc" 
                        ? query.OrderByDescending(d => d.Source) 
                        : query.OrderBy(d => d.Source),
                    "createdat" => parameters.SortOrder == "desc" 
                        ? query.OrderByDescending(d => d.CreatedAt) 
                        : query.OrderBy(d => d.CreatedAt),
                    _ => query.OrderByDescending(d => d.CreatedAt)
                };
            }
            else
            {
                query = query.OrderByDescending(d => d.CreatedAt);
            }
            
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
            
            return new PagedResult<DatasetDto>
            {
                Items = items.Select(MapToDto),
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }
        
        public async Task<DatasetDto?> GetByIdAsync(int id)
        {
            var dataset = await _repository.GetByIdAsync(id);
            return dataset != null ? MapToDto(dataset) : null;
        }
        
        public async Task<DatasetDto> CreateAsync(DatasetDto dto)
        {
            var dataset = new Dataset
            {
                Name = dto.Name,
                Source = dto.Source,
                Version = dto.Version,
                TotalSamples = dto.TotalSamples,
                VoiceSamples = dto.VoiceSamples,
                GaitSamples = dto.GaitSamples,
                MultiModalSamples = dto.MultiModalSamples,
                Description = dto.Description,
                License = dto.License,
                AccessLink = dto.AccessLink,
                IsPublic = dto.IsPublic,
                Citation = dto.Citation,
                CreatedAt = DateTime.UtcNow
            };
            
            await _repository.AddAsync(dataset);
            return MapToDto(dataset);
        }
        
        public async Task<DatasetDto> UpdateAsync(int id, DatasetDto dto)
        {
            var dataset = await _repository.GetByIdAsync(id);
            if (dataset == null)
                throw new KeyNotFoundException("Dataset not found");
            
            dataset.Name = dto.Name;
            dataset.Source = dto.Source;
            dataset.Version = dto.Version;
            dataset.TotalSamples = dto.TotalSamples;
            dataset.VoiceSamples = dto.VoiceSamples;
            dataset.GaitSamples = dto.GaitSamples;
            dataset.MultiModalSamples = dto.MultiModalSamples;
            dataset.Description = dto.Description;
            dataset.License = dto.License;
            dataset.AccessLink = dto.AccessLink;
            dataset.IsPublic = dto.IsPublic;
            dataset.Citation = dto.Citation;
            
            await _repository.UpdateAsync(dataset);
            return MapToDto(dataset);
        }
        
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
        
        private DatasetDto MapToDto(Dataset dataset)
        {
            return new DatasetDto
            {
                Id = dataset.Id,
                Name = dataset.Name,
                Source = dataset.Source,
                Version = dataset.Version,
                TotalSamples = dataset.TotalSamples,
                VoiceSamples = dataset.VoiceSamples,
                GaitSamples = dataset.GaitSamples,
                MultiModalSamples = dataset.MultiModalSamples,
                Description = dataset.Description,
                License = dataset.License,
                AccessLink = dataset.AccessLink,
                IsPublic = dataset.IsPublic,
                Citation = dataset.Citation,
                CreatedAt = dataset.CreatedAt
            };
        }
    }
}

