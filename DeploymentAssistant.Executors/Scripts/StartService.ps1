#Requires -Version 3.0
#Requires -RunAsAdministrator
# Script - function - StartService

param([String]$serviceName)

Start-Service $serviceName
