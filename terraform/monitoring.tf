resource "azurerm_log_analytics_workspace" "law-default" {
  location            = var.location
  name                = "${var.prefix}law-default"
  resource_group_name = azurerm_resource_group.rg-default.name
  retention_in_days   = 30

  tags = {
    Environment        = var.tag_environment
    Product            = var.tag_product
    "Service Offering" = var.tag_service_offering
  }

  depends_on = [
    azurerm_resource_group.rg-default
  ]
}

resource "azurerm_application_insights" "appi-default" {
  name                = "${var.prefix}ai-appinsights-default"
  resource_group_name = azurerm_resource_group.rg-default.name
  workspace_id        = azurerm_log_analytics_workspace.law-default.id
  location            = var.location
  application_type    = "Node.JS"
  sampling_percentage = 0
  retention_in_days   = "30"

  tags = {
    Environment        = var.tag_environment
    Product            = var.tag_product
    "Service Offering" = var.tag_service_offering
  }
}