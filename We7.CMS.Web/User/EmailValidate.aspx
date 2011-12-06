<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailValidate.aspx.cs"
    MasterPageFile="~/User/DefaultMaster/content.Master" Inherits="We7.CMS.Web.User.EmailValidate" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MyHeadPlaceHolder" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=ThemePath%>/css/article.css" media="screen" />
    <script src="/Admin/Ajax/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="/Admin/cgi-bin/article.js" type="text/javascript"></script>
    <script src="/Admin/cgi-bin/cookie.js" type="text/javascript"></script>
    <script src="/Admin/cgi-bin/tags.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <form id="Form2" method="post" runat="server">
    <div class="realRight ml10">
        <div class="box">
            <div class="tit clearfix">
                <div class="at_loc">
                    <span <%=EmailValidateStep == 1 ? " class='now' " : "" %>>1.填写邮箱地址</span> <span class="<%=GetStep1Str()%>">
                    </span><span <%=EmailValidateStep == 2 ? " class='now' " : "" %>>2.验证邮箱</span> <span
                        class="<%=GetStep2Str()%>"></span><span <%=EmailValidateStep == 3 ? " class='now' " : "" %>>
                            3.认证成功</span> <span class="<%=GetStep3Str()%>"></span>
                </div>
                <h3>
                    邮箱认证</h3>
            </div>
            <div class="con">
                <div class="telSuc clearfix" style="display: <%=EmailValidateStep==2 ? "none" : "block" %>">
                    <div id="Div1" class="telSuccont" runat="server">
                        <div style="display: <%=EmailValidateStep==3 ? "block" : "none" %>">
                            <u class="at_msg m_1_o"></u><strong class="teltit">您<%=IsEmailValidate ? "已经" : "还未" %>通过We7插件商店邮箱认证！</strong><br>
                            <span style="font-size: 14px;">
                                <asp:LinkButton ID="lbtnChange" runat="server" OnClick="lbtnChange_Click">修改</asp:LinkButton></span>
                        </div>
                        <table id="realIdFrom" class="telFrom" style="display: <%=EmailValidateStep==1 ? "block" : "none" %>">
                            <tbody>
                                <tr>
                                    <th valign="top">
                                        <span class="orange1">*</span>邮箱地址&nbsp;
                                    </th>
                                    <td>
                                        <span class="at_text t_1_d">
                                            <asp:TextBox ID="txtEmail" Enabled="false" runat="server" Width="150px" MaxLength="50" Height="28px"></asp:TextBox>
                                        </span>
                                        <p class="mt15">
                                            <asp:LinkButton ID="lbtnNext" runat="server" OnClick="lbtnNext_Click" CssClass="at_but b_1_y"><u></u>下一步</asp:LinkButton>
                                        </p>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <p>
                            <%=IsEmailValidate ? "现在开始" : "邮箱认证成功后"%>，您可以享有以下服务：</p>
                        <ul>
                            <li><strong>邮箱地址登录: </strong>可直接使用“邮箱账号”登录到We7插件商店。</li>
                            <li><strong>重要事件提醒: </strong>进行（支付/提现/选稿/中标）时，可及时收到邮件提醒。</li>
                            <li><strong>找回账号密码: </strong>忘记密码时，可使用邮件找回密码。</li>
                            <li><a href="Index.aspx">&lt;&lt;返回到我的We7</a></li>
                        </ul>
                    </div>
                </div>
                <div class="emailCerti" style="display: <%=EmailValidateStep==2 ? "block" : "none" %>">
                    <u class="at_msg m_1_o mr15"></u>
                    <p class="emailBt">
                        您的邮箱将收到一封认证确认邮件，请查收。<br>
                        点击邮件中的确认链接即可通过邮箱认证。</p>
                    <p style="display: none;" class="emailBtn">
                        <a onclick="gomail('163.com');" href="javascript:void(0)" class="at_but b_1_y"><u></u>
                            立即查收邮件</a></p>
                    <p class="f140">
                        如果您没有收到邮件</p>
                    <ul>
                        <li>邮件到达需要2-3分钟，若您长时间未收到邮件，建议您检查<strong>垃圾箱</strong></li>
                        <li>如果您邮箱填写有误或一直无法收到邮件，建议您更换邮箱</li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    </form>
</asp:Content>
