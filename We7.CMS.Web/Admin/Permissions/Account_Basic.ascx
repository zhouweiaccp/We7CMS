<%@ Control Language="C#" AutoEventWireup="true" Codebehind="Account_Basic.ascx.cs"
    Inherits="We7.CMS.Web.Admin.Permissions.Account_Basic" %>
<%@ Register TagPrefix="WEC" Namespace="We7.CMS.Controls" Assembly="We7.CMS.UI" %>
<div>
    <WEC:MessagePanel ID="Messages" runat="server">
    </WEC:MessagePanel>
</div>
<script type="text/javascript">
var subs_array = new Array("subPassword");// Put the id's of your hidden divs in this array
function displaySubs(the_sub){
	if (document.getElementById(the_sub).style.display==""){
	document.getElementById(the_sub).style.display = "none";
	return
  }
  for (i=0;i<subs_array.length;i++){
	var my_sub = document.getElementById(subs_array[i]);
	my_sub.style.display = "none";
	}
  document.getElementById(the_sub).style.display = "";
}

function selectUserID() {
    var url = "/admin/manage/DepartmentList.aspx";
    weShowModelDialog(url, onAdviceListCallback);
}

function onAdviceListCallback(v, t) {
//    debugger;
    var departmentID = document.getElementById("<%=DepartmentIDTextBox.ClientID %>");
    departmentID.value = v;
    var fullPathLabelID = document.getElementById("<%=FullPathLabel.ClientID %>");
    fullPathLabelID.innerHTML = t;    
}

function DeleteButtun_onclick() {
//  return we7.confirm("删除账户", {
//        ok: function () {
//            return true;
//        },
//        cancle: function () {
//            return false;
//        },
//        position: "center",
//        overlay: true
    //    });
    return confirm("您确定要删除此账户吗？此操作不能回复，并将删除此账户相关的其他权限数据！");
}

</script>
    

