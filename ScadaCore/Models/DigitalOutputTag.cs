using System.Xml.Linq;
using Lombok.NET;

namespace ScadaCore.Models;

[NoArgsConstructor]
public partial class DigitalOutputTag : OutputTag {
    public bool InitialValue { get; set; }
    
    private static string GetInitialValueXElementName() {
        return "initialValue";
    }

    public DigitalOutputTag(string name, string description, int inputOutputAddress, bool initialValue) : base(name,
        description,
        inputOutputAddress)
    {
        InitialValue = initialValue;
    }

    public DigitalOutputTag(XContainer digitalOutputTagXElement) : base(digitalOutputTagXElement) {
        InitialValue = bool.TryParse(
            digitalOutputTagXElement.Element(GetInitialValueXElementName())?.Value,
            out var initialValue
        ) && initialValue;
    }
    
    public static string GetParentXElementName() {
        return "digitalOutputTags";
    }

    public const string GetXName = "digitalOutputTag";
    
    public XElement GetXElementRepresentation() {
        var xElementRepresentation = new XElement(
            GetXName,
            new XElement(GetInitialValueXElementName(), InitialValue)
        );
        
        SetXAttributes(xElementRepresentation);
        return xElementRepresentation;
    }
}