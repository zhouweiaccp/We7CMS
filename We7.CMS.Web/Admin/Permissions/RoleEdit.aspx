<%@ Page Language="C#" MasterPageFile="~/admin/theme/classic/Content.Master" AutoEventWireup="true" Codebehind="RoleEdit.aspx.cs" Inherits="We7.CMS.Web.Admin.RoleEdit" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WEC" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">

    <link rel="stylesheet" type="text/css" href="<%=ThemePath%>/css/article.css" media="screen" />

    <script src="<%=AppPath%>/cgi-bin/article.js" type="text/javascript"></script>
    <script src="<%=AppPath%>/cgi-bin/cookie.js" type="text/javascript"></script>
    <script src="<%=AppPath%>/ajax/jquery/jquery.js" type="text/javascript"></script>
    <script src="<%=AppPath%>/cgi-bin/tags.js" type="text/javascript"></script>

  <h2 class="title">
        <asp:Image ID="RoleImage" runat="server" ImageUrl="~/admin/Images/icons_user.gif" />
        <asp:Label ID="RoleLabel" runat="server" Text="½ÇÉ«¹ÜÀí">
        </asp:Label>
        <span class="summary">
            <asp:Label ID="RoleSummaryLabel" runat="server" Text="">
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
        <div class="clear">
        </div>
        <div id="rightWrapper">
            <div id="container">
                <asp:PlaceHolder runat="server" ID="ContentHolder"></asp:PlaceHolder>
            </div>
        </div>
    </div>
</asp:Content>
