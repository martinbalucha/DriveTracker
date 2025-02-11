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
  resource_prefix = "dt-${var.environment}"
}

data "azurerm_client_config" "current" {}

resource "azurerm_resource_group" "resource-group" {
  name = "${local.resource_prefix}-resource-group"
  location = "${var.region}"
}

resource "azurerm_service_plan" "service-plan" {
    name = "${local.resource_prefix}-service-plan"
    location = azurerm_resource_group.resource-group.location
    resource_group_name = azurerm_resource_group.resource-group.name
    os_type = "Linux"
    sku_name = "F1"
}

resource "azurerm_linux_function_app" "web-api" {
  name = "${local.resource_prefix}-restapi"
  location = azurerm_resource_group.resource-group.location
  resource_group_name = azurerm_resource_group.resource-group.name
  service_plan_id = azurerm_service_plan.service-plan.id

  site_config {
    always_on = false
  }

  storage_key_vault_secret_id = azurerm_key_vault_secret.storage_key.id
}

resource "azurerm_storage_account" "storage_account" {
  name = "dt${var.environment}storage"
  resource_group_name = azurerm_resource_group.resource-group.name
  location = azurerm_resource_group.resource-group.location
  account_tier = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_key_vault" "key_vault" {
  name = "${local.resource_prefix}-key-vault"
  resource_group_name = azurerm_resource_group.resource-group.name
  location = azurerm_resource_group.resource-group.location
  tenant_id = "${data.azurerm_client_config.current.tenant_id}"
  sku_name = "standard"
}

resource "azurerm_key_vault_secret" "storage_key" {
  name = "${local.resource_prefix}-storage-key"
  key_vault_id = azurerm_key_vault.key_vault.id
  value = azurerm_storage_account.storage_account.primary_access_key
}

resource "azurerm_iothub" "iot-hub" {
  name = "${local.resource_prefix}-iothub"
  location = azurerm_resource_group.resource-group.location
  resource_group_name = azurerm_resource_group.resource-group.name

  sku {
    name = "F1"
    capacity = 1
  }
}

resource "azurerm_cosmosdb_account" "cosmos" {
  name = "${local.resource_prefix}-drivedb"
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
  name = "${local.resource_prefix}-drive-data"
  resource_group_name = azurerm_resource_group.resource-group.name
  account_name = azurerm_cosmosdb_account.cosmos.name
}


