using Microsoft.EntityFrameworkCore;
using ScadaCore.Data;
using ScadaCore.Models;

namespace ScadaCore.Repositories;

public class TagLogRepository(ValueAndAlarmContext valueAndAlarmContext) : ITagLogRepository {
    public async Task<TagLog?> GetTagLogAsync(int id) {
        var tagLog = await valueAndAlarmContext.TagLogs.FindAsync(id);
        if (tagLog == null)
            return null;

        return tagLog switch {
            AnalogTagLog analogTagLog => analogTagLog,
            DigitalTagLog digitalTagLog => digitalTagLog,
            _ => null
        };
    }
    
    public async Task<TagLog?> CreateTagLogAsync(TagLog tagLog) {
        TagLog? savedTagLog = tagLog switch {
            AnalogTagLog analogTagLog => (await valueAndAlarmContext.AnalogTagLogs.AddAsync(analogTagLog)).Entity,
            DigitalTagLog digitalTagLog => (await valueAndAlarmContext.DigitalTagLogs.AddAsync(digitalTagLog)).Entity,
            _ => null
        };
        if (savedTagLog == null)
            return null;

        return await valueAndAlarmContext.SaveChangesAsync() >= 0 ? savedTagLog switch {
            AnalogTagLog analogTagLog => analogTagLog,
            DigitalTagLog digitalTagLog => digitalTagLog,
            _ => null
        } : null;
    }
    
    public async Task<TagLog?> GetLatestLog(String tagName)
    {
        var logs = valueAndAlarmContext.TagLogs.Where(log => log.TagName == tagName);
            return logs
            .OrderByDescending(item => item.Timestamp)
            .FirstOrDefault();
    }
    
    public async Task<ICollection<TagLog>> GetAllTagLogsAsync()
    {
        var fetchedLogs = await valueAndAlarmContext.TagLogs.ToListAsync();
        var returnLogs = new List<TagLog>();

        fetchedLogs.ForEach(log => {
            if (log.GetType() == typeof(AnalogTagLog))
                returnLogs.Add((AnalogTagLog) log);
            else
                returnLogs.Add((DigitalTagLog) log);
        });
        return returnLogs;
    }
    
    public async Task<ICollection<TagLog>> GetAllAnalogTagLogsAsync()
    {
        var fetchedLogs = await valueAndAlarmContext.TagLogs.ToListAsync();
        var returnLogs = new List<TagLog>();

        fetchedLogs.ForEach(log => {
            if (log.GetType() == typeof(AnalogTagLog))
                returnLogs.Add((AnalogTagLog) log);
        });
        return returnLogs;
    }
    
    public async Task<ICollection<TagLog>> GetAllDigitalTagLogsAsync()
    {
        var fetchedLogs = await valueAndAlarmContext.TagLogs.ToListAsync();
        var returnLogs = new List<TagLog>();

        fetchedLogs.ForEach(log => {
            if (log.GetType() == typeof(DigitalTagLog))
                returnLogs.Add((DigitalTagLog) log);
        });
        return returnLogs;
    }
}