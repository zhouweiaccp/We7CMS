<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SimpleCondition.ascx.cs" Inherits="CModel.Container.system.SimpleCondition" %>
<asp:Table ID="tblQuery" runat="server" CellPadding="0" CellSpacing="1" CssClass="table_form"
    Caption="信息查询">
    <asp:TableRow ID="trQuery" runat="server">
        <asp:TableCell>
            <asp:Button ID="bttnQuery" CommandName="query" runat="server" Text="查询" OnClick="OnButtonSubmit"  CssClass="button_style" /></asp:TableCell>
    </asp:TableRow>
</asp:Table>
