using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MSEstatesAppLibrary.DataAccess;
using MSEstatesAppLibrary.Models;

namespace MSEstatesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AgentsController : ControllerBase
{
    private readonly IAgentData _agentData;
    private readonly ILogger<AgentsController> _logger;

    public AgentsController(IAgentData agentData, ILogger<AgentsController> logger)
    {
        _agentData = agentData;
        _logger = logger;
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    [ResponseCache(Duration = 7 * 24 * 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<ActionResult<AgentModel>> Get(string id)
    {
        try
        {
            var agent = await _agentData.GetAgentById(id);
            if (agent == null)
            {
                _logger.LogWarning("Agent not found: {Id}", id);
                return NotFound();
            }

            return Ok(agent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving agent");
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpPost]
    public async Task<ActionResult> Update([FromBody] AgentModel agent)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for AgentModel");
            return BadRequest(ModelState);
        }

        try
        {
            await _agentData.CreateAgent(agent);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating agent");
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize]
    [RequiredScope("Files.ReadWrite")]
    [HttpPut("{id}")]
    public async Task<ActionResult> Put(string id, [FromBody] AgentModel updatedAgent)
    {
        if (!ModelState.IsValid || string.IsNullOrEmpty(id))
        {
            _logger.LogWarning("Invalid input for AgentModel");
            return BadRequest("Invalid input");
        }

        try
        {
            await _agentData.UpdateAgent(id, updatedAgent);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating agent");
            return StatusCode(500, "Internal server error");
        }
    }
}