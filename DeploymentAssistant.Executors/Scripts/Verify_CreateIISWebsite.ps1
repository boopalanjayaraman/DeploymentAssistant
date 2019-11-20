#Requires -Version 3.0
# Script - function - VerifyCreateIISWebsite
 
param([String]$websiteName, [System.Collections.HashTable]$bindings, [String]$physicalPath, [Boolean]$override = $false)

Import-Module "WebAdministration"

$appPoolName = $websiteName + 'Pool'
$appPoolsFolderPath = "IIS:\AppPools\"
$appPoolPath =  $appPoolsFolderPath + $appPoolName

$websitesFolderPath = "IIS:\Sites\"
$websitePath = $websitesFolderPath + $websiteName

#check the physical directory
$sourceItem = (Get-Item $physicalPath -Force)
if(($null -eq $sourceItem) -or ($sourceItem.Count -eq 0))
{
    return 0
}

#check the application pool
$appPoolCount = (Get-Item $appPoolPath -ErrorAction SilentlyContinue).Count
if($appPoolCount -eq 0)
{
    return 0
}

#check the website 
$webSite = (Get-Item $websitePath -ErrorAction SilentlyContinue)
$webSiteCount = $webSite.Count
if($webSiteCount -eq 0)
{
    return 0
}
#check the bindings
$bindingsCount = $webSite.Bindings.Count
$originalBindingsCount = $bindings.Count
if($bindingsCount -ne $originalBindingsCount)
{
    return 0
}

return 1
 