<script type="text/javascript" src="/scripts/we7/we7.loader.js">
	$(document).ready(function () {
		we7('[title]').tip();
		we7('span[rel=xml-hint]').help();
		we7("#personalForm").attachValidator({
			inputEvent: 'blur',
            ajaxOnSoft:true,
            errorInputEvent:null
		});
        we7("#<%=NameTextBox.ClientID%>").validateRules({
            length:[5,20]
        }).validateAjax({
            url:function(){
                return "validForUserPermiss.ashx?type=0&kw="+$(this).val();
                },
            callback:function(json,textStatus){
                if(json=="")
                    return true;                
                else
                    return json;
            },
            error:function(data,textStatus){
                return "服务器忙";
            }
            });
        we7("#<%=EmailTextBox.ClientID %>").validateRules({
			type:"email"
		}).validateAjax({
            url:function(){
                return "validForUserPermiss.ashx?type=1&kw="+$(this).val();
                },
            callback:function(json,textStatus){
                json=$(this).val()==$("#EmailTextBox").val()?"":json;
                if(json=="")
                    return true;                
                else
                    return json;
            },
            error:function(data,textStatus){
                return "服务器忙";
            }
            });
        we7("#<%=PassWordText.ClientID %>").validateRules({
			length:[6,20]
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
<div id="conbox">
    <dl>
        <dt>»用户的基本信息<br />
            <img src="/admin/images/bulb.gif" align="absmiddle" alt="" />
            <label class="block_info">
                此处对用户的基本信息进行修改与编辑！</label>
        </dt>
        <dd>
            <table id="personalForm" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="formTitle">
                        部门：</td>
                    <td class="formValue">
                        <asp:Label ID="FullPathLabel" runat="server" Text="">
                        </asp:Label>
                         <a href="javascript:selectUserID();" >[移动到...]</a>
                    </td>
                </tr>
                <tr>
                    <td class="formTitle">
                        登录名称：</td>
                    <td class="formValue">
                        <asp:TextBox ID="NameTextBox" runat="server" Columns="50" MaxLength="128" required="required"></asp:TextBox>
                        
                        </td>
                </tr>
                <tr>
                    <td class="formTitle">
                        姓名：</td>
                    <td class="formValue">
                        <asp:TextBox ID="LastNameTextBox" runat="server" Columns="10" MaxLength="128" 
                            Width="306px"></asp:TextBox>&nbsp;</td>
                </tr>
                <tr>
                    <td class="formTitle">
                        密码：</td>
                    <td class="formValue" style="background-color:#eee" valign="top">
                    <asp:TextBox ID="PassWordText" Visible="false" runat="server" Columns="50" MaxLength="128" TextMode="Password" required="required"></asp:TextBox>&nbsp;&nbsp;&nbsp;<br />
                        <span runat="server" id="ResetPasswordSpan">                           
                           <asp:CheckBox ID="ResetPasswordCheckBox" name="ResetPasswordCheckBox" runat="server" Visible="False" onClick="displaySubs('subPassword')" onFocus="if(this.blur)this.blur()";  />
                            <label for="ResetPasswordCheckBox">重设密码</label>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td class="formTitle">
                    </td>
                    <td class="formValue" style="background-color:#eee">
                        <div id="subPassword" style="display: none">
                            <asp:TextBox ID="PasswordTextBox" runat="server" Columns="50" MaxLength="128" TextMode="Password"></asp:TextBox>
                            <br />
                            <asp:CheckBox ID="IsHashedCheckBox" runat="server"  Text="密码加密存储"/>
                            
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="formTitle">
                        电子邮件：</td>
                    <td class="formValue">
                        <asp:TextBox ID="EmailTextBox" runat="server" Columns="50" MaxLength="128" required="required" type="email"></asp:TextBox>
                        <input type="hidden" id="EmailTextBox" value='' />
                        <script>$("#EmailTextBox").val($("#<%=EmailTextBox.ClientID%>").val())</script>
                        </td>
                </tr>
                <tr>
                    <td class="formTitle">
                        描述：</td>
                    <td class="formValue">
                        <asp:TextBox ID="DescriptionTextBox" runat="server" TextMode="MultiLine" Rows="4" Columns="40"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="formTitle">
                        序号：</td>
                    <td class="formValue">
                        <asp:TextBox ID="IndexTextBox" runat="server" Text="0" Columns="5" MaxLength="4" min="0" title="排序序号"></asp:TextBox>
                        </td>
                </tr>
                <tr>
                    <td class="formTitle">
                        类型：</td>
                    <td class="formValue">
                        <asp:DropDownList ID="UserTypeDropDownList" runat="server">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="formTitle">
                        用户模型：</td>
                    <td class="formValue">
                        <asp:DropDownList ID="UserModelDropDownList" runat="server">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ModelStateDropDownList" runat="server">
                              <asp:ListItem Value="0">----</asp:ListItem>
                            <asp:ListItem Value="1">申请加入，等待审核</asp:ListItem>
                            <asp:ListItem Value="2">开通</asp:ListItem>
                        </asp:DropDownList>
                        <a href="/admin/ContentModel/EditModel.aspx?action=add&type=Account" target="_blank">新建用户模型</a> 
                      </td>
                </tr>
                <tr>
                    <td class="formTitle">
                        状态：</td>
                    <td class="formValue">
                        <asp:DropDownList ID="UserStateDropDownList" runat="server">
                            <asp:ListItem Text="禁用&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" Value="0" />
                            <asp:ListItem Text="启用" Value="1" />
                            <asp:ListItem Text="保留" Value="2" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="formTitle">
                        ID:</td>
                    <td class="formValue" id="idvalue">
                        <asp:Literal ID="IDLabel" runat="server" Text=""></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="formTitle">
                        创建时间：</td>
                    <td class="formValue">
                        <asp:Literal ID="CreatedLabel" runat="server" Text=""></asp:Literal>
                    </td>
                </tr>
                  <tr>
                    <td class="formTitle">
                        过期时间：</td>
                    <td class="formValue">
                        <asp:TextBox ID="OverdueTextBox" runat="server" Columns="50" MaxLength="128" title="过期用户将不能登录"></asp:TextBox>
                    </td>
                </tr>
               <tr>
                    <td></td>
                    <td><asp:CheckBox ID="SendMailCheckBox" name="SendMailCheckBox" runat="server" Text="同时给用户发送通知邮件" /></td>
                </tr>
                 <tr>
                <td></td>
                <td>  <asp:TextBox ID="MailBodyTextBox" runat="server" TextMode="MultiLine" Rows="4" Columns="40"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="height: 39px">
                    </td>
                    <td style="padding-top: 15px; padding-left: 0px; height: 39px">
                    <input class="Btn" id="SaveButton" type="button" value="保存当前设置" />
                        <input class="Btn" id="DeleteButtun" runat="server" type="submit" value="删除此账户"  onclick="return DeleteButtun_onclick()"  onserverclick="DeleteButton_Click"/>
                        </td>
                </tr>
 
            </table>
        </dd>
    </dl>
</div>
<div style="display: none">
    <asp:TextBox ID="RoleIDTextBox" runat="server" Text=""></asp:TextBox>
    <asp:TextBox ID="ParentTextBox" runat="server" Text="0"></asp:TextBox>
    <asp:TextBox ID="AccountIDTextBox" runat="server" Text="0"></asp:TextBox>
    <asp:TextBox ID="DepartmentIDTextBox" runat="server" Text=""></asp:TextBox>
      <input class="Btn" id="SaveButton" runat="server" type="submit" value="保存当前设置" onserverclick="SaveButton_Click" />
</div>