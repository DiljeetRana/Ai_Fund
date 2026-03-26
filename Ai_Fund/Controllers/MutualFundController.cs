using Microsoft.AspNetCore.Mvc;
using Ai_Fund.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Ai_Fund.Controllers;

[Authorize]
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
    public async Task<IActionResult> Ask([FromQuery] string query)
    {
        var userId = User.FindFirst("id")?.Value ?? User.FindFirst(ClaimTypes.Name)?.Value ?? "guest";

        var response = await _aiService.ProcessQueryAsync(query, userId);
        return Ok(response);
    }
}

