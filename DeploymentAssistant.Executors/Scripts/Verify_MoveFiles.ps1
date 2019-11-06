#Requires -Version 3.0
function VerifyMoveFiles()
{
	param([String]$destinationPath)

	$targetItems = (Get-ChildItem -Path $destinationPath -Recurse).FullName
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