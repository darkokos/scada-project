using AutoMapper;
using Common.RealTimeUnit;
using ScadaCore.Models;
using ScadaCore.Repositories;

namespace ScadaCore.Services;

public class TagLogService(ITagLogRepository tagLogRepository, IMapper mapper) : ITagLogService {
    public async Task<TagLog?> GetTagLogAsync(int id) {
        return null;
    }

    public async Task<TagLog?> CreateTagLogAsync(TagLog tagLog) {
        return await tagLogRepository.CreateTagLogAsync(tagLog);
    }
    
    public async Task<TagLog?> CreateAnalogTagLogAsync(AnalogValueDto dto) {
        return await tagLogRepository.CreateTagLogAsync(mapper.Map<AnalogTagLog>(dto));
    }

    public async Task<TagLog?> CreateDigitalTagLogAsync(DigitalValueDto dto) {
        return await tagLogRepository.CreateTagLogAsync(mapper.Map<DigitalTagLog>(dto));
    }

    public async Task<TagLog?> GetLatestLog(String tagName)
    {
        var task = tagLogRepository.GetLatestLog(tagName);
        task.Wait();
        return task.Result;
    }
}