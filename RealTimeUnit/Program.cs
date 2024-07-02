using Common.RealTimeUnit;
using Newtonsoft.Json;
using RealTimeUnit.Services;
using RealTimeUnit.Units;

namespace RealTimeUnit;

internal abstract class Program {
    private static async Task Main() {
        string? tagName;
        do {
            Console.Write("Enter this unit's tag identifier (non-null): ");
            tagName = Console.ReadLine();
        } while (tagName == null);

        var response = await RtuService.GetTag(tagName);
        if (!response.IsSuccessStatusCode) {
           Console.WriteLine(await response.Content.ReadAsStringAsync());
           return;
        }

        var rtuInformation =
            JsonConvert.DeserializeObject<RtuInformationDto>(await response.Content.ReadAsStringAsync());
        if (rtuInformation == null) {
            Console.WriteLine("Something went wrong while fetching the RTU information.");
            return;
        }

        var unit = rtuInformation switch {
            { IsAnalog: true, IsInput: true } => await AnalogInputUnit.Create(rtuInformation.TagName),
            { IsAnalog: true, IsInput: false } => await AnalogOutputUnit.Create(rtuInformation.TagName),
            { IsAnalog: false, IsInput: true } => await DigitalInputUnit.Create(rtuInformation.TagName),
            { IsAnalog: false, IsInput: false } => await DigitalOutputUnit.Create(rtuInformation.TagName)
        };
        if (unit == null) {
            Console.WriteLine("Something went wrong while fetching the RTU information.");
            return;
        }

        await unit.Start();
    }
}