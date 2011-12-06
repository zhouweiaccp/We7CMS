<%@ Page Language="C#" MasterPageFile="/admin/theme/classic/ContentNoMenu.Master" AutoEventWireup="true" CodeBehind="ObjectBindPermission.aspx.cs" Inherits="We7.CMS.Web.Admin.ObjectBindPermission" Title="无标题页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
 <link rel="stylesheet" type="text/css" href="<%=ThemePath%>/css/article.css" media="all" />
<h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_user.png" />
        <asp:Label ID="NameLabel" runat="server" Text="">
        </asp:Label>
</h2>
<div id="container">
    <asp:PlaceHolder runat="server" ID="ContentHolder"></asp:PlaceHolder>
</div>
</asp:Content>
