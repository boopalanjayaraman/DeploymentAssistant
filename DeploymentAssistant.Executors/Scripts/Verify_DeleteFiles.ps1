function GetFiles_Deleted()
{
	param([String]$targetPath)

	$targetItems = (Get-ChildItem -Path $targetPath -Recurse).FullName
    $count = $targetItems.Count
    return $count
} 