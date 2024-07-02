using System.Text;
using Newtonsoft.Json;

namespace ScadaCore.Services;

public class NotificationService : INotificationService
{
    public async Task SendTrendingNotification(string message) {
        using var httpClient = new HttpClient();
        var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("http://localhost:59767/Trending/notify", content);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Trending notification sent successfully.");
        }
        else
        {
            Console.WriteLine($"Failed to send trending notification: {response.StatusCode}");
        }
    }

    public async Task SendAlarmNotification(string message) {
        using var httpClient = new HttpClient();
        var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("http://localhost:59767/Alarm/notify", content);
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