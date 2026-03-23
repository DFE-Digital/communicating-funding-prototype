using CommunicationsAlpha2025.Versions.V2.Data.Interfaces;
using CommunicationsAlpha2025.Versions.V2.Data.Models;

namespace CommunicationsAlpha2025.Versions.V2.Data.Repositories;

public class SpecificationRepository(ApplicationDbContext context) : ISpecificationRepository
{
    public IQueryable<Specification_GQL> GetSpecifications()
    {
        return context.Specifications;
    }
}