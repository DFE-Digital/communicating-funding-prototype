resource "azurerm_resource_group" "rg-default" {
  location = var.location
  name     = "${var.prefix}rg-uks-cfp-default"

  // These tags are required by DfE's Azure policy
  tags = {
    Environment        = var.tag_environment
    "Parent Business"  = "Funding and Allocations"
    Portfolio          = "Grant Management Service"
    Product            = var.tag_product
    Service            = "Funding and Allocations"
    "Service Line"     = "Funding"
    "Service Offering" = var.tag_service_offering
  }
}
