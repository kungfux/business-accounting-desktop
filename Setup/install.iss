; ****************************************
; This file is a project for Inno Setup.
; http://www.jrsoftware.org/isinfo.php
; 
; Note: Use Unicode version of Inno Setup.
; ****************************************

#define AppName 'Business Accounting'
#define AppVersion '1.4'

#define AppExecutable 'Business Accounting.exe'
#define CompiledBinPath '..\BusinessAccounting\BusinessAccounting\bin\Release'
#define InitDatabaseFilePath '..\Database'

[Setup]
AppId={{88E31457-D99B-4590-9522-01DDD5C00D6D}}
AppName={#AppName}
AppVersion={#AppVersion}
AppVerName={#AppName} {#AppVersion}
VersionInfoVersion={#AppVersion}
VersionInfoCompany=Alexander Fuks
AppCopyright=Copyright © 2014-2017 Alexander Fuks
AppPublisher=Alexander Fuks
AppPublisherURL=https://github.com/kungfux/business-accounting
AppSupportURL=https://github.com/kungfux/business-accounting
AppUpdatesURL=https://github.com/kungfux/business-accounting
DefaultDirName={pf}\{#AppName}
DefaultGroupName={#AppName}
SetupIconFile=..\BusinessAccounting\BusinessAccounting\Resources\favicon.ico
UninstallDisplayIcon={app}\{#AppExecutable}
OutputDir=.
OutputBaseFilename={#AppName} Setup {#AppVersion}
CloseApplications=yes
AllowNoIcons=yes
SetupLogging=yes
PrivilegesRequired=admin
ArchitecturesAllowed=x86 x64 ia64
ArchitecturesInstallIn64BitMode=x64 ia64
DisableReadyPage=no
DisableReadyMemo=no

[Languages]
Name: "en"; MessagesFile: "compiler:Default.isl"
Name: "ru"; MessagesFile: "compiler:Languages\Russian.isl"

[Tasks]
Name: desktopicon; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"
Name: quicklaunchicon; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "{#CompiledBinPath}\x86\SQLite.Interop.dll"; DestDir: "{app}\x86"
Source: "{#CompiledBinPath}\x64\SQLite.Interop.dll"; DestDir: "{app}\x64"
Source: "{#CompiledBinPath}\Business Accounting.exe"; DestDir: "{app}"
Source: "{#CompiledBinPath}\Business Accounting.exe.config"; DestDir: "{app}"; Flags: confirmoverwrite uninsneveruninstall
Source: "{#CompiledBinPath}\MahApps.Metro.dll"; DestDir: "{app}"
Source: "{#CompiledBinPath}\XDatabase.dll"; DestDir: "{app}"
Source: "{#CompiledBinPath}\System.Data.SQLite.dll"; DestDir: "{app}"
Source: "{#CompiledBinPath}\MySql.Data.dll"; DestDir: "{app}"
Source: "{#CompiledBinPath}\System.Windows.Interactivity.dll"; DestDir: "{app}"
Source: "{#CompiledBinPath}\Google.Apis.Auth.dll"; DestDir: "{app}"
Source: "{#CompiledBinPath}\Google.Apis.Auth.PlatformServices.dll"; DestDir: "{app}"
Source: "{#CompiledBinPath}\Google.Apis.Core.dll"; DestDir: "{app}"
Source: "{#CompiledBinPath}\Google.Apis.dll"; DestDir: "{app}"
Source: "{#CompiledBinPath}\Google.Apis.Drive.v3.dll"; DestDir: "{app}"
Source: "{#CompiledBinPath}\Google.Apis.PlatformServices.dll"; DestDir: "{app}"
Source: "{#CompiledBinPath}\Newtonsoft.Json.dll"; DestDir: "{app}"
Source: "{#CompiledBinPath}\Zlib.Portable.dll"; DestDir: "{app}"
Source: "{#CompiledBinPath}\google_drive_api.json"; DestDir: "{app}"
Source: "{#InitDatabaseFilePath}\ba.sqlite"; DestDir: "{userappdata}\{#AppName}"; Flags: onlyifdoesntexist uninsneveruninstall

[Icons]
Name: "{group}\{#AppName}"; Filename: "{app}\{#AppExecutable}"; WorkingDir: "{app}"
Name: "{group}\Uninstall {#AppName}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#AppName}"; Filename: "{app}\{#AppExecutable}"; WorkingDir: "{app}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#AppExecutable}"; WorkingDir: "{app}"; Description: "{cm:LaunchProgram,{#AppName}}"; Flags: nowait postinstall skipifsilent

#include "scripts\products.iss"

#include "scripts\products\stringversion.iss"
#include "scripts\products\winversion.iss"
#include "scripts\products\fileversion.iss"
#include "scripts\products\dotnetfxversion.iss"

#include "scripts\products\msiproduct.iss"
#include "scripts\products\dotnetfx46.iss"
#include "scripts\products\vcredist2012.iss"

[CustomMessages]
win_sp_title=Windows %1 Service Pack %2

[Code]
function InitializeSetup(): boolean;
begin
	initwinversion();

  dotnetfx46(61);
  vcredist2012();
  Result := true;
end;
