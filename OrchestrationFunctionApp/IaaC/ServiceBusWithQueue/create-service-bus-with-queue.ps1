# Variables
$resourceGroupName = "z114-d-evnt-using-iaac"
$templateFile = "create-service-bus-with-queue.json"
$parametersFile = "create-service-bus-with-queue.parameters.json" # Path to the parameters file
$location = "westeurope"

# Create a resource group (if it doesn't exist)
New-AzResourceGroup -Name $resourceGroupName -Location $location

# Deploy the ARM template with parameters
New-AzResourceGroupDeployment `
  -ResourceGroupName $resourceGroupName `
  -TemplateFile $templateFile `
  -TemplateParameterFile $parametersFile
