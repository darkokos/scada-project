using Common.RealTimeUnit;
using Newtonsoft.Json;
using RealTimeUnit.Services;

namespace RealTimeUnit.Units;

public class AnalogOutputUnit(decimal currentValue) : IRtu {
    public static async Task<IRtu?> Create(string tagName) {
        var response = await RtuService.GetAnalogOutputUnitInformation(tagName);
        if (!response.IsSuccessStatusCode) {
           Console.WriteLine(await response.Content.ReadAsStringAsync());
           return null;
        }

        var analogOutputUnitDto =
            JsonConvert.DeserializeObject<AnalogOutputUnitDto>(await response.Content.ReadAsStringAsync());
        if (analogOutputUnitDto == null) {
            Console.WriteLine("Something went wrong while fetching the RTU information.");
            return null;
        }

        return new AnalogOutputUnit(analogOutputUnitDto.CurrentValue);
    }
    
    public async Task Start() {
        await Task.Run(() =>
            Console.WriteLine($"Started analog output unit. The initial value is: {currentValue}"));
    }
}