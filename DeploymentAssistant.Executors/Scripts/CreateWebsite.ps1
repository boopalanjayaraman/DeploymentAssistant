function CreateWebsite()
{
    param([String]$websiteName, [System.Collections.Generic.Dictionary[string, string]]$bindings, [String]$physicalPath, [Boolean]$override = $false)

    Import-Module "WebAdministration"

    $appPoolName = $websiteName + 'Pool'
    $appPoolsFolderPath = "IIS:\AppPools\"
    $appPoolPath =  $appPoolsFolderPath + $appPoolName

    #Create the directory if it does not exist. 
    if(![System.IO.Directory]::Exists($physicalPath))
    {
        (New-Item $physicalPath -type Directory)
    }
    
    #Create the pool if it does not exist, or if it exists, act as per override parameter
    $poolCount = (Get-Item $appPoolPath -ErrorAction SilentlyContinue).Count
    if($poolCount -eq 0)
    {
        (New-Item $appPoolPath)
    }
    else
    {
        if($override)
        {
            (Remove-WebAppPool $appPoolName)
            (New-Item $appPoolPath)
        }
    } 




}