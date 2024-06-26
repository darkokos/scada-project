namespace AlarmDisplay.Service;

public class AlarmService
{
    private readonly HttpClient _httpClient;

    public AlarmService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task SubscribeToAlarmsAsync()
    {
        while (true)
        {
            try
            {
                var response = await _httpClient.GetAsync("Alarm/subscribe");
                if (response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    DisplayAlarm(message);
                }
                else
                {
                    Console.WriteLine("Failed to subscribe, retrying...");
                    await Task.Delay(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                await Task.Delay(1000);
            }
        }
    }

    private void DisplayAlarm(string message)
    {
        var parts = message.Split(':');
        if (parts.Length == 2 && int.TryParse(parts[0], out int priority))
        {
            for (int i = 0; i < priority; i++)
            {
                Console.WriteLine($"Alarm (Priority {priority}): {parts[1]}");
            }
        }
        else
        {
            Console.WriteLine($"Invalid alarm message: {message}");
        }
    }
}