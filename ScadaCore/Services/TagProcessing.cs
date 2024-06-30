namespace ScadaCore.Services;

public class TagProcessing(ITagService tagService) : BackgroundService {
    protected override async Task ExecuteAsync(CancellationToken cancellationToken) {
    }
}