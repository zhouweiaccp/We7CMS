<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="/Widgets/WidgetCollection/网站页头/Header.LogoQueryQuick/Header.LogoQueryQuick.ascx" TagName="Header_LogoQueryQuick" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/导航类/ChannelMenu.TopMenu/ChannelMenu.TopMenu.ascx" TagName="ChannelMenu_TopMenu" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/导航类/ChannelMenu.CurrLocation/ChannelMenu.CurrentLocation.ascx" TagName="ChannelMenu_CurrentLocation" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/文章列表类/ArticleView.Default/ArticleView.Default.ascx" TagName="ArticleView_Default" TagPrefix="wew" %>

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
<we7design:We7ZonePlaceHolder id="bodyplaceholder" runat="server"><we7design:we7layout runat="server" id="we7layout_130933546681435">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933546681435_cloumn1">
 <we7design:we7layout runat="server" id="we7layout_130933546990939">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933546990939_cloumn1" style="" cssclass="wrapper">
 <wew:Header_LogoQueryQuick control="Header_LogoQueryQuick" filename="/Widgets/WidgetCollection/网站页头/Header.LogoQueryQuick/Header.LogoQueryQuick.ascx" id="Header_LogoQueryQuick_130933598301475" cssclass="Header_LogoQueryQuick" runat="server"></wew:Header_LogoQueryQuick></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130933584590735">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933584590735_cloumn1">
 <wew:ChannelMenu_TopMenu control="ChannelMenu_TopMenu" filename="/Widgets/WidgetCollection/导航类/ChannelMenu.TopMenu/ChannelMenu.TopMenu.ascx" id="ChannelMenu_TopMenu_130933598941974" cssclass="ChannelMenu_TopMenu" runat="server"></wew:ChannelMenu_TopMenu></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130933587106714">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933587106714_cloumn1" style="" cssclass="wrapper">
 <we7design:we7layout runat="server" id="we7layout_130933589841226">
 <we7design:we7layoutcolumn float="left" width="730" widthunit="px" runat="server" id="we7layout_130933589841226_cloumn1" style="" cssclass="">
 <we7design:we7layout runat="server" id="we7layout_130933601056912">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933601056912_cloumn1">
 <wew:ChannelMenu_CurrentLocation control="ChannelMenu_CurrentLocation" filename="/Widgets/WidgetCollection/导航类/ChannelMenu.CurrLocation/ChannelMenu.CurrentLocation.ascx" id="ChannelMenu_CurrentLocation_130933601890849" cssclass="ChannelMenu_CurrentLocation" runat="server"></wew:ChannelMenu_CurrentLocation></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130933600477083">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933600477083_cloumn1" style="" cssclass="mtop10">
 <wew:ArticleView_Default control="ArticleView_Default" filename="/Widgets/WidgetCollection/文章列表类/ArticleView.Default/ArticleView.Default.ascx" id="ArticleView_Default_130933604005558" cssclass="ArticleView_Default" runat="server"></wew:ArticleView_Default></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
 <we7design:we7layoutcolumn float="left" width="10" widthunit="px" runat="server" id="we7layout_130933589841226_cloumn2" style="" cssclass="">
 </we7design:we7layoutcolumn>
  <we7design:we7layoutcolumn float="left" width="220" widthunit="px" runat="server" id="we7layout_130933589841226_cloumn3" style="" cssclass="">
 <we7design:we7layout runat="server" id="we7layout_130933596079120">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933596079120_cloumn1">
 <wew:Sidebar_ChannelNav control="Sidebar_ChannelNav" filename="/Widgets/WidgetCollection/导航类/Sidebar.ChannelNav/Sidebar.ChannelNav.ascx" id="Sidebar_ChannelNav_130933610279495" cssclass="Sidebar_ChannelNav" runat="server"></wew:Sidebar_ChannelNav></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130933595735978">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933595735978_cloumn1" style="" cssclass="mtop10">
 <wew:Recommand_Newest control="Recommand_Newest" filename="/Widgets/WidgetCollection/文章列表类/Recommand.Newest/Recommand.Newest.ascx" id="Recommand_Newest_130933611190216" cssclass="Recommand_Newest" runat="server"></wew:Recommand_Newest></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130933595483273">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933595483273_cloumn1" style="" cssclass="mtop10">
 <wew:Login_Default control="Login_Default" filename="/Widgets/WidgetCollection/其他类/Login.Default/Login.Default.ascx" id="Login_Default_130933612336138" cssclass="Login_Default" runat="server"></wew:Login_Default><wew:Statistic_Default control="Statistic_Default" filename="/Widgets/WidgetCollection/其他类/Statistic.Default/Statistic.Default.ascx" id="Statistic_Default_13093361319301" cssclass="Statistic_Default" runat="server"></wew:Statistic_Default></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130933595058669">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933595058669_cloumn1" style="" cssclass="mtop10">
 </we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130933592063348">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130933592063348_cloumn1" style="" cssclass="wrapper mtop10">
 </we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_13093359239788">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_13093359239788_cloumn1" style="" cssclass="mtop10">
 <wew:Footer_Default control="Footer_Default" filename="/Widgets/WidgetCollection/网站页脚/Footer.Default/Footer.Default.ascx" id="Footer_Default_130933677698093" cssclass="Footer_Default" runat="server"></wew:Footer_Default></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:We7ZonePlaceHolder>
</div>
</body>
</html>