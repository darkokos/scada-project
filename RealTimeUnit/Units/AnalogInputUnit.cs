using RealTimeUnit.Services;

namespace RealTimeUnit.Units;

public class AnalogInputUnit(string tagName, TimeSpan scanTime, decimal lowLimit, decimal highLimit) : IRtu {
    public static async Task<IRtu> Create(string tagName) {
        var analogInputUnitDto = await RtuService.GetAnalogInputUnitInformation(tagName);

        return new AnalogInputUnit(
            analogInputUnitDto.TagName,
            analogInputUnitDto.ScanTime,
            analogInputUnitDto.LowLimit,
            analogInputUnitDto.HighLimit
        );
    }
    
    public void Start() {
        Console.WriteLine("Press ESC to stop measuring and sending values to the server.");
        do {
            while (!Console.KeyAvailable) {
                
            }
        } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
    }
}