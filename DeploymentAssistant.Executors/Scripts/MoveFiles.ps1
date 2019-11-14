#Requires -Version 3.0
# Script - function - MoveFiles

param([String]$sourcePath, [String]$destinationPath)

$sourceInfo = (Get-Item $sourcePath -ErrorAction SilentlyContinue)
#check if source path exists
if($sourceInfo.Count -eq 0)
{
    return 0
}

$isFile = $sourceInfo -is [System.IO.FileInfo]
if($isFile)
{
    #[System.IO.File]::Move($sourcePath, $destinationPath, $true)
    Copy-Item -Path $sourcePath -Destination $destinationPath -Force -ErrorAction Stop
    Remove-Item -Path $sourcePath
    return 1
}
else
{
    $sourceItems = (Get-ChildItem -Path $sourcePath -Recurse).FullName
    #validate the items
    if($sourceItems.Count -eq 0)
    {
        return 0
    }
    $count = $sourceItems.Count
    #[System.IO.Directory]::Move($sourcePath, $destinationPath)
    #Move-Item -Path $sourcePath -Destination $destinationPath -Force
    Foreach ($item in $sourceItems)
    {
        $sourceItemPath = Join-Path $sourcePath $item
        $destinationItemPath = Join-Path $destinationPath $item
        Copy-Item -Path $sourceItemPath -Destination $destinationItemPath -Force -ErrorAction Stop
        Remove-Item -Path $sourceItemPath -Force
    }
    
    return $count
}
