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
}