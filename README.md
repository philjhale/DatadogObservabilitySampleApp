# Azure Datadog observability sample app

The purpose of this repository is to evaluate Datadog's ability to monitor APIs deployed in
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

Install the Azure Kubernetes Service Command Line Interface if you have not already.
```
az aks install-cli
```

Configure kubectl to connect to your cluster.
```
az aks get-credentials --resource-group datadog-observability-sample-group --name datadog-observability-aks-cluster
```
Deploy the API.

```
kubectl apply -f deployment.yml
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



## References

- [Annotations](https://docs.datadoghq.com/agent/autodiscovery/?tab=kubernetes)
- [Daemon setup](https://docs.datadoghq.com/agent/kubernetes/daemonset_setup/)
- [Tracing setup](https://docs.datadoghq.com/tracing/setup/dotnet/)


