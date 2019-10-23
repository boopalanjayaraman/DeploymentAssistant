#Requires -Version 3.0
function GetIISWebsite_Stopped()
{
	param([String]$website)

    $state = (Get-WebSite -Name $website).State
    if($state -eq "Stopped")
    {
        return $true
    }
    else
    {
        return $false
    }
} 