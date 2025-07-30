resource "azurerm_key_vault" "kv-default" {
  name                       = "${var.prefix}kv-cfp-default"
  location                   = var.location
  resource_group_name        = azurerm_resource_group.rg-default.name
  soft_delete_retention_days = 7 // Secrets hang around for 7 days, giving us a grace period to restore
  purge_protection_enabled   = true
  tenant_id                  = data.azurerm_client_config.current.tenant_id
  sku_name                   = "standard"

  network_acls {
    default_action = "Allow"
    bypass         = "AzureServices"
  }

  tags = {
    Environment        = var.tag_environment
    Product            = var.tag_product
    "Service Offering" = var.tag_service_offering
  }
}

# Creates an access policy that app services can use to read keys
# from the key vault (mainly for env vars)
resource "azurerm_key_vault_access_policy" "kv-default-access-policy-general" {
  key_vault_id = azurerm_key_vault.kv-default.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = data.azurerm_client_config.current.object_id

  secret_permissions = ["Get", "List", "Set", "Delete", "Purge", "Restore", "Recover"]
}

# Creates an access policy that app services can use to read keys
# from the key vault (mainly for env vars)
resource "azurerm_key_vault_access_policy" "kv-default-access-policy-app-services" {
  key_vault_id = azurerm_key_vault.kv-default.id
  tenant_id = azurerm_linux_web_app.wa-prototype.identity[0].tenant_id
  object_id = azurerm_linux_web_app.wa-prototype.identity[0].principal_id

  key_permissions = [
    "Get"
  ]

  secret_permissions = [
    "Get",
  ]
}

resource "azurerm_key_vault_secret" "kv-default-prototype-pass" {
  name         = "prototype-password"
  value        = "1nIt14LV4lu3%" // Initial/default value. Is changed later.
  key_vault_id = azurerm_key_vault.kv-default.id

  depends_on = [
    azurerm_key_vault_access_policy.kv-default-access-policy-general
  ]
}

