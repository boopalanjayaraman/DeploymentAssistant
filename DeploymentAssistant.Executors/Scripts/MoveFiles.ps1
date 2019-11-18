#Requires -Version 3.0
# Script - function - MoveFiles

param([String]$sourcePath, [String]$destinationPath)

$sourceInfo = (Get-Item $sourcePath)
#check if source path exists
if(($null -eq $sourceInfo) -or  ($sourceInfo.Count -eq 0))
{
    return 0
}

$isFile = $sourceInfo -is [System.IO.FileInfo]
if($isFile)
{
    Copy-Item -Path $sourcePath -Destination $destinationPath -Force -ErrorAction Stop
    Remove-Item -Path $sourcePath -Force
    return 1
}
else
{
    $sourceItems = (Get-ChildItem -Path $sourcePath -Force).Name #Not using Recurse flag
    #validate the items
    if($sourceItems.Count -eq 0)
    {
        return 0
    }
    $count = $sourceItems.Count
    Foreach ($item in $sourceItems)
    {
        $sourceItemPath = Join-Path $sourcePath $item
        $destinationItemPath = Join-Path $destinationPath $item
        Copy-Item -Path $sourceItemPath -Destination $destinationItemPath -Force -Recurse -ErrorAction Stop
        Remove-Item -Path $sourceItemPath -Force -Recurse -Confirm:$false -ErrorAction SilentlyContinue
    }
    
    return $count
}
