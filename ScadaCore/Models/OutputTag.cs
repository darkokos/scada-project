using System.Xml.Linq;
using Lombok.NET;

namespace ScadaCore.Models;

[NoArgsConstructor]
public partial class OutputTag : Tag {
    
    // TODO: Have no idea what type this is supposed to be
    public int InitialValue { get; set; }

    protected OutputTag(XContainer outputTagXElement) : base(outputTagXElement) {
        InitialValue =
            int.TryParse(outputTagXElement.Element("initialValue")?.Value, out var initialValue)
                ? initialValue
                : -1;
    }

    private static string GetInitialValueXElementName() {
        return "initialValue";
    }

    protected new void SetXAttributes(XElement xElement) {
        xElement.AddFirst(new XElement(GetInitialValueXElementName(), InitialValue));
        base.SetXAttributes(xElement);
    }
}