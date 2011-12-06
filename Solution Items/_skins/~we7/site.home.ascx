<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="/Widgets/WidgetCollection/网站页头/Header.LogoQueryQuick/Header.LogoQueryQuick.ascx" TagName="Header_LogoQueryQuick" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/导航类/ChannelMenu.TopMenu/ChannelMenu.TopMenu.ascx" TagName="ChannelMenu_TopMenu" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/导航类/TagList.Hot/TagList.Hot.ascx" TagName="TagList_Hot" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/文章列表类/LatestNews.LatestTopPictureEx/LatestNews.LatestTopPicture.ascx" TagName="LatestNews_LatestTopPicture" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/文章列表类/ArticleList.Default/ArticleList.Default.ascx" TagName="ArticleList_Default" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/文章列表类/ArticleList.2ColumnWidthImage/ArticleList.2ColumnWidthImage.ascx" TagName="ArticleList_2ColumnWidthImage" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/文章列表类/NewsTab.Default/NewsTab.Default.ascx" TagName="NewsTab_Default" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/图文类/PictureArticleList.Default/PictureArticleList.Default.ascx" TagName="PictureArticleList_Default" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/文章列表类/ArticleList.SideList/ArticleList.SideList.ascx" TagName="ArticleList_SideList" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/文章列表类/Recommand.Newest/Recommand.Newest.ascx" TagName="Recommand_Newest" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/其他类/Login.Default/Login.Default.ascx" TagName="Login_Default" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/其他类/Statistic.Default/Statistic.Default.ascx" TagName="Statistic_Default" TagPrefix="wew" %>

<%@ Register Src="/Widgets/WidgetCollection/网站页脚/Footer.Default/Footer.Default.ascx" TagName="Footer_Default" TagPrefix="wew" %>

<html>
<head runat="server">
<title></title>
  <link href="/Admin/VisualTemplate/Style/VisualDesign.LayoutsBasics.css" rel="stylesheet" type="text/css">
 
