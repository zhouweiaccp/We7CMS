<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModelViewer.aspx.cs" MasterPageFile="~/Admin/theme/classic/content.Master"
    Inherits="We7.CMS.Web.Admin.Addins.ModelViewer" %>

<%@ Register Src="~/ModelUI/Panel/system/EditorPanel.ascx" TagName="EditorPanel"
    TagPrefix="uc1" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <link rel="stylesheet" type="text/css" href="../theme/classic/css/article.css" media="screen" />

    <script type="text/javascript" src="/Admin/Ajax/My97DatePicker/WdatePicker.js"></script>

    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="../Images/icons_article.gif" />
        <asp:Label ID="NameLabel" runat="server">
        </asp:Label>
        <span class="summary"><span id="navChannelSpan"></span>
            <asp:Label ID="SummaryLabel" runat="server" Text=" ">
            </asp:Label>
        </span>
    </h2>
    <div id="position">
        <asp:Literal ID="PagePathLiteral" runat="server"></asp:Literal>
    </div>
    <div id="mycontent">
        <uc1:EditorPanel ID="ucEditor" runat="server" PanelName="edit" />
    </div>
</asp:Content>
