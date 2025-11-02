using neuro_kinetic_backend.DTOs.Common;
using neuro_kinetic_backend.DTOs.TestRecord;

namespace neuro_kinetic_backend.Services
{
    public interface IUserTestRecordService
    {
        Task<PagedResult<UserTestRecordDto>> GetTestRecordsAsync(GetTestRecordsQuery query);
        Task<IEnumerable<UserTestRecordDto>> GetAllTestRecordsAsync();
        Task<UserTestRecordDto?> GetTestRecordByIdAsync(int id);
        Task<IEnumerable<UserTestRecordDto>> GetTestRecordsByUserIdAsync(int userId);
        Task<UserTestRecordDto> CreateTestRecordAsync(CreateUserTestRecordDto dto);
        Task<UserTestRecordDto> UpdateTestRecordAsync(int id, UpdateUserTestRecordDto dto);
        Task DeleteTestRecordAsync(int id);
    }
}

