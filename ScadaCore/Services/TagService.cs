using ScadaCore.Models;
using ScadaCore.Repositories;

namespace ScadaCore.Services;

public class TagService : ITagService
{
    public ITagRepository TagRepository;

    public TagService(ITagRepository tagRepository)
    {
        this.TagRepository = tagRepository;
    }
    
    public async Task<Tag?> GetTagAsync(string name)
    {
        var task = this.TagRepository.GetTagAsync(name);
        task.Wait();
        return task.Result;
    }

    public async Task<Tag?> CreateTagAsync(Tag tag) {
        return null;
    }

    public async Task<bool> DeleteTagAsync(Tag tag)
    {
        var task = this.TagRepository.DeleteTagAsync(tag);
        task.Wait();
        return task.Result;
    }
}