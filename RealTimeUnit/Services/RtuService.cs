using Common.RealTimeUnit;
using Newtonsoft.Json;

namespace RealTimeUnit.Services;

public static class RtuService {
    private const string RtuControllerUrl = "http://localhost:5038/Rtu/";

    public static async Task<RtuInformationDto?> GetTag(string tagName) {
        HttpResponseMessage response;
        using (var httpClient = new HttpClient()) {
            response = await httpClient.GetAsync($"{RtuControllerUrl}/{tagName}");
        }
        
        return JsonConvert.DeserializeObject<RtuInformationDto>(await response.Content.ReadAsStringAsync());
    }

    public static async Task<AnalogInputUnitDto?> GetAnalogInputUnitInformation(string tagName) {
        HttpResponseMessage response;
        using (var httpClient = new HttpClient()) {
            response = await httpClient.GetAsync($"{RtuControllerUrl}/analog/input/{tagName}");
        }
        
        return JsonConvert.DeserializeObject<AnalogInputUnitDto>(await response.Content.ReadAsStringAsync());
    }
    
    public static async Task<AnalogOutputUnitDto?> GetAnalogOutputUnitInformation(string tagName) {
        HttpResponseMessage response;
        using (var httpClient = new HttpClient()) {
            response = await httpClient.GetAsync($"{RtuControllerUrl}/analog/output/{tagName}");
        }
        
        return JsonConvert.DeserializeObject<AnalogOutputUnitDto>(await response.Content.ReadAsStringAsync());
    }
    
    public static async Task<DigitalInputUnitDto?> GetDigitalInputUnitInformation(string tagName) {
        HttpResponseMessage response;
        using (var httpClient = new HttpClient()) {
            response = await httpClient.GetAsync($"{RtuControllerUrl}/digital/input/{tagName}");
        }
        
        return JsonConvert.DeserializeObject<DigitalInputUnitDto>(await response.Content.ReadAsStringAsync());
    }
    
    public static async Task<DigitalOutputUnitDto?> GetDigitalOutputUnitInformation(string tagName) {
        HttpResponseMessage response;
        using (var httpClient = new HttpClient()) {
            response = await httpClient.GetAsync($"{RtuControllerUrl}/digital/output/{tagName}");
        }
        
        return JsonConvert.DeserializeObject<DigitalOutputUnitDto>(await response.Content.ReadAsStringAsync());
    }
}