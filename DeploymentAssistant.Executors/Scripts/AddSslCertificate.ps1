#Requires -Version 3.0
#Requires -RunAsAdministrator

function AddSslCertificate()
{
    param ([String]$CertificateSharePath, [String]$CertificateThumbPrint, [String]$pwd, [String]$hostIp = "", [String]$websiteName, [String]$port = "443", [String]$hostHeader = "", [String]$bindingIp = "", [String]$storeLocation = "LocalMachine", [String]$storeName = "WebHosting")

    Import-Module 'WebAdministration'

    #if it is not a pfx, return
    if(!$CertificateSharePath.ToLower().EndsWith(".pfx"))
    {
        return 0
    }

    #copy the certificate to target machine first - IMPORTANT: certificate installation works only from local paths. 
    $CertFileName = (Get-Item $CertificateSharePath).Name
    $localWinDir = $env:windir.Replace(":", "$")
    $TargetCopyPath = "\\" + $hostIp + "\" + $localWinDir + "\Temp\" + $CertFileName

    [System.IO.File]::Copy($CertificateSharePath, $TargetCopyPath, $true)

    #Import certificate into store then
    $securePwd = ConvertTo-SecureString -String $pwd -Force -AsPlainText
    $certStoreLocation = 'cert:\' + $storeLocation + '\' + $storeName
    $certImportAction = (Import-PfxCertificate -FilePath $TargetCopyPath -CertStoreLocation $certStoreLocation -Exportable -Password $securePwd )
    $thumbprint = $certImportAction.Thumbprint

    if([System.String]::IsNullOrWhiteSpace($CertificateThumbPrint))
    {
        $CertificateThumbPrint = $thumbprint
    }

    #add the binding to site with this ssl certificate's thumbprint
    $certPath = 'cert:\' + $storeLocation + '\' + $storeName + '\' + $CertificateThumbPrint

    if([String]::IsNullOrWhiteSpace($bindingIp))
    {
        $bindingIp = "*"
    }

    #If hostheader is non-null, then make sslflags 1
    $sslFlag = 0
    if(![String]::IsNullOrWhiteSpace($hostHeader))
    {
        $sslFlag = 1
    }
    (New-WebBinding -Name $websiteName -IPAddress $bindingIp -Protocol "https" -Port $port -HostHeader $hostHeader -SslFlags $sslFlag)    

    #map the certificate to this ip and port
    $mappingIp = $hostIp
    if([String]::IsNullOrWhiteSpace($mappingIp))
    {
        $mappingIp = "0.0.0.0"
    }
    $mappingPath = 'IIS:\SslBindings\' + $mappingIp + '!' + $port
    (Get-Item $certPath | New-Item $mappingPath)

    #delete the copied file
    [System.IO.File]::Delete($TargetCopyPath)

    return 1
}