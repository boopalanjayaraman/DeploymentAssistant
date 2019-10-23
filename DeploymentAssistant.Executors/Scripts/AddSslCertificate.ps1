#Requires -Version 3.0
#Requires -RunAsAdministrator

function AddSslCertificate()
{
    param ([String]$CertificateLocalPath, [String]$CertificateThumbPrint, [String]$pwd, [String]$hostIp = "", [String]$websiteName, [String]$port = "443", [String]$hostHeader = "", [String]$storeLocation = "LocalMachine", [String]$storeName = "WebHosting")

    Import-Module 'WebAdministration'

    #if it is not a pfx, return
    if(!$CertificateLocalPath.ToLower().EndsWith(".pfx"))
    {
        return 0
    }

    $securePwd = ConvertTo-SecureString -String $pwd -Force -AsPlainText

    #Import certificate into store first
    $certStoreLocation = 'cert:\' + $storeLocation + '\' + $storeName
    (Import-PfxCertificate -FilePath $CertificateLocalPath -CertStoreLocation $certStoreLocation -Exportable -Password $securePwd )

    #add the binding to site with this ssl certificate's thumbprint
    $certPath = 'cert:\' + $storeLocation + '\' + $storeName + '\' + $CertificateThumbPrint

    $bindingIp = $hostIp
    if([String]::IsNullOrWhiteSpace($bindingIp))
    {
        $bindingIp = "*"
    }
    (New-WebBinding -Name $websiteName -IPAddress $bindingIp -Protocol "https" -Port $port -HostHeader $hostHeader)

    #map the certificate to this ip and port
    $mappingIp = $hostIp
    if([String]::IsNullOrWhiteSpace($mappingIp))
    {
        $mappingIp = "0.0.0.0"
    }
    $mappingPath = 'IIS:\SslBindings\' + $mappingIp + '!' + $port
    (Get-Item $certPath | New-Item $mappingPath)

    return 1
}