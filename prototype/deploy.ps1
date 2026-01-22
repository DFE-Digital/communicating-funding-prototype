Write-Host "Creating a zip package for deployment..."
$publishRoot = Get-Location

$items = Get-ChildItem -Path $publishRoot -Recurse -File |
    Where-Object {
        $_.FullName -notmatch "node_modules" -and
        $_.Name -ne "publish.zip"
    } |
    ForEach-Object { $_.FullName } |
    Select-Object -Unique

$zipPath = Join-Path $publishRoot "publish.zip"
Compress-Archive -Path $items -DestinationPath $zipPath -Force

Write-Host "Deploying the CommunicationsAlpha2025 prototype UI to Azure..."
$env:AZURE_CLI_DISABLE_CONNECTION_VERIFICATION = 0

az webapp deploy --resource-group s255d01rg-uks-cfp-default --name s255d01as-cfp-prototype --src-path ./publish.zip --type zip