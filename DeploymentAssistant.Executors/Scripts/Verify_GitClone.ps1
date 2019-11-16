#Requires -Version 3.0

# Script - Function - Verifying Git Clone a repository
# Uses only a URL of a git repo, and hence only either public repos can be cloned
# OR private repos can be cloned using personal access tokens if any.
# dependencies - git 
# example - token - http://oauth2:sLMzfseGvh1u2ERb4zM2@gitlab.company.com:8888/GroupOrUserName/Project.git
# example - public - http://gitlab.company.com/GroupOrUserName/Project.git
param([String]$localDestinationPath, [String]$repoUrl)

if(([string]::IsNullOrWhiteSpace($localDestinationPath)) -or  ([string]::IsNullOrWhiteSpace($repoUrl)))
{
    return 0   
}

#change directory
Set-Location -Path $localDestinationPath

$gitChildFolder = (Get-ChildItem -Path $localDestinationPath -Force -Recurse -Filter ".git")
if($gitChildFolder.Count -gt 0)
{
    $projectFolder = $gitChildFolder.Parent.FullName
    #change directory
    Set-Location -Path $projectFolder
    #get cloned repo's url
    $repoCloneUrl = (git config --get remote.origin.url)
    if($null -ne $repoCloneUrl)
    {
        $project = $repoUrl.Substring($repoUrl.LastIndexOf('/'))
        $clone = $repoCloneUrl.Substring($repoCloneUrl.LastIndexOf('/'))
        if($project -eq $clone)
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
else
{
    return 0
}