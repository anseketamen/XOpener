@echo off

"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /t:winexe C:\xopener\helpers\XOpener-Convert.cs
if %errorlevel% neq 0 echo failed && pause && goto end

"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /t:winexe C:\xopener\helpers\XOpener-Trans.cs
if %errorlevel% neq 0 echo failed && goto end

copy XOpener-Trans.lnk "%appdata%\Microsoft\Windows\Start Menu\Programs\Startup"
start XOpener-Trans.exe

pause

:end