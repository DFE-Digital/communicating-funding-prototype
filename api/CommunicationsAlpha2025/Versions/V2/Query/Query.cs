using CommunicationsAlpha2025.Versions.V2.Application.Interfaces;
using CommunicationsAlpha2025.Versions.V2.Data;
using CommunicationsAlpha2025.Versions.V2.Data.Models;
using CommunicationsAlpha2025.Versions.V2.Models.Calculations;

namespace CommunicationsAlpha2025.Versions.V2.Query;

public class Query
{
    [UseProjection]
    public IQueryable<Specification_GQL> GetSpecification_GQL([Service] ISpecificationService service)
    {
        return service.GetSpecifications();
    }
}