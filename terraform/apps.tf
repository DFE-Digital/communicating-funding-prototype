# apps.tf
# Defines anything related to apps, app services, app service plans, etc.

resource "azurerm_service_plan" "asp-default" {
  location            = var.location
  name                = "${var.prefix}asp-cfp-default"
  os_type             = "Linux"
  resource_group_name = azurerm_resource_group.rg-default.name
  sku_name            = "B3"

  tags = {
    Environment        = var.tag_environment
    Product            = var.tag_product
    "Service Offering" = var.tag_service_offering
  }

  depends_on = [
    azurerm_resource_group.rg-default
  ]
}

resource "azurerm_linux_web_app" "wa-prototype" {
  ftp_publish_basic_authentication_enabled = false
  https_only                               = true
  location                                 = var.location
  name                                     = "${var.prefix}as-cfp-prototype"
  resource_group_name                      = azurerm_resource_group.rg-default.name
  service_plan_id                          = azurerm_service_plan.asp-default.id

  app_settings = {
    // Application Insights configuration
    APPLICATIONINSIGHTS_CONNECTION_STRING      = azurerm_application_insights.appi-default.connection_string
    XDT_MicrosoftApplicationInsights_Mode      = "Recommended"
    ApplicationInsightsAgent_EXTENSION_VERSION = "~3"

    // Password auth (requires a password to access the prototype)
    PASSWORD = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault_secret.kv-default-prototype-pass.versionless_id})"
    // Other bits
    NODE_ENV = "production"
  }

  identity {
    type = "SystemAssigned"
  }

  // We do not publish using basic auth, so we can disable this as a
  // security measure.
  webdeploy_publish_basic_authentication_enabled = false

  site_config {
    always_on = false
  }

  tags = {
    Environment        = var.tag_environment
    Product            = var.tag_product
    "Service Offering" = var.tag_service_offering
    # "hidden-link: /app-insights-resource-id" = "/subscriptions/51199e9b-8fa9-4269-825e-fa5d7cc2b857/resourceGroups/s255d01rg-uks-cfp-default/providers/microsoft.insights/components/s225d01as-cfp-prototype"
  }
}