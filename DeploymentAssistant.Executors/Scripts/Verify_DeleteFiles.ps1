#Requires -Version 3.0
# Script - function - VerifyDeleteFiles

param([String]$destinationPath)

$targetItems = (Get-ChildItem -Path $destinationPath -ErrorAction SilentlyContinue).FullName
$count = $targetItems.Count
if($null -ne $targetItems)
{
	return 0
}
if($count -eq 0)
{
	return 1
}
else
{
	return 0
}
