; ****************************************
; This file is a project for Inno Setup.
; http://www.jrsoftware.org/isinfo.php
; 
; Note: Use Unicode version of Inno Setup.
; ****************************************

[Setup]
AppId={{88E31457-D99B-4590-9522-01DDD5C00D6D}}
AppName=Business Accounting
AppVersion=1.2
AppVerName={cm:NameAndVersion,Business Accounting, 1.2}
AppPublisher=Alexander Fuks
DefaultDirName={pf}\Business Accounting
DefaultGroupName=Business Accounting
SetupIconFile=BusinessAccounting\BusinessAccounting\Resources\favicon.ico
UninstallDisplayIcon={app}\BusinessAccounting.exe
OutputDir=.
OutputBaseFilename=Business Accounting Setup v.1.2
CloseApplications=yes
AllowNoIcons=yes
SetupLogging=yes

[Languages]
Name: "English"; MessagesFile: "compiler:Default.isl"
Name: "Russian"; MessagesFile: "compiler:Languages\Russian.isl"

[Tasks]
Name: desktopicon; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"
Name: quicklaunchicon; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "BusinessAccounting\BusinessAccounting\bin\Release\x86\SQLite.Interop.dll"; DestDir: "{app}\x86"
Source: "BusinessAccounting\BusinessAccounting\bin\Release\x64\SQLite.Interop.dll"; DestDir: "{app}\x64"
Source: "BusinessAccounting\BusinessAccounting\bin\Release\Business Accounting.exe"; DestDir: "{app}"
Source: "BusinessAccounting\BusinessAccounting\bin\Release\Business Accounting.exe.config"; DestDir: "{app}"
Source: "BusinessAccounting\BusinessAccounting\bin\Release\MahApps.Metro.dll"; DestDir: "{app}"
Source: "BusinessAccounting\BusinessAccounting\bin\Release\XDatabase.dll"; DestDir: "{app}"
Source: "BusinessAccounting\BusinessAccounting\bin\Release\System.Data.SQLite.dll"; DestDir: "{app}"
Source: "BusinessAccounting\BusinessAccounting\bin\Release\MySql.Data.dll"; DestDir: "{app}"
Source: "BusinessAccounting\BusinessAccounting\bin\Release\System.Windows.Interactivity.dll"; DestDir: "{app}"
Source: "Database\ba.sqlite"; DestDir: "{userappdata}\Business Accounting"; Flags: onlyifdoesntexist uninsneveruninstall

[Icons]
Name: "{group}\Business Accounting"; Filename: "{app}\Business Accounting.exe"; WorkingDir: "{app}"
Name: "{group}\Uninstall Business Accounting"; Filename: "{uninstallexe}"
Name: "{commondesktop}\Business Accounting"; Filename: "{app}\Business Accounting.exe"; WorkingDir: "{app}"; Tasks: desktopicon

[Run]
Filename: "{app}\Business Accounting.exe"; WorkingDir: "{app}"; Description: "{cm:LaunchProgram,Business Accounting}"; Flags: nowait postinstall skipifsilent
