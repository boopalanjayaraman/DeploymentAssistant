function DeleteFiles()
{
	param([String]$targetPath)

	$targetInfo = (Get-Item $targetPath)
    $isFile = $targetInfo -is [System.IO.FileInfo]
    if($isFile)
    {
        [System.IO.File]::De($targetPath)
        return 1
    }
    else
    {
        $targetItems = (Get-ChildItem -Path $targetPath -Recurse).FullName
        if($targetItems.Count -eq 0)
        {
            return 0
        }
        $count = $targetItems.Count
        [System.IO.Directory]::Delete($targetPath, $true)
        return $count
    }
} 