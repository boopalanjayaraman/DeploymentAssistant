#Requires -Version 3.0
# Script - function - DeleteFiles

param([String]$destinationPath)


$targetInfo = (Get-Item $destinationPath)
#check if source path exists
if($targetInfo.Count -eq 0)
{
    return 0
}



$isFile = $targetInfo -is [System.IO.FileInfo]
if($isFile)
{
    Remove-Item -Path $destinationPath -Force
    return 1
}
else
{
    $targetItems = (Get-ChildItem -Path $destinationPath).FullName
    if($targetItems.Count -eq 0)
    {
        return 0
    }
    $count = $targetItems.Count
    Foreach ($item in $targetItems)
    {
        Remove-Item -Path $item -Force -Recurse -Confirm:$false -ErrorAction SilentlyContinue
    }
    return $count
}
