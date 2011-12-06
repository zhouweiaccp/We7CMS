<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Navigation.ascx.cs"
    Inherits="We7.Model.UI.Container.we7.Navigation" %>
<div class="toolbar">
    <li class="smallButton4">
        <asp:HyperLink ID="hlPublish" NavigateUrl="~/Admin/AddIns/ModelEditor.aspx" runat="server">发布信息</asp:HyperLink>
    </li>
    <li class="smallButton4">
        <asp:HyperLink ID="HyperLink1" NavigateUrl="~/Admin/AddIns/ModelList.aspx" runat="server">管理信息</asp:HyperLink>
    </li>
</div>
