variable "subscription_id" {
  type        = string
  description = "Azure subscription Id for the environment."
}

variable "location" {
  type        = string
  description = "The Azure region the services are deployed in."
  default     = "westeurope"
}

variable "prefix" {
  type        = string
  description = "The resource prefix such as s181t02."
}

# Tags required by the DfE's Azure policies.

variable "tag_environment" {
  type        = string
  description = "The tagged environment. Required by DfE Azure, and assigned as a tag to resources."
}

variable "tag_product" {
  type        = string
  description = "The tagged product. Required by DfE Azure, and assigned as a tag to resources."
  default     = "Calculate Funding Service (CFS)"
}

variable "tag_service_offering" {
  type        = string
  description = "The tagged service offering. Required by DfE Azure, and assigned as a tag to resources."
  default     = "Calculate Funding Service (CFS)"
}