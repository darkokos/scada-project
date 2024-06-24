using ScadaCore.Models;

namespace ScadaCore.Services;

public class TagService : ITagService {
    public async Task<Tag?> GetTagAsync(string name) {
        return null;
    }

    public async Task<Tag?> CreateTagAsync(Tag tag) {
        return null;
    }

    public async Task<bool> DeleteTagAsync(Tag tag) {
        return default;
    }
}