<%@ Control Language="C#" AutoEventWireup="true" Inherits="We7.Model.UI.Container.we7.SimpleEditor" %>
<table id="personalForm" cellpadding="0" cellspacing="1" class="table_form">
    <asp:Repeater ID="rpEditor" runat="server" OnItemDataBound="rpEditor_ItemDataBound">
    </asp:Repeater>
    <tr>
        <th>
        </th>
        <td>
            <asp:Button ID="bttnEdit" runat="server" CommandName="edit" Text="保存" OnClick="OnButtonSubmit"
                CssClass="button_style" />
        </td>
    </tr>
</table>
