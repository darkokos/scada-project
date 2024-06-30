﻿using System.Net;
using Common.RealTimeUnit;
using Newtonsoft.Json;
using RealTimeUnit.Services;
using RealTimeUnit.Units;

namespace RealTimeUnit;

internal abstract class Program {
    private static async Task Main() {
        string? tagName;
        do {
            Console.WriteLine("Enter this unit's tag identifier (non-null): ");
            tagName = Console.ReadLine();
        } while (tagName == null);

        var response = await RtuService.GetTag(tagName);
        if (response.StatusCode == HttpStatusCode.NotFound) {
            Console.WriteLine("Tag associated with input name was not found.");
            return;
        }

        var rtuInformation =
            JsonConvert.DeserializeObject<RtuInformationDto>(await response.Content.ReadAsStringAsync());
        if (rtuInformation == null) {
            Console.WriteLine("Something went wrong while fetching the RTU information.");
            return;
        }

        (rtuInformation switch {
            { isAnalog: true, isInput: true } => await AnalogInputUnit.Create(rtuInformation.TagName),
            { isAnalog: true, isInput: false } => await AnalogOutputUnit.Create(rtuInformation.TagName),
            { isAnalog: false, isInput: true } => await DigitalInputUnit.Create(rtuInformation.TagName),
            { isAnalog: false, isInput: false } => await DigitalOutputUnit.Create(rtuInformation.TagName)
        })?.Start();
    }
}