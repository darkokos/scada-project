using Trending.Services;

namespace Trending
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:59767/") };
            var trendingService = new TrendingService(httpClient);

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                Console.WriteLine("Stopping Trending Subscription Client...");
            };

            Console.WriteLine("Starting Trending Subscription Client...");
            await trendingService.SubscribeToTrendingAsync();
        }
    }
}