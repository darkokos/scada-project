using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Lombok.NET;

namespace ScadaCore.Models;

[NoArgsConstructor]
public partial class Tag {
    public string Name { get; set; }
    
    [StringLength(250, MinimumLength = 0, ErrorMessage = "{0} should be between {2} and {1} characters long.")]
    public string Description { get; set; }
    
    // TODO: Change the type of this when known
    [Required] public int InputOutputAddress { get; set; }

    protected Tag(XContainer tagXElement) {
        Name = tagXElement.Element("name")?.Value ?? "";
        Description = tagXElement.Element("description")?.Value ?? "";
        InputOutputAddress =
            int.TryParse(tagXElement.Element("inputOutputAddress")?.Value, out var inputOutputAddress)
                ? inputOutputAddress
                : -1;
    }

    public static string GetNameXElementName() {
        return "name";
    }
    
    private static string GetDescriptionXElementName() {
        return "description";
    }
    
    private static string GetInputOutputAddressXElementName() {
        return "inputOutputAddress";
    }

    protected void SetXAttributes(XElement xElement) {
        xElement.AddFirst(new XElement(GetInputOutputAddressXElementName(), InputOutputAddress));
        xElement.AddFirst(new XElement(GetDescriptionXElementName(), Description));
        xElement.AddFirst(new XElement(GetNameXElementName(), Name));
    }
}