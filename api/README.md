# Communicating Funding Prototype API

This API:
- Defines a prototype schema used to power the prototype UI
- Provides endpoints used by the prototype UI to fetch CFS data (stored in Azure)

## Dependencies

The following dependencies are required to use this project.

- [.NET Core SKD v9](https://dotnet.microsoft.com/en-us/download): for the dotnet project
- [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest): for deployments
- [PowerShell](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell?view=powershell-7.5): for helper scripts

## Running locally

To run the API locally, run:

```bash
$ dotnet run --project CommunicationsAlpha2025
```

## Testing

The test project offers API tests used to test whether we can, for each funding stream type in CFS, serialize and serve CFS data in our prototype data schema. Supported funding streams can be found in [CommunicationsAlpha2025/Versions/V2/Augmentations.cs](./CommunicationsAlpha2025/Versions/V2/Augmentations.cs).

Tests can be found in the [CommunicationsAlpha2025.Test](./CommunicationsAlpha2025.Test/) directory.

To run tests, run:
```bash
$ dotnet test
```

## Deploying

To deploy:
1. Ensure you're authenticated using `az login`, and scoped to subscription with ID `51199e9b-8fa9-4269-825e-fa5d7cc2b857`
2. Run `pwsh deploy.ps1`
