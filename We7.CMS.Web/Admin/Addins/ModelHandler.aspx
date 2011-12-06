<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModelHandler.aspx.cs" MasterPageFile="~/Admin/theme/classic/content.Master"
    Inherits="We7.CMS.Web.Admin.Addins.ModelHandler" %>

<%@ Register Src="~/ModelUI/Panel/system/MultiPanel.ascx" TagName="MultiPanel" TagPrefix="uc1" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <link rel="stylesheet" type="text/css" href="../theme/classic/css/article.css" media="screen" />
    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="../Images/icons_article.gif" />
        <asp:Label ID="NameLabel" runat="server" Text="信息管理">
        </asp:Label>
        <span class="summary"><span id="navChannelSpan"></span>
            <asp:Label ID="SummaryLabel" runat="server" Text=" ">
            </asp:Label>
        </span>
    </h2>
    <div id="position">
        <asp:Literal ID="PagePathLiteral" runat="server">控制台 > 内容管理 > 文章信息 > 全部文章 > 文章信息列表 </asp:Literal>
    </div>
    <div id="mycontent">
        <uc1:MultiPanel ID="ucMulti" runat="server" ModelName="Article" PanelName="multi" />
    </div>
</asp:Content>
