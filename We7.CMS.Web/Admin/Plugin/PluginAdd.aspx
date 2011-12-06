<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PluginAdd.aspx.cs" MasterPageFile="~/Admin/theme/classic/content.Master"
    Inherits="We7.CMS.Web.Admin.Modules.Plugin.PluginAdd" %>

<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=ThemePath%>/css/article.css" media="screen" />
  

    <script src="<%=AppPath%>/cgi-bin/article.js" type="text/javascript"></script>

    <script src="<%=AppPath%>/cgi-bin/cookie.js" type="text/javascript"></script>

    <script src="<%=AppPath%>/cgi-bin/tags.js" type="text/javascript"></script>

    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="/Admin/Images/icons_plugins.gif" />
        <asp:Label ID="NameLabel" runat="server" Text="安装插件">
        </asp:Label>
        <span class="summary"><span id="navChannelSpan"></span>
            <asp:Label ID="SummaryLabel" runat="server" Text="通过安装插件，可能轻易扩展网站后台及前台功能。">
            </asp:Label>
        </span>
    </h2>
    <div id="position">
        <asp:Literal ID="PagePathLiteral" runat="server"></asp:Literal>
    </div>
    <div id="mycontent">
        <div class="Tab menuTab">
            <ul class="Tabs">
                <asp:Label runat="server" ID="MenuTabLabel"></asp:Label>
            </ul>
        </div>
        <div>
            <img src="/admin/images/bulb.gif" align="absmiddle" />
            <label class="block_info">
               <asp:Literal runat="server" ID="InfoLiteral" Text="插件可以无限扩展We7的功能。您可以从We7 插件网站自动安装插件或者在这个页面上传 .zip 格式的插件包。"></asp:Literal></label>
        </div>
        <br />
        <div class="clear">
        </div>
        <div id="rightWrapper">
            <div id="container" style="display: table">
                <asp:PlaceHolder runat="server" ID="ContentHolder"></asp:PlaceHolder>
            </div>
        </div>
    </div>
</asp:Content>
