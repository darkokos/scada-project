using Microsoft.EntityFrameworkCore;
using ScadaCore.Data;
using ScadaCore.Models;

namespace ScadaCore.Repositories;
public class AlarmLogDbRepository : IAlarmLogRepository
{
    private readonly ValueAndAlarmContext _valueAndAlarmContext;

    public AlarmLogDbRepository(ValueAndAlarmContext valueAndAlarmContext)
    {
        _valueAndAlarmContext = valueAndAlarmContext;
    }

    public async Task<AlarmLog?> GetAlarmLogAsync(int id)
    {
        return await _valueAndAlarmContext.AlarmLogs.FindAsync(id);
    }

    public async Task<AlarmLog?> CreateAlarmLogAsync(AlarmLog alarmLog)
    {
        var savedAlarmLog = await _valueAndAlarmContext.AlarmLogs.AddAsync(alarmLog);
        await _valueAndAlarmContext.SaveChangesAsync();
        return savedAlarmLog.Entity;
    }
    public async Task<ICollection<AlarmLog>> GetAllAlarmLogsAsync()
    {
        return await _valueAndAlarmContext.AlarmLogs.ToListAsync();
    }
}