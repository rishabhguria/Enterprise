@echo off


rmdir /s /q buffers

rmdir /s /q logs

del /f AllFixLog.log

Start buy.bat

Start sell.bat