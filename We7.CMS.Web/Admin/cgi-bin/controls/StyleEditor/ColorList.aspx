<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ColorList.aspx.cs" Inherits="We7.CMS.Web.Admin.ColorList" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>É«²Ê±à¼­</title>
    <link rel="stylesheet" type="text/css" href="styleeditor.css" media="screen" />
    <style type="text/css">
.outerSlideContainer { BORDER-RIGHT: buttonface 1px inset; BORDER-TOP: buttonface 1px inset; MARGIN-LEFT: 10px; BORDER-LEFT: buttonface 1px inset; WIDTH: 340px; BORDER-BOTTOM: buttonface 1px inset; HEIGHT: 20px }
.sliderHandle {position: relative; cursor: default; BORDER-RIGHT: white 2px outset; BORDER-TOP: white 2px outset; FONT-SIZE: 12px; BACKGROUND: buttonface; OVERFLOW: hidden; BORDER-LEFT: white 2px outset; WIDTH: 33px; BORDER-BOTTOM: white 2px outset; HEIGHT: 18px; TEXT-ALIGN: center; Cursor:hand }
.lineContainer { WIDTH: 338px; POSITION: absolute; HEIGHT: 18px }
.line { FILTER: alpha(style=1); OVERFLOW: hidden; WIDTH: 338px; POSITION: relative; HEIGHT: 18px }
#colorBox { BORDER-RIGHT: buttonface 1px inset; BORDER-TOP: buttonface 1px inset; BORDER-LEFT: buttonface 1px inset; WIDTH: 20px; BORDER-BOTTOM: buttonface 1px inset; HEIGHT: 50px; visibility: hidden }
    </style>

    <script type="text/javascript">
var HexCharacters="0123456789ABCDEF";
function HexValue(decimal) {
    return HexCharacters.charAt((decimal>>4)&0xf)+HexCharacters.charAt(decimal&0xf)
}

function DecValue(hexadecimal) {
    return parseInt(hexadecimal.toUpperCase(),16)
}

function setColor(colorString) {
    var hx = document.HexConvert;
    hx.HexRed.value=colorString.substring(0,2)
    hx.HexGreen.value=colorString.substring(2,4)
    hx.HexBlue.value=colorString.substring(4,6)
    hx.DecRed.value=DecValue(hx.HexRed.value)
    hx.DecGreen.value=DecValue(hx.HexGreen.value)
    hx.DecBlue.value=DecValue(hx.HexBlue.value)
}

function DecFixed(decimal) {
    return Math.min(parseFloat(Math.abs(Math.floor(decimal))), 255)
}

function HexFixed(hexadecimal) {
    return HexValue(Math.min(parseFloat(Math.abs(Math.floor(DecValue(hexadecimal)))), 255))
}

function GetColor() {
    var hx = document.HexConvert;
    hx.txtColor.value = "#"+hx.HexRed.value+hx.HexGreen.value+hx.HexBlue.value;
}

function SyncDecimal() {
    var hx = document.HexConvert;
    hx.DecRed.value=DecFixed(hx.DecRed.value);
    hx.HexRed.value=HexValue(hx.DecRed.value);
    hx.DecBlue.value=DecFixed(hx.DecBlue.value);
    hx.HexBlue.value=HexValue(hx.DecBlue.value);
    hx.DecGreen.value=DecFixed(hx.DecGreen.value);
    hx.HexGreen.value=HexValue(hx.DecGreen.value);
    Preview();
    return;
}

function SyncHex() {
    var hx = document.HexConvert;
    hx.HexRed.value = HexFixed(hx.HexRed.value);
    hx.DecRed.value = DecValue(hx.HexRed.value);
    hx.HexGreen.value = HexFixed(hx.HexGreen.value);
    hx.DecGreen.value = DecValue(hx.HexGreen.value);
    hx.HexBlue.value = HexFixed(hx.HexBlue.value);
    hx.DecBlue.value = DecValue(hx.HexBlue.value);
    Preview();
    return
}

