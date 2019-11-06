#Requires -Version 3.0
function VerifyMoveFiles()
{
	param([String]$targetPath)

	$targetItems = (Get-ChildItem -Path $targetPath -Recurse).FullName
    $count = $targetItems.Count
	if($count -gt 0)
	{
		return 1
	}
	else
	{
		return 0
	}
} 