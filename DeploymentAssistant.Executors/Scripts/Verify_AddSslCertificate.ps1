#Requires -Version 3.0
#Requires -RunAsAdministrator
# script - function - VerifyAddSslCertificate

param ([String]$CertificateSharePath, [String]$CertificateThumbPrint, [String]$pwd, [String]$hostIp = "", [String]$websiteName, [String]$port = "443", [String]$hostHeader = "", [String]$bindingIp = "", [String]$storeLocation = "LocalMachine", [String]$storeName = "WebHosting")

Import-Module 'WebAdministration'

$mappingIp = $hostIp
if([String]::IsNullOrWhiteSpace($mappingIp))
{
    $mappingIp = "0.0.0.0"
}
$mappingPath = 'IIS:\SslBindings\' + $mappingIp + '!' + $port

$cert = (Get-Item $mappingPath -ErrorAction SilentlyContinue)
#if cert is not null
if(!($null -eq $cert))
{
    $sites = $cert.Sites
    if(!($null -eq $sites))
    {
        $siteValues = $site.Value
        if($siteValues -Contains $websiteName)
        {
            return 1
        }
        else
        {
            return 0
        }
    }
    else
    {
        return 0
    }
}
else
{
    return 0
}
