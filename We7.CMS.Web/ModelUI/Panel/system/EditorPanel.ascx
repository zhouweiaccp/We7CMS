<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditorPanel.ascx.cs"
    Inherits="We7.Model.UI.Panel.system.EditorPanel" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<%--<%@ Register Src="../../Container/system/CascadeEditor.ascx" TagName="CascadeEditor"
    TagPrefix="uc1" %>
<%@ Register Src="../../Container/system/SimpleCondition.ascx" TagName="SimpleCondition"
    TagPrefix="uc2" %>--%>
<asp:Panel ID="plEditor" runat="server">
    <asp:PlaceHolder ID="phNavigation" runat="server"></asp:PlaceHolder>
    <WEC:MessagePanelCtx ID="msg" runat="server">
    </WEC:MessagePanelCtx>
    <asp:PlaceHolder ID="phEditor" runat="server"></asp:PlaceHolder>
    <asp:PlaceHolder ID="phCondition" runat="server" Visible="false"></asp:PlaceHolder>
</asp:Panel>
