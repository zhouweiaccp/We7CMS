@ECHO OFF
SET CONFIG=Debug

SET CONFIG=Release
SET ZIP="c:\Program Files\7-zip\7z.exe"
SET BUILDER="C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe"

@ECHO Build projects...
%BUILDER% "We7 CMS.sln" /Build %CONFIG% > build.log.txt

@ECHO Building We7.CMS.Source.7z...
if not exist ..\Deploy\Source md ..\Deploy\Source
RD ..\Deploy\Source /S /Q
MD ..\Deploy\Source

MD "..\Deploy\Source\Solution Items"
MD "..\Deploy\Source\Solution Items\DLL"
XCOPY "Solution Items\DLL\*" "..\Deploy\Source\Solution Items\DLL"  /E /Y  /EXCLUDE:webFilter.txt

MD ..\Deploy\Source\We7.CMS.Accounts
XCOPY We7.CMS.Accounts\*.cs ..\Deploy\Source\We7.CMS.Accounts\  /E /Y
XCOPY We7.CMS.Accounts\*.csproj ..\Deploy\Source\We7.CMS.Accounts\  /E /Y
XCOPY We7.CMS.Accounts\*.xml ..\Deploy\Source\We7.CMS.Accounts\  /E /Y
XCOPY We7.CMS.Accounts\*.config ..\Deploy\Source\We7.CMS.Accounts\  /E /Y
DEL ..\Deploy\Source\We7.Share\We7.CMS.Accounts\*.vssscc /Q/S
DEL ..\Deploy\Source\We7.Share\We7.CMS.Accounts\*.vspscc /Q/S
rd ..\Deploy\Source\We7.CMS.Accounts\bin /S /Q
rd ..\Deploy\Source\We7.CMS.Accounts\obj /S /Q

MD ..\Deploy\Source\We7.CMS.Common
XCOPY We7.CMS.Common\*.cs ..\Deploy\Source\We7.CMS.Common\  /E /Y
XCOPY We7.CMS.Common\*.csproj ..\Deploy\Source\We7.CMS.Common\  /E /Y
XCOPY We7.CMS.Common\*.xml ..\Deploy\Source\We7.CMS.Common\  /E /Y
DEL ..\Deploy\Source\We7.Share\We7.CMS.Common\*.vssscc /Q/S
DEL ..\Deploy\Source\We7.Share\We7.CMS.Common\*.vspscc /Q/S
rd ..\Deploy\Source\We7.CMS.Common\bin /S /Q
rd ..\Deploy\Source\We7.CMS.Common\obj /S /Q

MD ..\Deploy\Source\We7.CMS.Config
XCOPY We7.CMS.Config\*.cs ..\Deploy\Source\We7.CMS.Config\  /E /Y
XCOPY We7.CMS.Config\*.csproj ..\Deploy\Source\We7.CMS.Config\  /E /Y
XCOPY We7.CMS.Config\*.xml ..\Deploy\Source\We7.CMS.Config\  /E /Y
DEL ..\Deploy\Source\We7.Share\We7.CMS.Config\*.vssscc /Q/S
DEL ..\Deploy\Source\We7.Share\We7.CMS.Config\*.vspscc /Q/S
rd ..\Deploy\Source\We7.CMS.Config\bin /S /Q
rd ..\Deploy\Source\We7.CMS.Config\obj /S /Q

MD ..\Deploy\Source\We7.CMS.Install
XCOPY We7.CMS.Install\*.cs ..\Deploy\Source\We7.CMS.Install\  /E /Y
XCOPY We7.CMS.Install\*.csproj ..\Deploy\Source\We7.CMS.Install\  /E /Y
XCOPY We7.CMS.Install\*.xml ..\Deploy\Source\We7.CMS.Install\  /E /Y
DEL ..\Deploy\Source\We7.Share\We7.CMS.Install\*.vssscc /Q/S
DEL ..\Deploy\Source\We7.Share\We7.CMS.Install\*.vspscc /Q/S
rd ..\Deploy\Source\We7.CMS.Install\bin /S /Q
rd ..\Deploy\Source\We7.CMS.Install\obj /S /Q

