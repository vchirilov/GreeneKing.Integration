// Parameters
param location string = resourceGroup().location
param storageAccountName string = 'amdarisgkafuncappsa'
param appServicePlanName string = 'amdaris-gk-app-func-plan'
param functionAppName string = 'amdaris-gk-app-func'
param storageSku string = 'Standard_LRS'

// Storage Account
resource storageAccount 'Microsoft.Storage/storageAccounts@2021-09-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: storageSku
  }
  kind: 'StorageV2'
}

// App Service Plan
resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: 'B1'
    tier: 'Basic'
    capacity: 1
  }
  kind: 'functionapp'
}

// Function App
resource functionApp 'Microsoft.Web/sites@2022-03-01' = {
  name: functionAppName
  location: location
  kind: 'functionapp'
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: storageAccount.properties.primaryEndpoints.blob
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet'
        }
        {
          name: 'ServiceBus__QueueName'
          value: 'No Queue Defined'
        }
        {
          name: 'ServiceBus__ConnectionString'
          value: 'connection_string_here'
        }
      ]
    }
  }
  dependsOn: [
    storageAccount
    appServicePlan
  ]
}

// Outputs
output storageAccountEndpoint string = storageAccount.properties.primaryEndpoints.blob
output functionAppUrl string = 'https://${functionApp.properties.defaultHostName}'