<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultiPanel.ascx.cs"
    Inherits="We7.Model.UI.Panel.system.MultiPanel" %>
<asp:Panel ID="plNavigation" runat="server">
    <asp:PlaceHolder ID="phNavigation" runat="server"></asp:PlaceHolder>
</asp:Panel>
<asp:Panel ID="plEditor" runat="server" Visible="false">
    <asp:PlaceHolder ID="phEditor" runat="server"></asp:PlaceHolder>
</asp:Panel>
<asp:Panel ID="plList" runat="server">   
    <asp:PlaceHolder ID="phCondition" runat="server"></asp:PlaceHolder>
    <asp:PlaceHolder ID="phList" runat="server"></asp:PlaceHolder>
     <asp:PlaceHolder ID="phCommand" runat="server"></asp:PlaceHolder>
    <asp:PlaceHolder ID="phPager" runat="server"></asp:PlaceHolder>
</asp:Panel>
