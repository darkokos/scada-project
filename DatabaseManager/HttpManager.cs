using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace DatabaseManager;
using Common.DatabaseManagerCommon;

public class HttpManager
{
    
    public static string ServerUrl = "http://localhost:59767/DatabaseManager/";

    public static async Task DeleteTag(DeleteTagDTO dto)
    {
        using (HttpClient client = new HttpClient())
        {
            string body = JsonConvert.SerializeObject(dto);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ServerUrl + "deleteTag", content);
            if (response.StatusCode == HttpStatusCode.OK)  Console.WriteLine("Successfully deleted the tag.");
            else
            {
                Console.WriteLine("Error deleting tag - status code: " + response.StatusCode);
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
        }
    }

    public static async Task ChangeTagScanning(ChangeScanTagDTO dto)
    {
        using (HttpClient client = new HttpClient())
        {
            string body = JsonConvert.SerializeObject(dto);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ServerUrl + "changeTagScanning", content);
            if (response.StatusCode == HttpStatusCode.OK)  Console.WriteLine("Successfully changed tag scanning option.");
            else
            {
                Console.WriteLine("Error changing tag scanning option - status code: " + response.StatusCode);
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
        }
    }

    public static async Task WriteTagOutputValue(WriteTagValueDTO dto)
    {
        using (HttpClient client = new HttpClient())
        {
            string body = JsonConvert.SerializeObject(dto);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ServerUrl + "writeTagValue", content);
            if (response.StatusCode == HttpStatusCode.OK)  Console.WriteLine("Successfully wrote tag output value.");
            else
            {
                Console.WriteLine("Error writing tag output value - status code: " + response.StatusCode);
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
        }
    }
        
    public static async Task<string> GetCurrentTagValues(ShowCurrentTagValuesDTO dto) {
        using (HttpClient client = new HttpClient())
        {
            string body = JsonConvert.SerializeObject(dto);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ServerUrl + "currentTagValues", content);
            if (response.StatusCode != HttpStatusCode.OK) {
                Console.WriteLine("Error getting current tag values - status code: " + response.StatusCode);
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
                return "";
            }
            return await response.Content.ReadAsStringAsync();
        }
    }

    public static async Task Register(RegisterDTO dto)
    {
        using (HttpClient client = new HttpClient())
        {
            string body = JsonConvert.SerializeObject(dto);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ServerUrl + "register", content);
            if (response.StatusCode == HttpStatusCode.OK)  Console.WriteLine("Successfully registered.");
            else
            {
                Console.WriteLine("Error registering - status code: " + response.StatusCode);
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
        }
    }

    public static async Task<string> Login(LoginDTO dto)
    {
        using (HttpClient client = new HttpClient())
        {
            string body = JsonConvert.SerializeObject(dto);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ServerUrl + "login", content);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine("Successfully logged in.");
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                Console.WriteLine("Error logging in - status code: " + response.StatusCode);
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
                return "";
            }
        }
    }
        
    public static async Task Logout(LogoutDTO dto)
    {
        using (HttpClient client = new HttpClient())
        {
            string body = JsonConvert.SerializeObject(dto);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ServerUrl + "logout", content);
            if (response.StatusCode == HttpStatusCode.OK)  Console.WriteLine("Successfully logged out.");
            else
            {
                Console.WriteLine("Error logging out - status code: " + response.StatusCode);
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
        }
    }
    public static async Task AddTag(AddTagDTO dto)
    {
        using (HttpClient client = new HttpClient())
        {
            string body = JsonConvert.SerializeObject(dto);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ServerUrl + "addTag", content);
            if (response.StatusCode == HttpStatusCode.OK)  Console.WriteLine("Successfully added tag.");
            else
            {
                Console.WriteLine("Error adding tag - status code: " + response.StatusCode);
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
        }
    }
    
    public static async Task AddAlarm(AddAlarmDTO dto)
    {
        using (HttpClient client = new HttpClient())
        {
            string body = JsonConvert.SerializeObject(dto);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ServerUrl + "addAlarm", content);
            if (response.StatusCode == HttpStatusCode.OK)  Console.WriteLine("Successfully added alarm.");
            else
            {
                Console.WriteLine("Error adding alarm - status code: " + response.StatusCode);
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
        }
    }
}