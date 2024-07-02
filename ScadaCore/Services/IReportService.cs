using Common.ReportManagerCommon;
using AlarmPriority = ScadaCore.Models.AlarmPriority;

namespace ScadaCore.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IReportService
{
        Task<ICollection<AlarmLogDto>> GetAlarmsInSpecificTimePeriod(DateTime startTime, DateTime endTime);
        
        Task<ICollection<AlarmLogDto>> GetAlarmsOfSpecificPriority(AlarmPriority priority);
        
        Task<ICollection<TagLogDto>> GetTagLogsInSpecificTimePeriod(DateTime startTime, DateTime endTime);
        
        Task<ICollection<TagLogDto>> GetAllLogsForSpecificTag(string tagName);
        
        Task<ICollection<TagLogDto>> GetLastLogOfAllAiTags();
        
        Task<ICollection<TagLogDto>> GetLastLogOfAllDiTags();
}