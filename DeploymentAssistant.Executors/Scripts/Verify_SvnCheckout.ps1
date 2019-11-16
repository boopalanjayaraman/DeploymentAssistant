#Requires -Version 3.0

# Script - Function - Verifying SVN checkout a repository
# dependencies - svn
param([String]$localDestinationPath, [String]$repoUrl)

if(([string]::IsNullOrWhiteSpace($localDestinationPath)) -or  ([string]::IsNullOrWhiteSpace($repoUrl)))
{
    return 0   
}

#change directory
Set-Location -Path $localDestinationPath

$svnChildFolder = (Get-ChildItem -Path $localDestinationPath -Force -Recurse -Filter ".svn")
if($svnChildFolder.Count -gt 0)
{
    $projectFolder = $svnChildFolder.Parent.FullName
    #change directory
    Set-Location -Path $projectFolder
    #get checkout repo's url
    $repoCheckoutUrl = (svn info --show-item relative-url)
    if($null -ne $repoCheckoutUrl)
    {
        $project = $repoUrl.Substring($repoUrl.LastIndexOf('/'))
        $checkout = $repoCheckoutUrl.Substring($repoCheckoutUrl.LastIndexOf('/'))
        if($project -eq $checkout)
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
 
return 1