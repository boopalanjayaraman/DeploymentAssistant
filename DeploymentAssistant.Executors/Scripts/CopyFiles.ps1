#Requires -Version 3.0

#Script - Function - CopyFiles

param([String]$sourcePath, [String]$destinationPath, [String[]]$excludeExtensions=$null, [String[]]$skipFolders=$null, [String[]]$skipFoldersIfExist=$null)

Write-Verbose "Running as $env:username"

$sourceInfo = (Get-Item $sourcePath -Force)

#check if source path exists
if(($null -eq $sourceInfo) -or  ($sourceInfo.Count -eq 0))
{
    throw "EXCEPTION: source path does not exist."
}

$isFile = $sourceInfo -is [System.IO.FileInfo]
#if source is a file, then copy the file alone.
if($isFile)
{
    #[System.IO.File]::Copy($sourcePath, $destinationPath, $true)
    Copy-Item -Path $sourcePath -Destination $destinationPath -Force
    return 1
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
        #Write-Verbose "Creating Directory $destinationItemPath" #-Verbose
        #Write-Verbose "destinationPath: $destinationPath, relativePath: $relativePath" #-Verbose
        New-Item -ItemType "directory" -Path $destinationItemPath -Force -ErrorAction Stop
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
        #Write-Verbose "Copying Item from $item to $destinationItemPath" -Verbose
        #Write-Verbose "destinationPath: $destinationPath, relativePath: $relativePath" -Verbose
        Copy-Item -Path $item -Destination $destinationItemPath -Force -ErrorAction Stop 
        $count++
    }
}
return $count
