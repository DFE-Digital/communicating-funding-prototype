using CommunicationsAlpha2025.Versions.V2.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunicationsAlpha2025.Versions.V2.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Specification_GQL> Specifications {get; set;}
    public DbSet<FundingStream> FundingStreams {get; set;}
}