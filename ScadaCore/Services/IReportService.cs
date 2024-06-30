using ScadaCore.Models;

namespace ScadaCore.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IReportService
{
        Task<IEnumerable<AlarmLog>> GetAlarmsInSpecificTimePeriod(DateTime startTime, DateTime endTime);
        Task<IEnumerable<AlarmLog>> GetAlarmsOfSpecificPriority(AlarmPriority priority);
        Task<IEnumerable<TagLog>> GetTagLogsInSpecificTimePeriod(DateTime startTime, DateTime endTime);
        Task<IEnumerable<TagLog>> GetAllLogsForSpecificTag(string tagName);
        
        Task<IEnumerable<TagLog>> GetLastLogOfAllAITags();
        Task<IEnumerable<TagLog>> GetLastLogOfAllDITags();
}