<link href="/Widgets/Themes/theme/Style.css" type="text/css" rel="stylesheet" class="themestyle" id="themestyle"><script src="/Widgets/Scripts/jquery.pack.js" type="text/javascript" class="jquerypack" id="jquerypack"></script><script src="/Widgets/Scripts/jquery.peex.js" type="text/javascript" class="jquerypeex" id="jquerypeex"></script><script src="/Widgets/Scripts/Plugins/Common.js" type="text/javascript" class="commonPlugin" id="commonPlugin"></script><link href="/Widgets/WidgetCollection/网站页头/Header.LogoQueryQuick/Style/Header.LogoQueryQuick.css" type="text/css" rel="stylesheet"><link href="/Widgets/WidgetCollection/导航类/ChannelMenu.TopMenu/Style/ChannelMenu.TopMenu.css" type="text/css" rel="stylesheet"><link href="/Widgets/WidgetCollection/导航类/TagList.Hot/Style/TagList.Hot.css" type="text/css" rel="stylesheet"><link href="/Widgets/WidgetCollection/文章列表类/LatestNews.LatestTopPictureEx/Style/LatestNews.LatestTopPicture.css" type="text/css" rel="stylesheet"><link href="/Widgets/WidgetCollection/文章列表类/ArticleList.2ColumnWidthImage/Style/ArticleList.2ColumnWidthImage.css" type="text/css" rel="stylesheet"><link href="/Widgets/WidgetCollection/文章列表类/ArticleList.Default/Style/ArticleList.Default.css" type="text/css" rel="stylesheet"><link href="/Widgets/WidgetCollection/图文类/PictureArticleList.Default/Style/PictureArticleList.Default.css" type="text/css" rel="stylesheet"><link href="/Widgets/WidgetCollection/网站页脚/Footer.Default/Style/Footer.Default.css" type="text/css" rel="stylesheet"><link href="/Widgets/WidgetCollection/文章列表类/ArticleList.SideList/Style/ArticleList.SideList.css" type="text/css" rel="stylesheet"><link href="/Widgets/WidgetCollection/文章列表类/Recommand.Newest/Style/Recommand.Newest.css" type="text/css" rel="stylesheet"><link href="/Widgets/WidgetCollection/其他类/Login.Default/Style/Login.Default.css" type="text/css" rel="stylesheet"><link href="/Widgets/WidgetCollection/其他类/Statistic.Default/Style/Statistic.Default.css" type="text/css" rel="stylesheet"><link href="/Widgets/WidgetCollection/文章列表类/NewsTab.Default/Style/NewsTab.Default.css" type="text/css" rel="stylesheet"></head>
<body>
<div id="pagecontainer">
<we7design:We7ZonePlaceHolder id="bodyplaceholder" runat="server"><we7design:we7layout runat="server" id="we7layout_130932719374951">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130932719374951_cloumn1">
 <we7design:we7layout runat="server" id="we7layout_130932729364237">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130932729364237_cloumn1" style="" cssclass="wrapper">
 <wew:Header_LogoQueryQuick control="Header_LogoQueryQuick" filename="/Widgets/WidgetCollection/网站页头/Header.LogoQueryQuick/Header.LogoQueryQuick.ascx" id="Header_LogoQueryQuick_130932797706529" cssclass="Header_LogoQueryQuick" runat="server"></wew:Header_LogoQueryQuick></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_13093274012044">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_13093274012044_cloumn1">
 <wew:ChannelMenu_TopMenu control="ChannelMenu_TopMenu" filename="/Widgets/WidgetCollection/导航类/ChannelMenu.TopMenu/ChannelMenu.TopMenu.ascx" id="ChannelMenu_TopMenu_130932799769838" cssclass="ChannelMenu_TopMenu" runat="server"></wew:ChannelMenu_TopMenu></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130932743353837">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130932743353837_cloumn1" style="" cssclass="wrapper">
 <wew:TagList_Hot control="TagList_Hot" filename="/Widgets/WidgetCollection/导航类/TagList.Hot/TagList.Hot.ascx" id="TagList_Hot_130932801572888" cssclass="TagList_Hot" runat="server"></wew:TagList_Hot></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_13093275295931">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_13093275295931_cloumn1" style="" cssclass="wrapper">
 <we7design:we7layout runat="server" id="we7layout_130932746263294">
 <we7design:we7layoutcolumn float="left" width="730" widthunit="px" runat="server" id="we7layout_130932746263294_cloumn1" style="" cssclass="">
 <we7design:we7layout runat="server" id="we7layout_130932767237073">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130932767237073_cloumn1">
 <wew:LatestNews_LatestTopPicture id="LatestNews_LatestTopPicture_130932802792449" ownerid1="{5c8ad3f7-cc2e-46da-89c0-70caae15e2db}" ownerid2="{aa692c55-2a29-4b48-b1d2-f2977fa0adb3}" ownerid3="{91a2ba3a-42e3-4521-957a-e0361ba6d7f7}" ownerid4="" ownerid5="" ownerid6="" ownerid7="" ownerid8="" ownerid9="" ownerid10="" tags="" thumbnailtag="flash" isshow="False" icon="" titlelength="30" pagesize="6" cssclass="LatestNews_LatestTopPicture" dateformat="[MM-dd]" slidersize="5" filename="/Widgets/WidgetCollection/文章列表类/LatestNews.LatestTopPictureEx/LatestNews.LatestTopPicture.ascx" runat="server"></wew:LatestNews_LatestTopPicture></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130932768916235">
 <we7design:we7layoutcolumn float="left" width="360" widthunit="px" runat="server" id="we7layout_130932768916235_cloumn1" style="" cssclass="">
 <wew:ArticleList_Default id="ArticleList_Default_130932851298798" marginleft10="True" margintop10="True" icon="" bordercolor="" tags="" includechildren="True" ownerid="{69f15b3b-d91d-4aa5-b45c-b2f5cd3e21de}" cssclass="ArticleList_Default" pagesize="10" dateformat="[MM-dd]" titlelength="30" filename="/Widgets/WidgetCollection/文章列表类/ArticleList.Default/ArticleList.Default.ascx" runat="server"></wew:ArticleList_Default></we7design:we7layoutcolumn>
 <we7design:we7layoutcolumn float="left" width="10" widthunit="px" runat="server" id="we7layout_130932768916235_cloumn2" style="" cssclass="">
 </we7design:we7layoutcolumn>
  <we7design:we7layoutcolumn float="left" width="360" widthunit="px" runat="server" id="we7layout_130932768916235_cloumn3" style="" cssclass="">
 <wew:ArticleList_2ColumnWidthImage id="ArticleList_2ColumnWidthImage_13093280748279" descriptionlength="30" thumbnailtag="sytp" marginleft10="True" bordercolor="" tags="" icon="" margintop10="True" ownerid="{6a209ce7-d5f2-4f87-b675-fa0625fe7c37}" picdesclength="20" cssclass="ArticleList_2ColumnWidthImage" includechildren="True" titlelength="30" pagesize="3" filename="/Widgets/WidgetCollection/文章列表类/ArticleList.2ColumnWidthImage/ArticleList.2ColumnWidthImage.ascx" runat="server"></wew:ArticleList_2ColumnWidthImage></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130932771687962">
 <we7design:we7layoutcolumn float="left" width="360" widthunit="px" runat="server" id="we7layout_130932771687962_cloumn1" style="" cssclass="">
 <wew:ArticleList_2ColumnWidthImage id="ArticleList_2ColumnWidthImage_13093285783281" descriptionlength="30" thumbnailtag="sytp" marginleft10="True" bordercolor="" tags="" icon="" margintop10="True" ownerid="{e345e8ff-5e52-4261-8a72-c578b86d9636}" picdesclength="20" cssclass="ArticleList_2ColumnWidthImage" includechildren="True" titlelength="30" pagesize="3" filename="/Widgets/WidgetCollection/文章列表类/ArticleList.2ColumnWidthImage/ArticleList.2ColumnWidthImage.ascx" runat="server"></wew:ArticleList_2ColumnWidthImage></we7design:we7layoutcolumn>
 <we7design:we7layoutcolumn float="left" width="10" widthunit="px" runat="server" id="we7layout_130932771687962_cloumn2" style="" cssclass="">
 </we7design:we7layoutcolumn>
  <we7design:we7layoutcolumn float="left" width="360" widthunit="px" runat="server" id="we7layout_130932771687962_cloumn3" style="" cssclass="">
 <wew:ArticleList_Default id="ArticleList_Default_130932857421419" marginleft10="True" margintop10="True" icon="" bordercolor="" tags="" includechildren="True" ownerid="{a5aa15e9-040c-4f61-a32e-368a55e3425b}" cssclass="ArticleList_Default" pagesize="10" dateformat="[MM-dd]" titlelength="30" filename="/Widgets/WidgetCollection/文章列表类/ArticleList.Default/ArticleList.Default.ascx" runat="server"></wew:ArticleList_Default></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130934850173021">
 <we7design:we7layoutcolumn float="left" width="360" widthunit="px" runat="server" id="we7layout_130934850173021_cloumn1" style="" cssclass="">
 <wew:NewsTab_Default id="NewsTab_Default_130937215746442" ownerid1="" ownerid2="{6213b8ea-a159-48c7-b5f5-24f21a6093e7}" ownerid3="" ownerid4="" ownerid5="" ownerid6="" ownerid7="" ownerid8="" ownerid9="" ownerid10="" margintop10="True" includechildren="True" marginleft10="True" icon="" bordercolor="" cssclass="NewsTab_Default" tags="" pagesize="10" dateformat="[MM-dd]" titlelength="30" filename="/Widgets/WidgetCollection/文章列表类/NewsTab.Default/NewsTab.Default.ascx" runat="server"></wew:NewsTab_Default></we7design:we7layoutcolumn>
 <we7design:we7layoutcolumn float="left" width="10" widthunit="px" runat="server" id="we7layout_130934850173021_cloumn2" style="" cssclass="">
 </we7design:we7layoutcolumn>
  <we7design:we7layoutcolumn float="left" width="360" widthunit="px" runat="server" id="we7layout_130934850173021_cloumn3" style="" cssclass="">
 <wew:NewsTab_Default id="NewsTab_Default_130936646426295" ownerid1="" ownerid2="{a5aa15e9-040c-4f61-a32e-368a55e3425b}" ownerid3="{316decde-3093-4c23-8b37-27df15a02524}" ownerid4="" ownerid5="" ownerid6="" ownerid7="" ownerid8="" ownerid9="" ownerid10="" tags="" bordercolor="" pagesize="10" titlelength="30" dateformat="[MM-dd]" margintop10="True" cssclass="NewsTab_Default" includechildren="True" icon="/_skins/we7/files/bg_t.gif" marginleft10="True" filename="/Widgets/WidgetCollection/文章列表类/NewsTab.Default/NewsTab.Default.ascx" runat="server"></wew:NewsTab_Default></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_13093277458947">
 <we7design:we7layoutcolumn float="left" width="360" widthunit="px" runat="server" id="we7layout_13093277458947_cloumn1" style="" cssclass="">
 <wew:ArticleList_Default id="ArticleList_Default_130932858798993" marginleft10="True" margintop10="True" icon="" bordercolor="" tags="" includechildren="True" ownerid="{af5457f8-8557-4024-8c90-d1a4e141cac9}" cssclass="ArticleList_Default" pagesize="10" dateformat="[MM-dd]" titlelength="30" filename="/Widgets/WidgetCollection/文章列表类/ArticleList.Default/ArticleList.Default.ascx" runat="server"></wew:ArticleList_Default></we7design:we7layoutcolumn>
 <we7design:we7layoutcolumn float="left" width="10" widthunit="px" runat="server" id="we7layout_13093277458947_cloumn2" style="" cssclass="">
 </we7design:we7layoutcolumn>
  <we7design:we7layoutcolumn float="left" width="360" widthunit="px" runat="server" id="we7layout_13093277458947_cloumn3" style="" cssclass="">
 <wew:ArticleList_Default id="ArticleList_Default_13093648316682" marginleft10="True" margintop10="True" icon="" bordercolor="" tags="" includechildren="True" ownerid="{d2272803-53fa-4099-b36a-10df56c87ddf}" cssclass="ArticleList_Default" pagesize="10" dateformat="[MM-dd]" titlelength="30" filename="/Widgets/WidgetCollection/文章列表类/ArticleList.Default/ArticleList.Default.ascx" runat="server"></wew:ArticleList_Default></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130932780615328">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130932780615328_cloumn1">
 <wew:PictureArticleList_Default id="PictureArticleList_Default_1309329117815100" ownerid="{5c8ad3f7-cc2e-46da-89c0-70caae15e2db}" titlelength="20" thumbnailtag="sytp" pagesize="1" tags="" icon="" cssclass="PictureArticleList_Default" bordercolor="" includechildren="True" filename="/Widgets/WidgetCollection/图文类/PictureArticleList.Default/PictureArticleList.Default.ascx" runat="server"></wew:PictureArticleList_Default></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
 <we7design:we7layoutcolumn float="left" width="10" widthunit="px" runat="server" id="we7layout_130932746263294_cloumn2" style="" cssclass="">
 </we7design:we7layoutcolumn>
  <we7design:we7layoutcolumn float="left" width="220" widthunit="px" runat="server" id="we7layout_130932746263294_cloumn3" style="" cssclass="">
 <we7design:we7layout runat="server" id="we7layout_130932779119839">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130932779119839_cloumn1">
 <wew:ArticleList_SideList id="ArticleList_SideList_130932920298594" thumbnailtag="flash" marginleft10="True" icon="" tags="" bordercolor="" margintop10="True" ownerid="{20dcf8e8-c7f8-46e9-9b6b-2b9ebe3d7875}" cssclass="ArticleList_SideList" pagesize="10" includechildren="True" titlelength="26" filename="/Widgets/WidgetCollection/文章列表类/ArticleList.SideList/ArticleList.SideList.ascx" runat="server"></wew:ArticleList_SideList></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130932778638220">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130932778638220_cloumn1" style="" cssclass="mtop10">
 <wew:Recommand_Newest id="Recommand_Newest_13093292297533" includechildren="True" dateformat="[MM-dd]" margintop10="True" bordercolor="" icon="" ownerid="" cssclass="Recommand_Newest" pagesize="10" titlelength="26" slidersize="5" filename="/Widgets/WidgetCollection/文章列表类/Recommand.Newest/Recommand.Newest.ascx" runat="server"></wew:Recommand_Newest><we7design:we7layout runat="server" id="we7layout_130943337821719">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130943337821719_cloumn1">
 <wew:ArticleList_SideList id="ArticleList_SideList_130943339684886" icon="" thumbnailtag="flash" marginleft10="True" tags="" isshow="False" bordercolor="" pagesize="10" ownerid="{1c17ab0d-5606-4a51-8d20-309ba26af51b}" cssclass="ArticleList_SideList" margintop10="True" includechildren="True" titlelength="26" filename="/Widgets/WidgetCollection/文章列表类/ArticleList.SideList/ArticleList.SideList.ascx" runat="server"></wew:ArticleList_SideList></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130943338298048">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130943338298048_cloumn1">
 <wew:ArticleList_SideList id="ArticleList_SideList_130943368251755" icon="" thumbnailtag="flash" marginleft10="True" tags="" isshow="False" bordercolor="" pagesize="10" ownerid="{3b886efa-485d-4d33-8b29-ba3fb0180975}" cssclass="ArticleList_SideList" margintop10="True" includechildren="True" titlelength="26" filename="/Widgets/WidgetCollection/文章列表类/ArticleList.SideList/ArticleList.SideList.ascx" runat="server"></wew:ArticleList_SideList></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130932778251858">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130932778251858_cloumn1" style="" cssclass="mtop10">
 <wew:Login_Default control="Login_Default" filename="/Widgets/WidgetCollection/其他类/Login.Default/Login.Default.ascx" id="Login_Default_130932926245829" cssclass="Login_Default" runat="server"></wew:Login_Default></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_13093277786174">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_13093277786174_cloumn1" style="" cssclass="mtop10">
 <wew:Statistic_Default control="Statistic_Default" filename="/Widgets/WidgetCollection/其他类/Statistic.Default/Statistic.Default.ascx" id="Statistic_Default_13093292837964" cssclass="Statistic_Default" runat="server"></wew:Statistic_Default></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130932786393281">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130932786393281_cloumn1" style="" cssclass="wrapper mtop10">
 </we7design:we7layoutcolumn>
</we7design:we7layout><we7design:we7layout runat="server" id="we7layout_130932795177344">
 <we7design:we7layoutcolumn float="none" widthunit="%" width="100" runat="server" id="we7layout_130932795177344_cloumn1" style="" cssclass="mtop10">
 <wew:Footer_Default control="Footer_Default" filename="/Widgets/WidgetCollection/网站页脚/Footer.Default/Footer.Default.ascx" id="Footer_Default_130932917918994" cssclass="Footer_Default" runat="server"></wew:Footer_Default></we7design:we7layoutcolumn>
</we7design:we7layout></we7design:We7ZonePlaceHolder>
</div>
</body>
</html>