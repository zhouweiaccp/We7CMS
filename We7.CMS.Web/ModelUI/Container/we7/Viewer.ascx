<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Viewer.ascx.cs" Inherits="We7.Model.UI.Container.we7.Viewer" %>
<div id="mycontent">
    <div class="Tab menuTab">
        <ul class="Tabs">
            <asp:Label runat="server" ID="MenuTabLabel"></asp:Label>
        </ul>
    </div>
    <div class="clear">
    </div>
    <div id="rightWrapper">
        <div id="container">
            <div id="conbox">
                <dl>
                    <dt>»<asp:Label runat="server" ID="ModelLabel"></asp:Label>基本信息<br>
                    </dt>
                </dl>
            </div>
            <table cellpadding="0" cellspacing="1" class="table_form">
                <asp:Repeater ID="rpEditor" runat="server" OnItemDataBound="rpEditor_ItemDataBound">
                </asp:Repeater>
                <tr>
                    <td colspan="2">
                        
                        <asp:Button ID="bttnEdit" runat="server" Text="编辑" OnClick="bttnEdit_Click"
                            CssClass="Btn" />
                        <asp:Button ID="bttnNew" runat="server" Text="新建" OnClick="bttnNew_Click"
                            CssClass="Btn" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
<script type="text/javascript">
</script>
