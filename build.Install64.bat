@ECHO OFF
SET CONFIG=Debug

SET CONFIG=Release
SET ZIP="c:\Program Files\7-zip\7z.exe"
SET BUILDER="C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\devenv.exe"

@ECHO Build projects...
%BUILDER% "We7 CMS.sln" /Build %CONFIG% > build.log.txt

@ECHO Building We7.CMS.Install.zip...
if not exist ..\Deploy md ..\Deploy
RD ..\Deploy\Install /S /Q
if not exist ..\Deploy\Install md ..\Deploy\Install
if not exist ..\Deploy\Install\website md ..\Deploy\Install\website

MD ..\Deploy\Install\website\_data
XCOPY We7.CMS.Web\_data\* ..\Deploy\Install\website\_data\  /E /Y

MD ..\Deploy\Install\website\_skins
XCOPY We7.CMS.Web\_skins\* ..\Deploy\Install\website\_skins\  /E /Y

MD ..\Deploy\Install\website\admin
XCOPY We7.CMS.Web\admin\* ..\Deploy\Install\website\admin\ /E /Y /Q
RD ..\Deploy\Install\website\Admin\fckeditor\editor\_source  /S /Q
RD ..\Deploy\Install\website\Admin\bin  /S /Q
RD ..\Deploy\Install\website\Admin\obj  /S /Q

DEL ..\Deploy\Install\website\Admin\fckeditor\fckpackager.exe

COPY  We7.CMS.Web\* ..\Deploy\Install\website\ 

MD ..\Deploy\Install\website\Bin
COPY We7.CMS.Web\Bin\*.dll ..\Deploy\Install\website\bin\  /Y
DEL ..\Deploy\Install\website\bin\System.Data.SQLite.dll /Y
XCOPY "Solution Items\DLL\System.Data.SQLite_64.DLL" ..\Deploy\Install\website\bin\System.Data.SQLite.dll /E /Y

MD ..\Deploy\Install\website\app_data
XCOPY We7.CMS.Web\app_data\*  ..\Deploy\Install\website\app_data\  /E /Y

MD ..\Deploy\Install\website\Go
COPY We7.CMS.Web\Go\* ..\Deploy\Install\website\Go\  /Y

MD ..\Deploy\Install\website\Install
XCOPY We7.CMS.Web\Install\* ..\Deploy\Install\website\Install\ /E /Y /Q

MD ..\Deploy\Install\website\Scripts
XCOPY We7.CMS.Web\Scripts\* ..\Deploy\Install\website\Scripts\ /E /Y /Q

MD ..\Deploy\Install\website\Widgets
XCOPY We7.CMS.Web\Widgets\* ..\Deploy\Install\website\Widgets\ /E /Y /Q

MD ..\Deploy\Install\website\User
XCOPY We7.CMS.Web\User\* ..\Deploy\Install\website\User\ /E /Y /Q
RD ..\Deploy\Install\website\User\bin  /S /Q
RD ..\Deploy\Install\website\User\obj  /S /Q

MD ..\Deploy\Install\website\ModelUI
XCOPY We7.CMS.Web\ModelUI\* ..\Deploy\Install\website\ModelUI\ /E /Y /Q
RD ..\Deploy\Install\website\ModelUI\bin  /S /Q
RD ..\Deploy\Install\website\ModelUI\obj  /S /Q

MD ..\Deploy\Install\website\Config
copy We7.CMS.Web\Config\urls.config ..\Deploy\Install\website\Config\
copy We7.CMS.Web\Config\Cache.config ..\Deploy\Install\website\Config\
copy We7.CMS.Web\Config\Render.config ..\Deploy\Install\website\Config\
copy We7.CMS.Web\Config\thumbnail.xml ..\Deploy\Install\website\Config\
copy We7.CMS.Web\Config\TemplateType.xml ..\Deploy\Install\website\Config\
copy We7.CMS.Web\Config\TagsWord.xml ..\Deploy\Install\website\Config\
copy We7.CMS.Web\Config\UrlTags.xml ..\Deploy\Install\website\Config\
copy We7.CMS.Web\Config\UserEmailConfig.xml ..\Deploy\Install\website\Config\
copy We7.CMS.Web\Config\video.config ..\Deploy\Install\website\Config\
copy We7.CMS.Web\Config\CategoryOptions.config ..\Deploy\Install\website\Config\
ATTRIB ..\Deploy\Install\website\Config\urls.config -R
ATTRIB ..\Deploy\Install\website\Config\Cache.config -R
ATTRIB ..\Deploy\Install\website\Config\Render.config -R
ATTRIB ..\Deploy\Install\website\Config\thumbnail.xml -R
ATTRIB ..\Deploy\Install\website\Config\TemplateType.xml -R
ATTRIB ..\Deploy\Install\website\Config\TagsWord.xml -R
ATTRIB ..\Deploy\Install\website\Config\UrlTags.xml -R
ATTRIB ..\Deploy\Install\website\Config\UserEmailConfig.xml -R
ATTRIB ..\Deploy\Install\website\Config\video.config -R
ATTRIB ..\Deploy\Install\website\Config\CategoryOptions.config -R
MD ..\Deploy\Install\website\Config\Dictionary
copy We7.CMS.Web\Config\Dictionary\Tags.xml ..\Deploy\Install\website\Config\Dictionary
copy We7.CMS.Web\Config\Dictionary\keywords.xml ..\Deploy\Install\website\Config\Dictionary
copy We7.CMS.Web\Config\Dictionary\provinces.xml ..\Deploy\Install\website\Config\Dictionary
copy We7.CMS.Web\Config\Dictionary\SignTemplate.xml ..\Deploy\Install\website\Config\Dictionary
ATTRIB ..\Deploy\Install\website\Config\Dictionary\Tags.xml -R
ATTRIB ..\Deploy\Install\website\Config\Dictionary\keywords.xml -R
ATTRIB ..\Deploy\Install\website\Config\Dictionary\provinces.xml -R
ATTRIB ..\Deploy\Install\website\Config\Dictionary\SignTemplate.xml -R
REM XCOPY We7.CMS.Web\Config\* ..\Deploy\Install\website\Config\ /E /Y /Q
REM DEL ..\Deploy\Install\website\Config\db.config
REM DEL ..\Deploy\Install\website\Config\site.config

