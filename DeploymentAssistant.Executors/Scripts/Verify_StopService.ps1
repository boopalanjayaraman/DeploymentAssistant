#Requires -Version 3.0
# script - function - VerifyStopService

param([String]$serviceName)

$status = (Get-Service $serviceName).Status
if($status -eq "Stopped")
{
    return 1
}
else
{
    return 0
}
