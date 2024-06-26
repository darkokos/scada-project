namespace Trending.Services;

public class TrendingService
{
    private readonly HttpClient _httpClient;

    public TrendingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task SubscribeToTrendingAsync()
    {
        while (true)
        {
            try
            {
                var response = await _httpClient.GetAsync("Trending/subscribe");
                if (response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Trending Update: {message}");
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
}