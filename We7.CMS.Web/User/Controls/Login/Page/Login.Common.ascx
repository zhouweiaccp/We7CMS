<!--### name="普通文字登录框" type="system" version="1.0" created="2009/12/03" 
desc="普通用户登录框，标准form post 提交模式" author="We7 Group" ###-->
<%@ Control Language="C#" AutoEventWireup="true" Inherits="We7.CMS.WebControls.LoginProvider" %>
<% Response.Cookies["AreYouHuman"].Value = We7.CMS.Web.Admin.CaptchaImage.GenerateRandomCode(); %>
<style type="text/css">
    li
    {
        list-style: none outside none;
        line-height:30px;
    }
</style>
<div style="float: left; padding-left: 30px;">
    <div class="Login_Common_<%=CssClass %>">
        <form id="<%=ActionID %>" action="/User/Controls/Login/CS/LoginAction.ashx" method="post"
        target="_self">
        <script type="text/javascript">
            function submitAction(action) {
                document.getElementById("<%=ActionID %>").action = "/User/Controls/Login/CS/LoginAction.ashx?Action=" + action;
                document.getElementById("<%=ActionID %>").submit();
                alert('<% Get("Message"); %>');
            }
        </script>
        <div style="display: <%= LoginNameDisplay %>">
            <%=Get("Author") %>| <a href="javascript:submitAction('logout');">退出</a>
        </div>
        <div style="<%=MessageDisplay %>">
            <%= (Get("Message") != null&Get("Message") != "") ? ("<script>alert('" + Get("Message") + "')</script>") : ""%>
        </div>
        <div style="display: <%= LoginInputDisplay %>">
            <li>用户名：<input type="text" name="Name" /></li>
            <li>密　码：<input type="password" name="Password" /></li>
        </div>
        <div style="display: <%=SelectValidate %>">
            <li>验证码：
                <input name="ValidateCode" type="text" style="width: 70px;" class="search" />
                <img alt="" src="/Admin/cgi-bin/controls/CaptchaImage/SmallJpegImage.aspx" style="width: 100px;
                    height: 22px; margin-bottom: -7px; cursor: pointer;" />
            </li>
        </div>
        <div class="bottom">
            <li>
                <input type="submit" value="登录" /></li>
            <li class="zc"><a href="/register.aspx">立即注册一个新用户</a></li>
            <li class="clear"></li>
        </div>
        <input name="IsSignin" type="hidden" value="<%=IsSignin %>" />
        <input name="ReturnUrl" type="hidden" value="<%=ReturnUrl %>" />
        <input name="ISValidate" type="hidden" value="<%=ISValidate %>" />
        <input name="_ActionID" type="hidden" value="<%=ActionID %>" />
        </form>
    </div>
    <div style="clear: left;">
    </div>
</div>
