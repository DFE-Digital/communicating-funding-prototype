using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace CommunicationsAlpha2025.Test;

[Collection("API")]
public class StatementsApiTests(WebApplicationFactory<Program> factory) 
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory = factory
        .WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddJsonFile("appsettings.Test.json", optional: false);
            });
            
            builder.UseEnvironment("Test");
        });
    
    [Theory]
    [ClassData(typeof(TestData.FundingStreamTestData))]
    public async Task Get_EndpointsReturnSuccessStatusCode(string fundingStreamType)
    {
        // Arrange
        HttpClient client = _factory.CreateClient();
        var requestUri = $"/api/v2/Statement/{fundingStreamType}";

        // Act
        HttpResponseMessage response = await client.GetAsync(requestUri);

        // Assert
        response.EnsureSuccessStatusCode();
    }
}