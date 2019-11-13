#Requires -Version 3.0
# script - function - VerifyStartService

param([String]$serviceName)

$status = (Get-Service $serviceName).Status
if($status -eq "Running")
{
    return 1
}
else
{
    return 0
}