MD ..\Deploy\Source\We7.CMS.Report
XCOPY We7.CMS.Report\*.cs ..\Deploy\Source\We7.CMS.Report\  /E /Y
XCOPY We7.CMS.Report\*.csproj ..\Deploy\Source\We7.CMS.Report\  /E /Y
XCOPY We7.CMS.Report\*.xml ..\Deploy\Source\We7.CMS.Report\  /E /Y
DEL ..\Deploy\Source\We7.Share\We7.CMS.Report\*.vssscc /Q/S
DEL ..\Deploy\Source\We7.Share\We7.CMS.Report\*.vspscc /Q/S
rd ..\Deploy\Source\We7.CMS.Report\bin /S /Q
rd ..\Deploy\Source\We7.CMS.Report\obj /S /Q

MD ..\Deploy\Source\We7.CMS.UI
XCOPY We7.CMS.UI\*.cs ..\Deploy\Source\We7.CMS.UI\  /E /Y
XCOPY We7.CMS.UI\*.csproj ..\Deploy\Source\We7.CMS.UI\  /E /Y
XCOPY We7.CMS.UI\*.xml ..\Deploy\Source\We7.CMS.UI\  /E /Y
DEL ..\Deploy\Source\We7.Share\We7.CMS.UI\*.vssscc /Q/S
DEL ..\Deploy\Source\We7.Share\We7.CMS.UI\*.vspscc /Q/S
rd ..\Deploy\Source\We7.CMS.UI\bin /S /Q
rd ..\Deploy\Source\We7.CMS.UI\obj /S /Q

MD ..\Deploy\Source\We7.CMS.Utils
XCOPY We7.CMS.Utils\*.cs ..\Deploy\Source\We7.CMS.Utils\  /E /Y
XCOPY We7.CMS.Utils\*.csproj ..\Deploy\Source\We7.CMS.Utils\  /E /Y
XCOPY We7.CMS.Utils\*.xml ..\Deploy\Source\We7.CMS.Utils\  /E /Y
DEL ..\Deploy\Source\We7.Share\We7.CMS.Utils\*.vssscc /Q/S
DEL ..\Deploy\Source\We7.Share\We7.CMS.Utils\*.vspscc /Q/S
rd ..\Deploy\Source\We7.CMS.Utils\bin /S /Q
rd ..\Deploy\Source\We7.CMS.Utils\obj /S /Q

MD ..\Deploy\Source\We7.CMS.Web
XCOPY We7.CMS.Web\* ..\Deploy\Source\We7.CMS.Web\  /E /Y
rd ..\Deploy\Source\We7.CMS.Web\bin /S /Q
rd ..\Deploy\Source\We7.CMS.Web\obj /S /Q
rd ..\Deploy\Source\We7.CMS.Web\obj /S /Q
rd ..\Deploy\Source\We7.CMS.Web\_data /S /Q
rd ..\Deploy\Source\We7.CMS.Web\_skins /S /Q
rd ..\Deploy\Source\We7.CMS.Web\App_Data\DB /S /Q
DEL ..\Deploy\Source\We7.CMS.Web\*.vssscc /Q/S
DEL ..\Deploy\Source\We7.CMS.Web\*.vspscc /Q/S
DEL ..\Deploy\Source\We7.CMS.Web\*.user /Q/S
REM rd ..\Deploy\Source\We7.CMS.Web\Widgets\WidgetCollection /S /Q
REM MD ..\Deploy\Source\We7.CMS.Web\Widgets\WidgetCollection
rd ..\Deploy\Source\We7.CMS.Web\USER\bin /S /Q
rd ..\Deploy\Source\We7.CMS.Web\ModelUI\bin /S /Q
rd ..\Deploy\Source\We7.CMS.Web\admin\bin /S /Q
MD  ..\Deploy\Source\We7.CMS.Web\_data
MD  ..\Deploy\Source\We7.CMS.Web\_skins
del ..\Deploy\Source\We7.CMS.Web\config\db.config

