<%@ Page Language="C#" MasterPageFile="~/admin/theme/classic/Contentnomenu.Master"
    AutoEventWireup="true" CodeBehind="ScanProcessHistory.aspx.cs" Inherits="We7.CMS.Web.Admin.ScanProcessHistory"
    Title="流转历史" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<%@ Register Src="controls/ProcessHistoryList.ascx" TagName="ProcessHistoryList"
    TagPrefix="uc1" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
<style>
    body
    {
        padding: 10px;
        margin: 10px;
        width: 90%;
    }
    </style>
    <div id="breadcrumb">
        <h2>
            <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/logo_info.gif" />
            <asp:Label ID="TitleLabel" runat="server" Text="浏览意见">
            </asp:Label>
            <span class="summary">
                <asp:Label ID="SummaryLabel" runat="server" Text="流转历程表">
                </asp:Label>
            </span>
        </h2>
        <div class="toolbar" style="float: right; text-align: right">
            <li class="smallButton4"><a href="javascript:window.close()">关闭窗口</a> </li>
            <li class="smallButton4">
                <asp:HyperLink ID="SubmitHyperLink" NavigateUrl="javascript:onPreviewClick();" runat="server">
                打印预览
                </asp:HyperLink>
            </li>
        </div>
        <hr />
        <div>
            <uc1:ProcessHistoryList ID="ProcessHistoryList1" runat="server" />
        </div>
    </div>
</asp:Content>
