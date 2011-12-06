<%@ Page Language="c#" AutoEventWireup="false" Inherits="We7.CMS.Install.Step3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<script language="javascript" src="js/setup.js"></script>
<script type="text/javascript">
var ok=true;

function hideall(force)
{
    var sel=document.getElementById("DbTypeDropDownList");
    if(sel && '<%=SelectDB %>' !='' && !force)
    {
        sel.value='<%=SelectDB %>';
        DbTypeChange("<%=SelectDB %>");
     }
     else
     {
            hide("tr1");
            hide("tr2");
            hide("tr3");
            hide("tr4");
            hide("tr5");
            hide("tr6");
            document.getElementById("DbTypeDropDownList").selectValue = 0;
      }
}
function DbTypeChange(type)
{
    show("tr6");
    document.getElementById('msg0').innerHTML='';
    switch(type)
    {
        case "SqlServer":
            show("tr1");
            show("tr2");
            show("tr3");
            show("tr4");
            hide("tr5");
            document.getElementById('msg0').innerHTML='<br>*您是否在使用SQL Server 2005 Express？ 请在“服务器名”项使用“主机名称\\SQLEXPRESS”。';
            break;
        case "MySql":
            show("tr1");
            show("tr2");
            show("tr3");
            show("tr4");
            hide("tr5");
            hide("tr6");
            break;
        case "Oracle":
            show("tr1");
            hide("tr2");
            show("tr3");
            show("tr4");
            hide("tr5");
             hide("tr6");
            break;
        case "SQLite":
        case "Access":
            hide("tr1");
            hide("tr2");
            hide("tr3");
            hide("tr4");
            show("tr5");
        break;
        default:
            hideall(true);
            break;
    }        
    
    document.getElementById("dbtype").value=type;
}

function SelectChange()
{
    DbTypeChange(document.getElementById("DbTypeDropDownList").value);
    ok=true;
}

function hide(id)
{
    document.getElementById(id).style.display = "none";
}

function show(id)
{
    document.getElementById(id).style.display = "";
}

function checkid(obj,id)
{
    var v = obj.value;
    
    if(v.length == 0)
    {
        document.getElementById('msg'+id).innerHTML='<span style=\'color:#ff0000\'>此处不能为空！</span>';
        ok=false;
    }
    else
    {
        document.getElementById('msg'+id).innerHTML='';
        ok=true;
    }
}

function isempty(id)
{
    if (document.getElementById(id).value.length==0)
        return true;
    else
        return false;
}

function checknull()
{
    if (document.getElementById("AdminPasswordTextBox").value == "" || document.getElementById("AdminPasswordTextBox").value.length < 6)
    {
        alert("系统管理员密码不能少于6位");
        document.getElementById("AdminPasswordTextBox").focus();
        return false;
    }
    if (document.getElementById("repwd").value == "")
    {
        alert("确认密码不能为空");
        document.getElementById("repwd").focus();
        return false;
    }
    if (document.getElementById("AdminPasswordTextBox").value != document.getElementById("repwd").value)
    {
        alert("系统管理员密码两次输入不同,请重新输入");
        document.getElementById("AdminPasswordTextBox").focus();
        document.getElementById("AdminPasswordTextBox").value = "";
        document.getElementById("repwd").value = "";
        return false;
    }
    if (document.getElementById("DbTypeDropDownList").value == 'Access' || document.getElementById("DbTypeDropDownList").value == 'SQLite')
    {
        if (!isempty('DbFileNameTextBox')) 
            document.Form1.submit();
        else
        {
            alert('DbFileNameTextBox 不能为空！');
            return false;
         }
    }
    else if(document.getElementById("DbTypeDropDownList").value != 'Oracle' )
    {
        if (!isempty('ServerTextBox') && !isempty('DatabaseTextBox') && !isempty('UserTextBox'))
            document.Form1.submit();
        else
        {
            alert('datasource 不能为空！')
            return false;
        }
    }
    document.Form1.submit();
}

</script>

