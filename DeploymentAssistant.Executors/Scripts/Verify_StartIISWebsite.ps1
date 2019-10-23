#Requires -Version 3.0
function GetIISWebsite_Started()
{
	param([String]$website)

    $state = (Get-WebSite -Name $website).State
    if($state -eq "Started")
    {
        return $true
    }
    else
    {
        return $false
    }
} 