#Requires -Version 3.0

# Script - Function - MSBuild 
# dependencies - msbuild 
param([String]$localMsBuildPath, [String]$solutionPath, [String]$buildTargets = "", [String]$buildProperties = "")

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

#set build targets
if([string]::IsNullOrWhiteSpace($buildTargets))
{
    $buildTargets = "-t:Build"
}
else
{
    $buildTargets = ("-t:{0}" -f $buildTargets)
}

#set build properties
if([string]::IsNullOrWhiteSpace($buildProperties))
{
    $buildProperties = "-p:Configuration=Release"
}
else
{
    $buildProperties = ("-p:{0}" -f $buildProperties)
}

#invoke build using & operator
$result = & "$($localMsBuildPath)" "$($solutionPath)" "$buildTargets" "$buildProperties" "-v:q" #verbosity:quiet

#check last exit code
if($LASTEXITCODE -ne 0)
{
    throw ("EXCEPTION: Build FAILED. Message - $($result)")
}
else
{
    return 1
}