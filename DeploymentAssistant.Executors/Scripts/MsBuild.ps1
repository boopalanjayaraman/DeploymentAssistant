#Requires -Version 3.0

# Script - Function - MSBuild 
# dependencies - msbuild 
param([String]$localMsBuildPath, [String]$solutionPath)

if(([string]::IsNullOrWhiteSpace($localMsBuildPath)) -or  ([string]::IsNullOrWhiteSpace($solutionPath)))
{
    throw "EXCEPTION: LocalMSBuildPath / SolutionPath are mandatory."
}

if(!$localMsBuildPath.EndsWith('MSBuild.exe', "CurrentCultureIgnoreCase"))
{
    throw "EXCEPTION: LocalMSBuildPath does not indicate an MsBuild executable"
}

$targetInfo = (Get-Item $solutionPath)
#check if source path exists
if(($null -eq $targetInfo) -or  ($targetInfo.Count -eq 0))
{
    throw "EXCEPTION: SolutionPath does not exist."
}

$msBuildItem = (Get-Item $localMsBuildPath)
#check if ms build path exists
if(($null -eq $msBuildItem) -or  ($msBuildItem.Count -eq 0))
{
    throw "EXCEPTION: LocalMSBuildPath does not exist."
}

#invoke build using & operator
$result = & "$($localMsBuildPath)" "$($solutionPath)"

if($result -Contains "Build Failed.")
{
    throw "EXCEPTION: Build FAILED."
}
else
{
    return 1
}