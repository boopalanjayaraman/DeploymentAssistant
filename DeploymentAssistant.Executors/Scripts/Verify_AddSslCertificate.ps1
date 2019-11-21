#Requires -Version 3.0
#Requires -RunAsAdministrator
# script - function - VerifyAddSslCertificate

param ([String]$CertificateSharePath, [String]$CertificateThumbPrint, [String]$pwd, [String]$websiteName, [String]$port = "443", [String]$hostHeader = "", [String]$bindingIp = "", [String]$storeLocation = "LocalMachine", [String]$storeName = "WebHosting")

Import-Module 'WebAdministration'
Import-Module 'PKI'

$mappingIp = $bindingIp
if(($null -eq $bindingIp) -or ([String]::IsNullOrWhiteSpace($bindingIp)))
{
    $mappingIp = "0.0.0.0"
}
$mappingPath = 'IIS:\SslBindings\' + $mappingIp + '!' + $port
if(($null -ne $hostHeader) -and (![String]::IsNullOrWhiteSpace($hostHeader)))
{
    $mappingPath = "$mappingPath!$hostHeader"
}

$cert = (Get-Item $mappingPath -ErrorAction SilentlyContinue)
#if cert is not null
if(($null -ne $cert) -and ($null -ne $cert.Sites) -and ($cert.Sites.Value -Contains $websiteName))
{
    return 1
}
else
{
    return 0
}

# if(!($null -eq $cert))
# {
#     $sites = $cert.Sites
#     if(!($null -eq $sites))
#     {
#         $siteValues = $sites.Value
#         if($siteValues -Contains $websiteName)
#         {
#             return 1
#         }
#         else
#         {
#             return 0
#         }
#     }
#     else
#     {
#         return 0
#     }
# }
# else
# {
#     return 0
# }