MD ..\Deploy\Source\We7.CMS.WebControls
XCOPY We7.CMS.WebControls\* ..\Deploy\Source\We7.CMS.WebControls\  /E /Y
DEL ..\Deploy\Source\We7.CMS.WebControls\*.vssscc /Q/S
DEL ..\Deploy\Source\We7.CMS.WebControls\*.vspscc /Q/S
rd ..\Deploy\Source\We7.CMS.WebControls\bin /S /Q
rd ..\Deploy\Source\We7.CMS.WebControls\obj /S /Q

MD ..\Deploy\Source\We7.Framework
XCOPY We7.Framework\*.cs ..\Deploy\Source\We7.Framework\  /E /Y
XCOPY We7.Framework\*.csproj ..\Deploy\Source\We7.Framework\  /E /Y
XCOPY We7.Framework\*.xml ..\Deploy\Source\We7.Framework\  /E /Y
DEL ..\Deploy\Source\We7.Framework\*.vssscc /Q/S
DEL ..\Deploy\Source\We7.Framework\*.vspscc /Q/S
rd ..\Deploy\Source\We7.Framework\bin /S /Q
rd ..\Deploy\Source\We7.Framework\obj /S /Q

MD ..\Deploy\Source\We7.Model.Core
XCOPY We7.Model.Core\*.cs ..\Deploy\Source\We7.Model.Core\  /E /Y
XCOPY We7.Model.Core\*.csproj ..\Deploy\Source\We7.Model.Core\  /E /Y
XCOPY We7.Model.Core\*.xml ..\Deploy\Source\We7.Model.Core\  /E /Y
DEL ..\Deploy\Source\We7.Model.Core\*.vssscc /Q/S
DEL ..\Deploy\Source\We7.Model.Core\*.vspscc /Q/S
rd ..\Deploy\Source\We7.Model.Core\bin /S /Q
rd ..\Deploy\Source\We7.Model.Core\obj /S /Q

MD ..\Deploy\Source\We7.Model.Service
XCOPY We7.Model.Service\* ..\Deploy\Source\We7.Model.Service\  /E /Y
DEL ..\Deploy\Source\We7.Model.Service\*.vssscc /Q/S
DEL ..\Deploy\Source\We7.Model.Service\*.vspscc /Q/S
rd ..\Deploy\Source\We7.Model.Service\bin /S /Q
rd ..\Deploy\Source\We7.Model.Service\obj /S /Q
DEL ..\Deploy\Source\We7.CMS.Service\*.vssscc /Q/S

MD ..\Deploy\Source\We7.Share
XCOPY We7.Share\*.cs ..\Deploy\Source\We7.Share\  /E /Y
XCOPY We7.Share\*.csproj ..\Deploy\Source\We7.Share\  /E /Y
XCOPY We7.Share\*.xml ..\Deploy\Source\We7.Share\  /E /Y
rd ..\Deploy\Source\We7.Share\Thinkment.Data\bin /S /Q
rd ..\Deploy\Source\We7.Share\Thinkment.Data\obj /S /Q
DEL ..\Deploy\Source\We7.Share\Thinkment.Data\*.vssscc /Q/S
DEL ..\Deploy\Source\We7.Share\Thinkment.Data\*.vspscc /Q/S
rd ..\Deploy\Source\We7.Share\We7.UrlRewriter\bin /S /Q
rd ..\Deploy\Source\We7.Share\We7.UrlRewriter\obj /S /Q
DEL ..\Deploy\Source\We7.Share\We7.UrlRewriter\*.vssscc /Q/S
DEL ..\Deploy\Source\We7.Share\We7.UrlRewriter\*.vspscc /Q/S

Copy .\Build.bat  ..\Deploy\Source\Build.bat /y
Copy .\We7CMS_2010.sln  ..\Deploy\Source\We7CMS_2010.sln /y
Copy .\∞Ê»®…Í√˜.html  ..\Deploy\Source\∞Ê»®…Í√˜.html /y
Copy .\«Îœ»‘ƒ∂¡.txt  ..\Deploy\Source\«Îœ»‘ƒ∂¡.txt /y

CD ..\Deploy\Source
DEL ..\We7.CMS.Source.7z
%ZIP% a -t7z ..\We7.CMS.Source.7z *
@ECHO finish We7.CMS.Source.7z!

@ECHO end
