using Common.DatabaseManagerCommon;
using Microsoft.AspNetCore.Mvc;

namespace ScadaCore.Controllers;

[ApiController]
[Route("[controller]")]
public class DatabaseManagerController : ControllerBase
{
    private readonly ILogger<DatabaseManagerController> _logger;

    public DatabaseManagerController(ILogger<DatabaseManagerController> logger)
    {
        _logger = logger;
    }

    [HttpPost("deleteTag")]
    public IActionResult DeleteTag([FromBody] DeleteTagDTO dto)
    {
        return Ok("");
    }
}