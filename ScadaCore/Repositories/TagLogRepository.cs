using ScadaCore.Data;
using ScadaCore.Models;

namespace ScadaCore.Repositories;

public class TagLogRepository(ValueAndAlarmContext valueAndAlarmContext) : ITagLogRepository {
    public async Task<TagLog?> GetTagLogAsync(int id) {
        return await valueAndAlarmContext.TagLogs.FindAsync(id);
    }
    
    public async Task<TagLog?> CreateTagLogAsync(TagLog tagLog) {
        var savedTagLog = await valueAndAlarmContext.TagLogs.AddAsync(tagLog);
        return await valueAndAlarmContext.SaveChangesAsync() >= 0 ? savedTagLog.Entity : null;
    }
}