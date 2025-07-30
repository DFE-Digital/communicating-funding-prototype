using Azure.Identity;
using CommunicationsAlpha2025.Data;
using Microsoft.Extensions.Azure;

namespace CommunicationsAlpha2025;

public partial class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddOpenApi();
        builder.Services.AddControllers();

        builder.Services.AddSingleton<IStaticDataProvider, StaticDataProvider>();

        builder.Services.AddAzureClients(async clientBuilder =>
        {
            const string defaultStorageAccountUriConfigKey = "DefaultStorageAccountUri";
            clientBuilder.AddBlobServiceClient(builder.Configuration[defaultStorageAccountUriConfigKey]
                ?? throw new InvalidOperationException($"Missing config property '{defaultStorageAccountUriConfigKey}'"));

            clientBuilder.UseCredential(new DefaultAzureCredential());
        });

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        if (!app.Environment.IsEnvironment("Test"))
        {
            app.UseHttpsRedirection();
        }

        app.MapControllers();
        app.Run();
    }
}