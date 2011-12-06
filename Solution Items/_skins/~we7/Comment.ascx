<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="/Widgets/WidgetCollection/网站页头/Header.LogoQueryQuick/Header.LogoQueryQuick.ascx" TagName="Header_LogoQueryQuick" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/导航类/ChannelMenu.TopMenu/ChannelMenu.TopMenu.ascx" TagName="ChannelMenu_TopMenu" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/导航类/ChannelMenu.CurrLocation/ChannelMenu.CurrentLocation.ascx" TagName="ChannelMenu_CurrentLocation" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/文章列表类/ArticleSingle.Default/ArticleSingle.Default.ascx" TagName="ArticleSingle_Default" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/评论类/CommentsList.Common/CommentsList.Common.ascx" TagName="CommentsList_Common" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/导航类/Sidebar.ChannelNav/Sidebar.ChannelNav.ascx" TagName="Sidebar_ChannelNav" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/文章列表类/Recommand.Newest/Recommand.Newest.ascx" TagName="Recommand_Newest" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/其他类/Statistic.Default/Statistic.Default.ascx" TagName="Statistic_Default" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/网站页脚/Footer.Default/Footer.Default.ascx" TagName="Footer_Default" TagPrefix="wew" %>

<html>
<head runat="server">
<title></title>
  <link href="/Admin/VisualTemplate/Style/VisualDesign.LayoutsBasics.css" rel="stylesheet" type="text/css">
 
