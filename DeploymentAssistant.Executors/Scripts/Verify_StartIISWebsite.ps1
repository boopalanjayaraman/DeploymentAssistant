#Requires -Version 3.0
#script - function - VerifyStartIISWebsite

param([String]$website)

$state = (Get-WebSite -Name $website).State
if($state -eq "Started")
{
    return 1
}
else
{
    return 0
}
