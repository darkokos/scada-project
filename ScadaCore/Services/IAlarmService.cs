using ScadaCore.Models;

namespace ScadaCore.Services;

public interface IAlarmService {
    Task<Alarm?> GetAlarmAsync(int id);
    
    Task<Alarm> CreateAlarmAsync(Alarm alarm);

    Task<bool> DeleteAlarmAsync(Alarm alarm);
}