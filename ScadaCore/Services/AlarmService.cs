using ScadaCore.Models;

namespace ScadaCore.Services;

public class AlarmService : IAlarmService {
    public async Task<Alarm?> GetAlarmAsync(int id) {
        return null;
    }

    public async Task<Alarm> CreateAlarmAsync(Alarm alarm) {
        return new Alarm();
    }

    public async Task<bool> DeleteAlarmAsync(Alarm alarm) {
        return default;
    }
}