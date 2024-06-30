using Common.RealTimeUnit;
using ScadaCore.Models;

namespace ScadaCore.Services;

public interface ITagLogService {
    Task<TagLog?> GetTagLogAsync(int id);
    
    Task<TagLog?> CreateTagLogAsync(TagLog tagLog);
    
    Task<TagLog?> CreateAnalogTagLogAsync(AnalogValueDto dto);
    
    Task<TagLog?> CreateDigitalTagLogAsync(DigitalValueDto dto);

    Task<TagLog?> GetLatestLog(String tagName);
}