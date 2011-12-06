<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CascadeEditor.ascx.cs"
    Inherits="CModel.Container.system.CascadeEditor" %>
<table cellpadding="0" cellspacing="1" class="table_form">
    <caption>
        内容添加</caption>
    <asp:Repeater ID="rpEditor" runat="server" OnItemDataBound="rpEditor_ItemDataBound">
    </asp:Repeater>
    <tr>
        <th>
        </th>
        <td>
            <asp:Button ID="bttnSave" runat="server" CommandName="add" Text="保存" OnClick="OnButtonSubmit"
                CssClass="button_style" />
            <asp:Button ID="bttnEdit" runat="server" CommandName="edit" Text="编辑" OnClick="OnButtonSubmit"
                CssClass="button_style" />
            <input type="reset" value="取消" class="button_style" />
        </td>
    </tr>
</table>