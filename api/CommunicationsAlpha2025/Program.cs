using System.Reflection;
using Azure.Identity;
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
            // V2
            opt.SwaggerDoc("v2", new OpenApiInfo()
            {
                Version = "v2",
                Title = "Communications Alpha 2025 Prototype API (V2)",
                License = new OpenApiLicense()
                {
                    Name = "MIT License",
                    Url = new Uri("https://github.com/DFE-Digital/communicating-funding-alpha-prototype/blob/main/LICENCE.txt")
                }
            });

            // V3
            opt.SwaggerDoc("v3", new OpenApiInfo()
            {
                Version = "v3",
                Title = "Communications Alpha 2025 Prototype API (V3)",
                License = new OpenApiLicense()
                {
                    Name = "MIT License",
                    Url = new Uri("https://github.com/DFE-Digital/communicating-funding-alpha-prototype/blob/main/LICENCE.txt")
                }
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            opt.CustomSchemaIds(type =>
                type.FullName!.Replace('.', '_') // or a custom formatting to make schemaIds unique
            );
        });

        // V2
        builder.Services.AddSingleton<Versions.V2.IStaticDataProvider, Versions.V2.StaticDataProvider>();
        builder.Services.AddSingleton<Versions.V2.IStatementFactory, Versions.V2.StatementFactory>();
        // V3
        builder.Services.AddSingleton<Versions.V3.IStaticDataProvider, Versions.V3.StaticDataProvider>();
        builder.Services.AddSingleton<Versions.V3.IStatementFactory, Versions.V3.StatementFactory>();

        builder.Services.AddAzureClients(clientBuilder =>
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
            // V2
            opt.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
            // V3
            opt.SwaggerEndpoint("/swagger/v3/swagger.json", "v3");

            opt.RoutePrefix = "";
        });

        app.MapControllers();
        app.Run();
    }
}