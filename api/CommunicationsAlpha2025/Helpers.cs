using System.Reflection;

namespace CommunicationsAlpha2025.Test;

public static class Helpers
{
    public static string GetSutProjectRootPath(string sutProjectName)
    {
        string? testAssemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string? currentDirectory = testAssemblyLocation;
        const int maxAttempts = 10;
        for (var i = 0; i < maxAttempts; i++)
        {
            if (currentDirectory == null)
                break;
            string sutCsprojPath = Path.Combine(currentDirectory, $"{sutProjectName}.csproj");
            if (File.Exists(sutCsprojPath))
            {
                return currentDirectory;
            }
            currentDirectory = Directory.GetParent(currentDirectory)?.FullName!;
        }
        throw new DirectoryNotFoundException($"Could not find the project root for '{sutProjectName}'. Searched up from: {testAssemblyLocation}");
    }
}