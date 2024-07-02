using ScadaCore.Models;
using ScadaCore.Repositories;

namespace ScadaCore.Services;

public class AlarmLogService(IEnumerable<IAlarmLogRepository> alarmLogRepositories) : IAlarmLogService {
    public async Task<AlarmLog?> GetAlarmLogAsync(int id) {
        return null;
    }

    public async Task<AlarmLog?> CreateAlarmLogAsync(AlarmLog alarmLog) {
        AlarmLog? persistedAlarmLog = null;
        foreach (var repository in alarmLogRepositories) {
            persistedAlarmLog = await repository.CreateAlarmLogAsync(alarmLog);
            if (persistedAlarmLog == null)
                return null;
        }

        return persistedAlarmLog;
    }
}