#Requires -Version 3.0
#Requires -RunAsAdministrator
# Script - function - StopIISWebsite()

param([String]$website)

Stop-WebSite -Name $website
 