#Requires -Version 3.0
#Requires -RunAsAdministrator

# Script - Function - CreateIISWebsite

param([String]$websiteName, [System.Collections.Generic.Dictionary[string, string]]$bindings, [String]$physicalPath, [Boolean]$override = $false)

Import-Module "WebAdministration"

$appPoolName = $websiteName + 'Pool'
$appPoolsFolderPath = "IIS:\AppPools\"
$appPoolPath =  $appPoolsFolderPath + $appPoolName

#Return zero if the directory does not exist. It has to exist physically before creating websites.
#Creating the physical directory on-the-fly will lead to empty directory getting mapped, in case of mistakes.
if(![System.IO.Directory]::Exists($physicalPath))
{
    return 0
}

#Create the pool if it does not exist, or if it exists, act as per override parameter
$poolCount = (Get-Item $appPoolPath -ErrorAction SilentlyContinue).Count
if($poolCount -eq 0)
{
    (New-WebAppPool $appPoolName)
}
else
{
    if($override)
    {
        (Remove-Website $websiteName -ErrorAction SilentlyContinue)
        (Remove-WebAppPool $appPoolName)
        (New-WebAppPool $appPoolName)
    }
}

#Create the website if it does not exist, or if it exists, act as per override parameter
$websitesFolderPath = "IIS:\Sites\"
$websitePath = $websitesFolderPath + $websiteName

# add bindings
$bindingsArray = @()
foreach ($item in $bindings) 
{
    $binding = {protocol=$item.Key; bindingInformation=$item.Value}
    $bindingsArray += $binding
}

$websiteCount = (Get-Item $websitePath -ErrorAction SilentlyContinue)
if($websiteCount -eq 0)
{
    (New-Item $websitePath -PhysicalPath $physicalPath -ApplicationPool $appPoolName -bindings $bindingsArray)
}
else
{
    if($override)
    {
        (Remove-Website $websiteName)
        (New-Item $websitePath -PhysicalPath $physicalPath -ApplicationPool $appPoolName -bindings $bindingsArray)
    }
}

return 1
