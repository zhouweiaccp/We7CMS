<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="GetPassword.aspx.cs" Inherits="We7.CMS.Web.User.GetPassword" Title="密码找回" %>

<%@ Register Src="/User/Controls/RetrievePassword.Common.ascx" TagName="RetrievePassword_Common" TagPrefix="wec" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="/user/style/main.css" media="screen" />
    <link rel="stylesheet" type="text/css" href="/user/style/realCer_stylem.css" media="screen" />
    <link rel="stylesheet" type="text/css" href="/user/style/auntion.css" media="screen" />
    <script type="text/javascript" src="/Scripts/jQuery/jquery-1.4.2.js"></script>
</head>
<body style="background-color: rgb(255, 255, 255);">
    <div id="head">
        <div id="logo">
            <a alt="" href="/user/index.aspx">
                <img alt="We7 CMS" src="/user/style/images/we7_logo_user.png"></a></div>
        <div id="banner">
        </div>
    </div>
    <div class="realCer">
        <div class="realRight ml10 mt10">
            <div class="mybox">
                <div class="mytit">
                    密码找回</div>
                <div class="con" style="text-align: center">
                    <wec:RetrievePassword_Common id="register1" cssclass="common" runat="server" mondelname=""
                        UseModel="false" EmailValidate="false" ShowProtocol="true">
                    </wec:RetrievePassword_Common>
                </div>
            </div>
        </div>
        <div class="clear">
        </div>
        <div id="toogle_panel">
        </div>
        <div id="foot">
            <a href="/">网站首页</a> | <a href="http://www.westengine.com/about-us/">关于我们</a> |
            <a class="link4" href="http://www.westengine.com/about-us/contact-us/">联系我们</a>
            | <a class="link4" href="/other/site-map/">网站地图</a> | <a class="link4" href="http://www.westengine.com/we7-cms/">
                产品首页</a>
            <br>
            (C)2010 西部动力（北京）科技有限公司 版权所有 Powered by We7 2.5, 京ICP备 050340009 号
        </div>
    </div>
</body>
</html>
