@ECHO OFF
SET CONFIG=Debug

SET CONFIG=Release
SET ZIP="c:\Program Files\7-zip\7z.exe"
SET BUILDER="C:\Program Files\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe"

@ECHO Build projects...
%BUILDER% "We7 CMS.sln" /Build %CONFIG% > build.log.txt

@ECHO Building We7.CMS.Install.zip...
RD ..\Deploy\Temp /S /Q
MD ..\Deploy\Temp

MD ..\Deploy\Temp\admin
XCOPY We7.CMS.Web\admin\* ..\Deploy\Temp\admin\ /E /Y /Q
RD ..\Deploy\Temp\Admin\fckeditor\editor\_source  /S /Q
RD ..\Deploy\Temp\Admin\bin  /S /Q
RD ..\Deploy\Temp\Admin\obj  /S /Q

DEL ..\Deploy\Temp\Admin\fckeditor\fckpackager.exe

COPY  We7.CMS.Web\* ..\Deploy\Temp\ 

MD ..\Deploy\Temp\Bin
COPY We7.CMS.Web\Bin\*.dll ..\Deploy\Temp\bin\  /Y

MD ..\Deploy\Temp\app_data
MD ..\Deploy\Temp\app_data\XML  
COPY We7.CMS.Web\app_data\XML\*.XML  ..\Deploy\Temp\app_data\XML\  /Y

MD ..\Deploy\Temp\Go
COPY We7.CMS.Web\Go\* ..\Deploy\Temp\Go\  /Y

MD ..\Deploy\Temp\Install
XCOPY We7.CMS.Web\Install\* ..\Deploy\Temp\Install\ /E /Y /Q

MD ..\Deploy\Temp\Scripts
XCOPY We7.CMS.Web\Scripts\* ..\Deploy\Temp\Scripts\ /E /Y /Q

MD ..\Deploy\Temp\Widgets
XCOPY We7.CMS.Web\Widgets\* ..\Deploy\Temp\Widgets\ /E /Y /Q

MD ..\Deploy\Temp\User
XCOPY We7.CMS.Web\User\* ..\Deploy\Temp\User\ /E /Y /Q
RD ..\Deploy\Temp\User\bin  /S /Q
RD ..\Deploy\Temp\User\obj  /S /Q

MD ..\Deploy\Temp\ModelUI
XCOPY We7.CMS.Web\ModelUI\* ..\Deploy\Temp\ModelUI\ /E /Y /Q
RD ..\Deploy\Temp\ModelUI\bin  /S /Q
RD ..\Deploy\Temp\ModelUI\obj  /S /Q

MD ..\Deploy\Temp\Config
XCOPY We7.CMS.Web\Config\* ..\Deploy\Temp\Config\ /E /Y /Q
DEL ..\Deploy\Temp\Config\db.config
DEL ..\Deploy\Temp\Config\site.config

MD ..\Deploy\Temp\_Data
MD ..\Deploy\Temp\_skins
MD ..\Deploy\Temp\App_Browsers
COPY We7.CMS.Web\App_Browsers\*.*  ..\Deploy\Temp\App_Browsers\  /Y

MD ..\Deploy\Temp\_Data

DEL ..\Deploy\Temp\*.cs /Q/S
XCOPY We7.CMS.Web\Widgets\* ..\Deploy\Temp\Widgets\ /E /Y /Q

MD ..\Deploy\Temp\We7Controls
XCOPY We7.CMS.Web\We7Controls\*  ..\Deploy\Temp\We7Controls\* /E /Y /Q

DEL ..\Deploy\Temp\*.csproj /Q/S
DEL ..\Deploy\Temp\*.csproj.user  /Q/S
RD ..\Deploy\Temp\admin\obj  /S /Q

RD ..\Deploy\Temp\.svn /Q/S
RD ..\Deploy\Temp\Admin\fckeditor\CVS /Q/S
RD ..\Deploy\Temp\install\SQL\.svn  /Q/S
REM RD ..\Deploy\Temp\We7Controls  /Q/S
REM RD ..\Deploy\Temp\We7Controls  /Q/S
RD ..\Deploy\Temp\Models  /Q/S

CD ..\Deploy\Temp
DEL ..\We7.CMS.Install.zip
%ZIP% a -tzip ..\We7.CMS.Install.zip *
@ECHO finish We7.CMS.Install.zip!

@ECHO Building We7.CMS Install-patch...
RD  ..\install-patch\install  /S /Q
MD ..\install-patch\install
XCOPY  Install\* ..\install-patch\install\* /Q/S
MD ..\install-patch\bin
COPY bin\ICSharpCode.SharpZipLib.dll  ..\install-patch\bin
COPY bin\We7.CMS.Config.dll  ..\install-patch\bin
COPY bin\We7.CMS.Install.dll  ..\install-patch\bin
COPY bin\We7.Utils.dll  ..\install-patch\bin

@ECHO Building We7.CMS.Upgrade.zip...
REM RD  Install /S/Q
REM DEL bin\We7.CMS.Install.dll
RD Config /S/Q
REM MD Config
REM MD Config\Models
RD Admin\Ajax\Ext2.0 /S/Q
REM RD Admin\fckeditor /S/Q

REM DEL bin\Microsoft.Office.Interop.Word.dll
REM DEL bin\MySql.Data.dll
REM DEL bin\System.Data.SQLite.dll
REM DEL bin\System.Web.Extensions.dll
REM DEL bin\System.Web.Extensions.dll

RD Models /S/Q
REM DEL Models\ModelConfig.xml /S/Q

DEL ..\We7.CMS.Upgrade.zip
%ZIP% a -tzip ..\We7.CMS.Upgrade.zip *

@ECHO end
