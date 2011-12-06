<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="/Widgets/WidgetCollection/网站页头/Header.LogoQueryQuick/Header.LogoQueryQuick.ascx" TagName="Header_LogoQueryQuick" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/导航类/ChannelMenu.TopMenu/ChannelMenu.TopMenu.ascx" TagName="ChannelMenu_TopMenu" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/导航类/ChannelMenu.CurrLocation/ChannelMenu.CurrentLocation.ascx" TagName="ChannelMenu_CurrentLocation" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/文章列表类/PagedArticleList.Default/PagedArticleList.Default.ascx" TagName="PagedArticleList_Default" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/导航类/Sidebar.ChannelNav/Sidebar.ChannelNav.ascx" TagName="Sidebar_ChannelNav" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/文章列表类/Recommand.Newest/Recommand.Newest.ascx" TagName="Recommand_Newest" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/其他类/Login.Default/Login.Default.ascx" TagName="Login_Default" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/其他类/Statistic.Default/Statistic.Default.ascx" TagName="Statistic_Default" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/网站页脚/Footer.Default/Footer.Default.ascx" TagName="Footer_Default" TagPrefix="wew" %>

<html>
<head runat="server">
<title></title>
  <link href="/Admin/VisualTemplate/Style/VisualDesign.LayoutsBasics.css" rel="stylesheet" type="text/css">
 
<link href="/Widgets/Themes/theme/Style.css" type="text/css" rel="stylesheet" class="themestyle" id="themestyle"><script src="/Widgets/Scripts/jquery.pack.js" type="text/javascript" class="jquerypack" id="jquerypack"></script><script src="/Widgets/Scripts/jquery.peex.js" type="text/javascript" class="jquerypeex" id="jquerypeex"></script><script src="/Widgets/Scripts/Plugins/Common.js" type="text/javascript" class="commonPlugin" id="commonPlugin"></script><link href="/_skins/we7/Style/UxStyle.css" type="text/css" rel="stylesheet"></head>
<body>
<div id="pagecontainer">
<we7design:We7ZonePlaceHolder id="bodyplaceholder" runat="server"><we7design:we7layout runat="server" id="we7layout_130933192282634">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933192282634_cloumn1">
 <we7design:we7layout runat="server" id="we7layout_130933254714514">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933254714514_cloumn1" style="" cssclass="wrapper">
 <wew:Header_LogoQueryQuick control="Header_LogoQueryQuick" filename="/Widgets/WidgetCollection/网站页头/Header.LogoQueryQuick/Header.LogoQueryQuick.ascx" id="Header_LogoQueryQuick_131244626753080" cssclass="Header_LogoQueryQuick" runat="server"></wew:Header_LogoQueryQuick></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130934208962079">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130934208962079_cloumn1">
 <wew:channelmenu_topmenu control="ChannelMenu_TopMenu" filename="/Widgets/WidgetCollection/导航类/ChannelMenu.TopMenu/ChannelMenu.TopMenu.ascx" id="ChannelMenu_TopMenu_130933256263485" cssclass="ChannelMenu_TopMenu" runat="server"></wew:channelmenu_topmenu></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130933209319721">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933209319721_cloumn1" style="" cssclass="wrapper">
 <we7design:we7layout runat="server" id="we7layout_13093321241877">
 <we7design:we7layoutcolumn float="left" width="730" widthunit="px" runat="server" id="we7layout_13093321241877_cloumn1" style="" cssclass="">
 <we7design:we7layout runat="server" id="we7layout_130933220178157">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933220178157_cloumn1" style="" cssclass="mtop10">
 <wew:ChannelMenu_CurrentLocation id="ChannelMenu_CurrentLocation_130933228802019" cssclass="ChannelMenu_CurrentLocation" filename="/Widgets/WidgetCollection/导航类/ChannelMenu.CurrLocation/ChannelMenu.CurrentLocation.ascx" runat="server"></wew:ChannelMenu_CurrentLocation></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130933218789050">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933218789050_cloumn1" style="" cssclass="mtop10">
 <wew:PagedArticleList_Default id="PagedArticleList_Default_130933976797792" margintop10="True" includechildren="True" dateformat="[MM-dd]" tags="" bordercolor="" icon="" isshow="False" titlelength="30" pager-requestpageindex="pi" pager-pagesize="20" cssclass="PagedArticleList_Default" pager-pagerdivclass="page_css page_line" ownerid="" pager-vmtemplatefilename="/Widgets/WidgetCollection/文章列表类/PagedArticleList.Default/vm/pager.vm" pager-pagerspanclass="pagecss" filename="/Widgets/WidgetCollection/文章列表类/PagedArticleList.Default/PagedArticleList.Default.ascx" runat="server"></wew:PagedArticleList_Default></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
 <we7design:we7layoutcolumn float="left" width="10" widthunit="px" runat="server" id="we7layout_13093321241877_cloumn2" style="" cssclass="">
 </we7design:we7layoutcolumn>
  <we7design:we7layoutcolumn float="left" width="220" widthunit="px" runat="server" id="we7layout_13093321241877_cloumn3" style="" cssclass="">
 <we7design:we7layout runat="server" id="we7layout_130933221724493">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933221724493_cloumn1">
 <wew:Sidebar_ChannelNav id="Sidebar_ChannelNav_130934881363517" icon="" bordercolor="" margintop10="True" cssclass="Sidebar_ChannelNav" ownerid="" filename="/Widgets/WidgetCollection/导航类/Sidebar.ChannelNav/Sidebar.ChannelNav.ascx" runat="server"></wew:Sidebar_ChannelNav></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130933221063147">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933221063147_cloumn1" style="" cssclass="mtop10">
 <wew:Recommand_Newest id="Recommand_Newest_130933262034713" tags="" margintop10="True" includechildren="True" isshow="False" bordercolor="" icon="" pagesize="10" ownerid="" cssclass="Recommand_Newest" dateformat="[MM-dd]" titlelength="26" slidersize="5" filename="/Widgets/WidgetCollection/文章列表类/Recommand.Newest/Recommand.Newest.ascx" runat="server"></wew:Recommand_Newest></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130933220775831">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933220775831_cloumn1" style="" cssclass="mtop10">
 <wew:Login_Default control="Login_Default" filename="/Widgets/WidgetCollection/其他类/Login.Default/Login.Default.ascx" id="Login_Default_130933263570286" cssclass="Login_Default" runat="server"></wew:Login_Default></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130933221438133">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933221438133_cloumn1" style="" cssclass="mtop10">
 <wew:Statistic_Default control="Statistic_Default" filename="/Widgets/WidgetCollection/其他类/Statistic.Default/Statistic.Default.ascx" id="Statistic_Default_130933264012976" cssclass="Statistic_Default" runat="server"></wew:Statistic_Default></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130933224477773">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933224477773_cloumn1" style="" cssclass="wrapper mtop10">
 </we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_13093322302078">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_13093322302078_cloumn1" style="" cssclass="mtop10">
 <wew:footer_default control="Footer_Default" filename="/Widgets/WidgetCollection/网站页脚/Footer.Default/Footer.Default.ascx" id="Footer_Default_130933235726098" cssclass="Footer_Default" runat="server"></wew:footer_default></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:We7ZonePlaceHolder>
</div>
</body>
</html>