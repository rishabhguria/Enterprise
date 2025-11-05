@echo off

REM When we will start inserting data in SM DB from client we will have to shrink the database before detaching

SSEUtil.exe -d name=NirvanaClientSecurityMaster.mdf
del .\SecurityMasterDB\NirvanaClientSecurityMaster_log.LDF /f/q
@echo on