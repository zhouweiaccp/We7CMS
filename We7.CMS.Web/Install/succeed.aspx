<%@ Page Language="C#" AutoEventWireup="true" Inherits="We7.CMS.Install.succeed" %>
<%@ Import Namespace="We7.CMS.Install" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<%=SetupPage.header%>
<body  class="pubbox_login">
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
                                      <h1>OK！安装成功</h1>
                                      </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td width="180" valign="top">
                            <%=SetupPage.logo%>
                        </td>
                        <td width="520" valign="top">
                            <div>
                                        <img src="images/succeed.jpg" alt="安装成功" style="margin-left:140px" /><br />
                                        <br />
                                        <asp:Literal  runat="server" ID="SummaryLiteral" ></asp:Literal>
                                        <br />
                                        接下来您将可以访问网站首页和后台。<br />
                                        如果进行系统设置，请在"登录"后台，选择"设置-常规"选项。
                           </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                             <div style="margin:10px 20px 30px 10px;text-align:right">
                                        <input type="button" onclick="javascript:window.location.href='../admin/signin.aspx';"    class="bprimarypub80"  value="登录后台">
                              </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
  </div>
    <%=SetupPage.footer%>
</body>
</html>
