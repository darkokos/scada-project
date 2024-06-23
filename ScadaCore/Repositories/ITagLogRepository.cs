using ScadaCore.Models;

namespace ScadaCore.Repositories;

public interface ITagLogRepository {
    Task<TagLog?> GetTagLogAsync(int id);
    
    Task<TagLog?> CreateTagLogAsync(TagLog tagLog);
}