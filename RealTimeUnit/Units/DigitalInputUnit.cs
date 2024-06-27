using RealTimeUnit.Services;

namespace RealTimeUnit.Units;

public class DigitalInputUnit(string tagName, TimeSpan scanTime) : IRtu {
    public static async Task<IRtu> Create(string tagName) {
        var digitalInputUnitDto = await RtuService.GetDigitalInputUnitInformation(tagName);

        return new DigitalInputUnit(digitalInputUnitDto.TagName, digitalInputUnitDto.ScanTime);
    }
    
    public void Start() {
        Console.WriteLine("Press ESC to stop measuring and sending values to the server.");
        do {
            while (!Console.KeyAvailable) {
                
            }
        } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
    }
}