<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/theme/classic/content.Master"
    CodeBehind="ModelControl.aspx.cs" Inherits="We7.CMS.Web.Admin.manage.ContentsSchema.ModelControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <h1>
        生成模型控件</h1>
    <asp:TextBox ID="txtModelName" runat="server"></asp:TextBox>
    <asp:Button ID="bttnGenerate" Text="生成" runat="server" OnClick="bttnGenerate_Click" />
</asp:Content>
