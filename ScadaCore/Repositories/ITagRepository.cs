using System.Collections.ObjectModel;
using ScadaCore.Models;

namespace ScadaCore.Repositories;

public interface ITagRepository {
    Task<Tag?> GetTagAsync(string name);

    Task<ICollection<Tag>> GetAllInputTags();
    
    Task<Tag?> CreateTagAsync(Tag tag);

    Task<bool> DeleteTagAsync(Tag tag);
    Task<Collection<Tag>> GetAllOutputTags();
}
