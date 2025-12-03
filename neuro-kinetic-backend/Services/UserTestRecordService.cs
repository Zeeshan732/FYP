using Microsoft.EntityFrameworkCore;
using neuro_kinetic_backend.Data;
using neuro_kinetic_backend.DTOs.Common;
using neuro_kinetic_backend.DTOs.TestRecord;
using neuro_kinetic_backend.Models;
using neuro_kinetic_backend.Repositories;

namespace neuro_kinetic_backend.Services
{
    public class UserTestRecordService : IUserTestRecordService
    {
        private readonly IRepository<UserTestRecord> _repository;
        private readonly ApplicationDbContext _context;
        
        public UserTestRecordService(IRepository<UserTestRecord> repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }
        
        public async Task<PagedResult<UserTestRecordDto>> GetTestRecordsAsync(GetTestRecordsQuery query)
        {
            var dbQuery = _context.UserTestRecords.AsQueryable();
            
            // Apply filters
            if (query.UserId.HasValue)
            {
                dbQuery = dbQuery.Where(t => t.UserId == query.UserId.Value);
            }
            
            if (!string.IsNullOrEmpty(query.Status))
            {
                dbQuery = dbQuery.Where(t => t.Status == query.Status);
            }
            
            if (!string.IsNullOrEmpty(query.TestResult))
            {
                dbQuery = dbQuery.Where(t => t.TestResult == query.TestResult);
            }
            
            // Apply sorting
            dbQuery = query.SortBy.ToLower() switch
            {
                "testdate" => query.SortOrder == "desc"
                    ? dbQuery.OrderByDescending(t => t.TestDate)
                    : dbQuery.OrderBy(t => t.TestDate),
                "accuracy" => query.SortOrder == "desc"
                    ? dbQuery.OrderByDescending(t => t.Accuracy)
                    : dbQuery.OrderBy(t => t.Accuracy),
                "testresult" => query.SortOrder == "desc"
                    ? dbQuery.OrderByDescending(t => t.TestResult)
                    : dbQuery.OrderBy(t => t.TestResult),
                "status" => query.SortOrder == "desc"
                    ? dbQuery.OrderByDescending(t => t.Status)
                    : dbQuery.OrderBy(t => t.Status),
                "username" => query.SortOrder == "desc"
                    ? dbQuery.OrderByDescending(t => t.UserName)
                    : dbQuery.OrderBy(t => t.UserName),
                _ => dbQuery.OrderByDescending(t => t.TestDate)
            };
            
            var totalCount = await dbQuery.CountAsync();
            var items = await dbQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();
            
            return new PagedResult<UserTestRecordDto>
            {
                Items = items.Select(MapToDto),
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }
        
        public async Task<IEnumerable<UserTestRecordDto>> GetAllTestRecordsAsync()
        {
            var records = await _repository.GetAllAsync();
            return records.OrderByDescending(r => r.TestDate).Select(MapToDto);
        }
        
        public async Task<UserTestRecordDto?> GetTestRecordByIdAsync(int id)
        {
            var record = await _repository.GetByIdAsync(id);
            return record != null ? MapToDto(record) : null;
        }
        
        public async Task<IEnumerable<UserTestRecordDto>> GetTestRecordsByUserIdAsync(int userId)
        {
            var records = await _repository.FindAsync(r => r.UserId == userId);
            return records.OrderByDescending(r => r.TestDate).Select(MapToDto);
        }
        
        public async Task<UserTestRecordDto> CreateTestRecordAsync(CreateUserTestRecordDto dto)
        {
            var record = new UserTestRecord
            {
                UserId = dto.UserId ?? 0,
                UserName = dto.UserName,
                TestDate = DateTime.UtcNow,
                TestResult = dto.TestResult ?? "Uncertain",
                Accuracy = dto.Accuracy ?? 0.0,
                Status = dto.Status ?? "Pending",
                VoiceRecordingUrl = dto.VoiceRecordingUrl,
                AnalysisNotes = dto.AnalysisNotes,
                CreatedAt = DateTime.UtcNow
            };
            
            await _repository.AddAsync(record);
            return MapToDto(record);
        }
        
        public async Task<UserTestRecordDto> UpdateTestRecordAsync(int id, UpdateUserTestRecordDto dto)
        {
            var record = await _repository.GetByIdAsync(id);
            if (record == null)
                throw new KeyNotFoundException("Test record not found");
            
            if (!string.IsNullOrEmpty(dto.TestResult))
                record.TestResult = dto.TestResult;
            
            if (dto.Accuracy.HasValue)
                record.Accuracy = dto.Accuracy.Value;
            
            if (!string.IsNullOrEmpty(dto.Status))
                record.Status = dto.Status;
            
            if (dto.VoiceRecordingUrl != null)
                record.VoiceRecordingUrl = dto.VoiceRecordingUrl;
            
            if (dto.AnalysisNotes != null)
                record.AnalysisNotes = dto.AnalysisNotes;
            
            await _repository.UpdateAsync(record);
            return MapToDto(record);
        }
        
        public async Task DeleteTestRecordAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
        
        private UserTestRecordDto MapToDto(UserTestRecord record)
        {
            return new UserTestRecordDto
            {
                Id = record.Id,
                UserId = record.UserId,
                UserName = record.UserName,
                TestDate = record.TestDate,
                TestResult = record.TestResult,
                Accuracy = record.Accuracy,
                Status = record.Status,
                VoiceRecordingUrl = record.VoiceRecordingUrl,
                AnalysisNotes = record.AnalysisNotes,
                CreatedAt = record.CreatedAt
            };
        }
    }
}



