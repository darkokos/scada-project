using System.Xml.Linq;
using Lombok.NET;

namespace ScadaCore.Models;

[NoArgsConstructor]
public partial class AnalogInputTag : InputTag {
    public HashSet<int> AlarmIds { get; set; }
    
    // TODO: Again, no idea what type these are supposed to be
    public int LowLimit { get; set; }
    public int HighLimit { get; set; }
    
    // TODO: Units??!
    public HashSet<string> Units { get; set; }

    private static string GetAlarmIdsXElementName() {
        return "alarmIds";
    }
    
    private static string GetAlarmIdXElementName() {
        return "alarmId";
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

    public AnalogInputTag(XContainer analogInputTagXElement) : base(analogInputTagXElement) {
        AlarmIds = new HashSet<int>();
        foreach (var alarmIdXElement in analogInputTagXElement
                     .Element(GetAlarmIdsXElementName())?
                     .Elements(GetAlarmIdXElementName()) ?? new List<XElement>()) {
            AlarmIds.Add(int.TryParse(alarmIdXElement.Value, out var alarmId) ? alarmId : -1);
        }
        
        LowLimit =
            int.TryParse(analogInputTagXElement.Element(GetLowLimitXElementName())?.Value, out var lowLimit)
                ? lowLimit
                : -1;
        HighLimit =
            int.TryParse(analogInputTagXElement.Element(GetHighLimitXElementName())?.Value, out var highLimit)
                ? highLimit
                : -1;
        Units = new HashSet<string>();
        foreach (var unit in analogInputTagXElement
                     .Element(GetUnitsXElementName())?
                     .Elements(GetUnitXElementName()) ?? new List<XElement>()) {
            Units.Add(unit.Value);
        }
    }

    public static string GetParentXElementName() {
        return "analogInputTags";
    }

    public const string GetXName = "analogInputTag";

    public XElement GetXElementRepresentation() {
        var alarmsXElement = new XElement(GetAlarmIdsXElementName());
        foreach (var alarmId in AlarmIds) {
            alarmsXElement.Add(new XElement(GetAlarmIdXElementName(), alarmId));
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