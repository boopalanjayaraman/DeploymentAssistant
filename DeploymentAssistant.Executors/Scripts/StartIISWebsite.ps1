#Requires -Version 3.0
#Requires -RunAsAdministrator
# Script - function - StartIISWebsite

param([String]$website)

Start-WebSite -Name $website
 