#Requires -Version 3.0
#Requires -RunAsAdministrator

function StartIISWebServer()
{
	Invoke-Command -ScriptBlock { iisreset /start }
} 