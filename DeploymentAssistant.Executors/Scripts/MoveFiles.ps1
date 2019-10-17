function MoveFiles()
{
	param([String]$sourcePath, [String]$destinationPath)

	$sourceItems = (Get-ChildItem -Path $sourcePath -Recurse).FullName
	
	#validate the items
	if($sourceItems.Count -eq 0)
	{
		return
	}

    $count = $sourceItems.Count

	#perform copy action one by one
	Foreach ($item in $sourceItems)
	{
        $relativePath = $item.Replace($sourcePath, '')
        $info = (Get-Item $item)
        $destinationItemPath = [System.IO.Path]::Combine($destinationPath, $relativePath)

        $isFolder = $info -is [System.IO.DirectoryInfo]
        if($isFolder)
        {
            [System.IO.Directory]::Move($item, $destinationItemPath)
        }
        else
        {
            [System.IO.File]::Copy($item, $destinationItemPath, $true)
        }
    }
    
    return $count
} 