using System.Xml.Linq;
using Common.DatabaseManagerCommon;
using Lombok.NET;

namespace ScadaCore.Models;

[NoArgsConstructor]
public partial class AnalogInputTag : InputTag {
    public HashSet<int> AlarmIds { get; set; }
    public decimal LowLimit { get; set; }
    public decimal HighLimit { get; set; }
    public string Unit { get; set; }

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
        
        LowLimit = decimal.TryParse(
            analogInputTagXElement.Element(GetLowLimitXElementName())?.Value,
            out var lowLimit
        ) ? lowLimit : -1;
        HighLimit = decimal.TryParse(
            analogInputTagXElement.Element(GetHighLimitXElementName())?.Value,
            out var highLimit
        ) ? highLimit : -1;
        Unit = analogInputTagXElement.Element(GetUnitXElementName())?.Value ?? "";
    }

    public AnalogInputTag(string name, string description, int inputOutputAddress, bool isSimulated, TimeSpan scanTime, bool isScanned, decimal lowLimit, decimal highLimit, string unit) : base(name, description,
        inputOutputAddress, isSimulated, scanTime, isScanned)
    {
        LowLimit = lowLimit;
        HighLimit = highLimit;
        Unit = unit;
        AlarmIds = new HashSet<int>();
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
        var xElementRepresentation = new XElement(
            GetXName,
            alarmsXElement,
            new XElement(GetLowLimitXElementName(), LowLimit),
            new XElement(GetHighLimitXElementName(), HighLimit),
            new XElement(GetUnitXElementName(), Unit)
        );
        
        SetXAttributes(xElementRepresentation);
        return xElementRepresentation;
    }
}