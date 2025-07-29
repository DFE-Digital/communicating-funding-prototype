provider "azurerm" {
  features {
    resource_group {
      prevent_deletion_if_contains_resources = false
    }
  }
  resource_provider_registrations = "none"
  subscription_id                 = var.subscription_id
  environment                     = "public"
  use_msi                         = false
  use_cli                         = true
  use_oidc                        = false
}
