@ECHO OFF
SET CONFIG=Debug


SET CONFIG=Release
SET ZIP="c:\Program Files\7-zip\7z.exe"
SET BUILDER="c:\Program Files\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe"

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
XCOPY We7.CMS.Accounts\*.xml ..\Deploy\Source\We7.CMS.Accounts\  /E /Y /EXCLUDE:webFilter.txt
XCOPY We7.CMS.Accounts\*.config ..\Deploy\Source\We7.CMS.Accounts\  /E /Y


MD ..\Deploy\Source\We7.CMS.Common /E /Y  
XCOPY We7.CMS.Common\*.cs ..\Deploy\Source\We7.CMS.Common\  /E /Y
XCOPY We7.CMS.Common\*.csproj ..\Deploy\Source\We7.CMS.Common\  /E /Y
XCOPY We7.CMS.Common\*.xml ..\Deploy\Source\We7.CMS.Common\  /E /Y  /EXCLUDE:webFilter.txt


MD ..\Deploy\Source\We7.CMS.Config /E /Y  
XCOPY We7.CMS.Config\*.cs ..\Deploy\Source\We7.CMS.Config\  /E /Y
XCOPY We7.CMS.Config\*.csproj ..\Deploy\Source\We7.CMS.Config\  /E /Y
XCOPY We7.CMS.Config\*.xml ..\Deploy\Source\We7.CMS.Config\  /E /Y /EXCLUDE:webFilter.txt


MD ..\Deploy\Source\We7.CMS.Install /E /Y 
XCOPY We7.CMS.Install\*.cs ..\Deploy\Source\We7.CMS.Install\  /E /Y
XCOPY We7.CMS.Install\*.csproj ..\Deploy\Source\We7.CMS.Install\  /E /Y
XCOPY We7.CMS.Install\*.xml ..\Deploy\Source\We7.CMS.Install\  /E /Y  /EXCLUDE:webFilter.txt
 

MD ..\Deploy\Source\We7.CMS.Report  /E /Y  
XCOPY We7.CMS.Report\*.cs ..\Deploy\Source\We7.CMS.Report\  /E /Y
XCOPY We7.CMS.Report\*.csproj ..\Deploy\Source\We7.CMS.Report\  /E /Y
XCOPY We7.CMS.Report\*.xml ..\Deploy\Source\We7.CMS.Report\  /E /Y /EXCLUDE:webFilter.txt


MD ..\Deploy\Source\We7.CMS.UI  /E /Y  
XCOPY We7.CMS.UI\*.cs ..\Deploy\Source\We7.CMS.UI\  /E /Y
XCOPY We7.CMS.UI\*.csproj ..\Deploy\Source\We7.CMS.UI\  /E /Y
XCOPY We7.CMS.UI\*.xml ..\Deploy\Source\We7.CMS.UI\  /E /Y /EXCLUDE:webFilter.txt


MD ..\Deploy\Source\We7.CMS.Utils /E /Y 
XCOPY We7.CMS.Utils\*.cs ..\Deploy\Source\We7.CMS.Utils\  /E /Y
XCOPY We7.CMS.Utils\*.csproj ..\Deploy\Source\We7.CMS.Utils\  /E /Y
XCOPY We7.CMS.Utils\*.xml ..\Deploy\Source\We7.CMS.Utils\  /E /Y  /EXCLUDE:webFilter.txt


MD ..\Deploy\Source\We7.CMS.Web


XCOPY We7.CMS.Web\Admin\* ..\Deploy\Source\We7.CMS.Web\Admin\  /E /Y /EXCLUDE:webFilter.txt
XCOPY We7.CMS.Web\Config\* ..\Deploy\Source\We7.CMS.Web\Config\  /E /Y /EXCLUDE:webFilter.txt
XCOPY We7.CMS.Web\Properties\* ..\Deploy\Source\We7.CMS.Web\Properties\  /E /Y /EXCLUDE:webFilter.txt
XCOPY We7.CMS.Web\Go\* ..\Deploy\Source\We7.CMS.Web\Go\   /E /Y /EXCLUDE:webFilter.txt
XCOPY We7.CMS.Web\Install\* ..\Deploy\Source\We7.CMS.Web\Install\  /E /Y  /EXCLUDE:webFilter.txt