function Preview() {
    var hx = document.HexConvert;
    if (navigator.appName != "Netscape") {
        self.frames[0].document.bgColor=hx.HexRed.value+hx.HexGreen.value+hx.HexBlue.value;
    }
    else {
        document.bgColor=hx.HexRed.value+hx.HexGreen.value+hx.HexBlue.value;
    }
    GetColor();
    return;
}
   
var dragobject = null;
var type;
var onchange = "";
var tx;
var ty;

function getReal(el, type, value) {
	temp = el;
	while ((temp != null) && (temp.tagName != "BODY")) {
		if (eval("temp." + type) == value) {
			el = temp;
			return el;
		}
		temp = temp.parentElement;
	}
	return el;
}

function moveme_onmousedown() {
	var tmp = getReal(window.event.srcElement, "className", "sliderHandle");	//Traverse the element tree
	if(tmp.className == "sliderHandle") {
		dragobject = tmp;			//This is a global reference to the current dragging object

		onchange = dragobject.getAttribute("onchange");	//Set the onchange function
		if (onchange == null) onchange = "";
		type = dragobject.getAttribute("type");			//Find the type

		if (type=="y")		//Vertical
			ty = (window.event.clientY - dragobject.style.pixelTop);
		else				//Horizontal
			tx = (window.event.clientX - dragobject.style.pixelLeft);

		window.event.returnValue = false;
		window.event.cancelBubble = true;
	}
	else {
		dragobject = null;	//Not draggable
	}	
}

function moveme_onmouseup() {
    var hx = document.HexConvert;
	if(dragobject) {
		dragobject = null;
	}
    var red   = Math.round(255*redSlider.value);
    var green = Math.round(255*greenSlider.value);
    var blue  = Math.round(255*blueSlider.value);
    hx.DecRed.value=red;
    hx.DecGreen.value=green;
    hx.DecBlue.value=blue;
    SyncDecimal();
}

function moveme_onmousemove() {
	if(dragobject) {
		if (type=="y") {
			if(event.clientY >= 0) {
				if ((event.clientY - ty >= 0) && (event.clientY - ty <= dragobject.parentElement.style.pixelHeight - dragobject.style.pixelHeight)) {
					dragobject.style.top = event.clientY - ty;
				}
				if (event.clientY - ty < 0) {
					dragobject.style.top = "0";
				}
				if (event.clientY - ty > dragobject.parentElement.style.pixelHeight - dragobject.style.pixelHeight - 0) {
					dragobject.style.top = dragobject.parentElement.style.pixelHeight - dragobject.style.pixelHeight;
				}

				dragobject.value = dragobject.style.pixelTop / (dragobject.parentElement.style.pixelHeight - dragobject.style.pixelHeight);
				eval(onchange.replace(/this/g, "dragobject"));
			}
		}
		else {
			if(event.clientX  >= 0) {
				if ((event.clientX  - tx >= 0) && (event.clientX - tx <= dragobject.parentElement.clientWidth - dragobject.style.pixelWidth)) {
					dragobject.style.left = event.clientX - tx;
				}
				if (event.clientX - tx < 0) {
					dragobject.style.left = "0";
				}
				if (event.clientX - tx > dragobject.parentElement.clientWidth - dragobject.style.pixelWidth - 0) {
					dragobject.style.left = dragobject.parentElement.clientWidth - dragobject.style.pixelWidth;
				}

				dragobject.value = dragobject.style.pixelLeft / (dragobject.parentElement.clientWidth - dragobject.style.pixelWidth);
				eval(onchange.replace(/this/g, "dragobject"));
			}
		}
		window.event.returnValue = false;
		window.event.cancelBubble = true;
	} 
}

function setValue(el, val) {
	el.value = val;
	if (el.getAttribute("TYPE") == "x") {
		el.style.left =  val * (el.parentElement.clientWidth - el.style.pixelWidth);
	}
	else{
		el.style.top =  val * (el.parentElement.clientHeight - el.style.pixelHeight);
    }
	eval(el.onchange.replace(/this/g, "el"))
}

