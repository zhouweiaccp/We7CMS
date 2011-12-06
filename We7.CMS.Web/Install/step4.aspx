
<%@ Page Language="c#" AutoEventWireup="false" Inherits="We7.CMS.Install.Step4" %>

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
                                      <h1>3、创建数据库表及初始化数据</h1>
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
                            <div>
                                您选择的数据库类型是: <b><%= Request["db"].ToString()%></b> <br /><br />
                                        <%
                                                      Response.Write("<p>系统将会执行如下操作, 这可能需要一些时间......\r\n");
                                                      Response.Write("</p>\r\n");
                                                      Response.Write("<p>\r\n");
                                                      Response.Write("&nbsp; 1.创建表</p>\r\n");
                                                      Response.Write("<p>\r\n");
                                                      Response.Write("    &nbsp; 2.更新表结构</p>\r\n");
                                                      Response.Write(" <p>\r\n");
                                                      Response.Write("&nbsp; 3.初始化数据</p>\r\n");
                                                      Response.Write(" <p>\r\n");
                                                      Response.Write("&nbsp;</p>\r\n");
                                         %>
                                 </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                  <div style="margin:10px 20px 30px 10px;text-align:right">
                                            <asp:Button ID="PrevPage" runat="server" Text="上一步" OnClick="PrevPage_Click"    class="bprimarypub80" ></asp:Button>&nbsp;&nbsp;
                                            <asp:Button ID="ResetDBInfo" runat="server" Text="开始运行" Enabled="true"    class="bprimarypub80" ></asp:Button>
                                     </div>
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
