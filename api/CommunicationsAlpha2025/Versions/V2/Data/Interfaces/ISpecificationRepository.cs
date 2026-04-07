using CommunicationsAlpha2025.Versions.V2.Data.Models;

namespace CommunicationsAlpha2025.Versions.V2.Data.Interfaces;

public interface ISpecificationRepository
{
    public IQueryable<Specification_GQL> GetSpecifications();
}