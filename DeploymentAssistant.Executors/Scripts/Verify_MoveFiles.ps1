#Requires -Version 3.0
#script - function - VerifyMoveFiles

param([String]$destinationPath)

$targetItems = (Get-ChildItem -Path $destinationPath -Recurse).FullName
$count = $targetItems.Count

$sourceItems = (Get-ChildItem -Path $sourcePath -Recurse).FullName
#validate the items
$sourceItemsNotMoved = 0
if($sourceItems.Count -gt 0)
{
	$sourceItemsNotMoved = 1
}

if(($count -gt 0) -and ($sourceItemsNotMoved -eq 0))
{
	return 1
}
else
{
	return 0
}
