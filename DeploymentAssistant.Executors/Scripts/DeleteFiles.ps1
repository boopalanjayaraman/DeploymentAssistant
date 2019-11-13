#Requires -Version 3.0
# Script - function - DeleteFiles

param([String]$destinationPath)

$targetInfo = (Get-Item $destinationPath -ErrorAction SilentlyContinue)
    #check if source path exists
    if($targetInfo.Count -eq 0)
    {
        return 0
    }

$isFile = $targetInfo -is [System.IO.FileInfo]
if($isFile)
{
    [System.IO.File]::Delete($destinationPath)
    return 1
}
else
{
    $targetItems = (Get-ChildItem -Path $destinationPath -Recurse).FullName
    if($targetItems.Count -eq 0)
    {
        return 0
    }
    $count = $targetItems.Count
    [System.IO.Directory]::Delete($destinationPath, $true)
    return $count
}
