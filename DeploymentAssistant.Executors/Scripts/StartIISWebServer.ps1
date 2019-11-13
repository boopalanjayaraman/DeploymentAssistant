#Requires -Version 3.0
#Requires -RunAsAdministrator

# Script - function - StartIISWebServer

Invoke-Command -ScriptBlock { iisreset /start }
 