document.onmousedown = moveme_onmousedown;
document.onmouseup = moveme_onmouseup;
document.onmousemove = moveme_onmousemove;

function update(el)  {
    var red   = Math.round(255*redSlider.value);
    var green = Math.round(255*greenSlider.value);
    var blue  = Math.round(255*blueSlider.value);
    var color = "RGB(" + red + "," + green + "," + blue + ")";
    redSlider.innerHTML = red;	
    greenSlider.innerHTML = green;
    blueSlider.innerHTML = blue;
    redLeft.style.background = "RGB(" + 0 + "," + green + "," + blue + ")";
    redRight.style.background = "RGB(" + 255 + "," + green + "," + blue + ")";
    greenLeft.style.background = "RGB(" + red + "," + 0 + "," + blue + ")";
    greenRight.style.background = "RGB(" + red + "," + 255 + "," + blue + ")";
    blueLeft.style.background = "RGB(" + red + "," + green + "," + 0 + ")";
    blueRight.style.background = "RGB(" + red + "," + green + "," + 255 + ")";
    Preview();
}

function init() {
    setValue(redSlider, 1);
    setValue(greenSlider, 1);
    setValue(blueSlider, 1);
}
function resetSlider() {
    setValue(redSlider, (window.HexConvert.DecRed.value)/255);
    setValue(greenSlider, (window.HexConvert.DecGreen.value/255));
    setValue(blueSlider, (window.HexConvert.DecBlue.value/255));
}

function CancelMe() {
    window.returnValue = null;
    window.close();
}

function SubmitMe() {
    var hx = document.HexConvert;
    window.returnValue = hx.txtColor.value;
    window.close();
}

    </script>

