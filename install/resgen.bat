:: Business Accounting 
:: Small Business Accounting Solution
:: Copyright (C) 2014 Fuks Alexander
:: <mailto:kungfux2010@gmail.com>
::
:: This program is free software: you can redistribute it and/or modify
:: it under the terms of the GNU General Public License as published by
:: the Free Software Foundation, either version 3 of the License, or
:: (at your option) any later version.
::
:: This program is distributed in the hope that it will be useful,
:: but WITHOUT ANY WARRANTY; without even the implied warranty of
:: MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
:: GNU General Public License for more details.
::
:: You should have received a copy of the GNU General Public License
:: along with this program.  If not, see <http://www.gnu.org/licenses/>.

:: This batch program should be used to generate language resources

@echo off
echo Business Accounting
echo Copyright (C) 2014 Fuks Alexander 
echo ^<mailto:kungfux2010@gmail.com^>
echo This program comes with ABSOLUTELY NO WARRANTY;
echo This is free software, and you are welcome to redistribute it under certain conditions.
echo.

:: ResGen.exe tool from SDK
set resgen="C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\ResGen.exe"
:: ResGen options
set resgenkeys=/compile

:: Check for missing tool
if not exist %resgen% set noresgen=1
if [%noresgen%]==[1]  echo STOP! resgen.exe was not found at %resgen%
if [%noresgen%]==[1]  echo Please edit path to SDK if needed
if [%noresgen%]==[1]  exit /B 1

:: Check that at least 1 input file was specified
if [%1]==[] set nofiles=1
if [%nofiles%]==[1] echo STOP! no input files were specified
if [%nofiles%]==[1] goto Help

if [%1]==[*] goto CompileAll

:: Compile resources
%resgen% %resgenkeys% %1 %2 %3 %4 %5 %6 %7 %8 %9
goto :EOF

:: Compile all available text files in lang dir
:CompileAll
for /f "tokens=*" %%G in ('dir /b "lang\*.txt"') do %resgen% %resgenkeys% "lang\\"%%G
goto :EOF

:: Display help
:Help
echo Syntax:
echo  resgen [file] - compile specified file;
echo  resgen *      - compile all files.
echo.
echo Example:
echo  resgen lang\en-US.txt
echo.
pause
goto :EOF