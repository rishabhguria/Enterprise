$logFile = "C:\Nirvana\Backoffice Automation\AltaDailyRunLog.txt"

# Check if the log file exists, if not create it
if (-not (Test-Path $logFile)) {
    New-Item -Path $logFile -ItemType File
}

$today = Get-Date -Format "yyyy-MM-dd"
$lastRunDate = Get-Content $logFile -Raw
$lastRunDate = $lastRunDate.Trim()
if ($today -eq $lastRunDate) {
    # Exit the script if the task has already run today
    exit 0

}
# Update the log file with today's date
Set-Content $logFile $today

#Invoke-WebRequest -Uri "https://prod-103.westus.logic.azure.com/workflows/ae700caf0e6c417896849cf2f452d7a3/triggers/manual/paths/invoke/Alta?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=hiEVKOKIE2L_PBQQiP2yOLHJfjwGcpLXw6_PNSzZdvE" -Method POST
