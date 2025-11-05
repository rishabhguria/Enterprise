strLink = "http://localhost:8080/drools-guvnor/org.drools.guvnor.Guvnor/backup"


'fldr = "C:\Nirvana\Drools-GuvnorRepositoryBackup\RepositoryBackups\"
fldr = CreateObject("Scripting.FileSystemObject").GetParentFolderName(WScript.ScriptFullName) &"\RepositoryBackups\"
'WScript.Echo fldr
Set oFSO = CreateObject("Scripting.FileSystemObject")

If Not oFSO.FolderExists( fldr) Then
Set objFolder = oFSO.CreateFolder(fldr)
End If 


' Get file name from URL.
' http://download.windowsupdate.com/microsoftupdate/v6/wsusscan/wsusscn2.cab -> wsusscn2.cab
strSaveName = Mid(strLink, InStrRev(strLink,"/") + 1, Len(strLink))
strSaveTo = fldr &"Repository" & strSaveName &"_" &Year(Date) &"-" &Month(Date) &"-" &Day(Date) &"_" &Hour(Now()) &"." &Minute(Now()) &"." &Second(Now()) &".zip"
'WScript.Echo Date
'WScript.Echo "HTTPDownload"
'WScript.Echo "-------------"
'WScript.Echo "Download: " & strLink
'WScript.Echo "Save to:  " & strSaveTo






' Create an HTTP object
Set objHTTP = CreateObject( "WinHttp.WinHttpRequest.5.1" )

' Download the specified URL
objHTTP.Open "GET", strLink, False
' Use HTTPREQUEST_SETCREDENTIALS_FOR_PROXY if user and password is for proxy, not for download the file.
objHTTP.SetCredentials "admin", "admin", HTTPREQUEST_SETCREDENTIALS_FOR_SERVER
objHTTP.Send

Set objFSO = CreateObject("Scripting.FileSystemObject")
If objFSO.FileExists(strSaveTo) Then
objFSO.DeleteFile(strSaveTo)
End If

If objHTTP.Status = 200 Then
Dim objStream
Set objStream = CreateObject("ADODB.Stream")
With objStream
.Type = 1 'adTypeBinary
.Open
.Write objHTTP.ResponseBody
.SaveToFile strSaveTo
.Close
End With
set objStream = Nothing
End If

If objFSO.FileExists(strSaveTo) Then
'WScript.Echo "Download `" & strSaveName & "` completed successfuly."
End If 