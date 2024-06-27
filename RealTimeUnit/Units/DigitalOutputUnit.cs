using RealTimeUnit.Services;

namespace RealTimeUnit.Units;

public class DigitalOutputUnit(bool currentValue) : IRtu {
    public static async Task<IRtu> Create(string tagName) {
        var digitalOutputUnitDto = await RtuService.GetDigitalOutputUnitInformation(tagName);

        return new DigitalOutputUnit(digitalOutputUnitDto.CurrentValue);
    }
    
    public void Start() {
        Console.WriteLine($"Started digital output unit. The initial value is: {(currentValue ? 1 : 0)}");
    }
}