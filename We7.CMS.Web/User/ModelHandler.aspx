<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModelHandler.aspx.cs" MasterPageFile="~/User/DefaultMaster/content.Master"
    Inherits="We7.CMS.Web.User.ModelHandler" %>

<%@ Register Src="../ModelUI/Panel/system/MultiPanel.ascx" TagName="MultiPanel" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MyHeadPlaceHolder" runat="server">
    <link rel="stylesheet" type="text/css" href="/ModelUI/skin/default.css" media="screen" />
    <script src="/Admin/Ajax/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Admin/Ajax/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <form id="form1" runat="server">
    <div class="realRight ml10">
        <div class="mybox">
            <uc1:multipanel id="MultiPanel1" runat="server" panelname="multi" />
        </div>
    </div>
    </form>
</asp:Content>
