using System.Text.Json;
using CommunicationsAlpha2025.Data;
using CommunicationsAlpha2025.Versions.V2.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommunicationsAlpha2025.Versions.V2.Controllers;

[Route("api/v2/[controller]")]
[ApiController]
public class StatementController(IStaticDataProvider staticDataProvider) : ControllerBase
{
    [HttpGet("{fundingStreamId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Get(string fundingStreamId)
    {
        try
        {
            JsonElement publishedProviderFundingStream 
                = staticDataProvider.GetPublishedProviderFundingStreamById(fundingStreamId);
            Statement statement = Statement.FromCfsDataDocument(publishedProviderFundingStream);
            return Ok(statement);
        }
        catch (NotSupportedException)
        {
            return StatusCode(StatusCodes.Status501NotImplemented, "The given statement type is not supported.");
        }
    }
}