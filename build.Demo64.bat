@ECHO OFF
SET CONFIG=Debug

SET CONFIG=Release
SET ZIP="c:\Program Files\7-zip\7z.exe"
SET BUILDER="C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe"

@ECHO Build projects...
%BUILDER% "We7 CMS.sln" /Build %CONFIG% > build.log.txt

@ECHO Building We7.CMS.Demo.64bit.7z...
if not exist ..\Deploy md ..\Deploy
RD ..\Deploy\Demo64 /S /Q
if not exist ..\Deploy\Demo64 md ..\Deploy\Demo64
if not exist ..\Deploy\Demo64\website md ..\Deploy\Demo64\website

MD ..\Deploy\Demo64\website\_data
XCOPY "Solution Items\_data\*" ..\Deploy\Demo64\website\_data\  /E /Y

MD ..\Deploy\Demo64\website\_skins
XCOPY "Solution Items\_skins\*" ..\Deploy\Demo64\website\_skins\  /E /Y

MD ..\Deploy\Demo64\website\admin
XCOPY We7.CMS.Web\admin\* ..\Deploy\Demo64\website\admin\ /E /Y /Q
RD ..\Deploy\Demo64\website\Admin\fckeditor\editor\_source  /S /Q
RD ..\Deploy\Demo64\website\Admin\bin  /S /Q
RD ..\Deploy\Demo64\website\Admin\obj  /S /Q

DEL ..\Deploy\Demo64\website\Admin\fckeditor\fckpackager.exe

COPY  We7.CMS.Web\* ..\Deploy\Demo64\website\ 

MD ..\Deploy\Demo64\website\Bin
COPY We7.CMS.Web\Bin\*.dll ..\Deploy\Demo64\website\bin\  /Y
DEL ..\Deploy\Demo64\website\bin\System.Data.SQLite.dll /Y
COPY "Solution Items\DLL\System.Data.SQLite_64.DLL" ..\Deploy\Demo64\website\bin\System.Data.SQLite.dll

MD ..\Deploy\Demo64\website\app_data
XCOPY We7.CMS.Web\app_data\*  ..\Deploy\Demo64\website\app_data\  /E /Y
XCOPY "Solution Items\64BIT\We7_CMS_DB.db" ..\Deploy\Demo64\website\app_data\db\  /E /Y

MD ..\Deploy\Demo64\website\Go
COPY We7.CMS.Web\Go\* ..\Deploy\Demo64\website\Go\  /Y

MD ..\Deploy\Demo64\website\Install
XCOPY We7.CMS.Web\Install\* ..\Deploy\Demo64\website\Install\ /E /Y /Q

MD ..\Deploy\Demo64\website\Scripts
XCOPY We7.CMS.Web\Scripts\* ..\Deploy\Demo64\website\Scripts\ /E /Y /Q

MD ..\Deploy\Demo64\website\Widgets
XCOPY We7.CMS.Web\Widgets\* ..\Deploy\Demo64\website\Widgets\ /E /Y /Q

MD ..\Deploy\Demo64\website\User
XCOPY We7.CMS.Web\User\* ..\Deploy\Demo64\website\User\ /E /Y /Q
RD ..\Deploy\Demo64\website\User\bin  /S /Q
RD ..\Deploy\Demo64\website\User\obj  /S /Q

MD ..\Deploy\Demo64\website\ModelUI
XCOPY We7.CMS.Web\ModelUI\* ..\Deploy\Demo64\website\ModelUI\ /E /Y /Q
RD ..\Deploy\Demo64\website\ModelUI\bin  /S /Q
RD ..\Deploy\Demo64\website\ModelUI\obj  /S /Q

MD ..\Deploy\Demo64\website\Config
XCOPY We7.CMS.Web\Config\* ..\Deploy\Demo64\website\Config\ /E /Y /Q
XCOPY "Solution Items\Config\*" ..\Deploy\Demo64\website\Config\ /E /Y /Q
DEL ..\Deploy\Demo64\website\Config\db.config
DEL ..\Deploy\Demo64\website\Config\site.config
COPY We7.CMS.Web\Config\site.config.bak ..\Deploy\Demo64\website\Config\site.config
COPY We7.CMS.Web\Config\db_sqlite.config.bak ..\Deploy\Demo64\website\Config\db.config

MD ..\Deploy\Demo64\website\_Data
MD ..\Deploy\Demo64\website\_skins
MD ..\Deploy\Demo64\website\App_Browsers
COPY We7.CMS.Web\App_Browsers\*.*  ..\Deploy\Demo64\website\App_Browsers\  /Y

MD ..\Deploy\Demo64\website\_Data

DEL ..\Deploy\Demo64\website\*.cs /Q/S
XCOPY We7.CMS.Web\Widgets\* ..\Deploy\Demo64\website\Widgets\ /E /Y /Q

REM MD ..\Deploy\Demo64\website\We7Controls
REM XCOPY We7.CMS.Web\We7Controls\*  ..\Deploy\Demo64\website\We7Controls\* /E /Y /Q

DEL ..\Deploy\Demo64\website\*.csproj /Q/S
DEL ..\Deploy\Demo64\website\*.csproj.user  /Q/S
RD ..\Deploy\Demo64\website\admin\obj  /S /Q
RD ..\Deploy\Demo64\website\admin\bin  /S /Q

RD ..\Deploy\Demo64\website\.svn /Q/S
RD ..\Deploy\Demo64\website\Admin\fckeditor\CVS /Q/S
RD ..\Deploy\Demo64\website\install\SQL\.svn  /Q/S
REM RD ..\Deploy\Demo64\website\We7Controls  /Q/S
REM RD ..\Deploy\Demo64\website\We7Controls  /Q/S
RD ..\Deploy\Demo64\website\Models  /Q/S

xcopy "Solution Items\Package\*" ..\Deploy\Demo64\ /E /Y

DEL ..\Deploy\Demo64\website\*.vssscc /Q/S
DEL ..\Deploy\Demo64\website\*.vspscc /Q/S 
CD ..\Deploy\Demo64
DEL We7.CMS.Demo.64bit.7z
%ZIP% a -t7z ..\We7.CMS.Demo.64bit.7z *
@ECHO finish We7.CMS.Demo.64bit.7z!



@ECHO end