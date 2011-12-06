<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InstallWidget.aspx.cs"
    Inherits="We7.CMS.Web.Admin.VisualTemplate.InstallWidget" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>安装部件</title>
    <script type="text/javascript" src="/Scripts/jquery/jquery-1.4.2.js"></script>
    <script type="text/javascript" src="/admin/theme/classic/js/common.js"></script>
    <script type="text/javascript" src="/admin/theme/classic/js/hoverIntent.js"></script>
    <script src="/admin/cgi-bin/DialogHelper.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="/admin/theme/classic/css/article.css" media="screen" />
    <script src="/admin/cgi-bin/article.js" type="text/javascript"></script>
    <script src="/admin/cgi-bin/cookie.js" type="text/javascript"></script>
    <script src="/admin/cgi-bin/tags.js" type="text/javascript"></script>
    <link rel="Stylesheet" href="" id="scrollshow" type="text/css" />
<%--    <script src="/Admin/Ajax/Mask.js" type="text/javascript"></script>--%>
    <script src="/Admin/VisualTemplate/Scripts/Mask.js" type="text/javascript"></script>
    <script src="/Admin/Ajax/Prototype/prototype.js" type="text/javascript"></script>
    <meta http-equiv="Content-Type" content="html/text; charset=utf-8" />
    <script type="text/javascript" src="/Admin/Ajax/Ext2.0/adapter/ext/ext-base.js"></script>
    <script type="text/javascript" src="/Admin/Ajax/Ext2.0/ext-all.js"></script>
    <script type="text/javascript" src="/Install/js/Plugin.js"></script>
    <script type="text/javascript">
    function submitSingleAction(action, type) {
        var param = {};
        param.action = action;
        param.plugin = type;        
        switch (action) {
            case "remoteinstall":
                param.title = "安装插件";
                param.message = "安装成功！";
                break;
            case "remoteupdate":
                param.message = "更新成功！";
                param.title = "更新插件";
                break;
            case "insctr":
                param.message = "安装成功！";
                param.title = "安装控件";
                break;
        }
        new MaskWin(param, closeParentWindow).showMessageProgressBar(param);
        return false;
    }

    function closeParentWindow() {
        //$("#cboxClose", parent.document).click();
        window.parent.document.getElementById('cboxClose').click();
    }

    function buildParam(elName) {
        var param = "";
        var list = document.getElementsByName(elName);
        for (var i = 0; i < list.length; i++) {
            if (list[i].checked)
                param += list[i].value + ",";
        }
        if (param.length > 0)
            param = param.substr(0, param.length - 1);
        return param;
    }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hdAction" runat="server" />
        <asp:HiddenField ID="hdUrl" runat="server" />
    </form>
</body>
<script type="text/javascript">
    var action = document.getElementById('<%= hdAction.ClientID%>').value;
    var purl = document.getElementById('<%= hdUrl.ClientID%>').value;
    //alert(action + "---" + purl);
    submitSingleAction(action, purl);
</script>
</html>
