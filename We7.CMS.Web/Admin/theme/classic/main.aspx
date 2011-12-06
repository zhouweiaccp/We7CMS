<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" MasterPageFile="content.Master"
    Inherits="We7.CMS.Web.Admin.main" %>

<%@ Register Src="../../tools/widget/SiteProfile.ascx" TagName="SiteProfile" TagPrefix="uc1" %>
<%@ Register Src="../../tools/widget/wid_myprocess.ascx" TagName="wid_myprocess"
    TagPrefix="uc2" %>
<%@ Register Src="../../tools/widget/wid_myarticle.ascx" TagName="wid_myarticle"
    TagPrefix="uc3" %>
<%@ Register Src="../../tools/widget/wid_Shop.ascx" TagName="wid_Shop" TagPrefix="uc5" %>
<%@ Register Src="../../tools/widget/wid_Product.ascx" TagName="wid_Product" TagPrefix="uc6" %>
<%@ Register Src="../../tools/widget/wid_guide.ascx" TagName="wid_guide" TagPrefix="uc4" %>
<%--<%@ Register src="../../tools/widget/wid_RecentComments.ascx" tagname="wid_RecentComments" tagprefix="uc4" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <!-- Require basic Easy Widgets style -->
    <link rel="stylesheet" type="text/css" media="screen" href="css/jquery.easywidgets.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="css/mywidgets.css" />
    <script src="js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="js/jquery.easywidgets.js" type="text/javascript"></script>
    <script src="js/mywidgets.js" type="text/javascript"></script>
    <h2 class="title">
        <asp:Image ID="RoleImage" runat="server" ImageUrl="~/admin/Images/icons_home.gif" />
        <asp:Label ID="RoleLabel" runat="server" Text="我的工作台">
        </asp:Label>
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server" Text="">
            </asp:Label>
        </span>
        <div id="buttonShow">
            <a onclick="$.fn.ShowEasyWidgets(); return false" href="#" title="显示所有挂件，包括以前关掉的">显示所有挂件</a>
        </div>
    </h2>
    <!-- Begin Easy Widgets plugin HTML markup -->
    <div class="widget-place" id="widget-place-1">        
        <uc2:wid_myprocess ID="wid_myprocess1" runat="server" />       
        <uc1:SiteProfile ID="wid_siteprofile1" runat="server" />
         <uc4:wid_guide ID="wid_guide1" runat="server" />
        <%--                   <uc3:wid_myarticle ID="wid_myarticle1" runat="server" />--%>       
    </div>
    </td>
    <!-- /place-1 -->
    <div class="widget-place" id="widget-place-2">
        <uc5:wid_Shop ID="wid_Shop1" runat="server" />
        <uc6:wid_Product ID="wid_Product1" runat="server" />        
    </div>
    </table>
    <!-- /place-2 -->
    <!-- End Easy Widgets plugin HTML markup -->
</asp:Content>
