<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Register.aspx.cs" Inherits="We7.CMS.Web.User.Register" Title="站点注册" %>

<%@ Register Src="/We7Controls/UserRegister/Page/UserRegister.Simple.ascx" TagName="UserRegister_Simple"
    TagPrefix="wec" %>
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
                    新用户注册</div>
                <div class="con">
                    <wec:UserRegister_Simple id="UserRegister_Simple1" cssclass="one" runat="server">
                    </wec:UserRegister_Simple>
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
            (C)2011 西部动力（北京）科技有限公司 版权所有 Powered by We7 2.7, 京ICP备 050340009 号
        </div>
    </div>
</body>
</html>
