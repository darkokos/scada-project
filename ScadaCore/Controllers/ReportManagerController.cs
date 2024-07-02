using ScadaCore.Models;
using ScadaCore.Services;

namespace ScadaCore.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ReportManagerController(IReportService reportService) : ControllerBase
{
    [HttpGet("alarms-in-specific-time-period")]
    public async Task<IActionResult> GetAlarmsInSpecificTimePeriod(
        [FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
    {
        var alarms = await reportService.GetAlarmsInSpecificTimePeriod(startTime, endTime);
        return Ok(alarms);
    }

    [HttpGet("alarms-of-specific-priority")]
    public async Task<IActionResult> GetAlarmsOfSpecificPriority(
        [FromQuery] string priority) {
        if (!Enum.TryParse(priority, out AlarmPriority alarmPriority))
            return BadRequest();
        
        var alarms = await reportService.GetAlarmsOfSpecificPriority(alarmPriority);
        return Ok(alarms);
    }

    [HttpGet("tag-values-in-specific-time-period")]
    public async Task<IActionResult> GetTagLogsInSpecificTimePeriod(
        [FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
    {
        var tagValues = await reportService.GetTagLogsInSpecificTimePeriod(startTime, endTime);
        return Ok(tagValues);
    }
    
    [HttpGet("all-values-for-specific-tag")]
    public async Task<IActionResult> GetAllLogsForSpecificTag(
        [FromQuery] string tagName)
    {
        var tagValues = await reportService.GetAllLogsForSpecificTag(tagName);
        return Ok(tagValues);
    }

    [HttpGet("last-values-all-ai-tags")]
    public async Task<IActionResult> GetLastValuesAllAiTags()
    {
        var aiTagValues = await reportService.GetLastLogOfAllAiTags();
        return Ok(aiTagValues);
    }

    [HttpGet("last-values-all-di-tags")]
    public async Task<IActionResult> GetLastValuesAllDiTags()
    {
        var diTagValues = await reportService.GetLastLogOfAllDiTags();
        return Ok(diTagValues);
    }
}