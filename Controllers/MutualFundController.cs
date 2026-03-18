using Microsoft.AspNetCore.Mvc;
using Ai_Fund.Services;

namespace Ai_Fund.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MutualFundController : ControllerBase
{
    private readonly IAiOrchestratorService _aiService;

    public MutualFundController(IAiOrchestratorService aiService)
    {
        _aiService = aiService;
    }

    [HttpGet("ask")]
    public async Task<IActionResult> Ask([FromQuery] string query, [FromQuery] string userId = "anonymous")
    {
        var response = await _aiService.ProcessQueryAsync(query, userId);
        return Ok(response);
    }
}
