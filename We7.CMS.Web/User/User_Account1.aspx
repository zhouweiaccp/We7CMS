<%@ Page Language="C#" MasterPageFile="~/User/DefaultMaster/content.Master" AutoEventWireup="true"
    CodeBehind="User_Account1.aspx.cs" Inherits="We7.CMS.Web.User.User_Account1" Title="帐号信息" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MyHeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <div class="realRight ml10">
        <div class="mybox">
            <div class="mytit">
                账号安全</div>
            <div class="con accSecur">
                <!---安全进度红色条条：直接更改 class="scroll" 里面em 的宽度就可设定安全级别-->
                <div class="secScroll clearfix">
                    <em class="frist"></em>
                    <p class="cont">
                        <strong>账号安全级别：</strong> <span class="scroll"><em class="<%=AccountSafeLevelClass %>"
                            style='width: <%=AccountSafeLevelNumber%>%;'></em></span>安全等级：<span class="orange1"></span>
                    </p>
                    <em class="last"></em>
                </div>
                <p class="mt10 fc6">
                    <u class="at_msg m_3_i mr5"></u>可通过以下方式提高安全级别：</p>
                <p class="mt5 pasOper">
                    <a href="Account_Password.aspx">修改密码</a> <a href="ChangeTradePWD.aspx" style="display: none">
                        修改安全密码</a>
                    <%--<a class="ml5" href="#succ">完善账号安全认证</a>--%>
                </p>
                <h3>
                    账号登录信息：</h3>
                <ul class="loginInfor">
                    <li>用户名：<%= CurrentAccount.LoginName%></li>
                    <li>邮箱登录帐号：<%= CurrentAccount.Email%><%--<a class="ml5" href="/cert/email">修改</a>--%></li>
                    <%--<li>手机登录帐号：未激活<a class="ml5" href="/cert/mobile">激活</a></li>--%>
                    <%--<li>上次登录：2011年02月16日 15:11:07</li>--%>
                </ul>
                <h3>
                    账号安全认证：</h3>
                <a name="succ"></a>
                <ul class="certiInfor mb15">
                    <%-- <li class="notCer"><a href="/real/index" class="at_but b_2_g"><u></u>立即认证</a> <u
                        class="at_msg m_3_i"></u><strong>实名认证</strong> <span>用于提升账号的安全性和提高信任度，认证后不能修改</span>
                    </li>
                    <li class="notCer"><a href="/bank/index" class="at_but b_2_g"><u></u>立即认证</a> <u
                        class="at_msg m_3_i"></u><strong>银行卡认证</strong> <span>通过银行卡认证才能将诚付宝余额提现</span>
                    </li>
                    <li class="notCer"><a href="/cert/mobile" class="at_but b_2_g"><u></u>立即认证</a> <u
                        class="at_msg m_3_i"></u><strong>手机认证</strong> <span>手机认证后，可使用手机登录、手机找回密码、接收各类重要提醒</span>
                    </li>--%>
                    <li class="<%=CertClass %>"><a href="/user/EmailValidate.aspx">
                        <%=CertText %></a> <u class="at_msg m_3_o"></u><strong>邮箱认证</strong> <span>邮箱认证后，可使用邮箱直接登录，邮箱找回密码，接收各类重要提醒</span>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</asp:Content>
