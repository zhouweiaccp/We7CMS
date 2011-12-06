<%@ Page Title="" Language="C#"  AutoEventWireup="true"
    CodeFile="InfoSharingConfig.aspx.cs" Inherits="We7.Plugin.SiteGroupPlugin.InfoSharing.InfoSharingConfig" %>

<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=ThemePath%>/css/tags.css" media="all" />
    <link rel="stylesheet" type="text/css" href="<%=ThemePath%>/css/article.css" media="screen" />

    <script src="<%=AppPath%>/cgi-bin/article.js" type="text/javascript"></script>

    <script src="<%=AppPath%>/cgi-bin/cookie.js" type="text/javascript"></script>

    <script src="<%=AppPath%>/ajax/jquery/jquery-1.2.1.min.js" type="text/javascript"></script>

    <script src="<%=AppPath%>/cgi-bin/tags.js" type="text/javascript"></script>

    <div>
    <h2  class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_share.gif"/>
        <asp:Label ID="NameLabel" runat="server" Text="共享参数配置">
        </asp:Label>
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server" Text="">这里设置共享的基础参数
            </asp:Label>
        </span>
    </h2>
    </div>
    <div id="position">
        <asp:Literal ID="PagePathLiteral" runat="server"></asp:Literal>
    </div>
    <div id="mycontent">
        <div class="Tab menuTab">
            <ul class="Tabs">
                <asp:Label runat="server" ID="MenuTabLabel"></asp:Label>
            </ul>
        </div>
        <div class="clear">
        </div>
        <div id="rightWrapper">
            <div id="container">
                <asp:PlaceHolder runat="server" ID="ContentHolder"></asp:PlaceHolder>
            </div>
        </div>
    </div>
</asp:Content>
