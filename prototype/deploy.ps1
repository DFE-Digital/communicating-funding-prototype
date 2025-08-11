Write-Host "Zipping prototype..."
zip -r publish.zip . -x "node_modules/*" "publish.zip"

Write-Host "Deploying the prototype to Azure..."
$env:AZURE_CLI_DISABLE_CONNECTION_VERIFICATION=0 # Avoids SSL errors when deploying behind the DfE VPN
az webapp deploy --resource-group s255d01rg-uks-cfp-default --name s255d01as-cfp-prototype --src-path ./publish.zip --type zip