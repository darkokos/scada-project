using ScadaCore.Models;

namespace ScadaCore.Services;

public interface ITagLogService {
    Task<TagLog?> GetTagLogAsync(int id);
    
    Task<TagLog?> CreateTagLogAsync(TagLog tagLog);

    Task<TagLog?> GetLatestLog(String tagName);
}