﻿using System.Net;
using Common.DatabaseManagerCommon;
using Microsoft.AspNetCore.Mvc;
using ScadaCore.Models;
using ScadaCore.Services;

namespace ScadaCore.Controllers;

[ApiController]
[Route("[controller]")]
public class DatabaseManagerController : ControllerBase
{
    private readonly ILogger<DatabaseManagerController> _logger;
    private IUserService userService;
    private ITagService tagService;
    private UserState userState;
    private ITagLogService TagLogService;
    private IAlarmService alarmService;

    public DatabaseManagerController(ILogger<DatabaseManagerController> logger, IUserService userService, UserState userState, ITagService tagService, ITagLogService tagLogService, IAlarmService alarmService)
    {
        _logger = logger;
        this.userState = userState;
        this.userService = userService;
        this.TagLogService = tagLogService;
        this.tagService = tagService;
        this.alarmService = alarmService;
    }

    [HttpPost("deleteTag")]
    public IActionResult DeleteTag([FromBody] DeleteTagDTO dto)
    {
        if (!userState.Data.ContainsKey(dto.username) || userState.Data[dto.username] != dto.token) return BadRequest("");
        var getTagTask = this.tagService.GetTagAsync(dto.TagName);
        getTagTask.Wait();
        var tag = getTagTask.Result;
        if (tag == null) return BadRequest("");

        var task = this.tagService.DeleteTagAsync(tag);
        task.Wait();
        
        return Ok("");
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterDTO dto)
    {
        var task = userService.GetUserAsync(dto.username);
        task.Wait();
        var existing_user = task.Result;
        if (existing_user != null) return BadRequest("User already exists!");

        User user = new User();
        user.Username = dto.username;
        user.Password = dto.password;
        task = userService.CreateUserAsync(user);
        task.Wait();
        
        return Ok("");
    }
    
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDTO dto)
    {
        var task = userService.GetUserAsync(dto.username);
        task.Wait();
        var existing_user = task.Result;

        if (existing_user == null || existing_user.Password != dto.password) return BadRequest("");
        string id = Guid.NewGuid().ToString();
        this.userState.Data[dto.username] = id;
        return Ok(id);
    }
    
    [HttpPost("logout")]
    public IActionResult Logout([FromBody] LogoutDTO dto)
    {
        if (!this.userState.Data.ContainsKey(dto.username)) return BadRequest("");
        if (this.userState.Data[dto.username] != dto.token) return BadRequest("");
        this.userState.Data.Remove(dto.username);
        return Ok("");
    }
    
    [HttpPost("changeTagScanning")]
    public async Task<IActionResult> ChangeTagScanning([FromBody] ChangeScanTagDTO dto)
    {
        if (!userState.Data.ContainsKey(dto.username) || userState.Data[dto.username] != dto.token) return BadRequest("");
        var getTagTask = this.tagService.GetTagAsync(dto.TagName);
        getTagTask.Wait();
        var tag = getTagTask.Result;
        if (tag == null) return BadRequest("");
        if (tag.GetType() != typeof(AnalogInputTag) && tag.GetType() != typeof(DigitalInputTag)) return BadRequest("");
        
        var inputTag = (InputTag)tag;
        inputTag.IsScanned = dto.state;

        var deleteTask = tagService.DeleteTagAsync(tag);
        deleteTask.Wait();
        var serviceResponse = await tagService.CreateTagAsync(tag);
        return serviceResponse.StatusCode switch {
            HttpStatusCode.BadRequest => BadRequest(serviceResponse.ErrorMessage),
            HttpStatusCode.InternalServerError => NotFound(serviceResponse.ErrorMessage),
            HttpStatusCode.OK => Ok(serviceResponse.Body),
            _ => StatusCode(StatusCodes.Status418ImATeapot)
        };
    }
    
    [HttpPost("currentTagValues")]
    public IActionResult currentTagValues([FromBody] ShowCurrentTagValuesDTO dto)
    {
        if (!userState.Data.ContainsKey(dto.username) || userState.Data[dto.username] != dto.token) return BadRequest("");
        var getTags = tagService.GetAllOutputTags();
        getTags.Wait();
        var tags = getTags.Result;
        String res = "";
        foreach (Tag tag in tags)
        {
            res += tag.Name;
            res += " - ";
            if (tag.GetType() == typeof(DigitalOutputTag)) res += ((DigitalOutputTag)tag).InitialValue;
            else res += ((AnalogOutputTag)tag).InitialValue;
            res += "\n";
        }
        return Ok(res);
    }

    [HttpPost("addTag")]
    public async Task<IActionResult> addTag([FromBody] AddTagDTO dto)
    {
        if (!userState.Data.ContainsKey(dto.username) || userState.Data[dto.username] != dto.token) return BadRequest("");
        Tag tag = null;
        if (dto.analogInput != null)
        {
            AddAnalogInputTag addTag = dto.analogInput;
            tag = new AnalogInputTag(addTag.Name, addTag.Description, addTag.InputOutputAddress, addTag.IsSimulated, addTag.ScanTime, addTag.IsScanned, addTag.LowLimit, addTag.HighLimit, addTag.Unit);
        } 
        else if (dto.analogOutput != null)
        {
            AddAnalogOutputTag addTag = dto.analogOutput;
            tag = new AnalogOutputTag(addTag.Name, addTag.Description, addTag.InputOutputAddress, addTag.InitialValue,
                addTag.LowLimit, addTag.HighLimit, addTag.Unit);
        } 
        else if (dto.digitalInput != null)
        {
            AddDigitalInputTag addTag = dto.digitalInput;
            tag = new DigitalInputTag(addTag.Name, addTag.Description, addTag.InputOutputAddress, addTag.IsSimulated,
                addTag.ScanTime, addTag.IsScanned);
        }
        else if (dto.digitalOutput != null)
        {
            AddDigitalOutputTag addTag = dto.digitalOutput;
            tag = new DigitalOutputTag(addTag.Name, addTag.Description, addTag.InputOutputAddress, addTag.InitialValue);
        }

        var serviceResponse = await tagService.CreateTagAsync(tag);
        return serviceResponse.StatusCode switch {
            HttpStatusCode.BadRequest => BadRequest(serviceResponse.ErrorMessage),
            HttpStatusCode.InternalServerError => NotFound(serviceResponse.ErrorMessage),
            HttpStatusCode.OK => Ok(serviceResponse.Body),
            _ => StatusCode(StatusCodes.Status418ImATeapot)
        };
    }
    
    [HttpPost("addAlarm")]
    public async Task<IActionResult> addAlarm([FromBody] AddAlarmDTO dto)
    {
        if (!userState.Data.ContainsKey(dto.username) || userState.Data[dto.username] != dto.token) return BadRequest("");
        var getTagTask = this.tagService.GetTagAsync(dto.TagName);
        getTagTask.Wait();
        var tag = getTagTask.Result;
        if (tag == null) return BadRequest("");
        if (tag.GetType() != typeof(AnalogInputTag)) return BadRequest("");

        var newAlarm = new Alarm((ScadaCore.Models.AlarmType)((int)dto.Type), (ScadaCore.Models.AlarmPriority)((int)dto.Priority), dto.Threshold, dto.Unit);
        var idTask = alarmService.GetNextId();
        idTask.Wait();
        newAlarm.Id = idTask.Result;
        var createAlarmTask = alarmService.CreateAlarmAsync(newAlarm);
        createAlarmTask.Wait();
        var alarm = createAlarmTask.Result;

        var analogTag = (AnalogInputTag) tag;
        analogTag.AlarmIds.Add(alarm.Id);

        this.tagService.DeleteTagAsync(tag).Wait();
        var serviceResponse = await tagService.CreateTagAsync(analogTag);
        return serviceResponse.StatusCode switch {
            HttpStatusCode.BadRequest => BadRequest(serviceResponse.ErrorMessage),
            HttpStatusCode.InternalServerError => NotFound(serviceResponse.ErrorMessage),
            HttpStatusCode.OK => Ok(serviceResponse.Body),
            _ => StatusCode(StatusCodes.Status418ImATeapot)
        };
    }
    
    [HttpPost("writeTagValue")]
    public async Task<IActionResult> writeTagValue([FromBody] WriteTagValueDTO dto)
    {
        if (!userState.Data.ContainsKey(dto.username) || userState.Data[dto.username] != dto.token) return BadRequest("");
        var getTagTask = this.tagService.GetTagAsync(dto.TagName);
        getTagTask.Wait();
        var tag = getTagTask.Result;
        if (tag.GetType() == typeof(DigitalOutputTag) && dto.boolValue != null)
        {
            var outputTag = (DigitalOutputTag)tag;
            outputTag.InitialValue = dto.boolValue ?? false;
            tagService.DeleteTagAsync(tag).Wait();
            var serviceResponse = await tagService.CreateTagAsync(tag);
            return serviceResponse.StatusCode switch {
                HttpStatusCode.BadRequest => BadRequest(serviceResponse.ErrorMessage),
                HttpStatusCode.InternalServerError => NotFound(serviceResponse.ErrorMessage),
                HttpStatusCode.OK => Ok(serviceResponse.Body),
                _ => StatusCode(StatusCodes.Status418ImATeapot)
            };

        } else if (tag.GetType() == typeof(AnalogOutputTag) && dto.decimalValue != null)
        {
            var outputTag = (AnalogOutputTag)tag;
            outputTag.InitialValue = dto.decimalValue ?? 0;
            tagService.DeleteTagAsync(tag).Wait();
            var serviceResponse = await tagService.CreateTagAsync(tag);
            return serviceResponse.StatusCode switch {
                HttpStatusCode.BadRequest => BadRequest(serviceResponse.ErrorMessage),
                HttpStatusCode.InternalServerError => NotFound(serviceResponse.ErrorMessage),
                HttpStatusCode.OK => Ok(serviceResponse.Body),
                _ => StatusCode(StatusCodes.Status418ImATeapot)
            };
        }
        else return BadRequest("");
    }
}