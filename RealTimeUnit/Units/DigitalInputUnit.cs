using System.Security.Cryptography;
using Common.RealTimeUnit;
using RealTimeUnit.Services;

namespace RealTimeUnit.Units;

public class DigitalInputUnit(string tagName, TimeSpan scanTime) : IRtu {
    public static async Task<IRtu> Create(string tagName) {
        var digitalInputUnitDto = await RtuService.GetDigitalInputUnitInformation(tagName);

        return new DigitalInputUnit(digitalInputUnitDto.TagName, digitalInputUnitDto.ScanTime);
    }

    private static bool GenerateValue() {
        return RandomNumberGenerator.GetInt32(2) == 0;
    }
    
    public async void Start() {
        Console.WriteLine("Press ESC to stop measuring and sending values to the server.");
        do {
            while (!Console.KeyAvailable) {
                Thread.Sleep(scanTime);

                await RtuService.SendDigitalValue(new DigitalValueDto(tagName, GenerateValue(), DateTime.Now));
            }
        } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
    }
}