XCOPY We7.CMS.Web\ModelUI\* ..\Deploy\Source\We7.CMS.Web\ModelUI\  /E /Y  /EXCLUDE:webFilter.txt
XCOPY We7.CMS.Web\Scripts\* ..\Deploy\Source\We7.CMS.Web\Scripts\ /E /Y /EXCLUDE:webFilter.txt
XCOPY We7.CMS.Web\User\* ..\Deploy\Source\We7.CMS.Web\User\  /E /Y   /EXCLUDE:webFilter.txt
XCOPY We7.CMS.Web\Widgets\* ..\Deploy\Source\We7.CMS.Web\Widgets\  /E /Y   /EXCLUDE:webFilter.txt

XCOPY We7.CMS.Web\App_Data\XML\* ..\Deploy\Source\We7.CMS.Web\App_Data\XML\  /E /Y

COPY We7.CMS.Web\We7.CMS.Web.csproj ..\Deploy\Source\We7.CMS.Web\We7.CMS.Web.csproj  
COPY We7.CMS.Web\We7.CMS.Web.csproj.user ..\Deploy\Source\We7.CMS.Web\We7.CMS.Web.csproj.user 

COPY We7.CMS.Web\admin.aspx ..\Deploy\Source\We7.CMS.Web\admin.aspx 
COPY We7.CMS.Web\default.aspx ..\Deploy\Source\We7.CMS.Web\default.aspx 
COPY We7.CMS.Web\Errors.* ..\Deploy\Source\We7.CMS.Web\Errors.*  
COPY We7.CMS.Web\favicon.ico ..\Deploy\Source\We7.CMS.Web\favicon.ico  
COPY We7.CMS.Web\Global.* ..\Deploy\Source\We7.CMS.Web\Global.*  
COPY We7.CMS.Web\Web.config ..\Deploy\Source\We7.CMS.Web\Web.config  
COPY We7.CMS.Web\安装说明.html ..\Deploy\Source\We7.CMS.Web\安装说明.html   


del ..\Deploy\Source\We7.CMS.Web\config\db.config


MD ..\Deploy\Source\We7.CMS.WebControls  
XCOPY We7.CMS.WebControls\* ..\Deploy\Source\We7.CMS.WebControls\  /E /Y /EXCLUDE:webFilter.txt


MD ..\Deploy\Source\We7.Framework 
XCOPY We7.Framework\*.cs ..\Deploy\Source\We7.Framework\  /E /Y
XCOPY We7.Framework\*.csproj ..\Deploy\Source\We7.Framework\  /E /Y
XCOPY We7.Framework\*.xml ..\Deploy\Source\We7.Framework\  /E /Y /EXCLUDE:webFilter.txt


MD ..\Deploy\Source\We7.Model.Core
XCOPY We7.Model.Core\*.cs ..\Deploy\Source\We7.Model.Core\  /E /Y
XCOPY We7.Model.Core\*.csproj ..\Deploy\Source\We7.Model.Core\  /E /Y
XCOPY We7.Model.Core\*.xml ..\Deploy\Source\We7.Model.Core\  /E /Y  /EXCLUDE:webFilter.txt


MD ..\Deploy\Source\We7.Model.Service 
XCOPY We7.Model.Service\* ..\Deploy\Source\We7.Model.Service\  /E /Y /EXCLUDE:webFilter.txt


MD ..\Deploy\Source\We7.Share 
XCOPY We7.Share\*.cs ..\Deploy\Source\We7.Share\  /E /Y
XCOPY We7.Share\*.csproj ..\Deploy\Source\We7.Share\  /E /Y
XCOPY We7.Share\*.xml ..\Deploy\Source\We7.Share\  /E /Y /EXCLUDE:webFilter.txt


Copy .\Build.bat  ..\Deploy\Source\Build.bat /y
Copy .\We7CMS_2010.sln  ..\Deploy\Source\We7CMS_2010.sln /y
Copy .\版权申明.html  ..\Deploy\Source\版权申明.html /y
Copy .\请先阅读.txt  ..\Deploy\Source\请先阅读.txt /y

CD ..\Deploy\Source
DEL ..\We7.CMS.Source.7z
%ZIP% a -t7z ..\We7.CMS.Source.7z *
@ECHO finish We7.CMS.Source.7z!


@ECHO end
