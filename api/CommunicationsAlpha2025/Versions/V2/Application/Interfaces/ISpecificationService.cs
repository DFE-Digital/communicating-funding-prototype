using CommunicationsAlpha2025.Versions.V2.Data.Models;

namespace CommunicationsAlpha2025.Versions.V2.Application.Interfaces;

public interface ISpecificationService
{
    public IQueryable<Specification_GQL> GetSpecifications();
}