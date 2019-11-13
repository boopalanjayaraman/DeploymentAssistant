#Requires -Version 3.0
#Requires -RunAsAdministrator
# Script - function - StopService

param([String]$serviceName)

Stop-Service $serviceName
