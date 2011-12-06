@ECHO OFF
@ECHO OFF
SET CONFIG=Debug

SET CONFIG=Release
SET ZIP="c:\Program Files\7-zip\7z.exe"
SET BUILDER="C:\Program Files\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe"

@ECHO Build projects...
%BUILDER% "We7 CMS.sln" /Build %CONFIG% > build.log.txt

@ECHO Building We7.CMS.Demo.32bit.7z...
if not exist ..\Deploy md ..\Deploy
RD ..\Deploy\Demo /S /Q
if not exist ..\Deploy\Demo md ..\Deploy\Demo
if not exist ..\Deploy\Demo\website md ..\Deploy\Demo\website

MD ..\Deploy\Demo\website\_data
XCOPY "Solution Items\_data\*" ..\Deploy\Demo\website\_data\  /E /Y

MD ..\Deploy\Demo\website\_skins
XCOPY "Solution Items\_skins\*" ..\Deploy\Demo\website\_skins\  /E /Y

MD ..\Deploy\Demo\website\admin
XCOPY We7.CMS.Web\admin\* ..\Deploy\Demo\website\admin\ /E /Y /Q
RD ..\Deploy\Demo\website\Admin\fckeditor\editor\_source  /S /Q
RD ..\Deploy\Demo\website\Admin\bin  /S /Q
RD ..\Deploy\Demo\website\Admin\obj  /S /Q

DEL ..\Deploy\Demo\website\Admin\fckeditor\fckpackager.exe

COPY  We7.CMS.Web\* ..\Deploy\Demo\website\ 

MD ..\Deploy\Demo\website\Bin
COPY We7.CMS.Web\Bin\*.dll ..\Deploy\Demo\website\bin\  /Y

MD ..\Deploy\Demo\website\app_data
XCOPY We7.CMS.Web\app_data\*  ..\Deploy\Demo\website\app_data\  /E /Y
XCOPY "Solution Items\32BIT\We7_CMS_DB.mdb" ..\Deploy\Demo\website\app_data\db\  /E /Y

MD ..\Deploy\Demo\website\Go
COPY We7.CMS.Web\Go\* ..\Deploy\Demo\website\Go\  /Y

MD ..\Deploy\Demo\website\Install
XCOPY We7.CMS.Web\Install\* ..\Deploy\Demo\website\Install\ /E /Y /Q

MD ..\Deploy\Demo\website\Scripts
XCOPY We7.CMS.Web\Scripts\* ..\Deploy\Demo\website\Scripts\ /E /Y /Q

MD ..\Deploy\Demo\website\Widgets
XCOPY We7.CMS.Web\Widgets\* ..\Deploy\Demo\website\Widgets\ /E /Y /Q

MD ..\Deploy\Demo\website\User
XCOPY We7.CMS.Web\User\* ..\Deploy\Demo\website\User\ /E /Y /Q
RD ..\Deploy\Demo\website\User\bin  /S /Q
RD ..\Deploy\Demo\website\User\obj  /S /Q

MD ..\Deploy\Demo\website\ModelUI
XCOPY We7.CMS.Web\ModelUI\* ..\Deploy\Demo\website\ModelUI\ /E /Y /Q
RD ..\Deploy\Demo\website\ModelUI\bin  /S /Q
RD ..\Deploy\Demo\website\ModelUI\obj  /S /Q

MD ..\Deploy\Demo\website\Config
XCOPY We7.CMS.Web\Config\* ..\Deploy\Demo\website\Config\ /E /Y /Q
XCOPY "Solution Items\Config\*" ..\Deploy\Demo\website\Config\ /E /Y /Q
DEL ..\Deploy\Demo\website\Config\db.config
DEL ..\Deploy\Demo\website\Config\site.config
COPY We7.CMS.Web\Config\site.config.bak ..\Deploy\Demo\website\Config\site.config
COPY We7.CMS.Web\Config\db.config.bak ..\Deploy\Demo\website\Config\db.config

MD ..\Deploy\Demo\website\_Data
MD ..\Deploy\Demo\website\_skins
MD ..\Deploy\Demo\website\App_Browsers
COPY We7.CMS.Web\App_Browsers\*.*  ..\Deploy\Demo\website\App_Browsers\  /Y

MD ..\Deploy\Demo\website\_Data

DEL ..\Deploy\Demo\website\*.cs /Q/S
XCOPY We7.CMS.Web\Widgets\* ..\Deploy\Demo\website\Widgets\ /E /Y /Q

REM MD ..\Deploy\Demo\website\We7Controls
REM XCOPY We7.CMS.Web\We7Controls\*  ..\Deploy\Demo\website\We7Controls\* /E /Y /Q

DEL ..\Deploy\Demo\website\*.csproj /Q/S
DEL ..\Deploy\Demo\website\*.csproj.user  /Q/S
RD ..\Deploy\Demo\website\admin\obj  /S /Q
RD ..\Deploy\Demo\website\admin\bin  /S /Q

RD ..\Deploy\Demo\website\.svn /Q/S
RD ..\Deploy\Demo\website\Admin\fckeditor\CVS /Q/S
RD ..\Deploy\Demo\website\install\SQL\.svn  /Q/S
REM RD ..\Deploy\Demo\website\We7Controls  /Q/S
REM RD ..\Deploy\Demo\website\We7Controls  /Q/S
RD ..\Deploy\Demo\website\Models  /Q/S

xcopy "Solution Items\Package\*" ..\Deploy\Demo\ /E /Y

DEL ..\Deploy\Demo\website\*.vssscc /Q/S
DEL ..\Deploy\Demo\website\*.vspscc /Q/S 
CD ..\Deploy\Demo
DEL ..\We7.CMS.Demo.32bit.7z
%ZIP% a -t7z ..\We7.CMS.Demo.32bit.7z *
@ECHO finish We7.CMS.Demo.32bit.7z!

@ECHO end