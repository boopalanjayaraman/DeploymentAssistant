#Requires -Version 3.0
function VerifyStopIISWebsite()
{
	param([String]$website)

    $state = (Get-WebSite -Name $website).State
    if($state -eq "Stopped")
    {
        return 1
    }
    else
    {
        return 0
    }
} 