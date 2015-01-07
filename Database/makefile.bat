:: This batch program should be used to build the database

@echo off

:: Configuration parameters
set newdbfile=ba.sqlite
set dbinitsql=initdb.sql
set dbfillsql=filldb.sql
set sqlite3=sqlite3.exe

:: Check for missing files
if not exist %dbinitsql% set nodbinitsql=1
if [%nodbinitsql%]==[1]  echo STOP! file was not found: %dbinitsql%
if [%nodbinitsql%]==[1]  exit /B 1

if not exist %dbinitsql% set nodbfillsql=1
if [%nodbfillsql%]==[1]  echo STOP! file was not found: %dbfillsql%
if [%nodbfillsql%]==[1]  exit /B 1

if not exist %sqlite3% set nosqlite=1
if [%nosqlite%]==[1]   echo STOP! sqlite3 util was not found at %sqlite3%
if [%nosqlite%]==[1]   exit /B 1

:: Check command line arguments
if [%1%]==[database-init] goto DatabaseInit
if [%1%]==[database-fill] goto DatabaseFill
if [%1%]==[database-all]  goto DatabaseInit
if [%1%]==[clean]         goto Clean

:: Display help
:Help
echo Available options:
echo ^ database-init  Initialize new database;
echo ^ database-fill  Fill database with data samples;
echo ^ database-all   Initialize and fill database (both options);
echo ^ clean          Delete all generated/copied files
echo.
echo Usage examples:
echo ^ makefile database-init
echo ^ makefile clean
pause
goto :EOF

:: Init new db
:DatabaseInit
echo Init new database ...
%sqlite3% %newdbfile% < %dbinitsql%
if [%1%]==[database-all] goto DatabaseFill
goto :EOF

:: Fill db
:DatabaseFill
echo Fill database with samples ...
%sqlite3% %newdbfile% < %dbfillsql%
goto :EOF

:: Cleanup workspace
:Clean
echo Clean up ...
del /Q %newdbfile%
goto :EOF