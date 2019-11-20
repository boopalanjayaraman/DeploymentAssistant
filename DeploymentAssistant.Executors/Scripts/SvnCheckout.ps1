#Requires -Version 3.0

# Script - Function - SVN checkout a SVN repository
# dependencies - svn 
param([String]$localDestinationPath, [String]$repoUrl, [String]$userName="", [String]$pwd="", [bool]$useCheckoutOrUpdate=$false)

if(([string]::IsNullOrWhiteSpace($localDestinationPath)) -or  ([string]::IsNullOrWhiteSpace($repoUrl)))
{
    throw "EXCEPTION: LocalDestinationPath / RepoURL are mandatory."  
}

$targetInfo = (Get-Item $localDestinationPath)
#check if source path exists
if(($null -eq $targetInfo) -or  ($targetInfo.Count -eq 0))
{
    throw "EXCEPTION: localDestinationPath does not exist."
}

#change directory
Set-Location -Path $localDestinationPath

$svnOutput = ""

if($useCheckoutOrUpdate -eq $false)
{
    #do only check out
    $svnOutput = (& "svn" "--non-interactive --username $userName --password $pwd checkout $repoUrl")
}
else
{
    #check and see if it is already under svn version control
    $svnChildFolder = (Get-ChildItem -Path $localDestinationPath -Force -Recurse -Filter ".svn")
    if($svnChildFolder.Count -gt 0)
    {
        $projectFolder = $svnChildFolder.Parent.FullName
        #change directory
        Set-Location -Path $projectFolder
        $svnOutput = (& "svn" "--non-interactive --username $userName --password $pwd update")
    }
    else
    {
        #run git clone
        $svnOutput = (& "svn" "--non-interactive --username $userName --password $pwd checkout $repoUrl")
    }
}

return 1