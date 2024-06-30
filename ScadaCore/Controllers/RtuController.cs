using System.Net;
using Common.RealTimeUnit;
using Microsoft.AspNetCore.Mvc;
using ScadaCore.Services;

namespace ScadaCore.Controllers;

[ApiController]
[Route("[controller]")]
public class RtuController(ITagService tagService, ITagLogService tagLogService) : ControllerBase {
    [HttpGet("{tagName}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RtuInformationDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RtuInformationDto>> GetTag(string tagName) {
        var rtuInformation = await tagService.GetTagForRtuAsync(tagName);
        return rtuInformation == null ? Ok(rtuInformation) : NotFound();
    }
    
    [HttpPost("analog/input/{tagName}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AnalogInputUnitDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AnalogInputUnitDto>> GetAnalogInputUnitInformation(
        string tagName,
        [FromBody] RegisterInputUnitDto dto
    ) {
        
        // TODO: Add key to driver
        
        var serviceResponse = await tagService.GetAnalogInputTagAsync(tagName);
        return serviceResponse.StatusCode switch {
            HttpStatusCode.NotFound => NotFound(serviceResponse.ErrorMessage),
            HttpStatusCode.BadRequest => BadRequest(serviceResponse.ErrorMessage),
            HttpStatusCode.OK => Ok(serviceResponse.Body),
            _ => StatusCode(StatusCodes.Status418ImATeapot)
        };
    }
    
    [HttpGet("analog/output/{tagName}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AnalogOutputUnitDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AnalogOutputUnitDto>> GetAnalogOutputUnitInformation(string tagName) {
        var serviceResponse = await tagService.GetAnalogOutputTagAsync(tagName);
        return serviceResponse.StatusCode switch {
            HttpStatusCode.NotFound => NotFound(serviceResponse.ErrorMessage),
            HttpStatusCode.BadRequest => BadRequest(serviceResponse.ErrorMessage),
            HttpStatusCode.OK => Ok(serviceResponse.Body),
            _ => StatusCode(StatusCodes.Status418ImATeapot)
        };
    }
    
    [HttpPost("digital/input/{tagName}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DigitalInputUnitDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DigitalInputUnitDto>> GetDigitalInputUnitInformation(
        string tagName,
        [FromBody] RegisterInputUnitDto dto
    ) {
        
        // TODO: Add key to driver
        
        var serviceResponse = await tagService.GetDigitalInputTagAsync(tagName);
        return serviceResponse.StatusCode switch {
            HttpStatusCode.NotFound => NotFound(serviceResponse.ErrorMessage),
            HttpStatusCode.BadRequest => BadRequest(serviceResponse.ErrorMessage),
            HttpStatusCode.OK => Ok(serviceResponse.Body),
            _ => StatusCode(StatusCodes.Status418ImATeapot)
        };
    }
    
    [HttpGet("digital/output/{tagName}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DigitalOutputUnitDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DigitalOutputUnitDto>> GetDigitalOutputUnitInformation(string tagName) {
        var serviceResponse = await tagService.GetDigitalOutputTagAsync(tagName);
        return serviceResponse.StatusCode switch {
            HttpStatusCode.NotFound => NotFound(serviceResponse.ErrorMessage),
            HttpStatusCode.BadRequest => BadRequest(serviceResponse.ErrorMessage),
            HttpStatusCode.OK => Ok(serviceResponse.Body),
            _ => StatusCode(StatusCodes.Status418ImATeapot)
        };
    }
    
    [HttpPost("analog")]
    public async Task<IActionResult> SendAnalogValue([FromBody] AnalogValueDto dto) {
        await tagLogService.CreateAnalogTagLogAsync(dto);
        return Ok();
    }
    
    [HttpPost("digital")]
    public async Task<IActionResult> SendDigitalValue([FromBody] DigitalValueDto dto) {
        await tagLogService.CreateDigitalTagLogAsync(dto);
        return Ok();
    }
}