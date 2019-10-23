#Requires -Version 3.0
#Requires -RunAsAdministrator

function StopIISWebServer()
{
	Invoke-Command -ScriptBlock { iisreset /stop }
} 