<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ArticlesMy.aspx.cs" Inherits="We7.CMS.Web.Admin.ArticlesMy" %>
<%@ Register Src="../controls/ArticleListControl.ascx" TagName="ArticleListControl"
    TagPrefix="uc1" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_article.gif"  />
        <asp:Label ID="Label1" runat="server" Text="нр╣дндуб">
        </asp:Label>
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server" Text="">
            </asp:Label>
         </span>
    </h2>

    <uc1:ArticleListControl ID="ArticleListControl" runat="server" IsMyArticle="true"  ContentType="0" />
    <br />
    <div style="display: none">
    <input id="FatherFileName" type="hidden" value="ArticlesMy.aspx">
    </div>
</asp:Content>