using System.Security.Cryptography;
using Common.RealTimeUnit;
using Newtonsoft.Json;
using RealTimeUnit.Services;

namespace RealTimeUnit.Units;

public class DigitalInputUnit(string tagName, TimeSpan scanTime, RSA key) : IRtu {
    public static async Task<IRtu?> Create(string tagName) {
        var key = RSA.Create();
        var response = await RtuService.GetDigitalInputUnitInformation(
            tagName,
            new RegisterInputUnitDto(key.ToXmlString(false))
        );
        if (!response.IsSuccessStatusCode) {
           Console.WriteLine(await response.Content.ReadAsStringAsync());
           return null;
        }

        var digitalInputUnitDto =
            JsonConvert.DeserializeObject<DigitalInputUnitDto>(await response.Content.ReadAsStringAsync());
        if (digitalInputUnitDto == null) {
            Console.WriteLine("Something went wrong while fetching the RTU information.");
            return null;
        }

        return new DigitalInputUnit(digitalInputUnitDto.TagName, digitalInputUnitDto.ScanTime, key);
    }

    private static bool GenerateValue() {
        return RandomNumberGenerator.GetInt32(2) == 0;
    }
    
    public async Task Start() {
        Console.WriteLine("Press ESC to stop measuring and sending values to the server.");
        do {
            while (!Console.KeyAvailable) {
                var digitalValueDto = new DigitalValueDto(tagName, GenerateValue(), DateTime.Now);
                digitalValueDto.Sign(key);
                var response = await RtuService.SendDigitalValue(digitalValueDto);
                if (!response.IsSuccessStatusCode) {
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                    continue;
                }
                
                var returnedDigitalValueDto =
                    JsonConvert.DeserializeObject<DigitalValueDto>(await response.Content.ReadAsStringAsync());
                if (returnedDigitalValueDto == null) {
                    Console.WriteLine("Something went wrong while fetching the RTU information.");
                }
                Console.WriteLine($"Sent value: {(returnedDigitalValueDto?.Value ?? default ? 1 : 0)}");
                
                Thread.Sleep(scanTime);
            }
        } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
    }
}