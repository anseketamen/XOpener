@echo off

"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /t:winexe ./xopener.cs
regedit /s %~dp0%xopener.reg

cd helpers

"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /t:winexe C:\xopener\helpers\xopener-convert.cs
if %errorlevel% neq 0 echo failed && pause && goto end

"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /t:winexe C:\xopener\helpers\xopener-trans.cs
if %errorlevel% neq 0 echo failed && goto end

copy xopener-trans.lnk "%appdata%\Microsoft\Windows\Start Menu\Programs\Startup"
start xopener-trans.exe

pause

:end