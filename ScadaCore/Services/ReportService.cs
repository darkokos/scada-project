using ScadaCore.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScadaCore.Models;
using ScadaCore.Repositories;

namespace ScadaCore.Services;

public class ReportService : IReportService
{
    private readonly IAlarmLogRepository _alarmLogRepository;
    private readonly ITagLogRepository _tagLogRepository;
    private readonly ITagRepository _tagRepository;

    public ReportService(IAlarmLogRepository alarmLogRepository, ITagLogRepository tagLogRepository, ITagRepository tagRepository)
    {
        _alarmLogRepository = alarmLogRepository;
        _tagLogRepository = tagLogRepository;
        _tagRepository = tagRepository;
    }

    public async Task<IEnumerable<AlarmLog>> GetAlarmsInSpecificTimePeriod(DateTime startTime, DateTime endTime)
    {
        var alarms = await _alarmLogRepository.GetAllAlarmLogsAsync();
        return alarms
            .Where(a => a.Timestamp >= startTime && a.Timestamp <= endTime)
            .OrderBy(a => a.Priority)
            .ThenBy(a => a.Timestamp);
    }

    public async Task<IEnumerable<AlarmLog>> GetAlarmsOfSpecificPriority(AlarmPriority priority)
    {
        var alarms = await _alarmLogRepository.GetAllAlarmLogsAsync();
        return alarms
            .Where(a => a.Priority == priority)
            .OrderBy(a => a.Timestamp);
    }

    public async Task<IEnumerable<TagLog>> GetTagLogsInSpecificTimePeriod(DateTime startTime, DateTime endTime)
    {
        var tagLogs = await _tagLogRepository.GetAllTagLogsAsync();
        return tagLogs
            .Where(t => t.Timestamp >= startTime && t.Timestamp <= endTime)
            .OrderBy(t => t.Timestamp);
    }

    public async Task<IEnumerable<TagLog>> GetLastLogOfAllAITags()
    {
        var tagLogs = await _tagLogRepository.GetAllTagLogsAsync();
        return tagLogs
            .Where(t => _tagRepository.GetTagAsync(t.TagName).GetType() == typeof(AnalogInputTag))
            .GroupBy(t => t.TagName)
            .Select(g => g.OrderByDescending(t => t.Timestamp).First())
            .OrderBy(t => t.Timestamp);
    }

    public async Task<IEnumerable<TagLog>> GetLastLogOfAllDITags()
    {
        var tagLogs = await _tagLogRepository.GetAllTagLogsAsync();
        return tagLogs
            .Where(t => _tagRepository.GetTagAsync(t.TagName).GetType() == typeof(DigitalInputTag))
            .GroupBy(t => t.TagName)
            .Select(g => g.OrderByDescending(t => t.Timestamp).First())
            .OrderBy(t => t.Timestamp);
    }
    
    public async Task<IEnumerable<TagLog>> GetAllLogsForSpecificTag(string tagName)
    {
        var tagLogs = await _tagLogRepository.GetAllTagLogsAsync();
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
            });
    }
}
