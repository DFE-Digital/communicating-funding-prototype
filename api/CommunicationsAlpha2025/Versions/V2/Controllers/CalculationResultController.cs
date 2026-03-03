using CommunicationsAlpha2025.Versions.V2.Models;
using CommunicationsAlpha2025.Versions.V2.Models.Calculations;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CommunicationsAlpha2025.Versions.V2.Controllers;

[ApiController]
[ApiExplorerSettings(GroupName = "v2")]
[Route("api/v2/[controller]")]
[Produces("application/json")]
public class CalculationResultController(
    IStaticDataProvider staticDataProvider) : ControllerBase
{

    /// <summary>
    /// Fetches a CalculationResult, to visualise Calculation Factors in the Prototype Kit
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<Statement[]> GetCalculationResult()
    {

        string CalcResult = staticDataProvider.GetPrototypeCalcResult();

        var calculationResults = CalculationResults.FromJson(CalcResult);

        return Ok(calculationResults);
    }
}