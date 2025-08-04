resource "azurerm_storage_account" "sa-default" {
  name                     = "${var.prefix}sacfpdefault"
  resource_group_name      = azurerm_resource_group.rg-default.name
  location                 = var.location
  account_tier             = "Standard"
  account_replication_type = "GRS"

  tags = {
    Environment        = var.tag_environment
    Product            = var.tag_product
    "Service Offering" = var.tag_service_offering
  }
}

# Stores data used for the prototype
resource "azurerm_storage_container" "sa-default-data" {
  name                  = "data"
  storage_account_id    = azurerm_storage_account.sa-default.id
  container_access_type = "blob"
}
