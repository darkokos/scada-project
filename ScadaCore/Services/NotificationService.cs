using System.Text;
using Newtonsoft.Json;

namespace ScadaCore.Services;

public class NotificationService
{
    private readonly HttpClient _httpClient;

    public NotificationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task SendTrendingNotification(string message)
    {
        var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("http://localhost:5038/Trending/notify", content);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Trending notification sent successfully.");
        }
        else
        {
            Console.WriteLine($"Failed to send trending notification: {response.StatusCode}");
        }
    }

    public async Task SendAlarmNotification(string message)
    {
        var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("http://localhost:5038/Alarm/notify", content);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Alarm notification sent successfully.");
        }
        else
        {
            Console.WriteLine($"Failed to send alarm notification: {response.StatusCode}");
        }
    }
}