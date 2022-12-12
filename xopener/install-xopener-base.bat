@echo off
chcp 65001

"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /nologo /t:winexe ./xopener.cs

regedit /s %~dp0%xopener-pico.reg

pause