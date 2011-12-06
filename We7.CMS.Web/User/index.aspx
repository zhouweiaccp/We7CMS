<%@ Page Language="C#" MasterPageFile="~/User/DefaultMaster/content.Master" AutoEventWireup="true"
    CodeBehind="index.aspx.cs" Inherits="We7.CMS.Web.User.index" Title="我的We7" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MyHeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <div class="realRight ml10">
        <div class="mybox">
            <div class="mytit">
                我的We7</div>
        </div>
        <div class="mt10">
            <div class="mybox">
                <div class="mytit">
                    账号信息</div>
                <div class="con">
                    <dl style="" class="acInfor clearfix fc6">
                        <dt>账号信息</dt>
                        <dd class="ml10">
                            <p>
                                用户名：<%=UserName%>
                            </p>
                            <p>
                            </p>
                        </dd>
                    </dl>
                    <dl class="acInfor clearfix fc6 mt10">
                        <dt>账号安全级别</dt><!--<i>长度最大184px,默认为绿色,红色则需要设置CLASS属性为'hot'-->
                        <dd class="ml10">
                            <p>
                                <span class="seu_Level"><i style="width: <%=AccountSafeLevelNumber%>%;" class="<%=AccountSafeLevelClass %>">
                                </i></span><b class="orange1 ml5">
                                    <img border="0" src="/User/style/images/<%= AccountSafeLevel == "低" ? "low.png" : "high.png" %>" />
                                </b>
                            </p>
                        </dd>
                    </dl>
                    <dl class="acInfor clearfix fc6 mt10">
                        <dt>资料完善度</dt><!--<i>长度最大184px,默认为绿色,红色则需要设置CLASS属性为'hot'-->
                        <dd class="ml10">
                            <p>
                                <span class="seu_Level"><i style="width: <%=InfomationNumber*1.84%>px;" class="<%=InfomationClass %>">
                                </i></span><b class="Green ml5">
                                    <%=InfomationNumber%>%</b></p>
                            <p>
                                <a href="<%=InfomationHref %>">立即完善资料</a>
                            </p>
                        </dd>
                    </dl>
                    <dl class="acInfor clearfix fc6 mt10">
                        <dt>安全认证</dt><!--<i>长度最大184px,默认为绿色,红色则需要设置CLASS属性为'hot'-->
                        <dd class="ml10">
                            <p class="mt5">
                                <u class="at_msg m_3_o mr5"></u>邮箱认证： <span class="<%=CertClass %>">&nbsp;<%=CertText%></span>&nbsp;<a
                                    href="/user/EmailValidate.aspx">修改</a>
                            </p>
                        </dd>
                    </dl>
                    <dl style="border: medium none; display:none;" class="acInfor clearfix fc6 mt10">
                        <dt>安全密码</dt><!--<i>长度最大184px,默认为绿色,红色则需要设置CLASS属性为'hot'-->
                        <dd class="ml10">
                            <b class="Green">
                                <%=IsAdvanceUser ? "已设置" : "未设置" %></b> <a class="ml5" href="<%=SafePWDUrl %>">
                                    <%=IsAdvanceUser ? "修改" : "设置" %></a>
                        </dd>
                    </dl>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
