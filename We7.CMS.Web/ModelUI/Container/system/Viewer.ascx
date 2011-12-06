<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Viewer.ascx.cs" Inherits="We7.Model.UI.Container.system.Viewer" %>
<table cellpadding="0" cellspacing="1" class="table_form">
    <asp:Repeater ID="rpEditor" runat="server" OnItemDataBound="rpEditor_ItemDataBound">
    </asp:Repeater>
    <tr id="trBttn" runat="server">
        <td>
        </td>
        <td>
            <asp:Button ID="bttnEdit" runat="server" CommandName="edit" Text="编辑" OnClick="bttnEdit_Click"
                CssClass="Btn" />
            <asp:Button ID="bttnSave" runat="server" CommandName="add" Text="新建" OnClick="bttnNew_Click"
                CssClass="Btn" />
        </td>
    </tr>
</table>
