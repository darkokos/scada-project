using System;
using AlarmDisplay.Service;

namespace AlarmDisplay
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5038/") };
            var alarmService = new AlarmService(httpClient);

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                Console.WriteLine("Stopping Alarm Subscription Client...");
            };

            Console.WriteLine("Starting Alarm Subscription Client...");
            await alarmService.SubscribeToAlarmsAsync();
        }
    }
}