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

    public DatabaseManagerController(ILogger<DatabaseManagerController> logger, IUserService userService, UserState userState, ITagService tagService, ITagLogService tagLogService)
    {
        _logger = logger;
        this.userState = userState;
        this.userService = userService;
        this.TagLogService = tagLogService;
        this.tagService = tagService;
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
        if (existing_user == null) _logger.LogInformation("ker");

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
    public IActionResult ChangeTagScanning([FromBody] ChangeScanTagDTO dto)
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
        var createTask = tagService.CreateTagAsync(tag);
        
        return Ok("");
    }
    
    [HttpPost("currentTagValues")]
    public IActionResult currentTagValues([FromBody] ShowCurrentTagValuesDTO dto)
    {
        if (!userState.Data.ContainsKey(dto.username) || userState.Data[dto.username] != dto.token) return BadRequest("");
        var getTags = tagService.GetAllInputTags();
        getTags.Wait();
        var tags = getTags.Result;
        _logger.LogInformation(tags.Count.ToString());
        String res = "";
        foreach (String tag in tags)
        {
            _logger.LogInformation(tag);
            res += tag;
            res += " - ";
            
            var task = TagLogService.GetLatestLog(tag);
            task.Wait();
            var log = task.Result;
            if (log == null) res += "/";
            else res += log.EmittedValue;
            
            res += "\n";
        }
        return Ok(res);
    }

    [HttpPost("addTag")]
    public IActionResult addTag([FromBody] AddTagDTO dto)
    {
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

        var task = tagService.CreateTagAsync(tag);
        task.Wait();

        return Ok("");
    }
}