#Requires -Version 3.0
#Requires -RunAsAdministrator
function StartIISWebsite()
{
	param([String]$website)

	Start-WebSite -Name $website
} 