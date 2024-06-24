using System.Xml.Linq;
using Lombok.NET;

namespace ScadaCore.Models;

// TODO: Conclude whether this needs to exist later, when known
[NoArgsConstructor]
public partial class DigitalOutputTag : OutputTag {
    public DigitalOutputTag(XContainer digitalOutputTagXElement) : base(digitalOutputTagXElement) { }
    
    public static string GetParentXElementName() {
        return "digitalOutputTags";
    }

    public const string GetXName = "digitalOutputTag";
    
    public XElement GetXElementRepresentation() {
        var xElementRepresentation = new XElement(GetXName);
        SetXAttributes(xElementRepresentation);
        return xElementRepresentation;
    }
}