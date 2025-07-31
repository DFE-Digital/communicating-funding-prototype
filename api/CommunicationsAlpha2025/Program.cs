using System.Reflection;
using Azure.Identity;
using CommunicationsAlpha2025.Data;
using Microsoft.Extensions.Azure;
using Microsoft.OpenApi.Models;

namespace CommunicationsAlpha2025;

public partial class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v2", new OpenApiInfo()
            {
                Version = "v2",
                Title = "Communications Alpha 2025 Prototype API",
                License = new OpenApiLicense()
                {
                    Name = "MIT License",
                    Url = new Uri("https://github.com/DFE-Digital/communicating-funding-alpha-prototype/blob/main/LICENCE.txt")
                }
            });
            
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
        
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

        if (!app.Environment.IsEnvironment("Test"))
        {
            app.UseHttpsRedirection();
        }
        
        app.UseSwagger();
        app.UseSwaggerUI(opt =>
        {
            opt.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
            opt.RoutePrefix = "";
        });

        app.MapControllers();
        app.Run();
    }
}