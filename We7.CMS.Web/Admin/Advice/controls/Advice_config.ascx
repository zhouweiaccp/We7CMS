<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Advice_Config.ascx.cs"
    Inherits="We7.CMS.Web.Admin.controls.Advice_Config" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WEC" %>
  <%@ Register TagPrefix="WEC" Namespace="We7.CMS.Controls" Assembly="We7.CMS.UI" %>

<script type="text/javascript">
    function onCancelHyperLinkClick() {
        window.close();
    }
    function ViewAdviceType() {
        var obj0 = document.getElementById('<%=RadlAdviceType.ClientID %>_0');
        var obj1 = document.getElementById('<%=RadlAdviceType.ClientID %>_1');
        var obj2 = document.getElementById('<%=RadlAdviceType.ClientID %>_2');

        var tab1 = document.getElementById('<%=AdviceContent_1.ClientID %>');
        var tab2 = document.getElementById('<%=AdviceContent_2.ClientID %>');
        var tab3 = document.getElementById('<%=AdviceContent_3.ClientID %>');

        if (obj0.checked) {
            tab1.style.display = '';
            tab3.style.display = 'none';
            tab2.style.display = 'none';
        }
        if (obj1.checked) {
            tab1.style.display = 'none';
            tab2.style.display = '';
            tab3.style.display = 'none';
        }
        if (obj2.checked) {
            tab1.style.display = 'none';
            tab2.style.display = 'none';
            tab3.style.display = '';
        }
    }
    function onCheckView(myCheck, myTable) {
    
        if (myTable == 1) {
            Table1.style.display = 'none';
        }
        else {
            if (myTable)
            {
                if( myCheck.checked) {
                    myTable.style.display = '';
                }
                else {
                    myTable.style.display = 'none';
                }
            }
        }
    }
    
    function onCheckViewByID(checkId,tableId)
    {
        var ch=document.getElementById(checkId);
        var tb=document.getElementById(tableId);
        onCheckView(ch,tb);
   }
        
    
</script>
    <WEC:MessagePanel ID="Messages" runat="server">
    </WEC:MessagePanel>
<div id="conbox">
    <dl>
        <dt>»编辑反馈模型办理流程<br>
            <img src="/admin/images/bulb.gif" align="absmiddle" /><label class="block_info">创建一个新的反馈模型配置文件</label>
            <dd>
                <table class="personalForm">
                    <tr>
                        <td class="formTitle">
                            办理模式：
                        </td>
                        <td>
                            <asp:RadioButtonList ID="RadlAdviceType" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="0" onclick="ViewAdviceType();" Selected="True">直接办理</asp:ListItem>
                                <asp:ListItem Value="1" onclick="ViewAdviceType();">转交办理</asp:ListItem>
                                <asp:ListItem Value="2" onclick="ViewAdviceType();">上报办理</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="formTitle">
                            &nbsp;
                        </td>
                        <td>
                            <div runat="server" id="AdviceContent">
                                <table id="AdviceContent_1"  runat="server"   class="selectContent">
                                    <tr>
                                        <td>
                                            <strong>允许受理员直接办理。</strong>
                                        </td>
                                    </tr>
                                </table>
                               <table id="AdviceContent_2" runat="server" class="selectContent">
                                    <tr>
                                        <td>
                                            <strong>根据反馈内容由受理人转交其他部门相关人员进行办理。</strong>
                                        </td>
                                    </tr>
