function StopIISWebsite()
{
	param([String]$website)

	Stop-WebSite -Name $website
} 