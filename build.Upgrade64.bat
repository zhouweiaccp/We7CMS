@ECHO OFF
SET CONFIG=Debug

SET CONFIG=Release
SET ZIP="c:\Program Files\7-zip\7z.exe"
SET BUILDER="C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe"

@ECHO Build projects...
%BUILDER% "We7 CMS.sln" /Build %CONFIG% > build.log.txt

@ECHO Building We7.CMS.Upgrade.64bit.zip...

if not exist ..\Deploy md ..\Deploy
RD  ..\Deploy\install-patch  /S /Q
MD ..\Deploy\install-patch

MD ..\Deploy\install-patch\admin
XCOPY  We7.CMS.Web\admin\* ..\Deploy\install-patch\admin\ /E /Y /Q
RD ..\Deploy\install-patch\admin\fckeditor\editor\_source  /S /Q
RD ..\Deploy\install-patch\admin\bin  /S /Q
RD ..\Deploy\install-patch\admin\obj  /S /Q
DEL ..\Deploy\install-patch\admin\fckeditor\fckpackager.exe

MD ..\Deploy\install-patch\install
XCOPY We7.CMS.Web\install\*  ..\Deploy\install-patch\install\  /E /Y

MD ..\Deploy\install-patch\app_data
MD ..\Deploy\install-patch\app_data\XML
XCOPY We7.CMS.Web\app_data\XML\*  ..\Deploy\install-patch\app_data\XML\  /E /Y

COPY  We7.CMS.Web\* ..\Deploy\install-patch\ 
MD ..\Deploy\install-patch\Bin
COPY We7.CMS.Web\Bin\*.dll ..\Deploy\install-patch\bin\  /Y

MD ..\Deploy\install-patch\Scripts
XCOPY We7.CMS.Web\Scripts\* ..\Deploy\install-patch\Scripts\ /E /Y /Q

MD ..\Deploy\install-patch\User
XCOPY We7.CMS.Web\User\* ..\Deploy\install-patch\User\ /E /Y /Q
RD ..\Deploy\install-patch\User\bin  /S /Q
RD ..\Deploy\install-patch\User\obj  /S /Q

MD ..\Deploy\install-patch\ModelUI
XCOPY We7.CMS.Web\ModelUI\* ..\Deploy\install-patch\ModelUI\ /E /Y /Q
RD ..\Deploy\install-patch\ModelUI\bin  /S /Q
RD ..\Deploy\install-patch\ModelUI\obj  /S /Q

MD ..\Deploy\install-patch\App_Browsers
COPY We7.CMS.Web\App_Browsers\*.*  ..\Deploy\install-patch\App_Browsers\  /Y

DEL ..\Deploy\install-patch\*.csproj /Q/S
DEL ..\Deploy\install-patch\*.csproj.user  /Q/S
RD ..\Deploy\install-patch\admin\obj  /S /Q
DEL ..\Deploy\install-patch\*.vssscc /Q/S
DEL ..\Deploy\install-patch\*.vspscc /Q/S 
DEL ..\Deploy\install-patch\*.cs /Q/S

CD ..\Deploy\install-patch
DEL ..\We7.CMS.Upgrade.64bit.zip
%ZIP% a -tzip ..\We7.CMS.Upgrade.64bit.zip *

@ECHO end
