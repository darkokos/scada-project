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

    public DatabaseManagerController(ILogger<DatabaseManagerController> logger, IUserService userService)
    {
        _logger = logger;
        this.userService = userService;
    }

    [HttpPost("deleteTag")]
    public IActionResult DeleteTag([FromBody] DeleteTagDTO dto)
    {
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
}