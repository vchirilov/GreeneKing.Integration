$rg = "z114-d-evnt"
New-AzResourceGroup -Name $rg -Location northeurope -Force
New-AzResourceGroupDeployment -Name "create-message-broker-01" -ResourceGroupName $rg -TemplateFile "gk-service-bus-pipeline-orchestration.json"