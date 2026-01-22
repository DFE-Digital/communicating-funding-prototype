Write-Host "Publishing the CommunicationsAlpha2025 API project..."
dotnet publish ./CommunicationsAlpha2025 -c Release -o ./publish

Write-Host "Creating a zip package for deployment..."
$publishRoot = Resolve-Path "./publish"

$items = Get-ChildItem -Path $publishRoot -Recurse -File |
    ForEach-Object { $_.FullName } |
    Select-Object -Unique

$zipPath = Join-Path (Get-Location) "publish.zip"
Compress-Archive -Path $items -DestinationPath $zipPath -Force

Write-Host "Deploying the CommunicationsAlpha2025 prototype API to Azure..."
$env:AZURE_CLI_DISABLE_CONNECTION_VERIFICATION = 0
az webapp deploy --resource-group s255d01rg-uks-cfp-default --name s255d01as-cfp-prototype-api --src-path ./publish.zip --type zip