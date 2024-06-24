using System.Xml.Linq;
using Lombok.NET;

namespace ScadaCore.Models;

[NoArgsConstructor]
public partial class AnalogInputTag : InputTag {
    public HashSet<Alarm> Alarms { get; set; }
    
    // TODO: Again, no idea what type these are supposed to be
    public int LowLimit { get; set; }
    public int HighLimit { get; set; }
    
    // TODO: Units??!
    public HashSet<string> Units { get; set; }

    public AnalogInputTag(XContainer analogInputTagXElement) : base(analogInputTagXElement) {
        
        // TODO: Get references to alarms, somehow
        Alarms = new HashSet<Alarm>();
        
        LowLimit =
            int.TryParse(analogInputTagXElement.Element("lowLimit")?.Value, out var lowLimit) ? lowLimit : -1;
        HighLimit =
            int.TryParse(analogInputTagXElement.Element("highLimit")?.Value, out var highLimit) ? highLimit : -1;
        Units = new HashSet<string>();
        foreach (var unit in analogInputTagXElement
                     .Element("units")?
                     .Elements("unit") ?? new List<XElement>()) {
            Units.Add(unit.Value);
        }
    }

    public static string GetParentXElementName() {
        return "analogInputTags";
    }

    public const string GetXName = "analogInputTag";

    private static string GetAlarmsXElementName() {
        return "alarms";
    }
    
    private static string GetAlarmXElementName() {
        return "alarm";
    }
    
    private static string GetLowLimitXElementName() {
        return "lowLimit";
    }
    
    private static string GetHighLimitXElementName() {
        return "highLimit";
    }
    
    private static string GetUnitsXElementName() {
        return "units";
    }
    
    private static string GetUnitXElementName() {
        return "unit";
    }

    public XElement GetXElementRepresentation() {
        var alarmsXElement = new XElement(GetAlarmsXElementName());
        foreach (var alarm in Alarms) {
            alarmsXElement.Add(new XElement(GetAlarmXElementName(), alarm.Id));
        }
        
        var unitsXElement = new XElement(GetUnitsXElementName());
        foreach (var unit in Units) {
            unitsXElement.Add(new XElement(GetUnitXElementName(), unit));
        }
        
        var xElementRepresentation = new XElement(
            GetXName,
            alarmsXElement,
            new XElement(GetLowLimitXElementName(), LowLimit),
            new XElement(GetHighLimitXElementName(), HighLimit),
            unitsXElement
        );
        
        SetXAttributes(xElementRepresentation);
        return xElementRepresentation;
    }
}