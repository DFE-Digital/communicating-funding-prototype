using System.Text.Json;
using CommunicationsAlpha2025.Data;
using CommunicationsAlpha2025.Versions.V2.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommunicationsAlpha2025.Versions.V2.Controllers;

[ApiController]
[Route("api/v2/[controller]")]
[Produces("application/json")]
public class StatementController(IStaticDataProvider staticDataProvider) : ControllerBase
{
    /// <summary>
    /// Provides statements for a given funding stream ID.
    /// </summary>
    [HttpGet("{fundingStreamId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Statement> Get(string fundingStreamId)
    {
        JsonElement publishedProviderFundingStream 
            = staticDataProvider.GetPublishedProviderFundingStreamById(fundingStreamId);
        Statement statement = Statement.FromCfsDataDocument(publishedProviderFundingStream);
        return Ok(statement);
    }
}