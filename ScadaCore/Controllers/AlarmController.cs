namespace ScadaCore.Controllers;
    
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("[controller]")]
public class AlarmController : ControllerBase
{
    private static readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _subscribers = new ConcurrentDictionary<string, TaskCompletionSource<string>>();
    private readonly ILogger<AlarmController> _logger;

    public AlarmController(ILogger<AlarmController> logger)
    {
        _logger = logger;
    }

    [HttpGet("subscribe")]
    public async Task<IActionResult> Subscribe(CancellationToken cancellationToken)
    {
        var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
        var id = Guid.NewGuid().ToString();
        _subscribers.TryAdd(id, tcs);

        cancellationToken.Register(() => {
            _subscribers.TryRemove(id, out var task);
            task.TrySetCanceled();
            _logger.LogInformation($"Subscription canceled for ID: {id}");
        });

        try
        {
            var result = await tcs.Task;
            _subscribers.TryRemove(id, out _); 
            return Ok(result);
        }
        catch (TaskCanceledException)
        {
            _logger.LogInformation("Subscription was canceled by the client.");
            return StatusCode(499); 
        }
    }

    [HttpPost("notify")]
    public IActionResult Notify([FromBody] string message)
    {
        int notifyCount = 0;
        foreach (var subscriber in _subscribers.Values)
        {
            if (!subscriber.Task.IsCompleted)
            {
                subscriber.TrySetResult(message);
                notifyCount++;
            }
        }

        _logger.LogInformation($"Notified {notifyCount} subscribers.");
        return Ok();
    }
}