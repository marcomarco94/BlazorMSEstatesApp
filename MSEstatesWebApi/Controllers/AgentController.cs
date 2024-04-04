using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using MSEstatesAppLibrary.DataAccess;
using MSEstatesAppLibrary.Models;

namespace MSEstatesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AgentController : Controller
{
    private readonly IAgentData _agentData;

    public AgentController(IAgentData agentData)
    {
        _agentData = agentData;
    }

    [Authorize(Roles = "Task.Write")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    [HttpPost]
    [Route("CreateAgent")]
    public async Task CreateAgent(AgentModel agent)
    {
        if (ModelState.IsValid) await _agentData.CreateAgent(agent);
    }

    [Authorize(Roles = "Task.Write")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    [HttpPatch]
    [Route("UpdateAgent/{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] AgentModel? updatedAgent)
    {
        if (updatedAgent == null || string.IsNullOrEmpty(id)) return BadRequest();

        await _agentData.UpdateAgent(id, updatedAgent);
        return NoContent();
    }

    [HttpGet("{id}", Name = "GetAgent")]
    [ResponseCache(Duration = 7 * 24 * 60 * 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<ActionResult<AgentModel>> Get(string id)
    {
        var agent = await _agentData.GetAgentById(id);
        return agent;
    }
}