<%--                                    <tr>
                                        <td>
                                            <asp:RadioButtonList ID="ToWhichDepartmentRadioButtonList" runat="server">
                                                <asp:ListItem Value="0" onclick="ViewAdviceType();" Selected="True">下级部门</asp:ListItem>
                                                <asp:ListItem Value="1" onclick="ViewAdviceType();">同级部门</asp:ListItem>
                                                <asp:ListItem Value="2" onclick="ViewAdviceType();">所有部门</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>--%>
                                </table>
                                <table id="AdviceContent_3" runat="server" border="0" cellpadding="2" cellspacing="3"
                                     class="selectContent">
                                    <tr  valign="top">
                                        <td colspan="2">
                                            <strong>上报办理审核流程</strong>
                                        </td>
                                    </tr>
                                    <tr class="tdbg" valign="top">
                                        <td  style="text-align:right;vertical-align:bottom">
                                            审核级数：
                                        </td>
                                        <td style="text-align:left;vertical-align:bottom">
                                            <asp:DropDownList ID="FlowSeriesDropDownList" runat="server">
                                                <asp:ListItem Value="1">1 级</asp:ListItem>
                                                <asp:ListItem Value="2">2 级</asp:ListItem>
                                                <asp:ListItem Value="3">3 级</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr  valign="top" style="display:none">
                                        <td style="text-align:right;vertical-align:middle">
                                            是否在部门内审核
                                        </td>
                                        <td style="text-align:left;vertical-align:baseline;padding-bottom:-4px">
                                        <asp:CheckBox ID="FlowInnerDepartCheckBox" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="formTitle" >
                            <asp:CheckBox ID="EmailCheckBox" runat="server" Text="邮件参与" onclick="onCheckView(this,'EmailTable');" />
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <table id="EmailTable" border="0" cellpadding="2" cellspacing="1"    class="selectContent"  runat="server">
                                <tr class="tdbg" valign="top">
                                    <td colspan="3" class="style2">
                                        <asp:CheckBoxList ID="EmailCheckBoxList" runat="server">
                                            <asp:ListItem Value="01">新反馈邮件通知受理人</asp:ListItem>
                                            <asp:ListItem Value="02">以邮件形式直接转交办理人</asp:ListItem>
                                            <asp:ListItem Value="03">邮件通知办理人，带链接进入后台办理</asp:ListItem>
                                            <%--<asp:ListItem Value="04">催办时发送催办邮件</asp:ListItem>--%>                                     
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:RadioButtonList ID="UseSystemMailRadioButtonList" runat="server">
                                            <asp:ListItem Value="0" onclick="onCheckView(this,1);">使用网站默认邮件地址</asp:ListItem>
                                            <asp:ListItem Value="1" onclick="onCheckView(this,Table1);">专用邮箱地址</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style4">
                                    </td>
                                    <td class="style3">
                                        <table id="Table1" style="display: none; background-color: #F0F0F0">
                                            <tr>
                                                <td class="style4">
                                                </td>
                                                <td class="style3">
                                                    邮件发送服务器(SMTP)：
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="SMTPServerTextBox" runat="server" Width="246px"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td class="style4">
                                                </td>
                                                <td class="style3">
                                                    邮件接收服务器(POP)：
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="SysPopServerTextBox" runat="server" Width="246px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style4">
                                                </td>
                                                <td>
                                                    发送邮件地址：
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="EmailAddressTextBox" runat="server" Width="246px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style4">
                                                </td>
                                                <td>
                                                    用户名：
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="MailUserTextBox" runat="server" Width="246px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style4">
                                                </td>
                                                <td>
                                                    密码：
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="MailPasswordTextBox" runat="server" Width="246px" TextMode="Password"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                          
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="formTitle"  >
                            <asp:CheckBox ID="NoteCheckBox" runat="server" Text="短信通知" onclick="onCheckView(this,NoteTable);" />
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <table id="NoteTable" class="selectContent"  runat="server" >
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="NoteCheckBoxList" runat="server" Width="394px">
                                            <asp:ListItem Value="通知受理人" onclick="ViewAdviceType();">通知受理人</asp:ListItem>
                                            <asp:ListItem Value="通知办理人" onclick="ViewAdviceType();">通知办理人</asp:ListItem>
                                            <asp:ListItem Value="催办时通知办理人" onclick="ViewAdviceType();" >催办时通知办理人</asp:ListItem>
                                            <asp:ListItem Value="通知审核人" onclick="ViewAdviceType();">通知审核人</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="formTitle">
                            超时提醒催办：
                        </td>
                        <td class="formValue">
                            <asp:TextBox ID="RemindDaysTxtBox" runat="server"  Width="29px" Height="20px">3</asp:TextBox>个工作日后提醒("0"为不提醒)
                            <asp:RangeValidator ID="RangeValidator1" runat="server" 
                                ControlToValidate="RemindDaysTxtBox" ErrorMessage="请输入0-100之间的数字！" 
                                MaximumValue="100" MinimumValue="0" Type="Integer">*</asp:RangeValidator>
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                                ShowMessageBox="True" ShowSummary="False" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button CssClass="Btn" ID="SaveButton" runat="server" Text="保存当前信息" OnClick="SaveButton_Click" />
                        </td>
                    </tr>
                </table>
</div>
