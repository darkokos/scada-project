using System.Xml;
using System.Xml.Linq;
using ScadaCore.Models;

namespace ScadaCore.Repositories;

public class AlarmRepository : IAlarmRepository {
    private const string XmlFilePath = "~/Config/alarmConfig.xml";
    
    public AlarmRepository() {
        Directory.CreateDirectory("~/Config");
        if (File.Exists(XmlFilePath))
            return;
        
        var alarmConfigTemplate = new XElement(Alarm.GetParentXElementName());
        using var xmlWriter = XmlWriter.Create(XmlFilePath);
        alarmConfigTemplate.Save(xmlWriter);
    }
    
    private static async Task<XElement> GetRootElement() {
        using var xmlReader = XmlReader.Create(XmlFilePath, new XmlReaderSettings{ Async = true});
        return await XElement.LoadAsync(xmlReader, LoadOptions.None, CancellationToken.None);
    }

    private static XElement? GetAlarmXElement(XContainer rootElement, int id) {
        return
            rootElement
                .Elements(Alarm.GetXName())
                .FirstOrDefault(child => int.TryParse(child.Value, out var childId) && childId == id);
    }
    
    public async Task<Alarm?> GetAlarmAsync(int id) {
        var rootElement = await GetRootElement();
        var alarmXElement = GetAlarmXElement(rootElement, id);
        return alarmXElement == null ? null : new Alarm(alarmXElement);
    }

    private static async Task SaveXElementAsync(XElement xElement) {
        await using var xmlWriter = XmlWriter.Create(XmlFilePath);
        await xElement.SaveAsync(xmlWriter, CancellationToken.None);
    }

    public async Task<Alarm> CreateAlarmAsync(Alarm alarm) {
        var rootElement = await GetRootElement();

        var alarmXElement =
            new XElement(
                Alarm.GetXName(),
                alarm.Id,
                new XAttribute(Alarm.GetTypeXAttributeName(), alarm.Type),
                new XAttribute(Alarm.GetPriorityXAttributeName(), alarm.Priority),
                new XAttribute(Alarm.GetThresholdXAttributeName(), alarm.Threshold),
                new XAttribute(Alarm.GetUnitXAttributeName(), alarm.Unit)
            );
        rootElement.Add(alarmXElement);
        
        await SaveXElementAsync(rootElement);
        return new Alarm(alarmXElement);
    }

    public async Task<bool> DeleteAlarmAsync(Alarm alarm) {
        var rootElement = await GetRootElement();
        
        var alarmToRemove = GetAlarmXElement(rootElement, alarm.Id);
        if (alarmToRemove == null)
            return false;
        
        alarmToRemove.Remove();

        await SaveXElementAsync(rootElement);
        return true;
    }
}