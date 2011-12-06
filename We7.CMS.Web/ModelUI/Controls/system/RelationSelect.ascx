<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RelationSelect.ascx.cs"
    Inherits="We7.Model.UI.Controls.system.RelationSelect" %>
<asp:DropDownList ID="ddlEnum" runat="server">
</asp:DropDownList>
<div style="display: none">
    <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click">LinkButton</asp:LinkButton><asp:LinkButton
        ID="LinkButton2" runat="server">LinkButton</asp:LinkButton></div>
<a href='javascript:$("#ctAdd").show();void(0);'>新建</a>
<div id="ctAdd" style="margin-left: 100px; padding: 3px; border: solid 1px #e0e0e0;
    position: absolute; background: none repeat scroll 0% 0% rgb(255, 255, 255);
    display: none;">
    <div style="border: solid 1px #f0f0f0;">
        <table>
            <tr>
                <td>
                    名称:
                </td>
                <td>
                    <input id="newName" /><input type="hidden" id="newID" runat="server"/>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <input id="btnAdd" onclick="Add();" style="width: 60px; padding: 0;" class="Btn"
                        type="button" value="添加" />
                    <input id="btnCancel" onclick="$('#ctAdd').hide();" style="width: 60px; padding: 0"
                        class="Btn" type="button" value="取消" />
                    <label id="msg">
                    </label>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        function Add() {
            if ($.trim($("#newName").val()) == "") {
                alert("名称不能为空!");
                return;
            }
            $("#msg").text('数据处理中');
            var newID = '<%=We7Helper.CreateNewID() %>';
            var newName = $("#newName").val();
            $.ajax({
                type: 'POST',
                url: '/ModelUI/Controls/page/RelationSelectEx.aspx',
                data: 'NewID=' + newID + '&NewName=' + newName + '&Model=<%=Control.Params["model"] %>',
                cache: false,
                success: function (msg) {
                    $("#<%=newID.ClientID %>").val(newID);
                    document.getElementById("<%=LinkButton1.ClientID %>").click();
                    //var selectID = '<%=ddlEnum.ClientID %>';
                    //                    var op = new Option(newName, newID);
                    //                    op.selected = 'selected';
                    //                    $("#" + selectID)[0].options.add(op);
                    $('#ctAdd').hide();
                }

            });
        }
    </script>
</div>
