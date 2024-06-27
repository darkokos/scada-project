using RealTimeUnit.Services;

namespace RealTimeUnit.Units;

public class AnalogOutputUnit(decimal currentValue) : IRtu {
    public static async Task<IRtu> Create(string tagName) {
        var analogOutputUnitDto = await RtuService.GetAnalogOutputUnitInformation(tagName);

        return new AnalogOutputUnit(analogOutputUnitDto.CurrentValue);
    }
    
    public void Start() {
        Console.WriteLine($"Started analog output unit. The initial value is: {currentValue}");
    }
}