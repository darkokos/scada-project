using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ScadaCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScadaCoreController : ControllerBase
    {
        private readonly ILogger<ScadaCoreController> _logger;
        private readonly HttpClient _httpClient;

        public ScadaCoreController(ILogger<ScadaCoreController> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
        }

        [HttpPost("updateTrending")]
        public async Task<IActionResult> UpdateTrending([FromBody] string message)
        {
            var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5040/Trending/notify", content);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Trending data sent successfully.");
                return Ok();
            }
            _logger.LogError($"Failed to send trending data: {response.StatusCode}");
            return StatusCode((int)response.StatusCode);
        }

        [HttpPost("updateAlarm")]
        public async Task<IActionResult> UpdateAlarm([FromBody] string message)
        {
            var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5040/Alarm/notify", content);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Alarm data sent successfully.");
                return Ok();
            }
            _logger.LogError($"Failed to send alarm data: {response.StatusCode}");
            return StatusCode((int)response.StatusCode);
        }
    }
}