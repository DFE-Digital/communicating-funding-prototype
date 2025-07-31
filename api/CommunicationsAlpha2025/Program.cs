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
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<IStaticDataProvider, StaticDataProvider>();

        builder.Services.AddAzureClients(async clientBuilder =>
        {
            const string defaultStorageAccountUriConfigKey = "DefaultStorageAccountUri";
            var defaultStorageAccountUri = new Uri(builder.Configuration[defaultStorageAccountUriConfigKey]
                                                   ?? throw new InvalidOperationException(
                                                       $"Missing config property '{defaultStorageAccountUriConfigKey}'"));
                                              
            clientBuilder.AddBlobServiceClient(defaultStorageAccountUri);

            clientBuilder.UseCredential(new AzureCliCredential());
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
        
        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapControllers();
        app.Run();
    }
}