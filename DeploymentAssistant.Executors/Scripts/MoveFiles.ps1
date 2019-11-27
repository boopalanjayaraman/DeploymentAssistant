#Requires -Version 3.0
# Script - function - MoveFiles

param([String]$sourcePath, [String]$destinationPath, [bool]$addTimeStampForFolder = $false)

$sourceInfo = (Get-Item $sourcePath -Force)
$originalDestinationPath = $destinationPath

#check if source path exists
if(($null -eq $sourceInfo) -or  ($sourceInfo.Count -eq 0))
{
    throw "EXCEPTION: source path does not exist."
}

$isFile = $sourceInfo -is [System.IO.FileInfo]
if($isFile)
{
    #move the item
    Copy-Item -Path $sourcePath -Destination $destinationPath -Force -ErrorAction Stop
    Remove-Item -Path $sourcePath -Force
    return 1
}

#enter FOLDER flow.
$sourceItems = (Get-ChildItem -Path $sourcePath -Force).Name # **NOT** using Recurse flag
#validate the items
if($sourceItems.Count -eq 0)
{
    throw "EXCEPTION: source path does not have any children to move."
}
$count = $sourceItems.Count

#create the destination folder if it does not exist.
$destinationCurrent = (Get-Item $destinationPath -Force -ErrorAction SilentlyContinue)
if($null -eq $destinationCurrent)
{
    # folder does not exist, create it.
    # (Creating directory always returns output to the execution pipeline, and affects the return values. 
    # Hence suppressing them with Out-Null)
    New-Item -Path $destinationPath -ItemType "directory" -Force | Out-Null 
    # adding time stamp to folder only when we create it newly. If it was already existing, don't touch it.
    if($addTimeStampForFolder -eq $true)
    {
        #add timestamp to destination folder
        $currentItem = (Get-Item $destinationPath -Force)
        $suffix = (Get-Date -Format "yyyyMMdd_HHmmss_ff")
        $newName = "$($currentItem.Name)_$($suffix)"
        Rename-Item -Path $destinationPath -NewName $newName -Force
        $destinationPath = Join-Path "$($currentItem.Parent.FullName)" "$($newName)"
    }
}


#Start moving files
Foreach ($item in $sourceItems)
{
    $sourceItemPath = Join-Path $sourcePath $item
    $destinationItemPath = Join-Path $destinationPath $item
    Copy-Item -Path $sourceItemPath -Destination $destinationItemPath -Force -Recurse -ErrorAction Stop
    Remove-Item -Path $sourceItemPath -Force -Recurse -Confirm:$false -ErrorAction SilentlyContinue
}

[HashTable]$result = @{}
$result.Count = [int]$count
if($originalDestinationPath -ne $destinationPath)
{
    #if we created a new folder as part of this script, we need to send this destination path for verification.
    $result.DestinationPath = $destinationPath
}
return $result
