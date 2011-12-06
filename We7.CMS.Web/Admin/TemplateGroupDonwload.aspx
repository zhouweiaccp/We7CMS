<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TemplateGroupDonwload.aspx.cs" Inherits="We7.CMS.Web.Admin.TemplateGroupDonwload" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
        <asp:Panel runat="Server" ID="ContentPanel">
            <h2   class="title">
                <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_look.gif" />
                <span>下载模板组 </span>
                <asp:Label ID="TemplateGroupNameLabel" runat="server" Text="">
                </asp:Label>
                <span></span>
              </h2>
              <p style="margin-left:20px">
              点击 “<asp:HyperLink ID="DonwloadHyperLink" runat="server">下载模板组</asp:HyperLink>”完成下载操作。
              </p>          
        
        </asp:Panel>
</asp:Content>
