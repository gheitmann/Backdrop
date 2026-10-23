@echo off
powershell.exe -NoProfile -STA -ExecutionPolicy Bypass -File "%~dp0Start-BackdropInstaller.ps1" %*
if errorlevel 1 pause
