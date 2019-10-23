#Requires -Version 3.0
function GetFiles_Moved()
{
	param([String]$targetPath)

	$targetItems = (Get-ChildItem -Path $targetPath -Recurse).FullName
    $count = $targetItems.Count
    return $count
} 