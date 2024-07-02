using System.Xml.Linq;
using Lombok.NET;

namespace ScadaCore.Models;

[NoArgsConstructor]
public partial class OutputTag : Tag {
    protected OutputTag(XContainer outputTagXElement) : base(outputTagXElement) { }
    protected OutputTag(string name, string description, int inputOutputAddress) : base(name, description,
        inputOutputAddress)
    { }

    protected new void SetXAttributes(XElement xElement) {
        base.SetXAttributes(xElement);
    }
}