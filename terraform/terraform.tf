terraform {
  backend "azurerm" {
    resource_group_name = "s255d01rg-uks-cfp-terraform"
    storage_account_name = "s255d01terraformstorage"
    container_name = "terraform"
    key = "s255d01terraformstorage/s255d01.tfstate"
  }

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "4.33.0"
    }
  }
}
