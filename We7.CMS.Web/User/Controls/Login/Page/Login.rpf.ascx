<!--### name="普通文字登录框(特定版)" type="system" version="1.0" created="2010/09/03" 
desc="普通用户登录框(特定版)，标准form post 提交模式" author="We7 Group" ###-->
<%@ Control Language="C#" AutoEventWireup="true" Inherits="We7.CMS.WebControls.LoginProvider" %>
<% Response.Cookies["AreYouHuman"].Value = We7.CMS.Web.Admin.CaptchaImage.GenerateRandomCode(); %>
    <h2>登录</h2>
        <p></p>
            
        <div class="primary">
<div class="featured">
<form id="<%=ActionID %>" action="/User/Controls/Login/CS/LoginAction.ashx" method="post" target="_self">
        <script type="text/javascript">
            function submitAction(action) {
                document.getElementById("<%=ActionID %>").action = "/User/Controls/Login/CS/LoginAction.ashx?Action=" + action;
                document.getElementById("<%=ActionID %>").submit();
                alert('<% Get("Message"); %>');
            }
        </script>
        <div>
        <input type="hidden" id="LoginReferer" value="/zh-CN/firefox/users/register" name="data[Login][referer]">
        
        <dl style="<%=MessageDisplay %>">
            <dd class="pad" style="color: Red">
                <%= (Get("Message") != null&Get("Message") != "") ? ("<script>alert('" + Get("Message") + "')</script>") : ""%>
            </dd>
        </dl>
        
        </div>
    <div class="container">
    <br /><br />
        <label for="LoginEmail">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 用户名</label>
        <%--<input type="text" id="Text1" value="" name="data[Login][email]">--%>
        <input type="text" name="Name" /
          </div>
    <div class="container">
        <label for="LoginPassword">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 密&nbsp;&nbsp; 码</label>
        <%--<input type="password" id="LoginPassword" value="" name="data[Login][password]">--%> 
        <input type="password" name="Password" />
           </div>
            <div class="container">
        <label for="ValidateCode">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 验证码</label>
        <%--<input type="password" id="LoginPassword" value="" name="data[Login][password]">--%> 
         <input name="ValidateCode" type="text" style="width: 70px;" /><img alt="" src="/Admin/cgi-bin/controls/CaptchaImage/SmallJpegImage.aspx" style="width: 100px;
                    height: 22px;" />
           </div>
   
    <div class="container">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <button class="prominent" type="submit">&nbsp;登 &nbsp;录&nbsp;</button>
    </div>
    <input name="IsSignin" type="hidden" value="<%=IsSignin %>" />
         <input name="ReturnUrl" type="hidden" value="<%=ReturnUrl %>" />
        <input name="ISValidate" type="hidden" value="<%=ISValidate %>" />
        <input name="_ActionID" type="hidden" value="<%=ActionID %>" />
</form>
</div>
</div>

<script charset="utf-8" type="text/javascript">
    // focus email field
    $(document).ready(function() { $('#LoginEmail').focus(); });
</script>
        <div class="secondary">
        <div class="article" id="login-help">
        <h3>登录问题?
        <ul>
            <li><a href="/register.aspx">我没有帐号。</a></li>
            <li><a href="/User/GetPassword.aspx">忘记密码。</a></li>
        </ul>
        </div>
        </div>