using ScadaCore.Models;

namespace ScadaCore.Services;

public interface IAlarmLogService {
    Task<AlarmLog?> GetAlarmLogAsync(int id);
    
    Task<AlarmLog?> CreateAlarmLogAsync(AlarmLog alarmLog);
}