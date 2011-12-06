<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SimpleEditorPanel.ascx.cs"
    Inherits="We7.Model.UI.Panel.system.SimpleEditorPanel" %>
<asp:Panel ID="plEditor" runat="server">
    <asp:PlaceHolder ID="phNavigation" runat="server"></asp:PlaceHolder>
    <asp:PlaceHolder ID="phEditor" runat="server"></asp:PlaceHolder>
    <asp:PlaceHolder ID="phCondition" runat="server" Visible="false"></asp:PlaceHolder>
</asp:Panel>
