#!/bin/bash

read -p "MySQL root password: " password

connectionString="server=db;uid=root;pwd=${password}"

kubectl apply -f - <<EOD
apiVersion: v1
kind: Secret
metadata:
  namespace: spacerts
  name: mysql-password
type: kubernetes.io/basic-auth
stringData:
  connectionString: "${connectionString}"
  password: "${password}"
EOD
