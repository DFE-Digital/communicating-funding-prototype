data "azurerm_client_config" "current" {}

resource "azurerm_key_vault" "kv-default" {
  name                       = "${var.prefix}kv-cfp-default"
  location                   = var.location
  resource_group_name        = azurerm_resource_group.rg-default.name
  soft_delete_retention_days = 7 // Secrets hang around for 7 days, giving us a grace period to restore
  purge_protection_enabled   = true
  tenant_id                  = data.azurerm_client_config.current.tenant_id
  sku_name                   = "standard"
}

data "azurerm_key_vault_secret" "kv-default-prototype-pass" {
  name         = "prototype-password"
  key_vault_id = azurerm_key_vault.kv-default.id
} 
