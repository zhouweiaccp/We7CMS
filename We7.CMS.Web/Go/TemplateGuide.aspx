<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TemplateGuide.aspx.cs" Inherits="We7.CMS.Web.TemplateGuide" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>We7温馨提示</title>
      <style type="text/css">
        BODY
        {
            color: #333;
            background-color: #F9FEE8;
        }
        #main{
            position:absolute;
            top:50%;
            left:50%;
            width:620px;
            height:400px;
            margin-top:-150px;
            margin-left:-300px;
            }
        .report
        {
        	background-color:#FFFFCC;display:inline; font-size:12px;
            line-height:1.2;
            padding:6px 10px 4px;
        }
        .action{font-size:29px; }
        a:hover, a:active, a:focus {
        color:#ff0000;
        text-decoration:underline;
        }
        </style>
    </head>
    <body>
    <div id="main">  
    <table style="width:100%">
    <tr>
    <td style="vertical-align:top; padding:5px; text-align:center">
            <p class="report"><asp:Label ID="TitleLabel" runat="server" Text=""></asp:Label></p>
             <p class="action"><asp:Literal   ID="ActionLiteral" runat="server" Text=""></asp:Literal></p>
            <p class="help"><asp:Literal   ID="HelpLiteral" runat="server" Text=""></asp:Literal></p>
    </td>
    </tr>
    <tr>
    <td  style="border-top:dotted 1px #ccc; font-size:14px; text-align:center; padding:15px ">
    <a href="javascript:history.back();">返回上一页</a> | <a href="/">返回首页</a> |  <a href="/admin/">返回后台</a> | <a href="http://we7.cn:8443/trac/newticket" target="_blank">报告Bug</a>  | <a href="http://help.we7.cn" target="_blank">帮助中心</a>
    </td>
    </tr>
    </table>
    </div>
</body>
</html>
