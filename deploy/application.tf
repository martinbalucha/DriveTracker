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

resource "azurerm_resource_group" "resource-group" {
  name = "${var.environment}-resource-group"
  location = "${vars.region}"
}

resource "azurerm_service_plan" "service-plan" {
    name = "${vars.environment}-service-plan"
    location = "${vars.region}"
    resource_group_name = azurerm_resource_group.resource-group.name
    os_type = "Linux"
    sku_name = "F1"
}

resource "azurerm_linux_function_app" "web-api" {
  name = "${var.environment}-restapi"
  location = "${vars.region}"
  resource_group_name = azurerm_resource_group.resource-group.id
  service_plan_id = azurerm_service_plan.service-plan.id

  site_config {
    always_on = false
  }
}

resource "azurerm_iothub" "iot-hub" {
  name = "${vars.environment}-iothub"
  location = "${vars.region}"
  resource_group_name = azurerm_resource_group.resource-group.id

  sku {
    name = "F1"
    capacity = 1
  }
}

