using System.Net;
using Common.RealTimeUnit;
using Microsoft.AspNetCore.Mvc;
using ScadaCore.Drivers;
using ScadaCore.Services;

namespace ScadaCore.Controllers;

[ApiController]
[Route("[controller]")]
public class RtuController(
    ITagService tagService,
    IAnalogRealTimeDriver analogRealTimeDriver,
    IDigitalRealTimeDriver digitalRealTimeDriver
) : ControllerBase {
    [HttpGet("{tagName}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RtuInformationDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RtuInformationDto>> GetTag(string tagName) {
        var rtuInformation = await tagService.GetTagForRtuAsync(tagName);
        return rtuInformation == null ? NotFound() : Ok(rtuInformation) ;
    }
    
    [HttpPost("analog/input/{tagName}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AnalogInputUnitDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AnalogInputUnitDto>> GetAnalogInputUnitInformation(
        string tagName,
        [FromBody] RegisterInputUnitDto dto
    ) {
        var serviceResponse = await tagService.GetAnalogInputTagAsync(tagName, dto);
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
        var serviceResponse = await tagService.GetDigitalInputTagAsync(tagName, dto);
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DigitalOutputUnitDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AnalogValueDto>> SendAnalogValue([FromBody] AnalogValueDto dto) {
        var tag = await tagService.GetTagAsync(dto.TagName);
        if (tag == null)
            return NotFound("The tag for this unit was not found.");

        var serviceResponse = analogRealTimeDriver.Write(tag.InputOutputAddress, dto);
        return serviceResponse.StatusCode switch {
            HttpStatusCode.BadRequest => BadRequest(serviceResponse.ErrorMessage),
            HttpStatusCode.OK => Ok(serviceResponse.Body),
            _ => StatusCode(StatusCodes.Status418ImATeapot)
        };
    }
    
    [HttpPost("digital")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DigitalOutputUnitDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AnalogValueDto>> SendDigitalValue([FromBody] DigitalValueDto dto) {
        var tag = await tagService.GetTagAsync(dto.TagName);
        if (tag == null)
            return NotFound("The tag for this unit was not found.");
        
        var serviceResponse = digitalRealTimeDriver.Write(tag.InputOutputAddress, dto);
        return serviceResponse.StatusCode switch {
            HttpStatusCode.BadRequest => BadRequest(serviceResponse.ErrorMessage),
            HttpStatusCode.OK => Ok(serviceResponse.Body),
            _ => StatusCode(StatusCodes.Status418ImATeapot)
        };
    }
}