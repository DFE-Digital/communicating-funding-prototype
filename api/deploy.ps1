Write-Host "Publishing the CommunicationsAlpha2025 API project..."
dotnet publish ./CommunicationsAlpha2025 -c Release -o ./publish

Write-Host "Creating a zip package for deployment..."
zip -r publish.zip ./publish

Write-Host "Deploying the CommunicationsAlpha2025 API project to Azure..."
$env:AZURE_CLI_DISABLE_CONNECTION_VERIFICATION=0 # Avoids SSL errors when deploying behind the DfE VPN
az webapp deploy --resource-group s255d01rg-uks-cfp-default --name s255d01as-cfp-prototype-api --src-path ./publish.zip --type zip