<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ThreeCascade.ascx.cs"
    Inherits="We7.Model.UI.Controls.we7.ThreeCascade" %>

<script type="text/javascript" src="/ModelUI/js/SearchAjax.js"></script>

<asp:Label ID="Field1Lable" runat="server"></asp:Label>：
<asp:DropDownList ID="Field1DropDownList" runat="server">
</asp:DropDownList>
<asp:Label ID="Field2Label" runat="server"></asp:Label>：
<asp:DropDownList ID="Field2DropDownList" runat="server">
    <asp:ListItem Value="">请选择</asp:ListItem>
</asp:DropDownList>
<asp:Label ID="Field3Label" runat="server"></asp:Label>：
<asp:DropDownList ID="Field3DropDownList" runat="server">
    <asp:ListItem Value="">请选择</asp:ListItem>
</asp:DropDownList>