MD ..\Deploy\Install\website\_Data
MD ..\Deploy\Install\website\_skins
MD ..\Deploy\Install\website\App_Browsers
COPY We7.CMS.Web\App_Browsers\*.*  ..\Deploy\Install\website\App_Browsers\  /Y

MD ..\Deploy\Install\website\_Data

DEL ..\Deploy\Install\website\*.cs /Q/S
XCOPY We7.CMS.Web\Widgets\* ..\Deploy\Install\website\Widgets\ /E /Y /Q
rd ..\Deploy\Install\website\Widgets\WidgetCollection /S /Q
MD ..\Deploy\Install\website\Widgets\WidgetCollection

REM MD ..\Deploy\Install\website\We7Controls
REM XCOPY We7.CMS.Web\We7Controls\*  ..\Deploy\Install\website\We7Controls\* /E /Y /Q

DEL ..\Deploy\Install\website\*.csproj /Q/S
DEL ..\Deploy\Install\website\*.csproj.user  /Q/S
RD ..\Deploy\Install\website\admin\obj  /S /Q

RD ..\Deploy\Install\website\.svn /Q/S
RD ..\Deploy\Install\website\Admin\fckeditor\CVS /Q/S
RD ..\Deploy\Install\website\install\SQL\.svn  /Q/S
REM RD ..\Deploy\Install\website\We7Controls  /Q/S
REM RD ..\Deploy\Install\website\We7Controls  /Q/S
RD ..\Deploy\Install\website\Models  /Q/S

DEL ..\Deploy\Install\website\*.vssscc /Q/S
DEL ..\Deploy\Install\website\*.vspscc /Q/S

DEL ..\Deploy\Install\website\App_Data\DB\* /q/s

rd ..\Deploy\Install\website\User\bin /S /Q
rd ..\Deploy\Install\website\ModelUI\bin /S /Q
rd ..\Deploy\Install\website\Admin\bin /S /Q
rd ..\Deploy\Install\website\_skins /S /Q
rd ..\Deploy\Install\website\_data /S /Q
MD ..\Deploy\Install\website\_skins
MD ..\Deploy\Install\website\_data

MD ..\Deploy\Install\website\Plugins
MD ..\Deploy\Install\website\Plugins\FullTextSearch
MD ..\Deploy\Install\website\Plugins\SiteGroupPlugin
XCOPY "Solution Items\SiteGroup\FullTextSearch" ..\Deploy\Install\website\Plugins\FullTextSearch /E /Y /Q
XCOPY "Solution Items\SiteGroup\SiteGroupPlugin" ..\Deploy\Install\website\Plugins\SiteGroupPlugin /E /Y /Q
XCOPY "Solution Items\SiteGroup\Bin\*" ..\Deploy\Install\website\Bin\ /E /Y /Q
XCOPY "Solution Items\SiteGroup\SiteGroupPlugin\Data\Relation\*" ..\Deploy\Install\website\app_data\XML /E /Y /Q
copy "Solution Items\SiteGroup\SiteGroupPlugin\UI\IDClientWebService.asmx" ..\Deploy\Install\website\

MD ..\Deploy\install\website\install\SQL\plugin_SiteGroupPlugin
MD ..\Deploy\install\website\install\SQL\plugin_FullTextSearch
XCOPY "Solution Items\SiteGroup\SiteGroupPlugin\Data\*.xml" ..\Deploy\install\website\install\SQL\plugin_SiteGroupPlugin /E /Y /Q
XCOPY "Solution Items\SiteGroup\FullTextSearch\Data\*.xml" ..\Deploy\install\website\install\SQL\plugin_FullTextSearch /E /Y /Q
MD ..\Deploy\Install\website\Widgets\WidgetCollection\商城下载类
XCOPY "Solution Items\SiteGroup\FullTextSearch\widget\*.*" ..\Deploy\Install\website\Widgets\WidgetCollection\商城下载类 /E /Y /Q
ATTRIB ..\Deploy\install\website\install\SQL\plugin_SiteGroupPlugin\*.* -s -r
ATTRIB ..\Deploy\install\website\install\SQL\plugin_FullTextSearch\*.* -s -r
ATTRIB ..\Deploy\Install\website\Widgets\WidgetCollection\商城下载类\FullTextSearch.Result\*.* -s -r
ATTRIB ..\Deploy\Install\website\Widgets\WidgetCollection\商城下载类\FullTextSearch.Result\style\*.* -s -r
ATTRIB ..\Deploy\Install\website\Widgets\WidgetCollection\商城下载类\FullTextSearch.Bar\*.* -s -r
ATTRIB ..\Deploy\Install\website\Widgets\WidgetCollection\商城下载类\FullTextSearch.Bar\style\*.* -s -r

ATTRIB ..\Deploy\Install\website\*.* -s -r
ATTRIB ..\Deploy\Install\website\app_data\XML\*.* -s -r

CD ..\Deploy\Install\website
DEL We7.CMS.Install.zip
%ZIP% a -tzip ..\..\We7.CMS.Install.64bit.zip *
@ECHO finish We7.CMS.Install.zip!

@ECHO end