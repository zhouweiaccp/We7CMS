<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/theme/classic/content.Master"
    CodeBehind="ModelTable.aspx.cs" Inherits="We7.CMS.Web.Admin.ContentModel.ModelTable" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <h1>
        生成模型表</h1>
    <asp:TextBox ID="txtModelName" runat="server"></asp:TextBox>
    <asp:Button ID="bttnGenerate" Text="生成" runat="server" OnClick="bttnGenerate_Click" />
</asp:Content>
