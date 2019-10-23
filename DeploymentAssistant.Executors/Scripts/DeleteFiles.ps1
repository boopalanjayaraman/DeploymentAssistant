#Requires -Version 3.0
function DeleteFiles()
{
	param([String]$targetPath)

    $targetInfo = (Get-Item $targetPath -ErrorAction SilentlyContinue)
     #check if source path exists
     if($targetInfo.Count -eq 0)
     {
         return 0
     }

    $isFile = $targetInfo -is [System.IO.FileInfo]
    if($isFile)
    {
        [System.IO.File]::Delete($targetPath)
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