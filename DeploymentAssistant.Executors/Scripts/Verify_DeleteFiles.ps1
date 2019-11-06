#Requires -Version 3.0
function VerifyDeleteFiles()
{
	param([String]$targetPath)

	$targetItems = (Get-Item -Path $targetPath -ErrorAction SilentlyContinue).FullName
    $count = $targetItems.Count
	if($count -eq 0)
	{
		return 1
	}
	else
	{
		return 0
	}
} 