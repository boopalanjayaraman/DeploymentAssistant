#Requires -Version 3.0
function GetService_Started()
{
    param([String]$serviceName)

    $status = (Get-Service $serviceName).Status
    if($status -eq "Running")
    {
        return $true
    }
    else
    {
        return $false
    }
}