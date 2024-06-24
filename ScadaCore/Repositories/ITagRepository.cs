using ScadaCore.Models;

namespace ScadaCore.Repositories;

public interface ITagRepository {
    Task<Tag?> GetTagAsync(string name);
    
    Task<Tag?> CreateTagAsync(Tag tag);

    Task<bool> DeleteTagAsync(Tag tag);
}