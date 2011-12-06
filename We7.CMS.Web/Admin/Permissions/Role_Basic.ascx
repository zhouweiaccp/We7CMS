<%@ Control Language="C#" AutoEventWireup="true" Codebehind="Role_Basic.ascx.cs"
    Inherits="We7.CMS.Web.Admin.Role_Basic" %>
<%@ Register TagPrefix="WEC" Namespace="We7.CMS.Controls" Assembly="We7.CMS.UI" %>
<div>
    <WEC:MessagePanel ID="Messages" runat="server">
    </WEC:MessagePanel>
</div>
<div id="conbox">
    <dl>
        <dt>»角色的基本信息<br />
            <img src="/admin/images/bulb.gif" align="absmiddle" alt="" />
            <label class="block_info">
                此处对角色的基本信息进行修改与编辑！</label>
        </dt>
        <dd>
            <table id="personalForm" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="formTitle">
                        角色名称：
                    </td>
                    <td class="formValue">
                        <input class="txt" id="NameTextBox" onchange="contentEdited(true);" maxlength="128"
                            runat="server" style="width: 353px" required="required" />
                    </td>
                </tr>
                <tr>
                    <td class="formTitle">
                        角色描述：</td>
                    <td class="formValue">
                        <textarea id="DescriptionTextBox" runat="server" rows="6" onchange="contentEdited(true);"
                            style="width: 353px" cols="40"></textarea>
                    </td>
                </tr>
                <tr>
                    <td class="formTitle">
                        角色类型：
                    </td>
                    <td class="formValue">
                        <asp:DropDownList ID="TypeDropDownList1" runat="server" onchange="contentEdited(true);">
                            <asp:ListItem Value="0" Selected="True">管理员角色</asp:ListItem>
                            <asp:ListItem Value="1">普通用户角色</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="TypeDropDownList2" runat="server" onchange="contentEdited(true);"
                            Visible="false" Enabled="false">
                            <asp:ListItem Value="2" Selected="True">站群角色</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="formTitle">
                        创建时间：
                    </td>
                    <td class="formValue">
                            <asp:Literal ID="CreatedLabel" runat="server" Text=""></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="formTitle">
                        唯一标识:
                    </td>
                    <td class="formValue" style="width: 385px;">
                            <asp:Literal ID="IDLabel" runat="server" Text=""></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td style="height: 39px">
                    </td>
                    <td style="padding-top: 15px; padding-left: 0px; height: 39px">
                      <input  class="button" id="SaveButton"  type="button" value="保存当前信息"/>
                    </td>
                </tr>
            </table>
        </dd>
    </dl>
</div>
<div style="display: none">
    <asp:TextBox ID="RoleIDTextBox" runat="server" Text=""></asp:TextBox>
     <input  class="button" id="SaveButton" runat="server" type="submit" value="保存当前信息" onserverclick="SaveButton_Click" />
</div>
<script type="text/javascript" src="/scripts/we7/we7.loader.js">
$(document).ready(function(){
we7('.tipit').tip();
		we7('span[rel=xml-hint]').help();
		we7("#personalForm").attachValidator({
			inputEvent: 'blur',
            ajaxOnSoft:true,
            errorInputEvent:null
		});
});
</script>
<script type="text/javascript">
    $(document).ready(function () {
        $("#SaveButton").bind("click", function () {
            var div = $("#personalForm");
            var enable = we7(div).validate();
            if (enable) {
                $("#<%=SaveButton.ClientID %>").click();
            }
        });
    });
</script>