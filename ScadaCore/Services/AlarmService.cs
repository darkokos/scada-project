using ScadaCore.Models;
using ScadaCore.Repositories;

namespace ScadaCore.Services;

public class AlarmService : IAlarmService
{
    private IAlarmRepository alarmRepository;

    public AlarmService(IAlarmRepository alarmRepository)
    {
        this.alarmRepository = alarmRepository;
    }
    public async Task<Alarm?> GetAlarmAsync(int id) {
        return null;
    }

    public async Task<Alarm> CreateAlarmAsync(Alarm alarm)
    {
        var task = alarmRepository.CreateAlarmAsync(alarm);
        task.Wait();
        return task.Result;
    }

    public async Task<int> GetNextId()
    {
        var task = this.alarmRepository.GetNextId();
        task.Wait();
        return task.Result;
    }

    public async Task<bool> DeleteAlarmAsync(Alarm alarm) {
        return default;
    }
}