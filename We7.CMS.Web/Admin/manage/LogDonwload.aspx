<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogDonwload.aspx.cs" MasterPageFile="~/admin/theme/classic/content.Master"
    Inherits="We7.CMS.Web.Admin.LogDonwload" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WEC" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">

    <script type="text/javascript">
        function copyCode(obj) {
            var rng = document.body.createTextRange();
            rng.moveToElementText(obj);
            rng.scrollIntoView();
            rng.select();
            rng.execCommand("Copy");
            rng.collapse(false);
            alert("高亮度包含的代码已被复制到剪贴板");
        }

        function saveCode(obj) {
            var winname = window.open('', '_blank', 'top=1000');
            winname.document.open('html', 'replace');
            winname.document.writeln(obj.value);
            winname.document.execCommand('saveas', 'true', 'logs.html');
            winname.close();
        }  
    </script>

    <h2 class="title">
        <asp:Image ID="LogImage" runat="server" ImageUrl="~/admin/Images/logo_task.gif" />
        <asp:Label ID="LogNameLabel" runat="server" Text="导出日志">
        </asp:Label>
        <span class="summary">
            <asp:Label ID="RoleSummaryLabel" runat="server" Text="">
            </asp:Label>
        </span>
    </h2>
    <div class="toolbar">
        <asp:HyperLink ID="RefreshHyperLink" NavigateUrl="Logs.aspx" runat="server">
            <asp:Image ID="RefreshImage" runat="server" ImageUrl="~/admin/Images/icon_refresh.gif" />
            刷新</asp:HyperLink>
        <span>|</span>
        <asp:HyperLink ID="ReturnHyperLink" runat="server" NavigateUrl="Logs.aspx">
            <asp:Image ID="ReturnImage" runat="server" ImageUrl="~/admin/Images/icon_prev.gif" />
            返回</asp:HyperLink>
    </div>
    <h3>
        <span>日志详细信息</span>
    </h3>
    <button onclick="saveCode(LogsTextBox)">
        保存为HTML</button>
    <button onclick="copyCode(LogsTextBox)">
        复制到剪贴板</button>
    <p>
        <asp:TextBox ID="LogsTextBox" runat="server" Height="374px" TextMode="MultiLine"
            Width="585px"></asp:TextBox></p>
</asp:Content>
