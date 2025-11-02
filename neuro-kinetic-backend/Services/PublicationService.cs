using Microsoft.EntityFrameworkCore;
using neuro_kinetic_backend.Data;
using neuro_kinetic_backend.DTOs.Common;
using neuro_kinetic_backend.DTOs.Publication;
using neuro_kinetic_backend.Models;
using neuro_kinetic_backend.Repositories;

namespace neuro_kinetic_backend.Services
{
    public class PublicationService : IPublicationService
    {
        private readonly IPublicationRepository _repository;
        private readonly ApplicationDbContext _context;
        
        public PublicationService(IPublicationRepository repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }
        
        public async Task<IEnumerable<PublicationDto>> GetAllAsync()
        {
            var publications = await _repository.GetAllAsync();
            return publications.Select(MapToDto);
        }
        
        public async Task<PagedResult<PublicationDto>> GetPagedAsync(QueryParameters parameters)
        {
            var query = _context.Publications.AsQueryable();
            
            // Apply sorting
            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                query = parameters.SortBy.ToLower() switch
                {
                    "title" => parameters.SortOrder == "desc" 
                        ? query.OrderByDescending(p => p.Title) 
                        : query.OrderBy(p => p.Title),
                    "createdat" => parameters.SortOrder == "desc" 
                        ? query.OrderByDescending(p => p.CreatedAt) 
                        : query.OrderBy(p => p.CreatedAt),
                    "year" => parameters.SortOrder == "desc" 
                        ? query.OrderByDescending(p => p.Year) 
                        : query.OrderBy(p => p.Year),
                    _ => query.OrderByDescending(p => p.CreatedAt)
                };
            }
            else
            {
                query = query.OrderByDescending(p => p.CreatedAt);
            }
            
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
            
            return new PagedResult<PublicationDto>
            {
                Items = items.Select(MapToDto),
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }
        
        public async Task<IEnumerable<PublicationDto>> GetFeaturedAsync()
        {
            var publications = await _repository.GetFeaturedAsync();
            return publications.Select(MapToDto);
        }
        
        public async Task<PagedResult<PublicationDto>> GetFeaturedPagedAsync(QueryParameters parameters)
        {
            var query = _context.Publications.Where(p => p.IsFeatured);
            
            if (!string.IsNullOrEmpty(parameters.SortBy))
            {
                query = parameters.SortBy.ToLower() switch
                {
                    "title" => parameters.SortOrder == "desc" 
                        ? query.OrderByDescending(p => p.Title) 
                        : query.OrderBy(p => p.Title),
                    "createdat" => parameters.SortOrder == "desc" 
                        ? query.OrderByDescending(p => p.CreatedAt) 
                        : query.OrderBy(p => p.CreatedAt),
                    _ => query.OrderByDescending(p => p.CreatedAt)
                };
            }
            else
            {
                query = query.OrderByDescending(p => p.CreatedAt);
            }
            
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
            
            return new PagedResult<PublicationDto>
            {
                Items = items.Select(MapToDto),
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }
        
        public async Task<PublicationDto?> GetByIdAsync(int id)
        {
            var publication = await _repository.GetByIdAsync(id);
            return publication != null ? MapToDto(publication) : null;
        }
        
        public async Task<IEnumerable<PublicationDto>> SearchAsync(string searchTerm)
        {
            var publications = await _repository.SearchAsync(searchTerm);
            return publications.Select(MapToDto);
        }
        
        public async Task<PagedResult<PublicationDto>> SearchPagedAsync(string searchTerm, QueryParameters parameters)
        {
            var publications = await _repository.SearchAsync(searchTerm);
            var totalCount = publications.Count();
            
            var sorted = parameters.SortBy?.ToLower() switch
            {
                "title" => parameters.SortOrder == "desc" 
                    ? publications.OrderByDescending(p => p.Title) 
                    : publications.OrderBy(p => p.Title),
                "createdat" => parameters.SortOrder == "desc" 
                    ? publications.OrderByDescending(p => p.CreatedAt) 
                    : publications.OrderBy(p => p.CreatedAt),
                _ => publications.OrderByDescending(p => p.CreatedAt)
            };
            
            var pagedItems = sorted
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToList();
            
            return new PagedResult<PublicationDto>
            {
                Items = pagedItems.Select(MapToDto),
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }
        
        public async Task<PublicationDto> CreateAsync(PublicationDto dto)
        {
            var publication = new Publication
            {
                Title = dto.Title,
                Abstract = dto.Abstract,
                Authors = dto.Authors,
                Journal = dto.Journal,
                Year = dto.Year,
                DOI = dto.DOI,
                Link = dto.Link,
                Type = Enum.Parse<PublicationType>(dto.Type),
                IsFeatured = dto.IsFeatured,
                Tags = dto.Tags,
                CreatedAt = DateTime.UtcNow
            };
            
            await _repository.AddAsync(publication);
            return MapToDto(publication);
        }
        
        public async Task<PublicationDto> UpdateAsync(int id, PublicationDto dto)
        {
            var publication = await _repository.GetByIdAsync(id);
            if (publication == null)
                throw new KeyNotFoundException("Publication not found");
            
            publication.Title = dto.Title;
            publication.Abstract = dto.Abstract;
            publication.Authors = dto.Authors;
            publication.Journal = dto.Journal;
            publication.Year = dto.Year;
            publication.DOI = dto.DOI;
            publication.Link = dto.Link;
            publication.Type = Enum.Parse<PublicationType>(dto.Type);
            publication.IsFeatured = dto.IsFeatured;
            publication.Tags = dto.Tags;
            publication.UpdatedAt = DateTime.UtcNow;
            
            await _repository.UpdateAsync(publication);
            return MapToDto(publication);
        }
        
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
        
        private PublicationDto MapToDto(Publication publication)
        {
            return new PublicationDto
            {
                Id = publication.Id,
                Title = publication.Title,
                Abstract = publication.Abstract,
                Authors = publication.Authors,
                Journal = publication.Journal,
                Year = publication.Year,
                DOI = publication.DOI,
                Link = publication.Link,
                Type = publication.Type.ToString(),
                IsFeatured = publication.IsFeatured,
                Tags = publication.Tags,
                CreatedAt = publication.CreatedAt
            };
        }
    }
}

