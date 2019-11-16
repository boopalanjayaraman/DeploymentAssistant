#Requires -Version 3.0

# Script - Function - MSBuild 
# dependencies - msbuild 
param([String]$localMsBuildPath, [String]$solutionPath)

if(([string]::IsNullOrWhiteSpace($localMsBuildPath)) -or  ([string]::IsNullOrWhiteSpace($solutionPath)))
{
    return 0   
}

if(!$localMsBuildPath.EndsWith('MSBuild.exe', "CurrentCultureIgnoreCase"))
{
    return 0 
}

$targetInfo = (Get-Item $solutionPath)
#check if source path exists
if(($null -eq $targetInfo) -or  ($targetInfo.Count -eq 0))
{
    return 0
}

$msBuildItem = (Get-Item $localMsBuildPath)
#check if ms build path exists
if(($null -eq $msBuildItem) -or  ($msBuildItem.Count -eq 0))
{
    return 0
}

#invoke build using & operator
$result = & "$($localMsBuildPath)" "$($solutionPath)"

if($result -Contains "Build Failed.")
{
    return 0
}
else
{
    return 1
}