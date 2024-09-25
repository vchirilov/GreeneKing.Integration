$rg = "z114-d-evnt"
New-AzResourceGroup -Name $rg -Location westeurope -Force
New-AzResourceGroupDeployment -Name "create-service-bus-01" -ResourceGroupName $rg -TemplateFile "gk-service-bus-pipeline-orchestration.json"