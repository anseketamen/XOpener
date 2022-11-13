@echo off

"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /t:winexe ./xopener.cs

regedit /s %~dp0%xopener.reg

pause