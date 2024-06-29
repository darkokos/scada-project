using ScadaCore.Models;
using ScadaCore.Repositories;

namespace ScadaCore.Services;

public class TagLogService : ITagLogService
{
    private ITagLogRepository TagLogRepository;
    public TagLogService(ITagLogRepository tagLogRepository)
    {
        this.TagLogRepository = tagLogRepository;
    }

    public async Task<TagLog?> GetTagLogAsync(int id) {
        return null;
    }

    public async Task<TagLog?> CreateTagLogAsync(TagLog tagLog) {
        return null;
    }


    public async Task<TagLog?> GetLatestLog(String tagName)
    {
        var task = TagLogRepository.GetLatestLog(tagName);
        task.Wait();
        return task.Result;
    }
}