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

variable "region" {
    description = "Deployment Region"
    type = string
}

provider "azurerm" {
    features {}
    subscription_id = "${var.subscription_id}"
}

locals {
  resource_prefix = "dt-${vars.environment}"
}

resource "azurerm_resource_group" "resource-group" {
  name = "${locals.resource_prefix}-resource-group"
  location = "${vars.region}"
}

resource "azurerm_service_plan" "service-plan" {
    name = "${locals.resource_prefix}-service-plan"
    location = azurerm_resource_group.resource-group.location
    resource_group_name = azurerm_resource_group.resource-group.name
    os_type = "Linux"
    sku_name = "F1"
}

resource "azurerm_linux_function_app" "web-api" {
  name = "${locals.resource_prefix}-restapi"
  location = azurerm_resource_group.resource-group.location
  resource_group_name = azurerm_resource_group.resource-group.id
  service_plan_id = azurerm_service_plan.service-plan.id

  site_config {
    always_on = false
  }
}

resource "azurerm_iothub" "iot-hub" {
  name = "${locals.resource_prefix}-iothub"
  location = azurerm_resource_group.resource-group.location
  resource_group_name = azurerm_resource_group.resource-group.id

  sku {
    name = "F1"
    capacity = 1
  }
}

resource "azurerm_cosmosdb_account" "cosmos" {
  name = "${locals.resource_prefix}-drivedb"
  location = azurerm_resource_group.resource-group.location
  resource_group_name = azurerm_resource_group.resource-group.name
  offer_type = "Standard"
  kind = "GlobalDocumentDB"
  free_tier_enabled = true

  consistency_policy {
    consistency_level = "Session"
  }

  geo_location {
    location = "North Europe"
    failover_priority = 0
  }
}

resource "azurerm_cosmosdb_table" "drive-table" {
  name = "${locals.resource_prefix}-drive-data"
  resource_group_name = azurerm_resource_group.resource-group.name
  account_name = azurerm_cosmosdb_account.cosmos.name
}


