﻿using System.Xml.Linq;
using Lombok.NET;

namespace ScadaCore.Models;

[NoArgsConstructor]
public partial class AnalogOutputTag : OutputTag {
    
    // TODO: Types
    public int LowLimit { get; set; }
    public int HighLimit { get; set; }
    
    // TODO: Units
    public HashSet<string> Units { get; set; }

    public AnalogOutputTag(XContainer analogOutputTagXElement) : base(analogOutputTagXElement) {
        LowLimit =
            int.TryParse(analogOutputTagXElement.Element("lowLimit")?.Value, out var lowLimit) ? lowLimit : -1;
        HighLimit =
            int.TryParse(analogOutputTagXElement.Element("highLimit")?.Value, out var highLimit) ? highLimit : -1;
        Units = new HashSet<string>();
        foreach (var unit in analogOutputTagXElement
                     .Element("units")?
                     .Elements("unit") ?? new List<XElement>()) {
            Units.Add(unit.Value);
        }
    }
    
    public static string GetParentXElementName() {
        return "analogOutputTags";
    }

    public const string GetXName = "analogOutputTag";

    private static string GetLowLimitXElementName() {
        return "lowLimit";
    }
    
    private static string GetHighLimitXElementName() {
        return "highLimit";
    }
    
    private static string GetUnitsXElementName() {
        return "units";
    }
    
    private static string GetUnitXElementName() {
        return "unit";
    }
    
    public XElement GetXElementRepresentation() {
        var unitsXElement = new XElement(GetUnitsXElementName());
        foreach (var unit in Units) {
            unitsXElement.Add(new XElement(GetUnitXElementName(), unit));
        }
        var xElementRepresentation = new XElement(
            GetXName,
            new XElement(GetLowLimitXElementName(), LowLimit),
            new XElement(GetHighLimitXElementName(), HighLimit),
            unitsXElement
        );
        
        SetXAttributes(xElementRepresentation);
        return xElementRepresentation;
    }
}
