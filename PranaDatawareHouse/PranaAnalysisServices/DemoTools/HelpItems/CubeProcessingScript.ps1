    # #######################################################################
    # Author:  (http://www.ssas-info.com)
    # This script processes all dimensions in one specified database
    # Save this script to the file ProcessAllDim1.ps1. To execute script:
    # PowerShell.exe c:\scripts\ProcessAllDim1.ps1 -ServerName 192.168.1.19 -DBName 'PranaAnalysisServices' -ProcessType ProcessFull -PrintStatusBefore Y -PrintStatusAfter Y -Transactional Y -Parallel Y
    # -ProcessType : ProcessFull | ProcessUpdate
    # -PrintStatusBefore : Y | N; If value Y, then will print dimension status before starting re-processing. Default value N
    # -PrintStatusAfter  : Y | N; If value N, then will print dimension status AFTER finishing re-processing. Default value N
    # -Transactional     : Y | N; If value Y, then will do all dimension re-processing in one single transaction. Default value Y
    # -Parallel          : Y | N; If value Y, then will do dimension re-processing in parallel. Default value Y
    # Expected values for ProcessType: 'ProcessUpdate','ProcessFull'
    # This script was written and tested on SSAS 2008, but it should also work with SSAS 2005
    # #######################################################################
    param($ServerName="192.168.1.19", $DBName="PranaAnalysisServices", $ProcessType="ProcessFull", $PrintStatusBefore="N", $PrintStatusAfter="N",$Transactional="Y",$Parallel="Y")

    ## Add the AMO namespace
    $loadInfo = [Reflection.Assembly]::LoadWithPartialName("Microsoft.AnalysisServices")

    if ($Transactional -eq "Y") {$TransactionalB=$true} else {$TransactionalB=$false}
    if ($Parallel -eq "Y") {$ParallelB=$true} else {$ParallelB=$false}

    $server = New-Object Microsoft.AnalysisServices.Server
    $server.connect($ServerName)

    if ($server.name -eq $null) {
     Write-Output ("Server '{0}' not found" -f $ServerName)
     break
    }

    $DB = $server.Databases.FindByName($DBName)
    if ($DB -eq $null) {
     Write-Output ("Database '{0}' not found" -f $DBName)
     break
    }
    Write-Output("Load start time {0}" -f (Get-Date -uformat "%H:%M:%S") )
    Write-Output("----------------------------------------------------------------")
    Write-Output("Server  : {0}" -f $Server.Name)
    Write-Output("Database: {0}" -f $DB.Name)
    Write-Output("DB State: {0}" -f $DB.State)
    Write-Output("DB Size : {0}MB" -f ($DB.EstimatedSize/1024/1024).ToString("#,##0"))
    Write-Output("Process : {0}" -f $ProcessType)
    Write-Output("----------------------------------------------------------------")
    if ($PrintStatusBefore -eq "Y") {Write-Output("   Dimension status before processing")}

    $server.CaptureXml=$TRUE
    #Print dimension info (if set in the parameter) and submit for processing
    foreach ($dim in $DB.Dimensions) {
     if ($PrintStatusBefore -eq "Y") { Write-Output ( "Dimension: {0} Status: {1}" -f $dim.Name.PadRight(35), $dim.State) }
     if ($dim.MiningModel -eq $null) { # We will not reprocess dimensions related to data mining model
      $dim.Process($ProcessType)
     }
    } # Dimensions
     
    $server.CaptureXML = $FALSE

    Write-Output("----------------------------------------------------------------")
    Write-Output("Dimension processing started.   Time: {0}" -f (Get-Date -uformat "%H:%M:%S"))
    $Result = $server.ExecuteCaptureLog($TransactionalB,$ParallelB)
    Write-Output("Dimension processing completed. Time: {0}" -f (Get-Date -uformat "%H:%M:%S"))


    Write-Output("----------------------------------------------------------------")
    Write-Output("*** Warnings and errors ***")
    foreach ($res in $Result) {
     foreach ($msg in $res.Messages) {
      if ($msg.Description -ne $null) {
        Write-Output("{0}" -f $msg.Description)
      } 
     }
    }

    $Server.Refresh($true) # Refresh to get updated values

    if ($PrintStatusAfter -eq "Y") {
     Write-Output("----------------------------------------------------------------")
     Write-Output("                             Dimension status after reprocessing")
     foreach ($dim in $DB.Dimensions) {
      Write-Output ( "Dimension: {0} Status: {1}" -f $dim.Name.PadRight(35), $dim.State)
     }
    }
    Write-Output("----------------------------------------------------------------")
    Write-Output("Load end time {0}" -f (Get-Date -uformat "%H:%M:%S") )