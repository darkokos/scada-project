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

    public DatabaseManagerController(ILogger<DatabaseManagerController> logger, IUserService userService, UserState userState, ITagService tagService)
    {
        _logger = logger;
        this.userState = userState;
        this.userService = userService;
        this.tagService = tagService;
    }

    [HttpPost("deleteTag")]
    public IActionResult DeleteTag([FromBody] DeleteTagDTO dto)
    {
        if (userState.Data.ContainsKey(dto.username) || userState.Data[dto.username] != dto.token) return BadRequest("");
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
}