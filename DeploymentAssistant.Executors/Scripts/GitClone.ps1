#Requires -Version 3.0

# Script - Function - Git Clone a repository
# Uses only a URL of a git repo, and hence only either public repos can be cloned
# OR private repos can be cloned using personal access tokens if any.
# dependencies - git 
# example - token - http://oauth2:sLMzfseGvh1u2ERb4zM2@gitlab.company.com:8888/GroupOrUserName/Project.git
# example - public - http://gitlab.company.com/GroupOrUserName/Project.git
param([String]$localDestinationPath, [String]$repoUrl, [bool]$useCloneOrPull=$false)

if(([string]::IsNullOrWhiteSpace($localDestinationPath)) -or  ([string]::IsNullOrWhiteSpace($repoUrl)))
{
    throw "EXCEPTION: Local destination path / Repo Url are mandatory parameters."   
}

$targetInfo = (Get-Item $localDestinationPath)
#check if source path exists
if(($null -eq $targetInfo) -or  ($targetInfo.Count -eq 0))
{
    throw "EXCEPTION: Local destination path does not exist."
}

#change directory
Set-Location -Path $localDestinationPath

if($useCloneOrPull -eq $false)
{
    #run git clone
    git clone $repoUrl --quiet
}
else
{
    #check and see if it is already under .git
    $gitChildFolder = (Get-ChildItem -Path $localDestinationPath -Force -Recurse -Filter ".git")
    if($gitChildFolder.Count -gt 0)
    {
        $projectFolder = $gitChildFolder.Parent.FullName
        #change directory
        Set-Location -Path $projectFolder
        git pull $repoUrl --quiet
    }
    else
    {
        #run git clone
        git clone $repoUrl --quiet
    }
}
#check last exit code
if($LASTEXITCODE -eq 0)
{
    return 1
}
else
{
    return 0
}


