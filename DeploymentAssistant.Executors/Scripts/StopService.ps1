function StopService()
{
    param([String]$serviceName)

    Stop-Service serviceName
}