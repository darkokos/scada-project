using System.Xml.Linq;
using Lombok.NET;

namespace ScadaCore.Models;

[NoArgsConstructor]
public partial class DigitalInputTag : InputTag {
    public DigitalInputTag(XContainer digitalInputTagXElement) : base(digitalInputTagXElement) { }
    
    public static string GetParentXElementName() {
        return "digitalInputTags";
    }

    public const string GetXName = "digitalInputTag";
    public DigitalInputTag(string name, string description, int inputOutputAddress, bool isSimulated, TimeSpan scanTime, bool isScanned) : base(name, description,
        inputOutputAddress, isSimulated, scanTime, isScanned)
    {
    }
    
    public XElement GetXElementRepresentation() {
        var xElementRepresentation = new XElement(GetXName);
        SetXAttributes(xElementRepresentation);
        return xElementRepresentation;
    }
}