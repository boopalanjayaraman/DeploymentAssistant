#Requires -Version 3.0
#Requires -RunAsAdministrator

# Script - Function - CreateIISWebsite

param([String]$websiteName, [System.Collections.HashTable]$bindings, [String]$physicalPath, [Boolean]$override = $false)

Import-Module "WebAdministration"

$appPoolName = $websiteName + 'Pool'

if($bindings.Count -le 0)
{
    throw "EXCEPTION: Bindings information is necessary to create the site."
}

#Return zero if the directory does not exist. It has to exist physically before creating websites.
#Creating the physical directory on-the-fly will lead to empty directory getting mapped, in case of mistakes.
$sourceItem = (Get-Item $physicalPath -Force)
if(($null -eq $sourceItem) -or ($sourceItem.Count -eq 0))
{
    throw "EXCEPTION: Physical path is mandatory."
}

#Create the pool if it does not exist, or if it exists, act as per override parameter
$poolState = $null
try
{
    $poolState = (Get-WebAppPoolState $appPoolName).Value
}
catch
{
    $poolState = $null
}

if($null -eq $poolState)
{
    (New-WebAppPool $appPoolName -Force)
}
else
{
    if($override -eq $true)
    {
        (Remove-Website $websiteName -ErrorAction SilentlyContinue)
        (Remove-WebAppPool $appPoolName -Force)
        (New-WebAppPool $appPoolName -Force)
    }
}
#Create the website if it does not exist, or if it exists, act as per override parameter
$websiteCount = 0
try {
    $websiteCount = (Get-Website $websiteName).Count
}
catch {
    $websiteCount = 0
}

if($websiteCount -eq 0)
{
    (New-WebSite -Name $websiteName -PhysicalPath $physicalPath -ApplicationPool $appPoolName -Force)
    foreach ($item in $bindings.Keys) 
    {
        $ip, $port, $hostheader = $bindings[$item].split(":")
        New-WebBinding -Name $websiteName -Protocol $item -IPAddress $ip -Port $port -HostHeader $hostheader -Force 
    }
}
else
{
    if($override -eq $true)
    {
        (Remove-Website $websiteName -Force)
        (New-WebSite -Name $websiteName -PhysicalPath $physicalPath -ApplicationPool $appPoolName -Force)
        foreach ($item in $bindings.Keys) 
        {
            $ip, $port, $hostheader = $bindings[$item].split(":")
            New-WebBinding -Name $websiteName -Protocol $item -IPAddress $ip -Port $port -HostHeader $hostheader -Force 
        }
    }
}

return 1
