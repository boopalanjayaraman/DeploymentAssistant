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

$credentialsEmpty = [string]::IsNullOrWhiteSpace($userName) -or [string]::IsNullOrWhiteSpace($pwd)
$svnOutput = ""

if($useCheckoutOrUpdate -eq $false)
{
    #do only check out
    if($credentialsEmpty -eq $true)
    {
        $svnOutput = (& "svn" "--non-interactive" "checkout" """$repoUrl""")
    }
    else
    {
        $svnOutput = (& "svn" "--non-interactive" "--username" "$userName" "--password" "$pwd" "checkout" """$repoUrl""")
    }
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
        if($credentialsEmpty -eq $true)
        {
            $svnOutput = (& "svn" "--non-interactive" "update")
        }
        else
        {
            $svnOutput = (& "svn" "--non-interactive" "--username" "$userName" "--password" "$pwd" "update")
        }
    }
    else
    {
        #run svn checkout
        if($credentialsEmpty -eq $true)
        {
            $svnOutput = (& "svn" "--non-interactive" "checkout" """$repoUrl""")
        }
        else
        {
            $svnOutput = (& "svn" "--non-interactive" "--username" "$userName" "--password" "$pwd" "checkout" """$repoUrl""")
        }
    }
}

#check last exit code
if($LASTEXITCODE -eq 0)
{
    return 1
}
else 
{
    throw ("EXCEPTION: $($svnOutput)")
}
