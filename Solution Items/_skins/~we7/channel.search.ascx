<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="/Widgets/WidgetCollection/网站页头/Header.LogoQueryQuick/Header.LogoQueryQuick.ascx" TagName="Header_LogoQueryQuick" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/导航类/ChannelMenu.TopMenu/ChannelMenu.TopMenu.ascx" TagName="ChannelMenu_TopMenu" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/静态类/Search_Title/Search.Title.ascx" TagName="Search_Title" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/搜索类/Search.ResultUI/PagedArticleList.DefaultSearch.ascx" TagName="PagedArticleList_DefaultSearch" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/文章列表类/Recommand.Newest/Recommand.Newest.ascx" TagName="Recommand_Newest" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/其他类/Login.Default/Login.Default.ascx" TagName="Login_Default" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/其他类/Statistic.Default/Statistic.Default.ascx" TagName="Statistic_Default" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/网站页脚/Footer.Default/Footer.Default.ascx" TagName="Footer_Default" TagPrefix="wew" %>

<html>
<head runat="server">
<title></title>
  <link href="/Admin/VisualTemplate/Style/VisualDesign.LayoutsBasics.css" rel="stylesheet" type="text/css">
 
<link href="/Widgets/Themes/theme/Style.css" type="text/css" rel="stylesheet" class="themestyle" id="themestyle"><script src="/Widgets/Scripts/jquery.pack.js" type="text/javascript" class="jquerypack" id="jquerypack"></script><script src="/Widgets/Scripts/jquery.peex.js" type="text/javascript" class="jquerypeex" id="jquerypeex"></script><script src="/Widgets/Scripts/Plugins/Common.js" type="text/javascript" class="commonPlugin" id="commonPlugin"></script><link href="/Widgets/WidgetCollection/网站页头/Header.LogoQueryQuick/Style/Header.LogoQueryQuick.css" type="text/css" rel="stylesheet"><link href="/Widgets/WidgetCollection/导航类/ChannelMenu.TopMenu/Style/ChannelMenu.TopMenu.css" type="text/css" rel="stylesheet"><link href="/Widgets/WidgetCollection/网站页脚/Footer.Default/Style/Footer.Default.css" type="text/css" rel="stylesheet"><link href="/Widgets/WidgetCollection/静态类/Search_Title/Style/Search_Title.css" type="text/css" rel="stylesheet"><link href="/Widgets/WidgetCollection/搜索类/Search.ResultUI/Style/PagedArticleList.DefaultSearch.css" type="text/css" rel="stylesheet"><link href="/Widgets/WidgetCollection/文章列表类/Recommand.Newest/Style/Recommand.Newest.css" type="text/css" rel="stylesheet"><link href="/Widgets/WidgetCollection/其他类/Login.Default/Style/Login.Default.css" type="text/css" rel="stylesheet"><link href="/Widgets/WidgetCollection/其他类/Statistic.Default/Style/Statistic.Default.css" type="text/css" rel="stylesheet"></head>
<body>
<div id="pagecontainer">
<we7design:We7ZonePlaceHolder id="bodyplaceholder" runat="server"><we7design:we7layout runat="server" id="we7layout_130933836547933">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933836547933_cloumn1">
 <we7design:we7layout runat="server" id="we7layout_130933836797533">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933836797533_cloumn1" style="" cssclass="wrapper">
 <wew:Header_LogoQueryQuick id="Header_LogoQueryQuick_130933856379122" iconwap="" iconhomepage="" iconcollection="" iconrss="" cssclass="Header_LogoQueryQuick" searchpage="search.aspx" iconlogin="" filename="/Widgets/WidgetCollection/网站页头/Header.LogoQueryQuick/Header.LogoQueryQuick.ascx" runat="server"></wew:Header_LogoQueryQuick></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130933841864159">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933841864159_cloumn1">
 <wew:ChannelMenu_TopMenu control="ChannelMenu_TopMenu" filename="/Widgets/WidgetCollection/导航类/ChannelMenu.TopMenu/ChannelMenu.TopMenu.ascx" id="ChannelMenu_TopMenu_130933857341122" cssclass="ChannelMenu_TopMenu" runat="server"></wew:ChannelMenu_TopMenu></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_13093384238312">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_13093384238312_cloumn1" style="" cssclass="wrapper">
 <we7design:we7layout runat="server" id="we7layout_130933844414578">
 <we7design:we7layoutcolumn float="left" width="730" widthunit="px" runat="server" id="we7layout_130933844414578_cloumn1" style="" cssclass="">
 <wew:Search_Title id="Search_Title_130933883011515" cssclass="Search_Title" filename="/Widgets/WidgetCollection/静态类/Search_Title/Search.Title.ascx" runat="server"></wew:Search_Title><we7design:we7layout runat="server" id="we7layout_130933852682615">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933852682615_cloumn1">
 <we7design:we7layout runat="server" id="we7layout_130933853101035">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933853101035_cloumn1">
 <wew:PagedArticleList_DefaultSearch id="PagedArticleList_DefaultSearch_130935902006790" pager-requestpageindex="pi" pager-pagesize="10" thumbnailtag="flash" pager-pagerdivclass="page_css page_line" ownerid="" pager-vmtemplatefilename="/Widgets/WidgetCollection/文章列表类/PagedArticleList.Default/vm/pager.vm" pager-pagerspanclass="pagecss" dateformat="[MM-dd]" includechildren="True" cssclass="PagedArticleList_DefaultSearch" titlelength="30" icon="" bordercolor="" margintop10="True" filename="/Widgets/WidgetCollection/搜索类/Search.ResultUI/PagedArticleList.DefaultSearch.ascx" runat="server"></wew:PagedArticleList_DefaultSearch></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
 <we7design:we7layoutcolumn float="left" width="10" widthunit="px" runat="server" id="we7layout_130933844414578_cloumn2" style="" cssclass="">
 </we7design:we7layoutcolumn>
  <we7design:we7layoutcolumn float="left" width="220" widthunit="px" runat="server" id="we7layout_130933844414578_cloumn3" style="" cssclass="">
 <we7design:we7layout runat="server" id="we7layout_130933852080650">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933852080650_cloumn1">
 <wew:Recommand_Newest id="Recommand_Newest_130936015342642" includechildren="True" dateformat="[MM-dd]" margintop10="True" bordercolor="" icon="" ownerid="{5c8ad3f7-cc2e-46da-89c0-70caae15e2db}" cssclass="Recommand_Newest" pagesize="10" titlelength="30" slidersize="5" filename="/Widgets/WidgetCollection/文章列表类/Recommand.Newest/Recommand.Newest.ascx" runat="server"></wew:Recommand_Newest></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130933851866710">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933851866710_cloumn1">
 <wew:Login_Default control="Login_Default" filename="/Widgets/WidgetCollection/其他类/Login.Default/Login.Default.ascx" id="Login_Default_130936017707528" cssclass="Login_Default" runat="server"></wew:Login_Default></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130933851514810">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933851514810_cloumn1">
 <wew:Statistic_Default control="Statistic_Default" filename="/Widgets/WidgetCollection/其他类/Statistic.Default/Statistic.Default.ascx" id="Statistic_Default_130936018424353" cssclass="Statistic_Default" runat="server"></wew:Statistic_Default></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130933854241546">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933854241546_cloumn1" style="" cssclass="wrapper">
 </we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_13093385371263">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_13093385371263_cloumn1">
 <wew:Footer_Default control="Footer_Default" filename="/Widgets/WidgetCollection/网站页脚/Footer.Default/Footer.Default.ascx" id="Footer_Default_130933861241036" cssclass="Footer_Default" runat="server"></wew:Footer_Default></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:We7ZonePlaceHolder>
</div>
</body>
</html>