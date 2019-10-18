function GetService_Stopped()
{
    param([String]$serviceName)

    $status = (Get-Service $serviceName).Status
    if($status -eq "Stopped")
    {
        return $true
    }
    else
    {
        return $false
    }
}