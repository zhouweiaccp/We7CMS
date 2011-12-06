<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountEditor.ascx.cs"
    Inherits="We7.Model.UI.Container.we7.SimpleEditor" %>
<div id="conbox">
    <dl>
        <dt>»用户的基本信息<br />
            <img src="/admin/images/bulb.gif" align="absmiddle" alt="" />
            <label class="block_info">
                此处对用户的基本信息进行修改与编辑！</label>
        </dt>
        <dd>
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
        </dd>
</div>
