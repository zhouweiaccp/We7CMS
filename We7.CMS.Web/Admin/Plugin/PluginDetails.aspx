<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PluginDetails.aspx.cs"
    Inherits="We7.CMS.Web.Admin.Modules.Plugin.PluginDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>

    <script>
        window.onload=function()
        {
            if(parent!=null)
            {
                var contentFrame=parent.document.getElementById("contentFrame");
                if(contentFrame)
                {
                    document.body.style.width=contentFrame.clientWidth;
                    document.body.style.height=contentFrame.clientHeight;
                    document.getElementById("menuTabDiv").style.width=contentFrame.clientWidth-30;
                }
            }
        }
    </script>

    <style>
        body
        {
            color: #333333;
            font-size: 12px;
            font-family: 宋体 Arial;
            line-height: 150%;
        }
    </style>
</head>
<body style="padding: 0; margin: 0; overflow: auto;">
    <form id="form1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=ThemePath%>/css/article.css" media="screen" />


    <script src="<%=AppPath%>/cgi-bin/article.js" type="text/javascript"></script>

    <script src="<%=AppPath%>/cgi-bin/cookie.js" type="text/javascript"></script>

    <script src="<%=AppPath%>/cgi-bin/tags.js" type="text/javascript"></script>

    <link rel="Stylesheet" href="" id="scrollshow" type="text/css" />

    <script src="/Admin/Ajax/Mask.js" type="text/javascript"></script>

    <script src="/Admin/Ajax/Prototype/prototype.js" type="text/javascript"></script>

    <meta http-equiv="Content-Type" content="html/text; charset=utf-8" />

    <script type="text/javascript" src="/Admin/Ajax/Ext2.0/adapter/ext/ext-base.js"></script>

    <script type="text/javascript" src="/Admin/Ajax/Ext2.0/ext-all.js"></script>

    <script type="text/javascript" src="/Admin/Ajax/Plugin.js"></script>

    <script type="text/javascript">
        function submitSingleAction()
        {
            var updatetype='<%=updatetype %>';
            var param={};              
            param.pltype="<%=PluginType %>".toLowerCase();         
            param.plugin='<%=Request.QueryString["key"] %>';
            switch(updatetype)
            {
                case "0":
                    param.title="安装插件:";
                    param.message="安装成功！";
                    param.action="remoteinstall"; 
                    parent.mask.showMessageProgressBar(param);                  
                    break;
                 case "10":
                    param.title="安装控件:";
                    param.message="安装成功！";
                    param.action="insctr"; 
                    parent.mask.showMessageProgressBar(param);                  
                    break;
                case "1":
                    param.title="更新插件:";
                    param.message="更新成功！";
                    param.action="remoteupdate";
                    parent.mask.showMessageProgressBar(param);
                    break;
                case  "2":
                    parent.mask.showFrame("/Admin/Plugin/LocalPluginUpdate.aspx?type="+"<%=Request.QueryString["key"] %>&pltype=plugin","更新插件",{width:281,height:55});
                    break;
                case "3":
                     param.title="更新控件:";
                    param.message="更新成功！";
                    param.action="insctr";
                    parent.mask.showMessageProgressBar(param);
                    break;
                case "4":
                    parent.mask.showFrame("/Admin/Plugin/LocalPluginUpdate.aspx?type="+"<%=Request.QueryString["key"] %>&pltype=control","更新插件",{width:281,height:55});
                    break;
            }
            
            return false;
        }    
    </script>

    <div>
        <div id="mycontent">
            <div style="position: absolute; top: -2px; left: -2px; background: #FFFFFF; height: 32px;
                width: 100%; padding: 0; margin: 0;">
            </div>
            <div id="menuTabDiv" class="Tab menuTab">
                <ul class="Tabs">
                    <asp:Label runat="server" ID="MenuTabLabel"></asp:Label>
                </ul>
            </div>
            <div id="rightWrapper">
                <div id="container" style="color: #333333;">
                    <div style="float: right; clear: right; width: 200px; height: 200px; margin: 15px;
                        margin-bottom: 20px !important;">
                        <div style="background: #D54E21; text-align: center; color: White; padding: 5px;
                            cursor: pointer; display: block;" onclick="submitSingleAction()">
                            <b>
                                <%=TitleText %></b></div>
                        <div style="margin-top: 10px; background: #CEE1EF; padding: 5px 5px 3px 5px">
                            <b>信息</b></div>
                        <div style="background: #EAF3FA; line-height: 200%; padding: 5px;">
                            <b>版本：</b><%=Version %><br />
                            <b>作者：</b><%=Author %><br />
                            <b>最后更新：</b><%=Update %><br />
                            <b>兼容到：</b><%=Compatible %><br />
                            <b>下载次数：</b><%=DownLoads %><br />
                            <a href="PluginAdd.aspx" target="_parent" style="color: #1F6FD5;">插件首页>></a>
                        </div>
                    </div>
                    <asp:Literal runat="server" ID="ContentLiteral" Visible="true"></asp:Literal>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
