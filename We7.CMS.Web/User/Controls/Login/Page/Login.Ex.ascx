<!--### name="新登陆控件" type="system" version="1.0" created="2009/12/03" 
desc="新登陆控件" author="We7 Group" ###-->
<%@ Control Language="C#" AutoEventWireup="true" Inherits="We7.CMS.WebControls.LoginProviderEx" %>
<% if (!IsSignIn)
   { %>
<% using (Html.BeginFrom())
   { %>
<input name="Action" type="hidden" value="login" />
<table style="width:100%;">
    <tr>
        <td>
            用户名:
        </td>
        <td>
            <input type="text" name="LoginName" />
        </td>
        <td>
            密码:
        </td>
        <td>
            <input type="password" name="Password" />
        </td>
        <td>
            <input type="submit" value="登陆" />
            </td>
            <td>
            <input type="button" onclick="window.open('/register.aspx')" value="注册新用户" />
        </td>
    </tr>
</table>
<%} %>
<%}
   else
   { %>
<% using (Html.BeginFrom())
   { %>
<input name="Action" type="hidden" value="logout" />
<table style="width:100%;">
    <tr>
        <td>
            欢迎<b><%=UserName%></b>
        </td>
        <td>
            <input type="submit" value="退出" />
            </td>
            <td>
            <input type="button" onclick="window.open('/register.aspx')" value="注册新用户" />
        </td>
    </tr>
</table>
<%}
   } %>
