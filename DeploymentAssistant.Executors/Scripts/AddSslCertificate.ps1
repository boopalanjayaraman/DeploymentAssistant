#Requires -Version 3.0
#Requires -RunAsAdministrator

# Script - Function - AddSslCertificate.ps1

param ([String]$CertificateSharePath, [String]$CertificateThumbPrint, [String]$pwd, [String]$websiteName, [String]$port = "443", [String]$hostHeader = "", [String]$bindingIp = "", [String]$storeLocation = "LocalMachine", [String]$storeName = "WebHosting")

Import-Module 'WebAdministration'
Import-Module 'PKI'

#if it is not a pfx, return
if(!$CertificateSharePath.ToLower().EndsWith(".pfx"))
{
    throw "EXCEPTION: certificate file should be a pfx file. (with private key)."
}

$CertFileExists= (Get-Item $CertificateSharePath -Force -ErrorAction SilentlyContinue)

if($null -eq $CertFileExists)
{
    throw "EXCEPTION: certificate file does not exist."
}

$pathType = [System.Uri]$CertificateSharePath
if($pathType.IsUnc)
{
    throw "EXCEPTION: Only Local paths can work well while adding a certificate with private key to store and mapping it to a website."
}

$TargetCopyPath = $CertificateSharePath

if(($null -eq $storeLocation) -or ([string]::IsNullOrWhiteSpace($storeLocation)))
{
    $storeLocation = "LocalMachine"
}
if(($null -eq $storeName) -or ([string]::IsNullOrWhiteSpace($storeName)))
{
    $storeName = "WebHosting"
}

#Import certificate into store then
$securePwd = ConvertTo-SecureString -String $pwd -Force -AsPlainText
$certStoreLocation = '{0}\{1}\{2}' -f 'cert:', "$storeLocation","$storeName"

$certImportAction = (Import-PfxCertificate -FilePath $TargetCopyPath -CertStoreLocation $certStoreLocation -Exportable -Password $securePwd)
$thumbprint = $certImportAction.Thumbprint

if($null -eq $thumbprint)
{
    throw "EXCEPTION: Some error happened during certificate import action."
}

if([System.String]::IsNullOrWhiteSpace($CertificateThumbPrint))
{
    $CertificateThumbPrint = $thumbprint
}
else
{
    $CertificateThumbPrint = $CertificateThumbPrint
}

#add the binding to site with this ssl certificate's thumbprint
$certPath = '{0}\{1}\{2}\{3}' -f 'cert:', "$storeLocation", "$storeName","$CertificateThumbPrint"

$webBindingIp = ""
if(($null -eq $bindingIp) -or ([String]::IsNullOrWhiteSpace($bindingIp)))
{
    $webBindingIp = "*"
}
else
{
    $webBindingIp = $bindingIp
}

#If hostheader is non-null, then make sslflags 1
$sslFlag = 0
if(($null -ne $hostHeader) -and (![String]::IsNullOrWhiteSpace($hostHeader)))
{
    $sslFlag = 1
}

#check if the binding already exists
$existingBinding = (Get-WebBinding -Name $websiteName -IPAddress $webBindingIp -Protocol "https" -Port $port -HostHeader $hostHeader)
if($null -eq $existingBinding)
{
    #else create the binding
    (New-WebBinding -Name $websiteName -IPAddress $webBindingIp -Protocol "https" -Port $port -HostHeader $hostHeader -SslFlags $sslFlag)    
}

#map the certificate to this ip and port
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

#check if the mapping already exists. If not, create the mapping.
$cert = (Get-Item $mappingPath -ErrorAction SilentlyContinue)
if(($null -eq $cert) -or ($null -eq $cert.Sites) -or !($cert.Sites.Value -Contains $websiteName))
{
    (Get-Item $certPath | New-Item $mappingPath)
}

return 1
