@echo off
chcp 65001
set DEBUGSYMBOLS=DISABLE_RESULT_WINDOW
rem set DEBUGSYMBOLS=ENABLE_RESULT_WINDOW

echo 常駐アプリを終了しています...
taskkill /F /IM xopener-trans.exe

echo XOpener を生成しています...
"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /nologo /define:%DEBUGSYMBOLS% /t:winexe ./xopener.cs

echo レジストリを登録しています...
rem なぜか相対パスだと弾かれるので絶対パスにする
regedit /s %~dp0%xopener-full.reg

cd helpers

echo XOpener-Convert を生成しています...
"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /nologo /define:%DEBUGSYMBOLS% /t:winexe C:\xopener\helpers\xopener-convert.cs
if %errorlevel% neq 0 echo failed && pause && goto end

echo XOpener-Convert (HTML) を生成しています...
"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /nologo /define:%DEBUGSYMBOLS% /t:winexe C:\xopener\helpers\xopener-convert-html.cs
if %errorlevel% neq 0 echo failed && pause && goto end

echo XOpener-Trans を生成しています...
"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /nologo /define:%DEBUGSYMBOLS% /t:winexe C:\xopener\helpers\xopener-trans.cs
if %errorlevel% neq 0 echo failed && goto end

echo スタートアップ ショートカットを作成しています...
copy xopener-trans.lnk "%appdata%\Microsoft\Windows\Start Menu\Programs\Startup"
if %errorlevel% neq 0 echo failed && goto end

echo 常駐アプリを起動しています...
start xopener-trans.exe
if %errorlevel% neq 0 echo failed && goto end

echo すべての処理が完了しました

pause

:end