<%=header%>
<body onload="hideall()"   class="pubbox_login">
    <form id="Form1" method="post" runat="server" onsubmit="return checknull();">
  <div >
    <table width="700" border="0" align="center" cellpadding="0" cellspacing="12" bgcolor="#999" class="login">
        <tr>
            <td bgcolor="#ffffff">
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="2" >
                            <table width="100%" border="0" cellspacing="0" cellpadding="8">
                                <tr>
                                    <td align="left">
                                      <h1>2、初始化数据环境</h1>
                                      </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" width="180">
                                <%=logo%>
                            </td>
                            <td valign="top" width="520">
                                <asp:Literal ID="msg" runat="server" Text="您当前网站的Web.config文件设置不正确, 请您确保其文件内容正确<BR><FONT color=#996600>详见《安装说明》</FONT>"
                                    Visible="False"></asp:Literal>
                                <table cellspacing="0" cellpadding="8" width="100%" border="0">
                                    <tr>
                                        <td>
                                            <p>
                                            </p>
                                            <table cellspacing="0" cellpadding="8" width="100%" border="0">
                                                <tr>
                                                    <td width="30%">网站的名称:</td>
                                                    <td style="width: 352px">
                                                    <asp:TextBox ID="WebsiteNameTextBox" runat="server"  Width="150px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>系统管理员名称:</td>
                                                    <td style="width: 352px">
                                                       <asp:TextBox ID="AdminNameTextBox" runat="server"  Width="150px"></asp:TextBox>
                                                     </td>
                                                </tr>
                                                <tr>
                                                    <td>系统管理员密码:<br />
                                                        (不得少于6位)</td>
                                                    <td style="width: 352px">
                                                    <asp:TextBox ID="AdminPasswordTextBox" runat="server" MaxLength="32" Size="20" Width="150px" TextMode="Password"></asp:TextBox>
                                                    *<br />
                                                        密码强度:<span id="showmsg"></span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>确认密码:</td>
                                                    <td style="width: 352px">
                                                        <input name="repwd" type="password" maxlength="32" id="repwd" class="FormBase" onfocus="this.className='FormFocus';" onblur="this.className='FormBase';" maxlength="32" size="20" style="width:150px;" />*
                                                        
                                                    </td>
                                                </tr>
                                               <%-- <tr>
                                                    <td>管理员邮箱:</td>
                                                    <td style="width: 352px">
                                                    <asp:TextBox  ID="AdminMailTextBox" runat="server"  MaxLength="50"  Size="20" ></asp:TextBox>
                                                   </td>
                                                </tr>--%>
                                                <tr>
                                                    <td style="background-color: #f5f5f5">
                                                        数据库类型:</td>
                                                    <td style="background-color: #f5f5f5; width: 352px;">
                                                    <asp:DropDownList ID="DbTypeDropDownList" runat="server" onchange="SelectChange(this)">
                                                            <asp:ListItem Value="0">请选择数据库类型</asp:ListItem>
                                                            <asp:ListItem Value="SqlServer">SqlServer</asp:ListItem>
                                                            <asp:ListItem Value="MySql">MySql</asp:ListItem>
                                                             <asp:ListItem Value="Oracle">Oracle</asp:ListItem>
															<asp:ListItem Value="SQLite">SQLite</asp:ListItem>
                                                            <asp:ListItem Value="Access">Access</asp:ListItem>
                                                      </asp:DropDownList>
                                                        <span id="msg0"></span>
                                                    </td>
                                                </tr>
                                                <tr id="tr1">
                                                    <td style="background-color: #f5f5f5">
                                                        服务器名或IP地址:</td>
                                                    <td style="background-color: #f5f5f5; width: 352px;">
                                                    <asp:TextBox  ID="ServerTextBox" runat="server" Width="150" Enabled="true" onblur="checkid(this,'1')">(local)</asp:TextBox>
                                                    *<span id="msg1"></span>
                                                     </td>
                                                </tr>
                                                <tr id="tr2">
                                                    <td style="background-color: #f5f5f5">
                                                        数据库名称:</td>
                                                    <td style="background-color: #f5f5f5; width: 352px;">
                                                    <asp:TextBox  ID="DatabaseTextBox" runat="server"  Width="150" Enabled="true" onblur="checkid(this,'2')">We7_CMS</asp:TextBox>
                                                    *<span id="msg2"></span>
                                                     </td>
                                                </tr>
                                                <tr id="tr3">
                                                    <td style="background-color: #f5f5f5">
                                                        数据库用户名称:</td>
                                                    <td style="background-color: #f5f5f5; width: 352px;">
                                                    <asp:TextBox  ID="UserTextBox" runat="server" Enabled="true" onblur="checkid(this,'3')" Width="150"></asp:TextBox>
                                                     *<span id="msg3"></span></td>
                                                </tr>
                                                <tr id="tr4">
                                                    <td style="background-color: #f5f5f5">
                                                        数据库用户密码:</td>
                                                    <td style="background-color: #f5f5f5; width: 352px;">
                                                    <asp:TextBox   ID="PasswordTextBox" runat="server"  Width="150" Enabled="true"
                                                            TextMode="Password"></asp:TextBox></td>
                                                </tr>
                                                <tr id="tr5">
                                                    <td style="background-color: #f5f5f5">
                                                        数据库文件:</td>
                                                    <td style="background-color: #f5f5f5; width: 352px;">
                                                    <asp:TextBox  ID="DbFileNameTextBox" runat="server"  Width="150px" onblur="checkid(this,'5')">We7_CMS_DB</asp:TextBox>
                                                        <span id="msg5"></span>( 默认:We7_CMS_DB.DB3 )</td>
                                                </tr>
                                                  <tr id="tr6">
                                                    <td><asp:CheckBox runat="server" ID="CreateNewDBCheckBox" Text="创建新数据库" Checked="true" /></td>
                                                    <td >
                                                    
                                                   </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                            <asp:Panel runat="server" ID="ConfigMsgPanel" Visible="false" >
                                  <font color="#ff6633">错误: 无法把设置写入"db.config"文件, 您可以将下面文件框内容保存为"db.config"文件, 然后通过FTP软件上传到<strong>网站Config目录</strong><br/>
                                    </font>
                                    <br>
                                    db.config 内容:
                                    <input type="button" value="复制到剪贴板" accesskey="c" onclick="HighlightAll(this.form.txtMsg)">
                                    <asp:TextBox  ID="txtMsg" runat="server" Height="180" TextMode="MultiLine"   Width="98%"></asp:TextBox>
                            </asp:Panel>
                                <div style="margin:10px 20px 30px 10px;text-align:right">
                                            <asp:Button ID="ResetDBInfo" runat="server" Text="下一步" OnClick="ResetDBInfo_Click"   class="bprimarypub80" ></asp:Button>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
  </div>
        <%=footer%>

    <script type="text/javascript">

    </script>
    <input type="hidden" id="dbtype" />
</form>
</body>
</html>
