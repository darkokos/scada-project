using System.Xml;
using System.Xml.Linq;
using ScadaCore.Models;

namespace ScadaCore.Repositories;

public class AlarmLogXmlRepository : IAlarmLogRepository {
    private const string XmlFilePath = "Logs/alarmLog.xml";

    public AlarmLogXmlRepository() {
        Directory.CreateDirectory("Logs");
        if (File.Exists(XmlFilePath))
            return;
        
        var alarmLogTemplate = new XElement(AlarmLog.GetParentXElementName());
        using var xmlWriter = XmlWriter.Create(XmlFilePath);
        alarmLogTemplate.Save(xmlWriter);
    }

    private static async Task<XElement> GetRootElement() {
        using var xmlReader = XmlReader.Create(XmlFilePath, new XmlReaderSettings{ Async = true});
        return await XElement.LoadAsync(xmlReader, LoadOptions.None, CancellationToken.None);
    }
    
    public async Task<AlarmLog?> GetAlarmLogAsync(int id) {
        var rootElement = await GetRootElement();

        var alarmLogXElement =
            rootElement
                .Elements()
                .FirstOrDefault(child => int.TryParse(child.Value, out var childId) && childId == id);
        
        return alarmLogXElement == null ? null : new AlarmLog(alarmLogXElement);
    }
    
    public async Task<AlarmLog?> CreateAlarmLogAsync(AlarmLog alarmLog) {
        var rootElement = await GetRootElement();

        var alarmLogXElement =
            new XElement(
                AlarmLog.GetXName(),
                alarmLog.Id,
                new XAttribute(AlarmLog.GetAlarmIdXAttributeName(), alarmLog.AlarmId),
                new XAttribute(AlarmLog.GetTypeXAttributeName(), alarmLog.Type),
                new XAttribute(AlarmLog.GetPriorityXAttributeName(), alarmLog.Priority),
                new XAttribute(AlarmLog.GetUnitXAttributeName(), alarmLog.Unit),
                new XAttribute(AlarmLog.GetTimestampXAttributeName(), alarmLog.Timestamp.ToString("dd/MM/yyyy"))
            );
        rootElement
            .Add(alarmLogXElement);

        await using var xmlWriter = XmlWriter.Create(XmlFilePath, new XmlWriterSettings{Async = true});
        await rootElement.SaveAsync(xmlWriter, CancellationToken.None);
        return new AlarmLog(alarmLogXElement);
    }

    public async Task<IEnumerable<AlarmLog>> GetAllAlarmLogsAsync()
    {
        return null;
    }
}