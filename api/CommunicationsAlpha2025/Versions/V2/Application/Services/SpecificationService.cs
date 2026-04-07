using CommunicationsAlpha2025.Versions.V2.Application.Interfaces;
using CommunicationsAlpha2025.Versions.V2.Data.Interfaces;
using CommunicationsAlpha2025.Versions.V2.Data.Models;
using NuGet.Protocol.Core.Types;

namespace CommunicationsAlpha2025.Versions.V2.Application.Services;

public class SpecificationService(ISpecificationRepository repository) : ISpecificationService
{
    public IQueryable<Specification_GQL> GetSpecifications()
    {
        return repository.GetSpecifications();
    }
}