</head>
<body onload="init()">
    <form id="HexConvert" runat="server">
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
                                Ñ¡Ôñ</asp:HyperLink>
                            <span>| </span>
                            <asp:HyperLink ID="CancelHyperLink" runat="server" NavigateUrl="javascript:CancelMe();">
                                <asp:Image ID="CancelImage" runat="server" ImageUrl="~/admin/Images/icon_cancel.gif" />
                                È¡Ïû</asp:HyperLink>
                            <span>| </span>
                        </div>
                        <table style="height: 101px" cellspacing="0" cellpadding="0" width="10%" border="0">
                            <tr>
                                <td align="center" width="10">
                                    <table style="height: 103" cellspacing="1" cellpadding="1" width="10%" border="0">
                                        <tr>
                                            <td width="10%" height="93">
                                                <table id="table1">
                                                    <tbody>
                                                        <tr>
                                                            <td align="right">
                                                                ºì£º</td>
                                                            <td>
                                                                <div class="outerSlideContainer">
                                                                    <div class="lineContainer" id="redRight" style="background: rgb(255,0,0)">
                                                                        <div class="line" id="redLeft" style="background: rgb(0,0,0)">
                                                                        </div>
                                                                    </div>
                                                                    <div class="sliderHandle" id="redSlider" style="width: 33px; height: 18px" type="x"
                                                                        value="0" onchange="update(this)">
                                                                        0
                                                                    </div>
                                                                </div>
                                                            </td>
                                                            <td valign="middle" align="center" rowspan="3">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                ÂÌ£º</td>
                                                            <td>
                                                                <div class="outerSlideContainer">
                                                                    <div class="lineContainer" id="greenRight" style="background: rgb(0,255,0)">
                                                                        <div class="line" id="greenLeft" style="background: rgb(0,0,0)">
                                                                        </div>
                                                                    </div>
                                                                    <div class="sliderHandle" id="greenSlider" style="width: 33px; height: 18px" type="x"
                                                                        value="0" onchange="update(this)">
                                                                        0
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                À¶£º</td>
                                                            <td>
                                                                <div class="outerSlideContainer">
                                                                    <div class="lineContainer" id="blueRight" style="background: rgb(0,0,255)">
                                                                        <div class="line" id="blueLeft" style="background: rgb(0,0,0)">
                                                                        </div>
                                                                    </div>
                                                                    <div class="sliderHandle" id="blueSlider" style="width: 33px; height: 18px" type="x"
                                                                        value="0" onchange="update(this)">
                                                                        0
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                            <td align="left" height="93">
                                                <table height="39" cellpadding="3" border="0">
                                                    <tr>
                                                        <td align="center">
                                                            <input onblur="SyncHex();resetSlider();" onfocus="HexRed.select()" type="text" maxlength="2"
                                                                size="2" value="FF" name="HexRed">
                                                        </td>
                                                        <td align="center">
                                                            <input onblur="SyncHex();resetSlider();" style="width: 25px" onfocus="HexGreen.select()"
                                                                type="text" maxlength="3" size="2" value="FF" name="HexGreen">
                                                        </td>
                                                        <td align="center">
                                                            <input onblur="SyncHex();resetSlider();" style="width: 25px" onfocus="HexBlue.select()"
                                                                type="text" maxlength="3" size="2" value="FF" name="HexBlue">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="1" rowspan="1">
                                                            <font face="Arial,Helvetica" size="1">ºì </font>
                                                        </td>
                                                        <td valign="top" align="center">
                                                            <font face="Arial,Helvetica" size="1">ÂÌ </font>
                                                        </td>
                                                        <td align="center">
                                                            <font face="Arial,Helvetica" size="1">À¶ </font>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table id="Table2" border="0">
                                                    <tr>
                                                        <td align="center">
                                                            <input onblur="SyncDecimal();resetSlider();" style="width: 28px" onfocus="DecRed.select()"
                                                                type="text" maxlength="3" size="3" value="255" name="DecRed">
                                                        </td>
                                                        <td align="center">
                                                            <input onblur="SyncDecimal();resetSlider();" style="width: 28px" onfocus="DecGreen.select()"
                                                                type="text" maxlength="3" size="3" value="255" name="DecGreen"></td>
                                                        <td align="center">
                                                            <input onblur="SyncDecimal();resetSlider();" style="width: 28px" onfocus="DecBlue.select()"
                                                                type="text" maxlength="3" size="3" value="255" name="DecBlue">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" width="10%">
                                                <table cellspacing="0" cellpadding="0" border="0">
                                                    <tr height="5">
                                                        <td bgcolor="#000000">
                                                            <a href="javascript: setColor('000000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#080808">
                                                            <a href="javascript: setColor('080808');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#101010">
                                                            <a href="javascript: setColor('101010');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#181818">
                                                            <a href="javascript: setColor('181818');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#202020">
                                                            <a href="javascript: setColor('202020');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#282828">
                                                            <a href="javascript: setColor('282828');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#303030">
                                                            <a href="javascript: setColor('303030');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#383838">
                                                            <a href="javascript: setColor('383838');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#404040">
                                                            <a href="javascript: setColor('404040');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#484848">
                                                            <a href="javascript: setColor('484848');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#505050">
                                                            <a href="javascript: setColor('505050');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#585858">
                                                            <a href="javascript: setColor('585858');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#606060">
                                                            <a href="javascript: setColor('606060');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#686868">
                                                            <a href="javascript: setColor('686868');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#707070">
                                                            <a href="javascript: setColor('707070');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#787878">
                                                            <a href="javascript: setColor('787878');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#808080">
                                                            <a href="javascript: setColor('808080');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#888888">
                                                            <a href="javascript: setColor('888888');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#909090">
                                                            <a href="javascript: setColor('909090');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#989898">
                                                            <a href="javascript: setColor('989898');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#a0a0a0">
                                                            <a href="javascript: setColor('A0A0A0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#a8a8a8">
                                                            <a href="javascript: setColor('A8A8A8');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#b0b0b0">
                                                            <a href="javascript: setColor('B0B0B0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#b8b8b8">
                                                            <a href="javascript: setColor('B8B8B8');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#c0c0c0">
                                                            <a href="javascript: setColor('C0C0C0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#c8c8c8">
                                                            <a href="javascript: setColor('C8C8C8');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#d0d0d0">
                                                            <a href="javascript: setColor('D0D0D0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#d8d8d8">
                                                            <a href="javascript: setColor('D8D8D8');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#e0e0e0">
                                                            <a href="javascript: setColor('E0E0E0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#e8e8e8">
                                                            <a href="javascript: setColor('E8E8E8');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#f0f0f0">
                                                            <a href="javascript: setColor('F0F0F0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#f8f8f8">
                                                            <a href="javascript: setColor('F8F8F8');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffffff">
                                                            <a href="javascript: setColor('FFFFFF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                    </tr>
                                                    <tr height="5">
                                                        <td bgcolor="#000000">
                                                            <a href="javascript: setColor('000000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#100000">
                                                            <a href="javascript: setColor('100000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#200000">
                                                            <a href="javascript: setColor('200000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#300000">
                                                            <a href="javascript: setColor('300000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#400000">
                                                            <a href="javascript: setColor('400000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#500000">
                                                            <a href="javascript: setColor('500000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#600000">
                                                            <a href="javascript: setColor('600000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#700000">
                                                            <a href="javascript: setColor('700000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#800000">
                                                            <a href="javascript: setColor('800000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#900000">
                                                            <a href="javascript: setColor('900000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#a00000">
                                                            <a href="javascript: setColor('A00000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#b00000">
                                                            <a href="javascript: setColor('B00000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#c00000">
                                                            <a href="javascript: setColor('C00000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#d00000">
                                                            <a href="javascript: setColor('D00000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#e00000">
                                                            <a href="javascript: setColor('E00000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#f00000">
                                                            <a href="javascript: setColor('F00000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ff0000">
                                                            <a href="javascript: setColor('FF0000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ff1010">
                                                            <a href="javascript: setColor('FF1010');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ff2020">
                                                            <a href="javascript: setColor('FF2020');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ff3030">
                                                            <a href="javascript: setColor('FF3030');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ff4040">
                                                            <a href="javascript: setColor('FF4040');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ff5050">
                                                            <a href="javascript: setColor('FF5050');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ff6060">
                                                            <a href="javascript: setColor('FF6060');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ff7070">
                                                            <a href="javascript: setColor('FF7070');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ff8080">
                                                            <a href="javascript: setColor('FF8080');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ff9090">
                                                            <a href="javascript: setColor('FF9090');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffa0a0">
                                                            <a href="javascript: setColor('FFA0A0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffb0b0">
                                                            <a href="javascript: setColor('FFB0B0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffc0c0">
                                                            <a href="javascript: setColor('FFC0C0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffd0d0">
                                                            <a href="javascript: setColor('FFD0D0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffe0e0">
                                                            <a href="javascript: setColor('FFE0E0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#fff0f0">
                                                            <a href="javascript: setColor('FFF0F0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffffff">
                                                            <a href="javascript: setColor('FFFFFF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                    </tr>
                                                    <tr height="5">
                                                        <td bgcolor="#000000">
                                                            <a href="javascript: setColor('000000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#101000">
                                                            <a href="javascript: setColor('101000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#202000">
                                                            <a href="javascript: setColor('202000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#303000">
                                                            <a href="javascript: setColor('303000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#404000">
                                                            <a href="javascript: setColor('404000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#505000">
                                                            <a href="javascript: setColor('505000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#606000">
                                                            <a href="javascript: setColor('606000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#707000">
                                                            <a href="javascript: setColor('707000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#808000">
                                                            <a href="javascript: setColor('808000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#909000">
                                                            <a href="javascript: setColor('909000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#a0a000">
                                                            <a href="javascript: setColor('A0A000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#b0b000">
                                                            <a href="javascript: setColor('B0B000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#c0c000">
                                                            <a href="javascript: setColor('C0C000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#d0d000">
                                                            <a href="javascript: setColor('D0D000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#e0e000">
                                                            <a href="javascript: setColor('E0E000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#f0f000">
                                                            <a href="javascript: setColor('F0F000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffff00">
                                                            <a href="javascript: setColor('FFFF00');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffff10">
                                                            <a href="javascript: setColor('FFFF10');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffff20">
                                                            <a href="javascript: setColor('FFFF20');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffff30">
                                                            <a href="javascript: setColor('FFFF30');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffff40">
                                                            <a href="javascript: setColor('FFFF40');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffff50">
                                                            <a href="javascript: setColor('FFFF50');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffff60">
                                                            <a href="javascript: setColor('FFFF60');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffff70">
                                                            <a href="javascript: setColor('FFFF70');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffff80">
                                                            <a href="javascript: setColor('FFFF80');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffff90">
                                                            <a href="javascript: setColor('FFFF90');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffffa0">
                                                            <a href="javascript: setColor('FFFFA0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffffb0">
                                                            <a href="javascript: setColor('FFFFB0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffffc0">
                                                            <a href="javascript: setColor('FFFFC0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffffd0">
                                                            <a href="javascript: setColor('FFFFD0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffffe0">
                                                            <a href="javascript: setColor('FFFFE0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#fffff0">
                                                            <a href="javascript: setColor('FFFFF0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffffff">
                                                            <a href="javascript: setColor('FFFFFF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                    </tr>
                                                    <tr height="5">
                                                        <td bgcolor="#000000">
                                                            <a href="javascript: setColor('000000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#001000">
                                                            <a href="javascript: setColor('001000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#002000">
                                                            <a href="javascript: setColor('002000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#003000">
                                                            <a href="javascript: setColor('003000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#004000">
                                                            <a href="javascript: setColor('004000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#005000">
                                                            <a href="javascript: setColor('005000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#006000">
                                                            <a href="javascript: setColor('006000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#007000">
                                                            <a href="javascript: setColor('007000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#008000">
                                                            <a href="javascript: setColor('008000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#009000">
                                                            <a href="javascript: setColor('009000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#00a000">
                                                            <a href="javascript: setColor('00A000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#00b000">
                                                            <a href="javascript: setColor('00B000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#00c000">
                                                            <a href="javascript: setColor('00C000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#00d000">
                                                            <a href="javascript: setColor('00D000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#00e000">
                                                            <a href="javascript: setColor('00E000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#00f000">
                                                            <a href="javascript: setColor('00F000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#00ff00">
                                                            <a href="javascript: setColor('00FF00');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#10ff10">
                                                            <a href="javascript: setColor('10FF10');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#20ff20">
                                                            <a href="javascript: setColor('20FF20');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#30ff30">
                                                            <a href="javascript: setColor('30FF30');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#40ff40">
                                                            <a href="javascript: setColor('40FF40');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#50ff50">
                                                            <a href="javascript: setColor('50FF50');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#60ff60">
                                                            <a href="javascript: setColor('60FF60');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#70ff70">
                                                            <a href="javascript: setColor('70FF70');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#80ff80">
                                                            <a href="javascript: setColor('80FF80');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#90ff90">
                                                            <a href="javascript: setColor('90FF90');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#a0ffa0">
                                                            <a href="javascript: setColor('A0FFA0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#b0ffb0">
                                                            <a href="javascript: setColor('B0FFB0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#c0ffc0">
                                                            <a href="javascript: setColor('C0FFC0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#d0ffd0">
                                                            <a href="javascript: setColor('D0FFD0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#e0ffe0">
                                                            <a href="javascript: setColor('E0FFE0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#f0fff0">
                                                            <a href="javascript: setColor('F0FFF0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffffff">
                                                            <a href="javascript: setColor('FFFFFF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                    </tr>
                                                    <tr height="5">
                                                        <td bgcolor="#000000">
                                                            <a href="javascript: setColor('000000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#001010">
                                                            <a href="javascript: setColor('001010');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#002020">
                                                            <a href="javascript: setColor('002020');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#003030">
                                                            <a href="javascript: setColor('003030');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#004040">
                                                            <a href="javascript: setColor('004040');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#005050">
                                                            <a href="javascript: setColor('005050');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#006060">
                                                            <a href="javascript: setColor('006060');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#007070">
                                                            <a href="javascript: setColor('007070');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#008080">
                                                            <a href="javascript: setColor('008080');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#009090">
                                                            <a href="javascript: setColor('009090');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#00a0a0">
                                                            <a href="javascript: setColor('00A0A0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#00b0b0">
                                                            <a href="javascript: setColor('00B0B0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#00c0c0">
                                                            <a href="javascript: setColor('00C0C0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#00d0d0">
                                                            <a href="javascript: setColor('00D0D0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#00e0e0">
                                                            <a href="javascript: setColor('00E0E0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#00f0f0">
                                                            <a href="javascript: setColor('00F0F0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#00ffff">
                                                            <a href="javascript: setColor('00FFFF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#10ffff">
                                                            <a href="javascript: setColor('10FFFF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#20ffff">
                                                            <a href="javascript: setColor('20FFFF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#30ffff">
                                                            <a href="javascript: setColor('30FFFF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#40ffff">
                                                            <a href="javascript: setColor('40FFFF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#50ffff">
                                                            <a href="javascript: setColor('50FFFF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#60ffff">
                                                            <a href="javascript: setColor('60FFFF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#70ffff">
                                                            <a href="javascript: setColor('70FFFF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#80ffff">
                                                            <a href="javascript: setColor('80FFFF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#90ffff">
                                                            <a href="javascript: setColor('90FFFF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#a0ffff">
                                                            <a href="javascript: setColor('A0FFFF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#b0ffff">
                                                            <a href="javascript: setColor('B0FFFF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#c0ffff">
                                                            <a href="javascript: setColor('C0FFFF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#d0ffff">
                                                            <a href="javascript: setColor('D0FFFF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#e0ffff">
                                                            <a href="javascript: setColor('E0FFFF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#f0ffff">
                                                            <a href="javascript: setColor('F0FFFF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffffff">
                                                            <a href="javascript: setColor('FFFFFF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                    </tr>
                                                    <tr height="5">
                                                        <td bgcolor="#000000">
                                                            <a href="javascript: setColor('000000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#000010">
                                                            <a href="javascript: setColor('000010');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#000020">
                                                            <a href="javascript: setColor('000020');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#000030">
                                                            <a href="javascript: setColor('000030');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#000040">
                                                            <a href="javascript: setColor('000040');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#000050">
                                                            <a href="javascript: setColor('000050');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#000060">
                                                            <a href="javascript: setColor('000060');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#000070">
                                                            <a href="javascript: setColor('000070');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#000080">
                                                            <a href="javascript: setColor('000080');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#000090">
                                                            <a href="javascript: setColor('000090');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#0000a0">
                                                            <a href="javascript: setColor('0000A0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#0000b0">
                                                            <a href="javascript: setColor('0000B0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#0000c0">
                                                            <a href="javascript: setColor('0000C0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#0000d0">
                                                            <a href="javascript: setColor('0000D0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#0000e0">
                                                            <a href="javascript: setColor('0000E0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#0000f0">
                                                            <a href="javascript: setColor('0000F0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#0000ff">
                                                            <a href="javascript: setColor('0000FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#1010ff">
                                                            <a href="javascript: setColor('1010FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#2020ff">
                                                            <a href="javascript: setColor('2020FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#3030ff">
                                                            <a href="javascript: setColor('3030FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#4040ff">
                                                            <a href="javascript: setColor('4040FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#5050ff">
                                                            <a href="javascript: setColor('5050FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#6060ff">
                                                            <a href="javascript: setColor('6060FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#7070ff">
                                                            <a href="javascript: setColor('7070FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#8080ff">
                                                            <a href="javascript: setColor('8080FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#9090ff">
                                                            <a href="javascript: setColor('9090FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#a0a0ff">
                                                            <a href="javascript: setColor('A0A0FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#b0b0ff">
                                                            <a href="javascript: setColor('B0B0FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#c0c0ff">
                                                            <a href="javascript: setColor('C0C0FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#d0d0ff">
                                                            <a href="javascript: setColor('D0D0FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#e0e0ff">
                                                            <a href="javascript: setColor('E0E0FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#f0f0ff">
                                                            <a href="javascript: setColor('F0F0FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffffff">
                                                            <a href="javascript: setColor('FFFFFF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                    </tr>
                                                    <tr height="5">
                                                        <td bgcolor="#000000">
                                                            <a href="javascript: setColor('000000');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#100010">
                                                            <a href="javascript: setColor('100010');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#200020">
                                                            <a href="javascript: setColor('200020');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#300030">
                                                            <a href="javascript: setColor('300030');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#400040">
                                                            <a href="javascript: setColor('400040');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#500050">
                                                            <a href="javascript: setColor('500050');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#600060">
                                                            <a href="javascript: setColor('600060');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#700070">
                                                            <a href="javascript: setColor('700070');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#800080">
                                                            <a href="javascript: setColor('800080');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#900090">
                                                            <a href="javascript: setColor('900090');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#a000a0">
                                                            <a href="javascript: setColor('A000A0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#b000b0">
                                                            <a href="javascript: setColor('B000B0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#c000c0">
                                                            <a href="javascript: setColor('C000C0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#d000d0">
                                                            <a href="javascript: setColor('D000D0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#e000e0">
                                                            <a href="javascript: setColor('E000E0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#f000f0">
                                                            <a href="javascript: setColor('F000F0');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ff00ff">
                                                            <a href="javascript: setColor('FF00FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ff10ff">
                                                            <a href="javascript: setColor('FF10FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ff20ff">
                                                            <a href="javascript: setColor('FF20FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ff30ff">
                                                            <a href="javascript: setColor('FF30FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ff40ff">
                                                            <a href="javascript: setColor('FF40FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ff50ff">
                                                            <a href="javascript: setColor('FF50FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ff60ff">
                                                            <a href="javascript: setColor('FF60FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ff70ff">
                                                            <a href="javascript: setColor('FF70FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ff80ff">
                                                            <a href="javascript: setColor('FF80FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ff90ff">
                                                            <a href="javascript: setColor('FF90FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffa0ff">
                                                            <a href="javascript: setColor('FFA0FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffb0ff">
                                                            <a href="javascript: setColor('FFB0FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffc0ff">
                                                            <a href="javascript: setColor('FFC0FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffd0ff">
                                                            <a href="javascript: setColor('FFD0FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffe0ff">
                                                            <a href="javascript: setColor('FFE0FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#fff0ff">
                                                            <a href="javascript: setColor('FFF0FF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                        <td bgcolor="#ffffff">
                                                            <a href="javascript: setColor('FFFFFF');resetSlider();Preview();">&nbsp;&nbsp;</a><br />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="center">
                                                <p align="left">
                                                    <font style="font-size: 12px" face="ËÎÌå"><strong>Ô¤ÀÀ</strong></font>
                                                    <br />
                                                    <iframe name="PreviewFrame" marginwidth="0" marginheight="0" src="" frameborder="1"
                                                        noresize width="90" scrolling="no" height="35"><font face="ËÎÌå" size="1" color="Black">
                                                            <center>
                                                                <b>²âÊÔ</b>
                                                            </center>
                                                        </font></iframe>
                                                    <br />
                                                    <font face="ËÎÌå" size="3">
                                                        <br />
                                                        <input id="txtColor" type="text" size="11" value="#FFFFFF" name="txtColor">
                                                    </font>
                                                </p>
                                            </td>
                                        </tr>
                                    </table>
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
