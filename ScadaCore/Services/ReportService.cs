using AutoMapper;
using Common.ReportManagerCommon;
using ScadaCore.Models;
using ScadaCore.Repositories;
using AlarmPriority = ScadaCore.Models.AlarmPriority;

namespace ScadaCore.Services;

public class ReportService(
    IAlarmLogRepository alarmLogRepository,
    ITagLogRepository tagLogRepository,
    IMapper mapper
) : IReportService
{
    public async Task<ICollection<AlarmLogDto>> GetAlarmsInSpecificTimePeriod(DateTime startTime, DateTime endTime)
    {
        var alarms = await alarmLogRepository.GetAllAlarmLogsAsync();
        return alarms
            .Where(a => a.Timestamp >= startTime && a.Timestamp <= endTime)
            .OrderBy(a => a.Priority)
            .ThenBy(a => a.Timestamp)
            .Select(mapper.Map<AlarmLogDto>)
            .ToList();
    }

    public async Task<ICollection<AlarmLogDto>> GetAlarmsOfSpecificPriority(AlarmPriority priority)
    {
        var alarms = await alarmLogRepository.GetAllAlarmLogsAsync();
        return alarms
            .Where(a => a.Priority == priority)
            .OrderBy(a => a.Timestamp)
            .Select(mapper.Map<AlarmLogDto>)
            .ToList();
    }

    public async Task<ICollection<TagLogDto>> GetTagLogsInSpecificTimePeriod(DateTime startTime, DateTime endTime)
    {
        var tagLogs = await tagLogRepository.GetAllTagLogsAsync();
        return tagLogs
            .Where(t => t.Timestamp >= startTime && t.Timestamp <= endTime)
            .OrderBy(t => t.Timestamp)
            .Select(mapper.Map<TagLogDto>)
            .ToList();
    }

    public async Task<ICollection<TagLogDto>> GetLastLogOfAllAiTags()
    {
        var tagLogs = await tagLogRepository.GetAllAnalogTagLogsAsync();
        return tagLogs
            .GroupBy(t => t.TagName)
            .Select(g => g.OrderByDescending(t => t.Timestamp).First())
            .OrderBy(t => t.Timestamp)
            .Select(mapper.Map<TagLogDto>)
            .ToList();
    }

    public async Task<ICollection<TagLogDto>> GetLastLogOfAllDiTags()
    {
        var tagLogs = await tagLogRepository.GetAllDigitalTagLogsAsync();
        return tagLogs
            .GroupBy(t => t.TagName)
            .Select(g => g.OrderByDescending(t => t.Timestamp).First())
            .OrderBy(t => t.Timestamp)
            .Select(mapper.Map<TagLogDto>)
            .ToList();
    }
    
    public async Task<ICollection<TagLogDto>> GetAllLogsForSpecificTag(string tagName)
    {
        var tagLogs = await tagLogRepository.GetAllTagLogsAsync();
        return tagLogs
            .Where(t => t.TagName == tagName)
            .OrderBy(t =>
            {
                switch (t)
                {
                    case AnalogTagLog analogTagLog:
                        return analogTagLog.EmittedValue;
                    case DigitalTagLog digitalTagLog:
                        return digitalTagLog.EmittedValue ? 1m : 0m;
                    default:
                        return decimal.MinusOne;
                }
            })
            .Select(mapper.Map<TagLogDto>)
            .ToList();
    }
}
