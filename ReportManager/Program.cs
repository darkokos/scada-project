using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Common.ReportManagerCommon;

namespace ReportManager;

class Program
{
    private static readonly HttpClient _httpClient = new HttpClient();

    static async Task Main(string[] args)
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("Report Manager");
            Console.WriteLine("1. Alarms in a specific time period");
            Console.WriteLine("2. Alarms of a specific priority");
            Console.WriteLine("3. Tag values in a specific time period");
            Console.WriteLine("4. Last values of all AI tags");
            Console.WriteLine("5. Last values of all DI tags");
            Console.WriteLine("6. All values of a specific tag");
            Console.WriteLine("0. Exit");
            Console.Write("Choose an option: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await GetAlarmsInTimePeriod();
                    break;
                case "2":
                    await GetAlarmsOfSpecificPriority();
                    break;
                case "3":
                    await GetTagValuesInTimePeriod();
                    break;
                case "4":
                    await GetLastValuesAllAITags();
                    break;
                case "5":
                    await GetLastValuesAllDITags();
                    break;
                case "6":
                    await GetAllValuesForSpecificTag();
                    break;
                case "0":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }
    }
    
    public static string ServerUrl = "http://localhost:59767/ReportManager";

    private static async Task GetAlarmsInTimePeriod()
    {
        Console.Write("Enter start time (yyyy-mm-dd): ");
        var startTime = Console.ReadLine();
        Console.Write("Enter end time (yyyy-mm-dd): ");
        var endTime = Console.ReadLine();
        
        var response = await _httpClient.GetAsync(
            ServerUrl + $"/alarms-in-specific-time-period?startTime={startTime}&endTime={endTime}"
            );
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            var alarms = JsonSerializer.Deserialize<IEnumerable<AlarmLog>>(data);
            Console.WriteLine("Time      | Priority | Type | Unit");
            foreach (var alarm in alarms)
            {
                Console.WriteLine($"{alarm.Timestamp, 10}|{alarm.Priority, 10}|{alarm.Type, 6}|{alarm.Unit, 5}");
            }
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("Error fetching alarms.");
        }
    }

    private static async Task GetAlarmsOfSpecificPriority()
    {
        Console.Write("Enter priority: ");
        var priority = Console.ReadLine();

        var response = await _httpClient.GetAsync(ServerUrl + $"/alarms-of-specific-priority?priority={priority}");
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            var alarms = JsonSerializer.Deserialize<IEnumerable<AlarmLog>>(data);
            Console.WriteLine("Time      | Priority | Type | Unit");
            foreach (var alarm in alarms)
            {
                Console.WriteLine($"{alarm.Timestamp, 10}|{alarm.Priority, 10}|{alarm.Type, 6}|{alarm.Unit, 5}");
            }
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("Error fetching alarms.");
        }
    }

    private static async Task GetTagValuesInTimePeriod()
    {
        Console.Write("Enter start time (yyyy-mm-dd): ");
        var startTime = Console.ReadLine();
        Console.Write("Enter end time (yyyy-mm-dd): ");
        var endTime = Console.ReadLine();

        var response = await _httpClient.GetAsync(ServerUrl + $"/tag-values-in-specific-time-period?startTime={startTime}&endTime={endTime}");
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            var tagLogs = JsonSerializer.Deserialize<IEnumerable<TagLog>>(data);
            Console.WriteLine("\nTag Type  |  Log ID  |  Timestamp  |  Value");
            foreach (var log in tagLogs)
            {
                switch (log)
                {
                    case AnalogTagLog analogTagLog:
                        Console.WriteLine($"Analog    |{analogTagLog.Id, 10}|{analogTagLog.Timestamp, 13}{analogTagLog.EmittedValue, 10}");
                        break;
                    case DigitalTagLog digitalTagLog:
                        Console.WriteLine($"Digital   |{digitalTagLog.Id, 10}|{digitalTagLog.Timestamp, 13}{digitalTagLog.EmittedValue, 10}");
                        break;
                    default:
                        Console.WriteLine("Unknown type");
                        break;
                }
            }
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("Error fetching tag values.");
        }
    }
    
    // NOTE: Im not sure what the log id is tbh
    private static async Task GetAllValuesForSpecificTag()
    {
        Console.Write("Enter tag Name: ");
        var tagName = Console.ReadLine();

        var response = await _httpClient.GetAsync(ServerUrl + $"/all-values-for-specific-tag?tagName={tagName}");
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            var tagLogs = JsonSerializer.Deserialize<IEnumerable<TagLog>>(data);
            Console.WriteLine("\nTag Type  |  Log ID  |  Timestamp  |  Value");
            foreach (var log in tagLogs)
            {
                switch (log)
                {
                    case AnalogTagLog analogTagLog:
                        Console.WriteLine($"Analog    |{analogTagLog.Id, 10}|{analogTagLog.Timestamp, 13}{analogTagLog.EmittedValue, 10}");
                        break;
                    case DigitalTagLog digitalTagLog:
                        Console.WriteLine($"Digital   |{digitalTagLog.Id, 10}|{digitalTagLog.Timestamp, 13}{digitalTagLog.EmittedValue, 10}");
                        break;
                    default:
                        Console.WriteLine("Unknown type");
                        break;
                }
            }
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("Error fetching tag values.");
        }
    }

    private static async Task GetLastValuesAllAITags()
    {
        var response = await _httpClient.GetAsync(ServerUrl + "/last-values-all-ai-tags");
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            var aiLogs = JsonSerializer.Deserialize<IEnumerable<TagLog>>(data);
            Console.WriteLine("\nTag Type  |  Log ID  |  Timestamp  |  Value");
            foreach (var log in aiLogs)
            {
                switch (log)
                {
                    case AnalogTagLog analogTagLog:
                        Console.WriteLine($"Analog    |{analogTagLog.Id, 10}|{analogTagLog.Timestamp, 13}{analogTagLog.EmittedValue, 10}");
                        break;
                    default:
                        Console.WriteLine("Unknown type");
                        break;
                }
            }
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("Error fetching AI tag values.");
        }
    }

    private static async Task GetLastValuesAllDITags()
    {
        var response = await _httpClient.GetAsync(ServerUrl + "/last-values-all-di-tags");
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            var diLogs = JsonSerializer.Deserialize<IEnumerable<TagLog>>(data);
            Console.WriteLine("\nTag Type  |  Log ID  |  Timestamp  |  Value");
            foreach (var log in diLogs)
            {
                switch (log)
                {
                    case AnalogTagLog analogTagLog:
                        Console.WriteLine($"Digital   |{analogTagLog.Id, 10}|{analogTagLog.Timestamp, 13}{analogTagLog.EmittedValue, 10}");
                        break;
                    default:
                        Console.WriteLine("Unknown type");
                        break;
                }
            }
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("Error fetching DI tag values.");
        }
    }
}