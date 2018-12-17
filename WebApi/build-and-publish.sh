#!/usr/bin/env bash
./docker-build.sh
./docker-push.sh
./kubectl-apply.sh
# If the YAML file hasn't changed but the Docker image has, kubectl -apply won't update the running Docker image. 
# Killing all the running pods will force Kubeternetes to get the latest image
# kubectl delete pods --all