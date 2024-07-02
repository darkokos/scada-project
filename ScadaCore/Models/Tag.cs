using System.Xml.Linq;
using Lombok.NET;

namespace ScadaCore.Models;

[NoArgsConstructor]
public partial class Tag {
    public string Name { get; set; }
    public string Description { get; set; }
    public int InputOutputAddress { get; set; }

    public static string GetNameXElementName() {
        return "name";
    }
    
    private static string GetDescriptionXElementName() {
        return "description";
    }
    
    public static string GetInputOutputAddressXElementName() {
        return "inputOutputAddress";
    }

    protected Tag(XContainer tagXElement) {
        Name = tagXElement.Element(GetNameXElementName())?.Value ?? "";
        Description = tagXElement.Element(GetDescriptionXElementName())?.Value ?? "";
        InputOutputAddress =
            int.TryParse(
                tagXElement.Element(GetInputOutputAddressXElementName())?.Value,
                out var inputOutputAddress
            ) ? inputOutputAddress : -1;
    }

    protected Tag(string name, string description, int inputOutputAddress)
    {
        Name = name;
        Description = description;
        InputOutputAddress = inputOutputAddress;
    }

    public static string GetRootXElementName() {
        return "tags";
    }

    protected void SetXAttributes(XElement xElement) {
        xElement.AddFirst(new XElement(GetInputOutputAddressXElementName(), InputOutputAddress));
        xElement.AddFirst(new XElement(GetDescriptionXElementName(), Description));
        xElement.AddFirst(new XElement(GetNameXElementName(), Name));
    }
}