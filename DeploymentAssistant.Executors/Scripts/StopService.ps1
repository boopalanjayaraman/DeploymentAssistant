#Requires -Version 3.0
#Requires -RunAsAdministrator
function StopService()
{
    param([String]$serviceName)

    Stop-Service $serviceName
}