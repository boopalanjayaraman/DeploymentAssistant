function StopIISWebServer()
{
	Invoke-Command -ScriptBlock { iisreset /stop }
} 