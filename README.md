# Azure Datadog observability sample app

The purpose of this repository is to evaluate Datadog's ability to monitor Function Apps and APIs deployed in
Azure Kubernetes Service. 

## Prerequisites

All
 * .NET Core SDK 2.1
 * An Azure account

Mac
  
  * Download and install [Docker for Mac](https://docs.docker.com/docker-for-mac/install/)
  * Preferences -> Kubernetes -> Enable Kubernetes

Windows
 * You're on your own. Probably Docker and Minikube
 
# Setup steps

## Create a resource group

Note, you may want to alter the names of all the Azure resources created.

```
az login
az group create --name=datadog-observability-sample-group --location=ukwest
```

## Create an Azure Kubernetes Cluster, deploy a web API and install the Datadog agent

Create the cluster.

```
az aks create --resource-group datadog-observability-sample-group --name datadog-observability-aks-cluster --node-count 1
```

Install the Azure Kubeternetes Service Command Line Interface if you have not already.
```
az aks install-cli
```

Configure kubectl to connect to your cluster.
```
az aks get-credentials --resource-group datadog-observability-sample-group --name datadog-observability-aks-cluster
```
Deploy the API.

```
kubectl apply -f deployment.yaml
# or in the WebApi folder
./kubectl-apply.sh
```

Deploy the Datadog agent
```
kubectl create -f datadog-agent.yml
```

Wait for public IP
```
kubectl get service observability-sample-service --watch
```

Once the public IP appears you can access the API in your browser. https://[[PublicIP]]:4000/api/dog








## Create and publish a Function App

A Function App requires a storage account. The storage account name must be less than 24 characters and only contains numbers and letters.

```
az storage account create --name observabilitystorage --resource-group observability-sample --sku Standard_LRS
az functionapp create --resource-group observability-sample --storage-account observabilitystorage --name ObservabilitySamplefunctions --runtime dotnet --consumption-plan-location ukwest
func azure functionapp publish GetDogImage
```


## Create an Application Insights resource

At the time of writing this can only be done via the Azure Portal. In the Azure Portal go to "Create a resource" then Application Insights, and hit Create. 

* Application Type - General
* Use existing resource group

Once deployed go to the Overview tab and copy the Instrumentation Key. Switch to the Functional App and add a new app setting with the name ```APPINSIGHTS_INSTRUMENTATIONKEY```. Next, because we're using Application Insights we no longer need the built in logging. To disable this, delete the ```AzureWebJobsDashboard``` app setting. Hit the Save button at the top of the page.
Copy the Instrumentation Key into the Kubernetes deployment file [here](https://github.com/philjhale/AzureObservabilitySampleApp/blob/master/WebApi/deployment.yaml#L24).

## Create an Azure Kubernetes Cluster and deploy a web API

Create the cluster.

```
az aks create --resource-group observability-sample --name observability-aks-cluster --node-count 1 --enable-addons monitoring --generate-ssh-keys
```

Install the Azure Kubeternetes Service Command Line Interface if you have not already.
```
az aks install-cli
```

Configure kubectl to connect to your cluster.
```
az aks get-credentials --resource-group observability-sample --name observability-aks-cluster
```
Deploy the API.

```
kubectl apply -f deployment.yaml
# or in the WebApi folder
./kubectl-apply.sh
```

Wait for public IP
```
kubectl get service observability-sample-service --watch
```

Once the public IP appears you can access the API in your browser. https://[[PublicIP]]:4000/api/dog

