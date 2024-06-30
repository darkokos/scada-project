using Common.RealTimeUnit;
using Newtonsoft.Json;
using RealTimeUnit.Services;

namespace RealTimeUnit.Units;

public class DigitalOutputUnit(bool currentValue) : IRtu {
    public static async Task<IRtu?> Create(string tagName) {
        var response = await RtuService.GetDigitalOutputUnitInformation(tagName);
        if (!response.IsSuccessStatusCode) {
           Console.WriteLine(await response.Content.ReadAsStringAsync());
           return null;
        }

        var digitalOutputUnitDto =
            JsonConvert.DeserializeObject<DigitalOutputUnitDto>(await response.Content.ReadAsStringAsync());
        if (digitalOutputUnitDto == null) {
            Console.WriteLine("Something went wrong while fetching the RTU information.");
            return null;
        }

        return new DigitalOutputUnit(digitalOutputUnitDto.CurrentValue);
    }
    
    public void Start() {
        Console.WriteLine($"Started digital output unit. The initial value is: {(currentValue ? 1 : 0)}");
    }
}