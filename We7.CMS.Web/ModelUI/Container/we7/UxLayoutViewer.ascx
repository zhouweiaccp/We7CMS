<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UxLayoutViewer.ascx.cs"
    Inherits="We7.Model.UI.Container.we7.UxLayoutViewer" %>
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
                    <td>
                        <asp:PlaceHolder ID="UxLayout" runat="server"></asp:PlaceHolder>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Button ID="bttnEdit" runat="server" CommandName="edit" Text="编辑" OnClick="bttnEdit_Click"
                            CssClass="Btn" />
                        <asp:Button ID="bttnSave" runat="server" CommandName="add" Text="新建" OnClick="bttnNew_Click"
                            CssClass="Btn" />
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

