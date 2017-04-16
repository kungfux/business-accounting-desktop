#include "isxdl\isxdl.iss"

[CustomMessages]
DependenciesDir=MyProgramDependencies

en.depdownload_msg=The following applications are required before setup can continue:%n%n%1%nDownload and install now?
ru.depdownload_msg=Следующие компоненты должны быть установлены прежде чем установка будет продолжена:%n%n%1%nСкачать и установить сейчас?

en.depdownload_memo_title=Download dependencies
ru.depdownload_memo_title=Загрузить зависимости

en.depinstall_memo_title=Install dependencies
ru.depinstall_memo_title=Установить зависимости

en.depinstall_title=Installing dependencies
ru.depinstall_title=Установка зависимостей

en.depinstall_description=Please wait while Setup installs dependencies on your computer.
ru.depinstall_description=Пожалуйста, подождите пока зависимости будут установлены.

en.depinstall_status=Installing %1...
ru.depinstall_status=Установка %1...

en.depinstall_missing=%1 must be installed before setup can continue. Please install %1 and run Setup again.
ru.depinstall_missing=%1 должен быть установлен прежде чем установка может быть продолжена. Пожалуйста, установите %1 и запустите установка снова.

en.depinstall_error=An error occured while installing the dependencies. Please restart the computer and run the setup again or install the following dependencies manually:%n
ru.depinstall_error=Произошла ошибка при установке зависимостей. Пожалуйста, перезапустите компьютер и запустите установку повторно или установите следующие зависимости вручную:%n

en.isxdl_langfile=
ru.isxdl_langfile=russian.ini


[Files]
Source: "scripts\isxdl\russian.ini"; Flags: dontcopy

[Code]
#include "code.pas"

[Setup]

