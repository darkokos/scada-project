using System.Xml.Linq;
using Lombok.NET;

namespace ScadaCore.Models;

[NoArgsConstructor]
public partial class AnalogOutputTag : OutputTag {
    public decimal InitialValue { get; set; }
    public decimal LowLimit { get; set; }
    public decimal HighLimit { get; set; }
    public string Unit { get; set; }
    
    private static string GetInitialValueXElementName() {
        return "initialValue";
    }

    private static string GetLowLimitXElementName() {
        return "lowLimit";
    }
    
    private static string GetHighLimitXElementName() {
        return "highLimit";
    }
    
    private static string GetUnitXElementName() {
        return "unit";
    }

    public AnalogOutputTag(string name, string description, int inputOutputAddress, decimal initialValue,
        decimal lowLimit, decimal highLimit, string unit) : base(name, description,
        inputOutputAddress)
    {
        InitialValue = initialValue;
        LowLimit = lowLimit;
        HighLimit = highLimit;
        Unit = unit;
    }
    
    public AnalogOutputTag(XContainer analogOutputTagXElement) : base(analogOutputTagXElement) {
        InitialValue = decimal.TryParse(
            analogOutputTagXElement.Element(GetInitialValueXElementName())?.Value,
            out var initialValue
        ) ? initialValue : -1;
        LowLimit = decimal.TryParse(
            analogOutputTagXElement.Element(GetLowLimitXElementName())?.Value,
            out var lowLimit
        ) ? lowLimit : -1;
        HighLimit = decimal.TryParse(
            analogOutputTagXElement.Element(GetHighLimitXElementName())?.Value,
            out var highLimit
        ) ? highLimit : -1;
        Unit = analogOutputTagXElement.Element(GetUnitXElementName())?.Value ?? "";
    }
    
    public static string GetParentXElementName() {
        return "analogOutputTags";
    }

    public const string GetXName = "analogOutputTag";

    public XElement GetXElementRepresentation() {
        var xElementRepresentation = new XElement(
            GetXName,
            new XElement(GetInitialValueXElementName(), InitialValue),
            new XElement(GetLowLimitXElementName(), LowLimit),
            new XElement(GetHighLimitXElementName(), HighLimit),
            new XElement(GetUnitXElementName(), Unit)
        );
        
        SetXAttributes(xElementRepresentation);
        return xElementRepresentation;
    }
}
