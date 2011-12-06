<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoryDoubleCascade.ascx.cs"
    Inherits="We7.Model.UI.Controls.system.CategoryDoubleCascade" %>

<script type="text/javascript" src="/ModelUI/js/CategoryDoubleCascade.js?20110113001"></script>

<asp:DropDownList ID="Field1DropDownList" runat="server">
</asp:DropDownList>
<asp:HiddenField ID="Field2Hidden" runat="server" Value="" />
<asp:DropDownList ID="Field2DropDownList" runat="server">
    <asp:ListItem Value="">请选择</asp:ListItem>
</asp:DropDownList>
