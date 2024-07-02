using ScadaCore.Models;

namespace ScadaCore.Repositories;

public interface IAlarmLogRepository {
    Task<AlarmLog?> GetAlarmLogAsync(int id);
    
    Task<AlarmLog?> CreateAlarmLogAsync(AlarmLog alarmLog);

    Task<ICollection<AlarmLog>> GetAllAlarmLogsAsync();
}