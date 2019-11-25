#Requires -Version 3.0

#Script - Function - CopyFiles

param([String]$sourcePath, [String]$destinationPath, [String[]]$excludeExtensions=$null, [String[]]$skipFolders=$null, [String[]]$skipFoldersIfExist=$null, [bool]$addTimeStampForFolder = $false)

$sourceInfo = (Get-Item $sourcePath -Force)
$originalDestinationPath = $destinationPath

#check if source path exists
if(($null -eq $sourceInfo) -or  ($sourceInfo.Count -eq 0))
{
    throw "EXCEPTION: source path does not exist."
}

$isFile = $sourceInfo -is [System.IO.FileInfo]
#if source is a file, then copy the file alone.
if($isFile)
{
    Copy-Item -Path $sourcePath -Destination $destinationPath -Force
    return 1
}

#enter FOLDER flow.
#create the destination folder if it does not exist.
$destinationCurrent = (Get-Item $destinationPath -Force -ErrorAction SilentlyContinue)
if($null -eq $destinationCurrent)
{
    #folder does not exist, create it.
    New-Item -Path $destinationPath -ItemType "directory" -Force | Out-Null 
    # Creating directory always returns output to the execution pipeline, and affects the return values. 
    # Hence suppressing them with Out-Null
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

$sourceItems = (Get-ChildItem -Path $sourcePath -Recurse -Force).FullName
#validate the items
if($sourceItems.Count -eq 0)
{
    throw "EXCEPTION: source children items count: 0."
}

$count = 0
#perform copy action one by one
Foreach ($item in $sourceItems)
{
    $relativePath = [Regex]::Replace($item, [regex]::Escape($sourcePath), '', [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
    
    $info = (Get-Item $item -Force)
    $destinationItemPath = Join-Path $destinationPath $relativePath

    $isFolder = $info -is [System.IO.DirectoryInfo]
    if($isFolder)
    {
        #skip this folder if it is in skipFolders list
        if($null -ne $skipFolders)
        {
            if($skipFolders -Contains $relativePath)
            {
                Continue
            }
            $isPartOfSkipFolder = $false
            #if it is a sub folder that is part of a skip-folder
            Foreach ($folder in $skipFolders)
            {
                $folderPath = Join-Path $folder "\"
                if($relativePath.StartsWith($folderPath, "CurrentCultureIgnoreCase"))
                {
                    $isPartOfSkipFolder = $true
                    break
                }
            }
            if($isPartOfSkipFolder)
            {
                continue
            }
        }
        #skip this folder if it is in skipFolderIfExists list else create it.
        if($null -ne $skipFoldersIfExist) 
        {
            if(($skipFoldersIfExist -Contains $relativePath) -and ([System.IO.Directory]::Exists($destinationItemPath)))
            {
                Continue
            }
            $isPartOfSkipFolder = $false 
            Foreach ($folder in $skipFoldersIfExist)
            {
                $folderPath = Join-Path $folder "\"
                if($relativePath.StartsWith($folderPath, "CurrentCultureIgnoreCase"))
                {
                    $isPartOfSkipFolder = $true
                    break
                }
            }
            if($isPartOfSkipFolder)
            {
                continue
            }
        } 
        #Create the directory 
        New-Item -ItemType "directory" -Path $destinationItemPath -Force -ErrorAction Stop | Out-Null 
        # Creating directory always returns output to the execution pipeline, and affects the return values. 
        # Hence suppressing them with Out-Null
        $count++
        
    }
    else
    {
        #entering file flow
        $extension = $info.Extension
        if(($null -ne $excludeExtensions) -and ($excludeExtensions -Contains $extension))
        {
            Continue
        }
        if($null -ne $skipFolders)
        {
            $isPartOfSkipFolder = $false 
            #if it is part of the skip-folder list, continue
            Foreach ($folder in $skipFolders)
            {
                $folderPath = Join-Path $folder "\"
                if($relativePath.StartsWith($folderPath, "CurrentCultureIgnoreCase"))
                {
                    $isPartOfSkipFolder = $true 
                    break
                }
            }
            if($isPartOfSkipFolder)
            {
                continue
            }
        }
        
        #if file exists already, and the folder is added to skipIfAlreadyExists list, then skip.
        $fileExists = [System.IO.File]::Exists($destinationItemPath)
        if($fileExists)
        {
            if($null -ne $skipFoldersIfExist)
            {
                $isPartOfSkipFolder = $false
                Foreach ($folder in $skipFoldersIfExist)
                {
                    $folderPath = Join-Path $folder "\"
                    if($relativePath.StartsWith($folderPath, "CurrentCultureIgnoreCase"))
                    {
                        $isPartOfSkipFolder = $true
                        break
                    }
                }
                if($isPartOfSkipFolder)
                {
                    continue
                }
            }
        }
        #Copy the file 
        Copy-Item -Path $item -Destination $destinationItemPath -Force -ErrorAction Stop 
        $count++
    }
}

[HashTable]$result = @{}
$result.Count = [int]$count
if($originalDestinationPath -ne $destinationPath)
{
    #if we created a new folder as part of this script, we need to send this destination path for verification.
    $result.DestinationPath = $destinationPath
}
return $result
