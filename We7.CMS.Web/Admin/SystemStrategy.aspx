<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SystemStrategy.aspx.cs" Inherits="We7.CMS.Web.Admin.SystemStrategy" %>
<%@ Register src="controls/StrategySet.ascx" tagname="StrategySet" tagprefix="WEC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
        <link rel="shortcut icon" href="/favicon.ico" />
        <link rel="stylesheet" type="text/css" href="/Admin/theme/classic/css/main.css" media="screen" />
    <link rel="stylesheet" type="text/css" href="/Admin/theme/classic/css/article.css" media="screen" />
</head>
<body class="we7-admin">
    <form id="form1" runat="server">
    <div> 
        <WEC:StrategySet ID="ucStrtgy" runat="server" Mode="CONVENTION" />
    </div>
    </form>
</body>
</html>
