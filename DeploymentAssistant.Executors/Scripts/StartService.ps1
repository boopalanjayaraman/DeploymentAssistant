function StartService()
{
    param([String]$serviceName)

    Start-Service serviceName
}