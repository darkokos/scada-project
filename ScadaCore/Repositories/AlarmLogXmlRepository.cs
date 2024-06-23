using System.Xml;
using System.Xml.Linq;
using ScadaCore.Models;

namespace ScadaCore.Repositories;

public class AlarmLogXmlRepository : IAlarmLogRepository {
    private const string XmlFilePath = "~/Config/alarmConfig.xml";

    public AlarmLogXmlRepository() {
        Directory.CreateDirectory("~/Config");
        using var streamWriter = File.AppendText(XmlFilePath);
    }

    private static async Task<XElement> GetRootElement() {
        using var xmlReader = XmlReader.Create(XmlFilePath);
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
                alarmLog.Id.ToString(),
                new XAttribute("alarmId", alarmLog.AlarmId),
                new XAttribute("type", alarmLog.Type),
                new XAttribute("valueName", alarmLog.ValueName),
                new XAttribute("timestamp", alarmLog.Timestamp.ToString("dd/MM/yyyy"))
                );
        rootElement
            .Add(alarmLogXElement);

        await using var xmlWriter = XmlWriter.Create(XmlFilePath);
        await rootElement.SaveAsync(xmlWriter, CancellationToken.None);
        return new AlarmLog(alarmLogXElement);
    }
}