using System.Xml.Linq;
using Lombok.NET;

namespace ScadaCore.Models;

// TODO: Conclude whether this needs to exist later, when known
[NoArgsConstructor]
public partial class DigitalInputTag : InputTag {
    public DigitalInputTag(XContainer digitalInputTagXElement) : base(digitalInputTagXElement) { }
    
    public static string GetParentXElementName() {
        return "digitalInputTags";
    }

    public const string GetXName = "digitalInputTag";
    
    public XElement GetXElementRepresentation() {
        var xElementRepresentation = new XElement(GetXName);
        SetXAttributes(xElementRepresentation);
        return xElementRepresentation;
    }
}