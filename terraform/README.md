# Communicating Funding Alpha Terraform

This directory holds the infrastructure definitions for our project's Azure resources.

## Structure

The following files contain resource definitions:
- [main.tf](./main.tf): for top-level resources, such as resource groups
- [apps.tf](./apps.tf): for app services, app services plans, and related resources
- [monitoring.tf](./monitoring.tf): for monitoring-related resources, such as log analytics
- [provider.tf](./provider.tf): for the Terraform provider
- [secrets.tf](./secrets.tf): for resources relating to secrets, such as key vaults and secret definitions
- [terraform.tf](./terraform.tf): for general Terraform configuration
- [data.tf](./data.tf): for reusable data definitions
- [vars.tf](./vars.tf): for variable definitions used throughout the other config files

## Environments

Values for specific environments can be found in the `env/` folder.

You can use these by using the `-var-file=...` flag when running `terraform plan` or `terraform apply`.