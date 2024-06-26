using ScadaCore.Models;

namespace ScadaCore.Services;

public interface ITagService {
    Task<Tag?> GetTagAsync(string name);
    
    Task<Tag?> CreateTagAsync(Tag tag);

    Task<bool> DeleteTagAsync(Tag tag);
}