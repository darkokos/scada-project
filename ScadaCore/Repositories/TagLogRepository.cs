using Microsoft.EntityFrameworkCore;
using ScadaCore.Data;
using ScadaCore.Models;

namespace ScadaCore.Repositories;

public class TagLogRepository(ValueAndAlarmContext valueAndAlarmContext) : ITagLogRepository {
    public async Task<TagLog?> GetTagLogAsync(int id) {
        return await valueAndAlarmContext.TagLogs.FirstOrDefaultAsync(tagLog => tagLog.Id == id);
    }
    
    public async Task<TagLog?> CreateTagLogAsync(TagLog tagLog) {
        var savedTagLog = await valueAndAlarmContext.TagLogs.AddAsync(tagLog);
        return await valueAndAlarmContext.SaveChangesAsync() >= 0 ? savedTagLog.Entity : null;
    }
}