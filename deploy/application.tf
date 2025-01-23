terraform {
    required_providers {
        azurerm = {
            source = "hashicorp/azurerm"
            version = ">= 1.0.0"
        }
    }
}

variable "environment" {
    description = "Drive Tracker's environment (dev, prod, ...)"
    type = string
}

variable "subscription_id" {
    description = "ID of an Azure subscription"
    type = string
}

provider "azurerm" {
    features {}
    subscription_id = "${var.subscription_id}"
}

resource "azurerm_resource_group" "resource-group" {
  name = "${var.environment}-resource-group"
  location = "West Europe"
}

