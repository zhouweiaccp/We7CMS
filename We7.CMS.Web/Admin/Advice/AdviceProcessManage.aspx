<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/admin/theme/classic/content.Master"
    CodeBehind="AdviceProcessManage.aspx.cs" Inherits="We7.CMS.Web.Admin.AdviceProcessManage" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WEC" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">

    <link rel="stylesheet" type="text/css" href="<%=ThemePath%>/css/article.css" media="screen" />

    <script language="JavaScript" type="text/JavaScript">

        function MM_jumpMenu(targ, selObj, restore) {
            eval(targ + ".location='" + selObj.options[selObj.selectedIndex].value + "'");
            if (restore) selObj.selectedIndex = 0;
        }

    </script>

    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icon_settings.png" />
        <asp:Label ID="NameLabel" runat="server" Text="反馈监控管理">
        </asp:Label>
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server" Text="反馈信息监控管理">
            </asp:Label>
        </span>
        <div id="adviceTypeList" runat="server" class="channelSelect">
            <asp:DropDownList ID="AdviceTypeDropDownList" runat="server" EnableViewState="false"
                onChange="MM_jumpMenu('top',this,0)" Visible="false">
            </asp:DropDownList>
        </div>
    </h2>
    <div id="position">
        <asp:Literal ID="PagePathLiteral" runat="server"></asp:Literal>
    </div>
    <div id="mycontent">
        <div class="Tab menuTab">
            <ul class="Tabs">
                <asp:Label runat="server" ID="MenuTabLabel"></asp:Label>
            </ul>
        </div>
        <div id="rightWrapper">
            <div id="container">
                <asp:PlaceHolder runat="server" ID="ContentHolder"></asp:PlaceHolder>
            </div>
        </div>
    </div>
</asp:Content>
