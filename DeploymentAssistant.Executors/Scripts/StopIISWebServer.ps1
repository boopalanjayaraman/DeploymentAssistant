#Requires -Version 3.0
#Requires -RunAsAdministrator

# Script - function - StopIISWebServer()

Invoke-Command -ScriptBlock { iisreset /stop }
 