using System.Security.Cryptography;
using Common.RealTimeUnit;
using RealTimeUnit.Services;

namespace RealTimeUnit.Units;

public class DigitalInputUnit(string tagName, TimeSpan scanTime, AsymmetricAlgorithm key) : IRtu {
    public static async Task<IRtu> Create(string tagName) {
        var key = RSA.Create();
        var digitalInputUnitDto =
            await RtuService
                .GetDigitalInputUnitInformation(
                    tagName,
                    new RegisterInputUnitDto(key.ToXmlString(false))
                );

        return new DigitalInputUnit(digitalInputUnitDto.TagName, digitalInputUnitDto.ScanTime, key);
    }

    private static bool GenerateValue() {
        return RandomNumberGenerator.GetInt32(2) == 0;
    }
    
    public async void Start() {
        Console.WriteLine("Press ESC to stop measuring and sending values to the server.");
        do {
            while (!Console.KeyAvailable) {
                Thread.Sleep(scanTime);

                var digitalValueDto = new DigitalValueDto(tagName, GenerateValue(), DateTime.Now);
                digitalValueDto.Sign(key);
                await RtuService.SendDigitalValue(digitalValueDto);
            }
        } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
    }
}