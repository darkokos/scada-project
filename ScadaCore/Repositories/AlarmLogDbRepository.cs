using ScadaCore.Data;
using ScadaCore.Models;

namespace ScadaCore.Repositories;

public class AlarmLogDbRepository(ValueAndAlarmContext valueAndAlarmContext) : IAlarmLogRepository {
    public async Task<AlarmLog?> GetAlarmLogAsync(int id) {
        return await valueAndAlarmContext.AlarmLogs.FindAsync(id);
    }

    public async Task<AlarmLog?> CreateAlarmLogAsync(AlarmLog alarmLog) {
        var savedAlarmLog = await valueAndAlarmContext.AlarmLogs.AddAsync(alarmLog);
        return await valueAndAlarmContext.SaveChangesAsync() >= 0 ? savedAlarmLog.Entity : null;
    }
}