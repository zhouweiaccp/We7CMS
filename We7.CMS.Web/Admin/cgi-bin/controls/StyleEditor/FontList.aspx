<%@ Page Language="C#" AutoEventWireup="true" Codebehind="FontList.aspx.cs" Inherits="We7.CMS.Web.Admin.FontList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>字体选择器</title>
    <link rel="stylesheet" type="text/css" href="styleeditor.css" media="screen" />

    <script type="text/javascript">
function TrimString(str) {
    if(typeof(str) != "string") {
	    throw "Invalid type.";
    }
    if(str == null) {
	    return null;
    }
    if(str == "") {
	    return "";
    }
		
    while(str.length > 0 && str.charAt(0) == " ") {
	    str = str.substring(1, str.length);
    }
    while(str.length > 0 && str.charAt(str.length - 1) == " ") {
	    str = str.substring(0, str.length - 1);
    }
    return str;
}

function LoadMe() {
	var fonts = window.dialogArguments;
	if(fonts != null &&　fonts != "") {
		var lbFont = document.all["lbSelectedFont"];
		fontsArray = fonts.split(",");
		for(var i = 0; i < fontsArray.length; i++) {
			var font = TrimString(fontsArray[i]);
			if(font.charAt(0) == "'")
				font = font.substring(1, font.length - 1);
			var opt = document.createElement("option");
			lbFont.options.add(opt);
			opt.innerText  = font;
			opt.value = font;
		}
	}
}

function AddSysFont() {
    
	var lbSysFont = document.all["lbSysFont"];
	var lbSelectedFont = document.all["lbSelectedFont"];
	var opt = document.createElement("option");
	if(lbSysFont.selectedIndex < 0) return;
	
	lbSelectedFont.options.add(opt);
	opt.innerText = lbSysFont.options[lbSysFont.selectedIndex].innerText;
	opt.value = opt.innerText;
	lbSelectedFont.selectedIndex = lbSelectedFont.options.length - 1;
}

function AddCommonFont() {
	var ddlCommonFont = document.all["ddlCommonFont"];
	var lbSelectedFont = document.all["lbSelectedFont"];
	var opt = document.createElement("option");
	if(ddlCommonFont.selectedIndex < 0) return;
	lbSelectedFont.options.add(opt);
	opt.innerText = ddlCommonFont.options[ddlCommonFont.selectedIndex].innerText;
	opt.value = opt.innerText;
}

function AddCustomFont() {
    if(document.all["tbCustomFont"].value.length == 0) return;
	var lbSelectedFont = document.all["lbSelectedFont"];
	var opt = document.createElement("option");
	lbSelectedFont.options.add(opt);
	opt.innerText = document.all["tbCustomFont"].value;
	opt.value = opt.innerText;
}

function MoveUp() {
	var lbSelectedFont = document.all["lbSelectedFont"];
	if(lbSelectedFont.selectedIndex < 0) {
		alert("请先选中一个字体。");
		return;
	}
	if(lbSelectedFont.selectedIndex == 0)
		return;
	
	var curOption = lbSelectedFont.options[lbSelectedFont.selectedIndex];
	var upperOption = lbSelectedFont.options[lbSelectedFont.selectedIndex - 1];
	var innerText = curOption.innerText;
	var value = curOption.value;
	curOption.innerText = upperOption.innerText;
	curOption.value = upperOption.value;
	upperOption.innerText = innerText;
	upperOption.value = value;
	
	lbSelectedFont.selectedIndex--;
}

function MoveDown() {
	var lbSelectedFont = document.all["lbSelectedFont"];
	if(lbSelectedFont.selectedIndex < 0)
	{
		alert("请先选中一个字体。");
		return;
	}
	if(lbSelectedFont.selectedIndex == lbSelectedFont.options.length - 1)
		return;
	
	var curOption = lbSelectedFont.options[lbSelectedFont.selectedIndex];
	var netherOption = lbSelectedFont.options[lbSelectedFont.selectedIndex + 1];
	var innerText = curOption.innerText;
	var value = curOption.value;
	curOption.innerText = netherOption.innerText;
	curOption.value = netherOption.value;
	netherOption.innerText = innerText;
	netherOption.value = value;
	
	lbSelectedFont.selectedIndex++;
}

