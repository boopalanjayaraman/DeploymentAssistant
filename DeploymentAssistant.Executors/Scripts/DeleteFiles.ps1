#Requires -Version 3.0
# Script - function - DeleteFiles

param([String]$destinationPath)


$targetInfo = (Get-Item $destinationPath)
#check if source path exists
if(($null -eq $targetInfo) -or  ($targetInfo.Count -eq 0))
{
    throw "EXCEPTION: destination path does not exist."
}



$isFile = $targetInfo -is [System.IO.FileInfo]
if($isFile)
{
    Remove-Item -Path $destinationPath -Force
    return 1
}
else
{
    $targetItems = (Get-ChildItem -Path $destinationPath -Force).FullName
    if($targetItems.Count -eq 0)
    {
        throw "EXCEPTION: destination path folder does not have any children."
    }
    $count = $targetItems.Count
    Foreach ($item in $targetItems)
    {
        Remove-Item -Path $item -Force -Recurse -Confirm:$false -ErrorAction SilentlyContinue
    }
    return $count
}
