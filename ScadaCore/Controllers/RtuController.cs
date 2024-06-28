using Common.RealTimeUnit;
using Microsoft.AspNetCore.Mvc;

namespace ScadaCore.Controllers;

[ApiController]
[Route("[controller]")]
public class RtuController : ControllerBase {
    [HttpGet("{tagName}")]
    public async Task<IActionResult> GetTag(string tagName) {
        return StatusCode(StatusCodes.Status501NotImplemented);
    }
    
    [HttpGet("analog/input/{tagName}")]
    public async Task<IActionResult> GetAnalogInputUnitInformation(string tagName) {
        return StatusCode(StatusCodes.Status501NotImplemented);
    }
    
    [HttpGet("analog/output/{tagName}")]
    public async Task<IActionResult> GetAnalogOutputUnitInformation(string tagName) {
        return StatusCode(StatusCodes.Status501NotImplemented);
    }
    
    [HttpGet("digital/input/{tagName}")]
    public async Task<IActionResult> GetDigitalInputUnitInformation(string tagName) {
        return StatusCode(StatusCodes.Status501NotImplemented);
    }
    
    [HttpGet("digital/output/{tagName}")]
    public async Task<IActionResult> GetDigitalOutputUnitInformation(string tagName) {
        return StatusCode(StatusCodes.Status501NotImplemented);
    }
    
    [HttpPost("analog")]
    public async Task<IActionResult> SendAnalogValue([FromBody] AnalogValueDto dto) {
        return StatusCode(StatusCodes.Status501NotImplemented);
    }
    
    [HttpPost("digital")]
    public async Task<IActionResult> SendDigitalValue([FromBody] DigitalValueDto dto) {
        return StatusCode(StatusCodes.Status501NotImplemented);
    }
}