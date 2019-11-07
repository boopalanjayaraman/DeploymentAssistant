#Requires -Version 3.0
#Requires -RunAsAdministrator
function StartService()
{
    param([String]$serviceName)

    Start-Service $serviceName
}