function RemoveFont() {
	var lbSelectedFont = document.all["lbSelectedFont"];
	if(lbSelectedFont.selectedIndex < 0)
	{
		alert("请先选中一个字体。");
		return;
	}
	var selectedIndex = lbSelectedFont.selectedIndex;
	lbSelectedFont.options.remove(lbSelectedFont.selectedIndex);
	if(selectedIndex == 0 && lbSelectedFont.options.length > 0)
		lbSelectedFont.selectedIndex = 0;
	else
		lbSelectedFont.selectedIndex = selectedIndex - 1;
}

function SubmitMe() {
	var lbSelectedFont = document.all["lbSelectedFont"];
	var fonts = "";
	for(var i = 0; i < lbSelectedFont.length; i++) {
		var font = lbSelectedFont.options[i].innerText;
		if(font.indexOf(" ") >= 0)
			font = "'" + font + "'";
		fonts += font + ", ";
	}
	if(fonts.length > 0) {
		fonts = fonts.substring(0, fonts.length - 2);
    }
	window.returnValue = fonts;
	window.close();
}

function CancelMe() {
	window.returnValue = window.dialogArguments;
	window.close();
}
    </script>

</head>
<body onload="LoadMe();" onkeypress="if(event.keyCode == 13) SubmitMe();else if(event.keyCode == 27) CancelMe();">
    <form id="mainForm" runat="server">
        <div id="wrap">
            <div id="header">
                <div id="site-name">
                    </div>
            </div>
            <div id="content-wrap">
                <div id="content">
                    <div id="breadcrumb">
                        <div class="toolbar">
                            <asp:HyperLink ID="SelectHyperLink" runat="server" NavigateUrl="javascript:SubmitMe();">
                                <asp:Image ID="SelectImage" runat="server" ImageUrl="~/admin/Images/icon_ok.gif" />
                                选择</asp:HyperLink>
                            <span>| </span>
                            <asp:HyperLink ID="CancelHyperLink" runat="server" NavigateUrl="javascript:CancelMe();">
                                <asp:Image ID="CancelImage" runat="server" ImageUrl="~/admin/Images/icon_cancel.gif" />
                                取消</asp:HyperLink>
                            <span>| </span>
                        </div>
                        <table>
                            <tr>
                                <td valign="top">
                                    <div>
                                        <span>已安装的字体</span>
                                    </div>
                                    <select id="lbSysFont" runat="server" size="5" style="width: 200px">
                                        <option selected>Agency FB</option>
                                        <option>Algerian</option>
                                        <option>Arial</option>
                                        <option>Arial Black</option>
                                    </select>
                                </td>
                                <td>
                                    <input type="button" value=" &nbsp;> > &nbsp; " id="btnAddSysFont" onclick="AddSysFont();" />
                                </td>
                                <td rowspan="3" valign="top">
                                    <div>
                                        <span>选定的字体</span>
                                    </div>
                                    <select id="lbSelectedFont" size="12" style="width: 200px">
                                    </select>
                                </td>
                                <td rowspan="2" valign="top">
                                    <br />
                                    <img style="cursor: hand;" alt="上移" src="images/font_up.GIF" id="imgMoveUp" onclick="MoveUp();" /><br />
                                    <br />
                                    <img alt="下移" src="images/font_down.GIF" id="imgMoveDown" onclick="MoveDown();" /><br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <span>一般字体</span>
                                    </div>
                                    <select id="ddlCommonFont" size="1" style="width: 200px">
                                        <option selected>Monospace</option>
                                        <option>Serif</option>
                                        <option>Sans-Serif</option>
                                        <option>Cursive</option>
                                        <option>Fantasy</option>
                                    </select>
                                </td>
                                <td>
                                    <div>
                                        <span>&nbsp;</span>
                                    </div>
                                    <input type="button" value=" &nbsp;> > &nbsp; " id="btnAddCommonFont" onclick="AddCommonFont();" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div>
                                        <span>自定义字体</span>
                                    </div>
                                    <input type="text" size="22" id="tbCustomFont" style="width: 200px" /></td>
                                <td>
                                    <div>
                                        <span>&nbsp;</span>
                                    </div>
                                    <input type="button" value=" &nbsp;> > &nbsp; " id="btnAddCustomFont" onclick="AddCustomFont();" />
                                </td>
                                <td>
                                    <img id="imgRemove" alt="移除" src="images/font_remove.ico" width="16" onclick="RemoveFont();" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
