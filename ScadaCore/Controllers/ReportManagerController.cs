using ScadaCore.Models;
using ScadaCore.Services;

namespace ScadaCore.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ReportManagerController : ControllerBase
{
    private readonly ILogger<ReportManagerController> _logger;
        private readonly IReportService _reportService;

    public ReportManagerController(ILogger<ReportManagerController> logger, IReportService reportService)
    {
        _logger = logger;
        _reportService = reportService;
    }

    [HttpGet("alarms-in-specific-time-period")]
    public async Task<IActionResult> GetAlarmsInSpecificTimePeriod(
        [FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
    {
        var alarms = await _reportService.GetAlarmsInSpecificTimePeriod(startTime, endTime);
        return Ok(alarms);
    }

    [HttpGet("alarms-of-specific-priority")]
    public async Task<IActionResult> GetAlarmsOfSpecificPriority(
        [FromQuery] string priority)
    {
        Enum.TryParse(priority, out AlarmPriority alarmPriority);
        
        var alarms = await _reportService.GetAlarmsOfSpecificPriority(alarmPriority);
        return Ok(alarms);
    }

    [HttpGet("tag-values-in-specific-time-period")]
    public async Task<IActionResult> GetTagLogsInSpecificTimePeriod(
        [FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
    {
        var tagValues = await _reportService.GetTagLogsInSpecificTimePeriod(startTime, endTime);
        return Ok(tagValues);
    }
    
    [HttpGet("all-values-for-specific-tag")]
    public async Task<IActionResult> GetAllLogsForSpecificTag(
        [FromQuery] string tagName)
    {
        var tagValues = await _reportService.GetAllLogsForSpecificTag(tagName);
        return Ok(tagValues);
    }

    [HttpGet("last-values-all-ai-tags")]
    public async Task<IActionResult> GetLastValuesAllAITags()
    {
        var aiTagValues = await _reportService.GetLastLogOfAllAITags();
        return Ok(aiTagValues);
    }

    [HttpGet("last-values-all-di-tags")]
    public async Task<IActionResult> GetLastValuesAllDITags()
    {
        var diTagValues = await _reportService.GetLastLogOfAllDITags();
        return Ok(diTagValues);
    }
}