function StartIISWebServer()
{
	Invoke-Command -ScriptBlock { iisreset /start }
} 