using ScadaCore.Models;

namespace ScadaCore.Repositories;

public interface IAlarmRepository {
    Task<Alarm?> GetAlarmAsync(int id);
    
    Task<Alarm> CreateAlarmAsync(Alarm alarm);

    Task<bool> DeleteAlarmAsync(Alarm alarm);
}