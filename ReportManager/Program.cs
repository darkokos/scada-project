using System.Globalization;
using Common.ReportManagerCommon;
using Newtonsoft.Json;

namespace ReportManager;

internal abstract class Program
{
    private static readonly HttpClient HttpClient = new();

    private static async Task Main()
    {
        var running = true;
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
                    await GetLastValuesAllAiTags();
                    break;
                case "5":
                    await GetLastValuesAllDiTags();
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
    
    private const string ServerUrl = "http://localhost:59767/ReportManager";

    private static async Task GetAlarmsInTimePeriod()
    {
        DateTime startTime;
        while (true) {
            Console.Write("Enter start time (dd/MM/yyyy HH:mm:ss): ");
            var input = Console.ReadLine();
            if (input == null)
                continue;
            
            if (
                DateTime.TryParseExact(
                    input,
                    "dd/MM/yyyy HH:mm:ss",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out startTime
                )
            )
                break;
            
            Console.WriteLine("Wrong input format.");
        }
        
        DateTime endTime;
        while (true) {
            Console.Write("Enter end time (dd/MM/yyyy HH:mm:ss): ");
            var input = Console.ReadLine();
            if (input == null)
                continue;

            if (
                DateTime.TryParseExact(
                    input,
                    "dd/MM/yyyy HH:mm:ss",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out endTime
                )
            )
                break;

            Console.WriteLine("Wrong input format.");
        }

        var response = await HttpClient.GetAsync(
            ServerUrl + $"/alarms-in-specific-time-period?startTime={startTime.ToString("MM/dd/yyyy HH:mm:ss")}&endTime={endTime.ToString("MM/dd/yyyy HH:mm:ss")}"
            );
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            var alarms = JsonConvert.DeserializeObject<List<AlarmLogDto>>(data);
            if (alarms == null) {
                Console.WriteLine("Error fetching all requested alarms.");
                return;
            }
            
            Console.WriteLine("Time               | Priority | Type | Unit");
            foreach (var alarm in alarms)
            {
                Console.WriteLine($"{alarm.Timestamp.ToString("dd/MM/yyyy HH:mm:ss"), 19}|{alarm.Priority, 10}|{alarm.Type, 6}|{alarm.Unit, 5}");
            }
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("Error fetching alarms.");
        }
    }

    private static async Task GetAlarmsOfSpecificPriority() {
        AlarmPriority alarmPriority;
        while (true) {
            Console.Write("Enter priority (Low/Medium/High): ");
            var input = Console.ReadLine();
            if (input == null)
                continue;

            if (Enum.TryParse(input, out alarmPriority))
                break;
            
            Console.WriteLine("Wrong priority format.");
        }

        var response = await HttpClient.GetAsync(ServerUrl + $"/alarms-of-specific-priority?priority={alarmPriority.ToString()}");
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            var alarms = JsonConvert.DeserializeObject<List<AlarmLogDto>>(data);
            if (alarms == null) {
                Console.WriteLine("Error fetching all requested alarms.");
                return;
            }
            
            Console.WriteLine("Time               | Priority | Type | Unit");
            foreach (var alarm in alarms)
            {
                Console.WriteLine($"{alarm.Timestamp.ToString("dd/MM/yyyy HH:mm:ss"), 19}|{alarm.Priority, 10}|{alarm.Type, 6}|{alarm.Unit, 5}");
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
        DateTime startTime;
        while (true) {
            Console.Write("Enter start time (dd/MM/yyyy HH:mm:ss): ");
            var input = Console.ReadLine();
            if (input == null)
                continue;
            
            if (
                DateTime.TryParseExact(
                    input,
                    "dd/MM/yyyy HH:mm:ss",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out startTime
                )
            )
                break;
            
            Console.WriteLine("Wrong input format.");
        }
        
        DateTime endTime;
        while (true) {
            Console.Write("Enter end time (dd/MM/yyyy HH:mm:ss): ");
            var input = Console.ReadLine();
            if (input == null)
                continue;
            
            if (
                DateTime.TryParseExact(
                    input,
                    "dd/MM/yyyy HH:mm:ss",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out endTime
                )
            )
                break;
            
            Console.WriteLine("Wrong input format.");
        }

        var response = await HttpClient.GetAsync(
            ServerUrl + $"/tag-values-in-specific-time-period?startTime={startTime.ToString("MM/dd/yyyy HH:mm:ss")}&endTime={endTime.ToString("MM/dd/yyyy HH:mm:ss")}"
        );
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            var tagLogs = JsonConvert.DeserializeObject<List<TagLogDto>>(data);
            if (tagLogs == null) {
                Console.WriteLine("Error fetching all requested values for all tags.");
                return;
            }
            
            Console.WriteLine("\nTag Type  |  Log ID  |  Timestamp  |  Value");
            foreach (var log in tagLogs)
            {
                Console.WriteLine($"Analog    |{log.Id, 10}|{log.Timestamp, 13}{log.EmittedValue, 10}");
            }
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("Error fetching tag values.");
        }
    }
    
    private static async Task GetAllValuesForSpecificTag()
    {
        Console.Write("Enter tag Name: ");
        var tagName = Console.ReadLine();

        var response = await HttpClient.GetAsync(ServerUrl + $"/all-values-for-specific-tag?tagName={tagName}");
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            var tagLogs = JsonConvert.DeserializeObject<List<TagLogDto>>(data);
            if (tagLogs == null) {
                Console.WriteLine("Error fetching all values for requested tag.");
                return;
            }
            
            Console.WriteLine("\nTag Type  |  Log ID  |  Timestamp  |  Value");
            foreach (var log in tagLogs) {
                Console.WriteLine($"Analog    |{log.Id,10}|{log.Timestamp,13}{log.EmittedValue,10}");
            }
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("Error fetching tag values.");
        }
    }

    private static async Task GetLastValuesAllAiTags()
    {
        var response = await HttpClient.GetAsync(ServerUrl + "/last-values-all-ai-tags");
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            var aiLogs = JsonConvert.DeserializeObject<List<TagLogDto>>(data);
            if (aiLogs == null) {
                Console.WriteLine("Error fetching the latest analog input tags.");
                return;
            }
                
            Console.WriteLine("\nTag Type  |  Log ID  |     Timestamp     |  Value");
            foreach (var log in aiLogs)
            {
                Console.WriteLine($"Analog    |{log.Id, 10}|{log.Timestamp.ToString("dd/MM/yyyy HH:mm:ss"), 19}|{log.EmittedValue, 10}");
            }
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("Error fetching AI tag values.");
        }
    }

    private static async Task GetLastValuesAllDiTags()
    {
        var response = await HttpClient.GetAsync(ServerUrl + "/last-values-all-di-tags");
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            var diLogs = JsonConvert.DeserializeObject<List<TagLogDto>>(data);
            if (diLogs == null) {
                Console.WriteLine("Error fetching the latest digital input tags.");
                return;
            }
            
            Console.WriteLine("\nTag Type  |  Log ID  |     Timestamp     |  Value");
            foreach (var log in diLogs)
            {
                Console.WriteLine(
                    $"Digital   |{log.Id, 10}|{log.Timestamp.ToString("dd/MM/yyyy HH:mm:ss"), 19}|{log.EmittedValue, 10}"
                );
            }
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("Error fetching DI tag values.");
        }
    }
}