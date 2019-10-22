function CopyFiles()
{
	param([String]$sourcePath, [String]$destinationPath, [System.Collections.Generic.List[String]]$excludeExtensions, [System.Collections.Generic.List[String]]$skipFolders, [System.Collections.Generic.List[String]]$skipFoldersIfExist)

    $sourceInfo = (Get-Item $sourcePath -ErrorAction SilentlyContinue)
    #check if source path exists
    if($sourceInfo.Count -eq 0)
    {
        return 0
    }

    $isFile = $sourceInfo -is [System.IO.FileInfo]
    #if source is a file, then copy the file alone.
    if($isFile)
    {
        [System.IO.File]::Copy($sourcePath, $destinationPath, $true)
        return 1
    }

	$sourceItems = (Get-ChildItem -Path $sourcePath -Recurse).FullName
	#validate the items
	if($sourceItems.Count -eq 0)
	{
		return 0
    }
    $count = 0
	#perform copy action one by one
	Foreach ($item in $sourceItems)
	{
        $relativePath = $item.Replace($sourcePath, '')
        $info = (Get-Item $item)
        $destinationItemPath = [System.IO.Path]::Combine($destinationPath, $relativePath)

        $isFolder = $info -is [System.IO.DirectoryInfo]
        if($isFolder)
        {
            #skip this folder if it is in skipFolders list
            if($skipFolders.Contains($relativePath))
            {
                Continue
            }
            #skip this folder if it is in skipFolderIfExists list else create it.
            if($skipFoldersIfExist.Contains($relativePath) -and [System.IO.Directory]::Exists($destinationItemPath))
            {
                Continue
            } 
            else
            {
                [System.IO.Directory]::CreateDirectory($destinationItemPath)
                $count++
            }
        }
        else
        {
            #entering file flow
            $extension = $info.Extension
            if($excludeExtensions.Contains($extension))
            {
                Continue
            }
            #if it is part of the skip-folder list, continue
            Foreach ($folder in $skipFolders)
            {
                if($relativePath.StartsWith($folder))
                {
                    Continue
                }
            }
            #if file exists already, and the folder is added to skipIfAlreadyExists list, then skip.
            $fileExists = [System.IO.File]::Exists($destinationItemPath)
            if($fileExists)
            {
                Foreach ($folder in $skipFoldersIfExist)
                {
                    if($relativePath.StartsWith($folder))
                    {
                        Continue
                    }
                }
            }
            [System.IO.File]::Copy($item, $destinationItemPath, $true)
            $count++
        }
    }
    return $count
}