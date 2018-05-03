az group create --name ndccontoso --location eastus
az iot hub create --resource-group ndccontoso --name contosohub --sku F1
az acr create --resource-group ndccontoso --name ndccontosoreg --sku Basic --admin-enabled
az eventhubs namespace create --resource-group ndccontoso --name ndccontosons
az eventhubs eventhub create --resource-group ndccontoso --namespace-name ndccontosons --name ndccontosoeh
az appservice plan create --name ndccontosoapp --resource-group ndccontoso --sku S1
az webapp create --name ndccontosofunc --resource-group ndccontoso --plan ndccontosoapp
az webapp create --name ndccontosobot --resource-group ndccontoso --plan ndccontosoapp

# Currently Databricks via Azure Portal