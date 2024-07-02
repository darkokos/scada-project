using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;

namespace ScadaCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrendingController : ControllerBase
    {
        private static readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _subscribers = new ConcurrentDictionary<string, TaskCompletionSource<string>>();
        private readonly ILogger<TrendingController> _logger;

        public TrendingController(ILogger<TrendingController> logger)
        {
            _logger = logger;
        }

        [HttpGet("subscribe")]
        public async Task<IActionResult> Subscribe(CancellationToken cancellationToken)
        {
            var taskCompletionSource = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
            var id = Guid.NewGuid().ToString();
            _subscribers.TryAdd(id, taskCompletionSource);

            cancellationToken.Register(() => {
                _subscribers.TryRemove(id, out var task);
                task?.TrySetCanceled();
                _logger.LogInformation($"Subscription canceled: {id}");
            });

            try
            {
                var result = await taskCompletionSource.Task;
                _subscribers.TryRemove(id, out _);
                return Ok(result);
            }
            catch (TaskCanceledException)
            {
                _logger.LogWarning("Subscription was canceled.");
                return StatusCode(499); 
            }
        }

        [HttpPost("notify")]
        public IActionResult Notify([FromBody] string message)
        {
            foreach (var subscriber in _subscribers.Values)
            {
                if (!subscriber.Task.IsCompleted)
                {
                    subscriber.TrySetResult(message);
                }
            }

            _logger.LogInformation("All subscribers have been notified.");
            return Ok();
        }
    }
}