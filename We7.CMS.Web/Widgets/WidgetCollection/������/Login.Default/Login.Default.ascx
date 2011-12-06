<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Login.Default.cs" Inherits="We7.CMS.Web.Widgets.Login_Default" %>
<script type="text/C#" runat="server">
    [ControlDescription(Desc = "登录", Author = "老莫")]
    string MetaData;
</script>
<div class="<%=CssClass %>">
    <div class="user">
        <h3 <%=BackgroundIcon() %>>
            会员中心</h3>
        <div <%=SetBoxBorderColor() %> class="user_info">
            <div class="user_login" style='display: <%=IsLogin?"none":"block"%>'>
                <img align="absmiddle" alt="正在加载数据..." src="/admin/ajax/Images/loading.gif" id="loginStatusSign2"
                    style="display: none;">
                <ul class="userlogin" style="" id="loginField2">
                    <li>用户名：<input type="text" class="inputtext" maxlength="20" name="username" id="username"></li>
                    <li>密&nbsp;&nbsp;码：<input type="password" class="inputtext" name="userpass" id="password"></li>
                    <li style="display: none;" id="validateField2">验证码：<input type="text" style="width: 50px;"
                        class="inputtext" maxlength="6" name="validate" id="validationCode2"><img align="absmiddle"
                            src="" title="换一个" id="validateSign2" class="validateSign" style="display: none;"></li>
                    <li class="save">
                        <label>
                            <input type="checkbox" name="saveinfo">下次自动登录</label></li>
                    <li class="reg_user">
                        <input type="submit" class="user_login_submit" value="" id="login2">&nbsp;&nbsp;</li>
                    <li class="reg"><a class="reg_password" title="忘记密码" href="/User/GetPassword.aspx">忘记密码？</a>&nbsp;&nbsp;<a
                        title="立即注册" href="/Register.aspx">立即注册</a></li>
                </ul>
            </div>
            <ul class="loginedinfo" id="loginedUserInfo2" style='display: <%=IsLogin?"block":"none" %>'>
                <li><b>
                    <%=AccountName %></b>，您好！欢迎登陆。</li>
                <li>今天是<%=DateTime.Now.ToString("yyyy年MM月dd日") %></li>
                <li>经验积分：0 分</li>
                <li><a href="/User/Index.aspx">会员中心</a>|<a href="javascript:void(0)" name="logout"
                    id="logout2">退出登录</a></li>
            </ul>
        </div>
    </div>
</div>
<script type="text/javascript">
    $(function () {
        $("#login2").click(function () {

            var u = $("#username").val();
            if (!u || u == "") {
                alert("用户名不能为空");
                return;
            }

            var p = $("#password").val();
            if (!p || p == "") {
                alert("密码不能为空");
                return;
            }

            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "/User/Ajax.asmx/Login",
                data: "{ username:'" + u + "', password:'" + p + "'}",
                dataType: 'json',
                success: function (result) {
                    if (result) {
                        if (result[0] == "true") {
                            location.reload();
                        }
                        else {
                            alert(result[1]);
                        }
                    }
                    else {
                        alert("登陆失败,请检验你的用户名与密码是否正确");
                    }
                },
                error: function (result) {
                    alert("应用程序错误，请与管理员联系!");
                }
            });

        });

        //登出
        $("#logout2").click(function () {

            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "/User/Ajax.asmx/Signout",
                data: "{}",
                dataType: 'json',
                success: function (result) {
                    if (result.length > 0)
                        alert(result);
                    location.reload();
                },
                error: function (result) {
                    alert("应用程序错误，请与管理员联系!");
                }
            });

        });
    });
</script>
