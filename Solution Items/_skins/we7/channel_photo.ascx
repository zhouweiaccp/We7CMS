<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="/Widgets/WidgetCollection/网站页头/Header.LogoQueryQuick/Header.LogoQueryQuick.ascx" TagName="Header_LogoQueryQuick" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/导航类/ChannelMenu.TopMenu/ChannelMenu.TopMenu.ascx" TagName="ChannelMenu_TopMenu" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/导航类/ChannelMenu.CurrLocation/ChannelMenu.CurrentLocation.ascx" TagName="ChannelMenu_CurrentLocation" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/图文类/PagedPictureArticleList.Default/PagedPictureArticleList.Default.ascx" TagName="PagedPictureArticleList_Default" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/导航类/Sidebar.ChannelNav/Sidebar.ChannelNav.ascx" TagName="Sidebar_ChannelNav" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/文章列表类/Recommand.Newest/Recommand.Newest.ascx" TagName="Recommand_Newest" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/其他类/Statistic.Default/Statistic.Default.ascx" TagName="Statistic_Default" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/网站页脚/Footer.Default/Footer.Default.ascx" TagName="Footer_Default" TagPrefix="wew" %>

<html>
<head runat="server">
<title></title>
  <link href="/Admin/VisualTemplate/Style/VisualDesign.LayoutsBasics.css" rel="stylesheet" type="text/css">
 
<link href="/Widgets/Themes/theme/Style.css" type="text/css" rel="stylesheet" class="themestyle" id="themestyle"><script src="/Widgets/Scripts/jquery.pack.js" type="text/javascript" class="jquerypack" id="jquerypack"></script><script src="/Widgets/Scripts/jquery.peex.js" type="text/javascript" class="jquerypeex" id="jquerypeex"></script><script src="/Widgets/Scripts/Plugins/Common.js" type="text/javascript" class="commonPlugin" id="commonPlugin"></script><link href="/_skins/we7/Style/UxStyle.css" type="text/css" rel="stylesheet"></head>
<body>
<div id="pagecontainer">
<we7design:We7ZonePlaceHolder id="bodyplaceholder" runat="server"><we7design:we7layout runat="server" id="we7layout_130941965341857">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130941965341857_cloumn1">
 <we7design:we7layout runat="server" id="we7layout_130941965659620">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130941965659620_cloumn1" style="" cssclass="wrapper">
 <wew:Header_LogoQueryQuick control="Header_LogoQueryQuick" filename="/Widgets/WidgetCollection/网站页头/Header.LogoQueryQuick/Header.LogoQueryQuick.ascx" id="Header_LogoQueryQuick_131244467713621" cssclass="Header_LogoQueryQuick" runat="server"></wew:Header_LogoQueryQuick></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130941968632741">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130941968632741_cloumn1">
 <wew:ChannelMenu_TopMenu control="ChannelMenu_TopMenu" filename="/Widgets/WidgetCollection/导航类/ChannelMenu.TopMenu/ChannelMenu.TopMenu.ascx" id="ChannelMenu_TopMenu_130942076742246" cssclass="ChannelMenu_TopMenu" runat="server"></wew:ChannelMenu_TopMenu></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130941968199327">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130941968199327_cloumn1" style="" cssclass="wrapper">
 <we7design:we7layout runat="server" id="we7layout_130942044295739">
 <we7design:we7layoutcolumn float="left" width="730" widthunit="px" runat="server" id="we7layout_130942044295739_cloumn1" style="" cssclass="">
 <we7design:we7layout runat="server" id="we7layout_130942060670965">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130942060670965_cloumn1">
 <wew:ChannelMenu_CurrentLocation control="ChannelMenu_CurrentLocation" filename="/Widgets/WidgetCollection/导航类/ChannelMenu.CurrLocation/ChannelMenu.CurrentLocation.ascx" id="ChannelMenu_CurrentLocation_130942077426289" cssclass="ChannelMenu_CurrentLocation" runat="server"></wew:ChannelMenu_CurrentLocation></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130942060300839">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130942060300839_cloumn1" style="" cssclass="mtop10">
 <wew:PagedPictureArticleList_Default id="PagedPictureArticleList_Default_130942959987016" margintop10="True" thumbnailtag="product" dateformat="[MM-dd]" includechildren="True" icon="" bordercolor="" tags="" imageonly="True" pager-pagesize="10" pager-pagerdivclass="page_css page_line" cssclass="PagedPictureArticleList_Default" pager-requestpageindex="pi" ownerid="" titlelength="30" pager-pagerspanclass="pagecss" pager-vmtemplatefilename="/Widgets/WidgetCollection/文章列表类/PagedArticleList.Default/vm/pager.vm" filename="/Widgets/WidgetCollection/图文类/PagedPictureArticleList.Default/PagedPictureArticleList.Default.ascx" runat="server"></wew:PagedPictureArticleList_Default></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
 <we7design:we7layoutcolumn float="left" width="10" widthunit="px" runat="server" id="we7layout_130942044295739_cloumn2" style="" cssclass="">
 </we7design:we7layoutcolumn>
  <we7design:we7layoutcolumn float="left" width="220" widthunit="px" runat="server" id="we7layout_130942044295739_cloumn3" style="" cssclass="">
 <we7design:we7layout runat="server" id="we7layout_130942062140376">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130942062140376_cloumn1">
 <wew:Sidebar_ChannelNav control="Sidebar_ChannelNav" filename="/Widgets/WidgetCollection/导航类/Sidebar.ChannelNav/Sidebar.ChannelNav.ascx" id="Sidebar_ChannelNav_130942081821343" cssclass="Sidebar_ChannelNav" runat="server"></wew:Sidebar_ChannelNav></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130942061681235">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130942061681235_cloumn1" style="" cssclass="mtop10">
 <wew:Recommand_Newest control="Recommand_Newest" filename="/Widgets/WidgetCollection/文章列表类/Recommand.Newest/Recommand.Newest.ascx" id="Recommand_Newest_130942083305585" cssclass="Recommand_Newest" runat="server"></wew:Recommand_Newest></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130942063323695">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130942063323695_cloumn1" style="" cssclass="mtop10">
 <wew:Statistic_Default control="Statistic_Default" filename="/Widgets/WidgetCollection/其他类/Statistic.Default/Statistic.Default.ascx" id="Statistic_Default_130942085051563" cssclass="Statistic_Default" runat="server"></wew:Statistic_Default></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130942064118375">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130942064118375_cloumn1" style="" cssclass="wrapper mtop10">
 </we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130942063700340">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130942063700340_cloumn1" style="" cssclass="mtop10">
 <wew:Footer_Default control="Footer_Default" filename="/Widgets/WidgetCollection/网站页脚/Footer.Default/Footer.Default.ascx" id="Footer_Default_130942086806846" cssclass="Footer_Default" runat="server"></wew:Footer_Default></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:We7ZonePlaceHolder>
</div>
</body>
</html>