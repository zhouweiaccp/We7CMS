<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CacheRefresh.aspx.cs" Inherits="We7.CMS.Web.Admin.manage.CacheRefresh" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:content id="We7Content" contentplaceholderid="MyContentPlaceHolder" runat="server">
    <h2 class="title">
        <asp:Image ID="LogImage" runat="server" ImageUrl="~/admin/Images/icon_settings.png" />
        <asp:Label ID="TitleLabel2" runat="server" Text="静态页管理">
        </asp:Label>
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server" Text="生成网站静态页,以提高网站的访问速度">
            </asp:Label>
        </span>
    </h2>
     <WEC:MessagePanel runat="Server" ID="Messages">
    </WEC:MessagePanel>
    <hr style="width:100%" />
    <div>
        <asp:CheckBoxList id="chkItem" runat="server" RepeatColumns="5" RepeatDirection="Horizontal" ></asp:CheckBoxList>
    </div>
    <div>
         <asp:Button ID="bttnGenerate" runat="server" Text="刷新所先项缓存"  onclick="bttnGenerate_Click" />
    </div>
</asp:content>
