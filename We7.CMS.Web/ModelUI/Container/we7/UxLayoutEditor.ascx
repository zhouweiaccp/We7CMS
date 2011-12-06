<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UxLayoutEditor.ascx.cs"
    Inherits="We7.Model.UI.Container.we7.UxLayoutEditor" %>
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
                <tr>
                    <td style="padding:0; margin:0; text-align:left">
                        <asp:PlaceHolder ID="UxLayout" runat="server"></asp:PlaceHolder>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="bttnSave" runat="server" CommandName="add" Text="添加" OnClick="bttnSave_Click"
                            CssClass="Btn" />
                        <asp:Button ID="bttnEdit" runat="server" CommandName="edit" Text="保存修改" OnClick="bttnEdit_Click"
                            CssClass="Btn" />
                        <input type="button" id="modelreset" value="清空" class="Btn" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>

<script type="text/javascript">
    //<![CDATA[
    $(function(){
        $("#modelreset").click(function(){
            window.location='<%=NewUrl %>';
        });
    });
    //]]>
</script>

