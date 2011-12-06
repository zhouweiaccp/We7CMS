
<%@ Page Language="c#" AutoEventWireup="false" Inherits="We7.CMS.Install.signin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>

<%=header%>
<body  class="pubbox_login">
    <form id="Form1" method="post" runat="server">
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
                                      <h1>系统管理员登录（仅限超级管理员）</h1>
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
                              <table class="loginform" cellpadding="0" callspacing="0" style="height: 120px">
                                <tbody>
                                    <tr>
                                        <th>
                                            密 码：
                                        </th>
                                        <td>
                                            <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password" Columns="30"></asp:TextBox>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr  >
                                        <th>
                                            验证码：
                                        </th>
                                        <td>
                                              <asp:TextBox ID="VerifyCodeTextBox" runat="server" ToolTip="验证码不区分大小写"></asp:TextBox>
                                        </td>
                                        <td>
                                            <img id="imgVerify" src="VerifyCode.aspx?" alt="看不清？点击更换" onclick="this.src=this.src+'?'" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Button ID="LoginButton" runat="server" Text="登录" Enabled="true"    class="bprimarypub80" ></asp:Button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <br /><br />
                            <br /><br />
                                    <span style="background-color:#080; color:#efe;  padding:1px 4px">注</span> 
                                    如果您是在第一次运行或试图初始化数据库的时候看到这个界面，<br />
                                    说明系统存在数据库配置文件，您可以手工删除 Config目录下的db.config，
                               <br /> 然后再运行 /install/index.aspx 。<br /><br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                              
                                </td>
                            <td>

                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        </div>
        <%=footer%>
    </form>
</body>
</html>