<link href="/Widgets/Themes/theme/Style.css" type="text/css" rel="stylesheet" class="themestyle" id="themestyle"><script src="/Widgets/Scripts/jquery.pack.js" type="text/javascript" class="jquerypack" id="jquerypack"></script><script src="/Widgets/Scripts/jquery.peex.js" type="text/javascript" class="jquerypeex" id="jquerypeex"></script><script src="/Widgets/Scripts/Plugins/Common.js" type="text/javascript" class="commonPlugin" id="commonPlugin"></script><link href="/Widgets/WidgetCollection/网站页头/Header.LogoQueryQuick/Style/Header.LogoQueryQuick.css" type="text/css" rel="stylesheet"><link href="/Widgets/WidgetCollection/导航类/ChannelMenu.TopMenu/Style/ChannelMenu.TopMenu.css" type="text/css" rel="stylesheet"><link href="/Widgets/WidgetCollection/导航类/ChannelMenu.CurrLocation/Style/ChannelMenu.CurrentLocation.css" type="text/css" rel="stylesheet"><link href="/Widgets/WidgetCollection/导航类/Sidebar.ChannelNav/Style/Sidebar.ChannelNav.css" type="text/css" rel="stylesheet"><link href="/Widgets/WidgetCollection/文章列表类/Recommand.Newest/Style/Recommand.Newest.css" type="text/css" rel="stylesheet"><link href="/Widgets/WidgetCollection/其他类/Statistic.Default/Style/Statistic.Default.css" type="text/css" rel="stylesheet"><link href="/Widgets/WidgetCollection/网站页脚/Footer.Default/Style/Footer.Default.css" type="text/css" rel="stylesheet"><link href="/Widgets/WidgetCollection/文章列表类/ArticleSingle.Default/Style/ArticleSingle.Default.css" type="text/css" rel="stylesheet"></head>
<body>
<div id="pagecontainer">
<we7design:We7ZonePlaceHolder id="bodyplaceholder" runat="server"><we7design:we7layout runat="server" id="we7layout_130938145502086">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130938145502086_cloumn1">
 <we7design:we7layout runat="server" id="we7layout_130938146767485">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130938146767485_cloumn1" style="" cssclass="wrapper">
 <wew:Header_LogoQueryQuick control="Header_LogoQueryQuick" filename="/Widgets/WidgetCollection/网站页头/Header.LogoQueryQuick/Header.LogoQueryQuick.ascx" id="Header_LogoQueryQuick_130938156559491" cssclass="Header_LogoQueryQuick" runat="server"></wew:Header_LogoQueryQuick></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130938148140479">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130938148140479_cloumn1">
 <wew:ChannelMenu_TopMenu control="ChannelMenu_TopMenu" filename="/Widgets/WidgetCollection/导航类/ChannelMenu.TopMenu/ChannelMenu.TopMenu.ascx" id="ChannelMenu_TopMenu_130938157251391" cssclass="ChannelMenu_TopMenu" runat="server"></wew:ChannelMenu_TopMenu></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130938147694872">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130938147694872_cloumn1" style="" cssclass="wrapper">
 <we7design:we7layout runat="server" id="we7layout_13093814975896">
 <we7design:we7layoutcolumn float="left" width="730" widthunit="px" runat="server" id="we7layout_13093814975896_cloumn1" style="" cssclass="">
 <we7design:we7layout runat="server" id="we7layout_130938152454435">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130938152454435_cloumn1">
 <wew:ChannelMenu_CurrentLocation control="ChannelMenu_CurrentLocation" filename="/Widgets/WidgetCollection/导航类/ChannelMenu.CurrLocation/ChannelMenu.CurrentLocation.ascx" id="ChannelMenu_CurrentLocation_13093815765059" cssclass="ChannelMenu_CurrentLocation" runat="server"></wew:ChannelMenu_CurrentLocation><we7design:we7layout runat="server" id="we7layout_130938181471187">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130938181471187_cloumn1">
 <wew:ArticleSingle_Default id="ArticleSingle_Default_130941631560514" dateformat="[MM-dd]" bordercolor="" titlelength="30" cssclass="ArticleSingle_Default" pagesize="3" filename="/Widgets/WidgetCollection/文章列表类/ArticleSingle.Default/ArticleSingle.Default.ascx" runat="server"></wew:ArticleSingle_Default></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130938151760682">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130938151760682_cloumn1">
 <wew:CommentsList_Common control="CommentsList_Common" filename="/Widgets/WidgetCollection/评论类/CommentsList.Common/CommentsList.Common.ascx" id="CommentsList_Common_130938158932786" cssclass="CommentsList_Common" runat="server"></wew:CommentsList_Common></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
 <we7design:we7layoutcolumn float="left" width="10" widthunit="px" runat="server" id="we7layout_13093814975896_cloumn2" style="" cssclass="">
 </we7design:we7layoutcolumn>
  <we7design:we7layoutcolumn float="left" width="220" widthunit="px" runat="server" id="we7layout_13093814975896_cloumn3" style="" cssclass="">
 <we7design:we7layout runat="server" id="we7layout_130938153802996">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130938153802996_cloumn1">
 <wew:Sidebar_ChannelNav control="Sidebar_ChannelNav" filename="/Widgets/WidgetCollection/导航类/Sidebar.ChannelNav/Sidebar.ChannelNav.ascx" id="Sidebar_ChannelNav_130938160552032" cssclass="Sidebar_ChannelNav" runat="server"></wew:Sidebar_ChannelNav></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130938153469492">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130938153469492_cloumn1" style="" cssclass="mtop10">
 <wew:Recommand_Newest control="Recommand_Newest" filename="/Widgets/WidgetCollection/文章列表类/Recommand.Newest/Recommand.Newest.ascx" id="Recommand_Newest_130938161750898" cssclass="Recommand_Newest" runat="server"></wew:Recommand_Newest></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130938153151083">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130938153151083_cloumn1" style="" cssclass="mtop10">
 <wew:Statistic_Default control="Statistic_Default" filename="/Widgets/WidgetCollection/其他类/Statistic.Default/Statistic.Default.ascx" id="Statistic_Default_130938162484884" cssclass="Statistic_Default" runat="server"></wew:Statistic_Default></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130938154918919">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130938154918919_cloumn1" style="" cssclass="wrapper mtop10">
</we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130938154505744">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130938154505744_cloumn1" style="" cssclass="mtop10">
 <wew:Footer_Default control="Footer_Default" filename="/Widgets/WidgetCollection/网站页脚/Footer.Default/Footer.Default.ascx" id="Footer_Default_130938166074481" cssclass="Footer_Default" runat="server"></wew:Footer_Default></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:We7ZonePlaceHolder>
</div>
</body>
</html>