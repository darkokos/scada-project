using System.Xml.Linq;
using Lombok.NET;

namespace ScadaCore.Models;

[NoArgsConstructor]
public partial class InputTag : Tag {
    public bool IsSimulated { get; set; }
    public TimeSpan ScanTime { get; set; }
    public bool IsScanned { get; set; }
    
    private static string GetIsSimulatedXElementName() {
        return "isSimulated";
    }
    
    private static string GetScanTimeXElementName() {
        return "scanTime";
    }
    
    private static string GetIsScannedXElementName() {
        return "isScanned";
    }

    protected InputTag(XContainer inputTagXElement) : base(inputTagXElement) {
        IsSimulated = bool.TryParse(
            inputTagXElement.Element(GetIsSimulatedXElementName())?.Value,
            out var isSimulated
        ) && isSimulated;
        ScanTime = TimeSpan.FromSeconds(
            double.TryParse(
                inputTagXElement.Element(GetScanTimeXElementName())?.Value,
                out var scanTime
            ) ? scanTime : -1
        );
        IsScanned = bool.TryParse(
            inputTagXElement.Element(GetIsScannedXElementName())?.Value,
            out var isScanned
        ) && isScanned;
    }

    protected InputTag(string name, string description, int inputOutputAddress, bool isSimulated, TimeSpan scanTime, bool isScanned) : base(name, description,
        inputOutputAddress)
    {
        IsSimulated = isSimulated;
        IsScanned = isScanned;
        ScanTime = scanTime;
    }

    protected new void SetXAttributes(XElement xElement) {
        xElement.AddFirst(new XElement(GetIsScannedXElementName(), IsScanned));
        xElement.AddFirst(new XElement(GetScanTimeXElementName(), ScanTime.TotalSeconds));
        xElement.AddFirst(new XElement(GetIsSimulatedXElementName(), IsSimulated));
        base.SetXAttributes(xElement);
    }
}