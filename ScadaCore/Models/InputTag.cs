using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Lombok.NET;

namespace ScadaCore.Models;

[NoArgsConstructor]
public partial class InputTag : Tag {
    
    // TODO: Whatever type this is supposed to be
    [Required] public string Driver { get; set; }

    // TODO: This is supposed to represent a period. I don't know whether there is a better type to represent that, or
    // whether it will be necessary at all
    [Required] public int ScanTime { get; set; }
    [Required] public bool IsScanned { get; set; }

    protected InputTag(XContainer inputTagXElement) : base(inputTagXElement) {
        Driver = inputTagXElement.Element("driver")?.Value ?? "";
        ScanTime = int.TryParse(inputTagXElement.Element("scanTime")?.Value, out var scanTime) ? scanTime : -1;
        IsScanned =
            bool.TryParse(inputTagXElement.Element("isScanned")?.Value, out var isScanned) && isScanned;
    }

    private static string GetDriverXElementName() {
        return "driver";
    }
    
    private static string GetScanTimeXElementName() {
        return "scanTime";
    }
    
    private static string GetIsScannedXElementName() {
        return "isScanned";
    }

    protected new void SetXAttributes(XElement xElement) {
        xElement.AddFirst(new XElement(GetIsScannedXElementName(), IsScanned));
        xElement.AddFirst(new XElement(GetScanTimeXElementName(), ScanTime));
        xElement.AddFirst(new XElement(GetDriverXElementName(), Driver));
        base.SetXAttributes(xElement);
    }
}