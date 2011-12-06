<%@ Import Namespace="We7.CMS" %>
<%@ Import Namespace="We7.CMS.Install" %>
<%@ Page Language="c#" AutoEventWireup="false" Inherits="We7.CMS.Install.index" %>
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
                                      <h1>欢迎安装
                                            <%=SetupPage.producename%>
                                      </h1>  
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
                            <br>
                            <br>
                            <table id="Table2" cellspacing="1" cellpadding="1" width="90%" align="center" border="0">
                                <tr>
                                    <td>
                                        <p>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 欢迎您选择安装<%=SetupPage.producename%></p>
                                        <p>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 本向导将协助您一步步的安装和初始化系统.</p>
                                        <p>
                                            &nbsp;&nbsp;&nbsp; &nbsp;&nbsp; 强烈建议您在运行本向导前仔细阅读程序包中的《安装说明》文档, 如果您已经阅读过, 请点击下一步.</p>
                                    </td>
                                </tr>
                            </table>
                            <p>
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                           <div style="margin:10px 20px 30px 10px;text-align:right">
                                        <input type="button" onclick="javascript:window.location.href='step2.aspx';" value="下一步" class="bprimarypub80"></td>
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
