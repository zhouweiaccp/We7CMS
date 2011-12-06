<%@ Page Language="c#" AutoEventWireup="false" Inherits="We7.CMS.Install.SetupPage"  %>
<%@import namespace="We7.CMS"%>
<%@import namespace="We7.CMS.Install"%>
<%@import namespace="We7.CMS.Config"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

    protected void FillConfigButton_Click(object sender, EventArgs e)
    {
        bool ret=CDHelper.Instance.MigrateConfig();
        if (ret)
        {
            this.Response.Redirect("succeed.aspx");
        }
        else
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", @"<script>alert(""迁移配置参数数据出现错误，请与管理员联系。"");&lt;/script>");
    }
</script>

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
                                      <h1>升级站点配置文件</h1>
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
                            本操作将把原来数据库中SiteSetting中的信息重新写入到\config\site.config中，并将sitesetting表删除。
                            
                            </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                  <div style="margin:10px 20px 30px 10px;text-align:right">
                                            <asp:Button ID="FillConfigButton" runat="server" Text="开始执行" Enabled="true"    
                                                class="bprimarypub80" onclick="FillConfigButton_Click" ></asp:Button>
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
