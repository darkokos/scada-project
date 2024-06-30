using System.Text;
using Common.RealTimeUnit;
using Newtonsoft.Json;

namespace RealTimeUnit.Services;

public static class RtuService {
    private const string RtuControllerUrl = "http://localhost:59767/Rtu";

    public static async Task<HttpResponseMessage> GetTag(string tagName) {
        using var httpClient = new HttpClient();
        return await httpClient.GetAsync($"{RtuControllerUrl}/{tagName}");
    }

    public static async Task<HttpResponseMessage> GetAnalogInputUnitInformation(
        string tagName,
        RegisterInputUnitDto dto
    ) {
        var requestContent =
            new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        using var httpClient = new HttpClient();
        return await httpClient.PostAsync($"{RtuControllerUrl}/analog/input/{tagName}", requestContent);
    }
    
    public static async Task<HttpResponseMessage> GetAnalogOutputUnitInformation(string tagName) {
        using var httpClient = new HttpClient();
        return await httpClient.GetAsync($"{RtuControllerUrl}/analog/output/{tagName}");
    }
    
    public static async Task<HttpResponseMessage> GetDigitalInputUnitInformation(
        string tagName,
        RegisterInputUnitDto dto
    ) {
        var requestContent =
            new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        using var httpClient = new HttpClient();
        return await httpClient.PostAsync($"{RtuControllerUrl}/digital/input/{tagName}", requestContent);
    }
    
    public static async Task<HttpResponseMessage> GetDigitalOutputUnitInformation(string tagName) {
        using var httpClient = new HttpClient();
        return await httpClient.GetAsync($"{RtuControllerUrl}/digital/output/{tagName}");
    }

    public static async Task<HttpResponseMessage> SendAnalogValue(AnalogValueDto dto) {
        var requestContent =
            new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        using var httpClient = new HttpClient();
        return await httpClient.PostAsync($"{RtuControllerUrl}/analog", requestContent);
    }
    
    public static async Task<HttpResponseMessage> SendDigitalValue(DigitalValueDto dto) {
        var requestContent =
            new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        using var httpClient = new HttpClient();
        return await httpClient.PostAsync($"{RtuControllerUrl}/digital", requestContent);
    }
}