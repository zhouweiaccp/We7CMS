<%@ Page Language="C#" MasterPageFile="~/admin/theme/classic/ContentNoMenu.Master" AutoEventWireup="true" CodeBehind="TemplateGroupInfo.aspx.cs" Inherits="We7.CMS.Web.Admin.TemplateGroupInfo" Title="模板组基本信息" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
<script type="text/javascript">
function closeMe()
{
    window.opener.location.href = window.opener.location.href; 
    if (window.opener.progressWindow) 
    { 
        window.opener.progressWindow.close(); 
    } 
    window.close(); 
}
</script>
<style type="text/css">
  body
  {
  	 overflow:hidden;
  }
</style>
<h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icon_settings.png" />
        <asp:Label ID="NameLabel" runat="server" Text="修改模板组基本信息">
        </asp:Label>
</h2>
<asp:PlaceHolder runat="server" ID="ContentHolder"></asp:PlaceHolder>   
</asp:Content>
