using System.Security.Cryptography;
using Common.RealTimeUnit;
using RealTimeUnit.Services;

namespace RealTimeUnit.Units;

public class AnalogInputUnit(
    string tagName,
    TimeSpan scanTime,
    decimal lowLimit,
    decimal highLimit,
    AsymmetricAlgorithm key
) : IRtu {
    public static async Task<IRtu> Create(string tagName) {
        var key = RSA.Create();
        var analogInputUnitDto =
            await RtuService
                .GetAnalogInputUnitInformation(
                    tagName,
                    new RegisterInputUnitDto(key.ToXmlString(false))
                );

        return new AnalogInputUnit(
            analogInputUnitDto.TagName,
            analogInputUnitDto.ScanTime,
            analogInputUnitDto.LowLimit,
            analogInputUnitDto.HighLimit,
            key
        );
    }

    private decimal GenerateValue() {
        return RandomNumberGenerator.GetInt32((int) lowLimit, (int) highLimit + 1);
    }
    
    public async void Start() {
        Console.WriteLine("Press ESC to stop measuring and sending values to the server.");
        do {
            while (!Console.KeyAvailable) {
                Thread.Sleep(scanTime);

                var dto = new AnalogValueDto(tagName, GenerateValue(), DateTime.Now);
                dto.Sign(key);
                await RtuService.SendAnalogValue(dto);
            }
        } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
    }
}