#Requires -Version 3.0
#script - function - VerifyCopyFiles
#only a simple version

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
