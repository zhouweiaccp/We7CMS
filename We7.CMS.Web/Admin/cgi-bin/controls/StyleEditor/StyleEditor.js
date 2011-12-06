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


var selectedIndex = -1;
var styleCollection;
var borderObj;

//============================================== 选项卡 ==========================================================
function InitTabs() {
	tabIds = new Array("tabFont", "tabBg", "tabText", "tabPos", "tabLayout", "tabEdge", "tabList", "tabFilter", "tabOther");
	selectedIndex = 0;
	SwitchTab("tabFont");
}

function GetTabIndex(tabId) {
	for(var i = 0; i < tabIds.length; i++)
	{
		if(tabIds[i] == tabId)
			return i;
	}
	return -1;
}

function RecordSelectedIndex(tabId) {
	selectedIndex = GetTabIndex(tabId);
}

function GetSelectedIndex() {
	return selectedIndex;
}

function SwitchTab(tabId) {
	RecordSelectedIndex(tabId);
	for(var i = 0; i < tabIds.length; i++)
	{
		document.all[tabIds[i]].className = "InActive";
		var contentStyle = document.all[tabIds[i] + "Content"].style;
		contentStyle.visibility = "hidden";
		contentStyle.top = "8px";
	}
	document.all[tabId].className = "Active";
	var contentStyle = document.all[tabId + "Content"].style;
	contentStyle.visibility = "visible";
	contentStyle.top = "8px";
	
	switch(tabId)
	{
		case "tabFont":
		case "tabBg":
		case "tabText":
		case "tabFilter":
			ShowTextExample();
			break;
		case "tabPos":
		case "tabLayout":
		case "tabOther":
			document.all["tdExample"].innerHTML = "";
			document.all["tdExample"].style.backgroundColor = "silver";
			break;
		case "tabEdge":
			edg_ShowExample();
			break;
		case "tabList":
			lst_ShowExample();
			break;
	}
}

function HighlightTab(tabId) {
	try {
		for(var i = 0; i < tabIds.length; i++) {
			document.all[tabIds[i]].className = "InActive";
		}
		document.all[tabIds[GetSelectedIndex()]].className = "Active";
		document.all[tabId].className = "Highlight";
	}catch(e){}
}

function UnHighlightTab(tabId) {
	try {
		var curIndex = GetTabIndex(tabId);
		if(curIndex == GetSelectedIndex())
			document.all[tabId].className = "Active";
		else
			document.all[tabId].className = "InActive";
	}catch(e){}
}

//================================================= 窗体 ================================================
function GetStyleText() {
	var style = font_GetStyleString();
	style += bg_GetStyleString();
	style += txt_GetStyleString();
	style += pos_GetStyleString();
	style += lay_GetStyleString();
	style += edg_GetStyleString();
	style += lst_GetStyleString();
	style += flt_GetStyleString();
	style += oth_GetStyleString();
	
	if(style.length > 0) {
		style = style.substring(0, style.length - 1);
    }
	return style;
}

function LoadStyle(v) {
	InitTabs();
	ParseStyleString(v);
	font_LoadMe();
	bg_LoadMe();
	txt_LoadMe();
	pos_LoadMe();
	lay_LoadMe();
	edg_LoadMe();
	lst_LoadMe();
	flt_LoadMe();
	oth_LoadMe();
	ShowTextExample();
}

function ParseStyleString(style) {
	styleCollection = new Array();
	
	if(style == null || style == "")
		return;				
	
	var styleArray = style.split(";");
	for(var i = 0; i < styleArray.length; i++)
	{
		var nameValue = TrimString(styleArray[i]).split(":");
		var name = TrimString(nameValue[0]);
		var value = TrimString(nameValue[1]);
		if(nameValue.length > 2)
			value = TrimString(styleArray[i].substring(styleArray[i].indexOf(":") + 1, styleArray[i].length));
		styleCollection.push(new Array(name,value));
	}
}

function GetStyleValue(styleName)
{
	for(var i = 0; i < styleCollection.length; i++)
	{
		var styleItem = styleCollection[i];
		var name = styleItem[0];
		var value = styleItem[1];
		if(name.toLowerCase() == styleName.toLowerCase())
			return value;
	}
	return "";
}
function SetStyleValue(name, value)
{
	for(var i = 0; i < styleCollection.length; i++)
	{
		var styleItem = styleCollection[i];
		if(name.toLowerCase() == styleItem[0].toLowerCase())
		{
			styleItem[1] = value;
			return;
		}
	}
	styleCollection.push(new Array(name, value));
}
function SetListBoxIndex(lisbBoxId, value, byValue)
{
	var lb = document.all[lisbBoxId];
	lb.selectedIndex = -1;
	for(var i = 0; i < lb.options.length; i++)
	{
		var item = byValue ? lb.options[i].value : lb.options[i].innerText;
		if(item == value)
		{
			lb.selectedIndex = i;
			return;
		}
	}
}
function GetListBoxIndex(listBoxId, value, byValue)
{
	var options = document.all[listBoxId].options;
	value = value.toLowerCase();
	for(var i = 0; i < options.length; i++)
	{
		var item = byValue ? options[i].value : options[i].innerText;
		if(item.toLowerCase() == value)
			return i;
	}
	return -1;
}
function GetListBoxOption(listBoxId, value, byValue)
{
	var index = GetListBoxIndex(listBoxId, value, byValue);
	if(index >= 0)
		return document.all[listBoxId].options[index];
	else
		return null;
}

function ParseSizeString(sizeString)
{ // 返回 sizeObj，Type 属性表明 sizeString 的类型：0 表示“特定”；1 表示“绝对”；2 表示“相对”
	var sizeObj = new Object();
	if(sizeString == null || sizeString == "")
	{
		sizeObj.Value = "";
		sizeObj.Unit = "";
		sizeObj.Type = 0;
		return sizeObj;
	}
	
	sizeString = sizeString.toLowerCase();
	
	if(sizeString.length == 1)
	{
		try{parseInt(sizeString)}catch(e){sizeString = "";}
		sizeObj.Value = sizeString;
		sizeObj.Unit = "";
		sizeObj.Type = 0;
		return sizeObj;
	}
		
	ParseCustomSize(sizeString, sizeObj);
	

	if(sizeObj.Type >= 0)
		return sizeObj;
		
	var absValues = new Array("xx-small", "x-small", "small", "medium", "large", "x-large", "xx-large");
	for(var i = 0; i < absValues.length; i++)
	{
		var absValue = absValues[i];
		if(sizeString == absValue)
		{
			sizeObj.Value = absValue;
			sizeObj.Unit = "";
			sizeObj.Type = 1;
			return sizeObj;
		}
	}
	
	var revValues = new Array("smaller", "larger");
	for(var i = 0; i < revValues.length; i++)
	{
		var revValue = revValues[i];
		if(sizeString == revValue)
		{
			sizeObj.Value = revValue;
			sizeObj.Unit = "";
			sizeObj.Type = 2;
			return sizeObj;
		}
	}
	
	// sizeString 不是以上三种类型，而是一个无效值。
	sizeObj.Value = "";
	sizeObj.Unit = "";
	sizeObj.Type = 0;
	return sizeObj;
}
function ParseCustomSize(sizeString, sizeObj)
{
	
	var sizeUnit = new Array("pt","pc","mm","cm","in","em","ex","%","px");
	for(var i = 0; i < sizeUnit.length; i++)
	{
		var unit = sizeUnit[i];
		if(sizeString.substring(sizeString.length - unit.length) == unit)
		{
			sizeObj.Value = sizeString.substring(0, sizeString.length - unit.length);
			sizeObj.Unit = unit;
			sizeObj.Type = 0;
			if(isNaN(parseInt(sizeObj.Value)))
			{
				sizeObj.Value = "";
				sizeObj.Type = -1;
			}
			return;
		}
	}
	
	sizeObj.Type = -1;
}
function GetStyleItemFromControl(controlId, styleName,byValue)
{
	var control = document.all[controlId];
	var styleValue = "";
	switch(control.tagName.toLowerCase())
	{
		case "input":
			if(control.type.toLowerCase() == "text")
			styleValue = control.value;
			break;
		case "select":
			if(control.options.length > 0 && control.selectedIndex >= 0)
			{
				var opt = control.options[control.selectedIndex];
				styleValue = byValue ? opt.value : opt.innerText;
			}
			break;
		default:
			throw "传入的控件类型 \"" + control.tagName.toLowerCase() + "\"不对。";
	}
	if(styleValue.length > 0)
		return styleName + ":" + styleValue + ";";
	else
		return "";
}
function ShowTextExample()
{
	var style = font_GetStyleString();
	style += bg_GetStyleString();
	style += txt_GetStyleString();
	style += flt_GetStyleString();
	document.all["tdExample"].style.cssText = style;
	document.all["tdExample"].innerHTML = 
	"<div style=\"" + style + "\">示例文本。 Example Text.</div>";
}
// 根据三个控件组合成的 size 选择功能来获取 size 的值。
function GetSizeString(ddlRegularSizeId, tbSizeId, ddlSizeUnitId)
{
	var size = "";
	var isCustom = true;
	if(ddlRegularSizeId != null)
	{
		var ddl = document.all[ddlRegularSizeId];
		if(ddl.selectedIndex >= 0)
		{
			var ddlValue = ddl.options[ddl.selectedIndex].value;
			isCustom = (ddlValue == "custom");
			if(ddlValue.length > 0 && !isCustom)
				size = ddlValue;
		}
	}
	
	if(isCustom)
	{
		var tbValue = document.all[tbSizeId].value;
		if(tbValue.length > 0)
		{
			var ddlUnit = document.all[ddlSizeUnitId];
			if(ddlUnit.selectedIndex >= 0)
				size = tbValue + ddlUnit.options[ddlUnit.selectedIndex].innerText;
		}
	}
	
	return size;
}

//=============================================== 字体 ==================================================

function font_OpenSelectFontWindow()
{
	var fonts = window.showModalDialog("FontList.aspx", document.all["font_tbFontFamily"].value, 
		"dialogHeight:480px;dialogWidth:640px;center:yes;resizable:no;scroll:no;status:no;help:no");
	
	document.all["font_tbFontFamily"].value = fonts;
	ShowTextExample();
}
// body.onload
function font_LoadMe()
{
	// 字体名称
	document.all["font_tbFontFamily"].value = GetStyleValue("font-family");
	
	// 颜色
	document.all["font_tbColor"].value = GetStyleValue("color");
	// 斜体
	SetListBoxIndex("font_ddlFontStyle", GetStyleValue("font-style"), true);
	// 小写字母
	SetListBoxIndex("font_ddlFontVariant", GetStyleValue("font-variant"), true);
	
	// 大小
	var fontSizeObj = ParseSizeString(GetStyleValue("font-size"));
	font_ChangeSizeType(fontSizeObj.Type);
	document.all["font_ddlFontUnit"].selectedIndex = -1;
	document.all["font_ddlAbsFontSize"].selectedIndex = -1;
	document.all["font_ddlRevFontSize"].selectedIndex = -1;
	switch(fontSizeObj.Type)
	{
		case 0:
			document.all["font_tbFontSize"].value = fontSizeObj.Value;
			SetListBoxIndex("font_ddlFontUnit", fontSizeObj.Unit, true);
			break;
		case 1:
			SetListBoxIndex("font_ddlAbsFontSize", fontSizeObj.Value, true);
			break;
		case 2:
			SetListBoxIndex("font_ddlRevFontSize", fontSizeObj.Value, true);
			break;
	}
	
	// 效果
	var txtDec = GetStyleValue("text-decoration");
	if(txtDec != "")
	{
		var txtDecArray = txtDec.toLowerCase().split(" ");
		for(var i = 0; i < txtDecArray.length; i++)
		{
			if(txtDecArray[i] == "none")
			{
				document.all["font_cbTxtDec_None"].checked = true;
				font_cbTxtDec_None_onclick(true);
			}
			else if(txtDecArray[i] == "underline")
				document.all["font_cbTxtDec_Underline"].checked = true;
			else if(txtDecArray[i] == "line-through")
				document.all["font_cbTxtDec_LineThrough"].checked = true;
			else if(txtDecArray[i] == "overline")
				document.all["font_cbTxtDec_Overline"].checked = true;
		}
	}
	
	// 粗体
	var fontWeight = GetStyleValue("font-weight");
	font_ChangeFontWeightType(font_ParseFontWeight(fontWeight));
	SetListBoxIndex("font_ddlAbsFontWeight", fontWeight, true);
	SetListBoxIndex("font_ddlRevFontWeight", fontWeight, true);
	
	// 大写
	SetListBoxIndex("font_ddlTxtTrans", GetStyleValue("text-transform"), true);
}
function font_ChangeSizeType(type)
{
	document.all["font_rbFontSize" + type].checked = true;
	document.all["font_tbFontSize"].disabled = (type != 0);
	document.all["font_ddlFontUnit"].disabled = (type != 0);
	document.all["font_ddlAbsFontSize"].disabled = (type != 1);
	document.all["font_ddlRevFontSize"].disabled = (type != 2);
	ShowTextExample();
}
function font_cbTxtDec_None_onclick(checked)
{
	document.all["font_cbTxtDec_Underline"].disabled = checked;
	document.all["font_cbTxtDec_LineThrough"].disabled = checked;
	document.all["font_cbTxtDec_Overline"].disabled = checked;
	ShowTextExample();
}
function font_ChangeFontWeightType(type)
{
	document.all["font_rbFontWeight" + type].checked = true;
	document.all["font_ddlAbsFontWeight"].disabled = (type != 0);
	document.all["font_ddlRevFontWeight"].disabled = (type != 1);
	ShowTextExample();
}
function font_ParseFontWeight(value)
{
	// 返回值表示 font-weight 的类型。0 代表“绝对”，1 代表“相对”
	if(value == null || value == "")
		return 0;
	
	var type = 0;
	switch(value.toLowerCase())
	{
		case "normal":
		case "bold":
			return 0;
		case "lighter":
		case "bolder":
			return 1;
	}
	
	return 0;
}
function font_GetStyleString()
{
	var cssText = "";
	cssText += GetStyleItemFromControl("font_tbFontFamily", "font-family");
	cssText += GetStyleItemFromControl("font_tbColor","color");
	cssText += GetStyleItemFromControl("font_ddlFontStyle", "font-style", true);
	cssText += GetStyleItemFromControl("font_ddlFontVariant","font-variant", true);
	
	var fontSize = "";
	if(document.all["font_rbFontSize0"].checked)
		fontSize = GetSizeString(null, "font_tbFontSize", "font_ddlFontUnit");
	else if(document.all["font_rbFontSize1"].checked)
	{
		var ddl = document.all["font_ddlAbsFontSize"];
		if(ddl.selectedIndex >= 0)
			fontSize = ddl.options[ddl.selectedIndex].value;
	}
	else if(document.all["font_rbFontSize2"].checked)
	{
		var ddl = document.all["font_ddlRevFontSize"];
		if(ddl.selectedIndex >= 0)
			fontSize = ddl.options[ddl.selectedIndex].value;
	}
	if(fontSize.length > 0)
		cssText += "font-size:" + fontSize + ";";
	
	if(document.all["font_cbTxtDec_None"].checked)
		cssText += "text-decoration:none;";
	else
	{
		var txtDec = "";
		if(document.all["font_cbTxtDec_Underline"].checked)
			txtDec = "underline";
		if(document.all["font_cbTxtDec_LineThrough"].checked)
			txtDec += " line-through";
		if(document.all["font_cbTxtDec_Overline"].checked)
			txtDec += " overline";
		txtDec = TrimString(txtDec);
		if(txtDec.length > 0)
			cssText += "text-decoration:" + txtDec + ";";
	}
	
	if(document.all["font_rbFontWeight0"].checked)
		cssText += GetStyleItemFromControl("font_ddlAbsFontWeight", "font-weight", true);
	else if(document.all["font_rbFontWeight1"].checked)
		cssText += GetStyleItemFromControl("font_ddlRevFontWeight", "font-weight", true);
	cssText += GetStyleItemFromControl("font_ddlTxtTrans","text-transform", true);
	
	return cssText;
}

//=============================================== 背景 ==================================================
function bg_LoadMe()
{
	// 背景色
	var bgColor = GetStyleValue("background-color");
	if(bgColor == "transparent")
	{
		document.all["bg_cbBgTrans"].checked = true;
		bg_SetBgColorTransparent(true);
	}
	else
	{
		bg_SetBgColorTransparent(false);
		document.all["bg_tbBgColor"].value = bgColor;
	}
	
	// 图像
	var bgImage = GetStyleValue("background-image");
	if(bgImage != null)
	{
		if(bgImage.substring(0, 3).toLowerCase() == "url")
		{
			var left = bgImage.indexOf("(") + 1;
			var right = bgImage.indexOf(")");
			document.all["bg_tbBgImage"].value = bgImage.substring(left, right);
			bg_tbBgImage_onchange();
		}
		// 不使用背景图像
		else if(bgImage == "none")
		{
			document.all["bg_cbNoneImage"].checked = true;
			bg_cbNoneImage_onclick();
		}
	}
	// 平铺
	SetListBoxIndex("bg_ddlRepeat", GetStyleValue("background-repeat"), true);
	// 滚动
	SetListBoxIndex("bg_ddlAttach", GetStyleValue("background-attachment"), true);
	
	// 位置
	bg_LoadPos();
}
function bg_SetBgColorTransparent(trans)
{
	document.all["bg_tbBgColor"].disabled = trans;
	document.all["bg_btnSelectColor"].disabled = trans;
}
function bg_BgTrans_change()
{
	bg_SetBgColorTransparent(document.all["bg_cbBgTrans"].checked );
}

function bg_LoadPos()
{
	var pos = GetStyleValue("background-position").toLowerCase();
	var posX, posY;
	if(pos != "")
	{
		var posArray = pos.split(" ");
		posX = posArray[0];
		if(posArray.length > 1)
			posY = posArray[1];
		else
			posY = "";
	}
	else
	{
		posX = GetStyleValue("background-position-x").toLowerCase();
		posY = GetStyleValue("background-position-y").toLowerCase();
	}
	
	var sizeObj = new Object();
	ParseCustomSize(posX, sizeObj);
	if(sizeObj.Type == 0)
	{
		SetListBoxIndex("bg_ddlPosX", "custom", true);
		document.all["bg_tbPosX"].value = sizeObj.Value;
		SetListBoxIndex("bg_ddlPosXUnit", sizeObj.Unit, false);
	}
	if(sizeObj.Type < 0)
	{
		SetListBoxIndex("bg_ddlPosX", posX, true);
		bg_ddlPosX_onchange();
	}
	
	ParseCustomSize(posY, sizeObj);
	if(sizeObj.Type == 0)
	{
		SetListBoxIndex("bg_ddlPosY", "custom", true);
		document.all["bg_tbPosY"].value = sizeObj.Value;
		SetListBoxIndex("bg_ddlPosYUnit", sizeObj.Unit, false);
	}
	if(sizeObj.Type < 0)
	{
		SetListBoxIndex("bg_ddlPosY", posY, true);
		bg_ddlPosY_onchange();
	}

}
function bg_ddlPosX_onchange()
{
	var ddlPoxX = document.all["bg_ddlPosX"];
	var isCustom = (ddlPoxX.options[ddlPoxX.selectedIndex].value == "custom");
	document.all["bg_tbPosX"].disabled = !isCustom;
	document.all["bg_ddlPosXUnit"].disabled = !isCustom;
	ShowTextExample();
}
function bg_ddlPosY_onchange()
{
	var ddlPoxY = document.all["bg_ddlPosY"];
	var isCustom = (ddlPoxY.options[ddlPoxY.selectedIndex].value == "custom");
	document.all["bg_tbPosY"].disabled = !isCustom;
	document.all["bg_ddlPosYUnit"].disabled = !isCustom;
	ShowTextExample();
}
function bg_tbBgImage_onchange()
{
	if(document.all["bg_tbBgImage"].value == "undefined") document.all["bg_tbBgImage"].value="";
	bg_DisableBgImage(document.all["bg_tbBgImage"].value == "");
	ShowTextExample();
}
function bg_DisableBgImage(disable)
{
	document.all["bg_ddlRepeat"].disabled = disable;
	document.all["bg_ddlAttach"].disabled = disable;
	document.all["bg_ddlPosX"].disabled = disable;
	document.all["bg_ddlPosY"].disabled = disable;
	document.all["bg_tbPosX"].disabled = disable;
	document.all["bg_tbPosY"].disabled = disable;
	document.all["bg_ddlPosXUnit"].disabled = disable;
	document.all["bg_ddlPosYUnit"].disabled = disable;
}
function bg_cbNoneImage_onclick()
{
	var checked = document.all["bg_cbNoneImage"].checked;
	document.all["bg_tbBgImage"].disabled = checked;
	if(checked)
		bg_DisableBgImage(true);
	else
		bg_tbBgImage_onchange();
	ShowTextExample();
}
function bg_GetStyleString()
{
	var cssText = "";
	if(document.all["bg_cbBgTrans"].checked)
		cssText += "background-color:transparent;";
	else
		cssText += GetStyleItemFromControl("bg_tbBgColor", "background-color");
	
	if(document.all["bg_cbNoneImage"].checked)
		cssText += "background-image:none;";
	else
	{
		if(document.all["bg_tbBgImage"].value == "undefined") document.all["bg_tbBgImage"].value="";
		var bgImage = document.all["bg_tbBgImage"].value;
		if(bgImage.length > 0 )
			cssText += "background-image:url(" + bgImage + ");";
		cssText += GetStyleItemFromControl("bg_ddlRepeat", "background-repeat", true);
		cssText += GetStyleItemFromControl("bg_ddlAttach", "background-attachment", true);
		var posX = GetSizeString("bg_ddlPosX", "bg_tbPosX", "bg_ddlPosXUnit");
		var posY = GetSizeString("bg_ddlPosY", "bg_tbPosY", "bg_ddlPosYUnit");
		if(posX.length > 0 || posY.length > 0)
		{
			if(posX.length > 0 && posY.length > 0)
				cssText += "background-position:" + posX + " " + posY + ";";
			else if(posX.length > 0)
				cssText += "background-position-x:" + posX + ";";
			else if(posY.length > 0)
				cssText += "background-position-y:" + posY + ";";
		}
	}
	
	return cssText;
}



//=============================================== 文本 ==================================================
function txt_LoadMe()
{
	// 水平对齐
	SetListBoxIndex("txt_ddlAlign", GetStyleValue("text-align"), true);
	txt_ddlAlign_onchange();
	// 垂直对齐
	SetListBoxIndex("txt_ddlVAlign", GetStyleValue("vertical-align"), true);
	// 调整
	SetListBoxIndex("txt_ddlJustify", GetStyleValue("text-justify"), true);
	
	// 字母间距
	var sizeObj = new Object();
	var letSpa = GetStyleValue("letter-spacing");
	if(letSpa != "")
	{
		if(letSpa == "normal")
		{
			SetListBoxIndex("txt_ddlLetSpa", "normal", true);
		}
		else
		{
			ParseCustomSize(letSpa, sizeObj);
			if(sizeObj.Type == 0)
			{
				SetListBoxIndex("txt_ddlLetSpa", "custom", true);
				document.all["txt_tbLetSpa"].value = sizeObj.Value;
				SetListBoxIndex("txt_ddlLetSpaUnit", sizeObj.Unit, false);
			}
		}
		txt_ddlLetSpa_onchange();
	}
	
	// 行间距
	var lineHeight = GetStyleValue("line-height");
	if(lineHeight != "")
	{
		if(lineHeight == "normal")
		{
			SetListBoxIndex("txt_ddlLineHeight", "normal", true);
		}
		else
		{
			ParseCustomSize(lineHeight, sizeObj);
			if(sizeObj.Type == 0)
			{
				SetListBoxIndex("txt_ddlLineHeight", "custom", true);
				document.all["txt_tbLineHeight"].value = sizeObj.Value;
				SetListBoxIndex("txt_ddlLineHeightUnit", sizeObj.Unit, false);
			}
		}
		txt_ddlLineHeight_onchange();
	}
	
	// 缩进
	ParseCustomSize(GetStyleValue("text-indent"), sizeObj);
	if(sizeObj.Type == 0)
	{
		document.all["txt_tbIndent"].value = sizeObj.Value;
		SetListBoxIndex("txt_ddlIndentUnit", sizeObj.Unit, false);
	}
	// 文本方向
	SetListBoxIndex("txt_ddlDirection", GetStyleValue("direction"), true);
}
function txt_ddlAlign_onchange()
{
	var ddlAlign = document.all["txt_ddlAlign"];
	var item = ddlAlign.options[ddlAlign.selectedIndex].value;
	document.all["txt_ddlJustify"].disabled = (item != "justify");
	ShowTextExample();
}
function txt_ddlLetSpa_onchange()
{
	var ddlLetSpa = document.all["txt_ddlLetSpa"];
	var item = ddlLetSpa.options[ddlLetSpa.selectedIndex].value;
	var disabled = (item != "custom");
	document.all["txt_tbLetSpa"].disabled = disabled;
	document.all["txt_ddlLetSpaUnit"].disabled = disabled;
	ShowTextExample();
}
function txt_ddlLineHeight_onchange()
{
	var ddlLineHeight = document.all["txt_ddlLineHeight"];
	var item = ddlLineHeight.options[ddlLineHeight.selectedIndex].value;
	var disabled = (item != "custom");
	document.all["txt_tbLineHeight"].disabled = disabled;
	document.all["txt_ddlLineHeightUnit"].disabled = disabled;
	ShowTextExample();
}
function txt_GetStyleString()
{
	var cssText = "";
	
	var textAlign = GetStyleItemFromControl("txt_ddlAlign", "text-align", true);
	cssText += textAlign;
	if(textAlign.length > 0)
		cssText += GetStyleItemFromControl("txt_ddlJustify", "text-justify", true);
	
	cssText += GetStyleItemFromControl("txt_ddlVAlign", "vertical-align", true);
	var letterSpacing = GetSizeString("txt_ddlLetSpa","txt_tbLetSpa", "txt_ddlLetSpaUnit");
	if(letterSpacing.length > 0)
		cssText += "letter-spacing:" + letterSpacing + ";";
	
	var lineHeight = GetSizeString("txt_ddlLineHeight", "txt_tbLineHeight", "txt_ddlLineHeightUnit");
	if(lineHeight.length > 0)
		cssText += "line-height:" + lineHeight + ";";
	
	var textIndent = GetSizeString(null, "txt_tbIndent", "txt_ddlIndentUnit");
	if(textIndent.length > 0)
		cssText += "text-indent:" + textIndent + ";";
	
	cssText += GetStyleItemFromControl("txt_ddlDirection", "direction", true);
	return cssText;
}
//=============================================== 位置 ==================================================
function pos_LoadMe()
{
	SetListBoxIndex("pos_ddlPos", GetStyleValue("position"), true);
	pos_ddlPos_onchange();
	
	// Top
	var sizeObj = new Object();
	ParseCustomSize(GetStyleValue("top"), sizeObj);
	if(sizeObj.Type == 0)
	{
		document.all["pos_tbTop"].value = sizeObj.Value;
		SetListBoxIndex("pos_ddlTopUnit", sizeObj.Unit, false);
	}
	// Left
	ParseCustomSize(GetStyleValue("left"), sizeObj);
	if(sizeObj.Type == 0)
	{
		document.all["pos_tbLeft"].value = sizeObj.Value;
		SetListBoxIndex("pos_ddlLeftUnit", sizeObj.Unit, false);
	}
	// Width
	ParseCustomSize(GetStyleValue("width"), sizeObj);
	if(sizeObj.Type == 0)
	{
		document.all["pos_tbWidth"].value = sizeObj.Value;
		SetListBoxIndex("pos_ddlWidthUnit", sizeObj.Unit, false);
	}
	// Height
	ParseCustomSize(GetStyleValue("height"), sizeObj);
	if(sizeObj.Type == 0)
	{
		document.all["pos_tbHeight"].value = sizeObj.Value;
		SetListBoxIndex("pos_ddlHeightUnit", sizeObj.Unit, false);
	}

	// Z-Index
	document.all["pos_tbZIndex"].value = GetStyleValue("z-index");
}
function pos_ddlPos_onchange()
{
	var ddlPos = document.all["pos_ddlPos"];
	var item = ddlPos.options[ddlPos.selectedIndex].value;
	var disabled = (item != "relative" && item != "absolute");
	document.all["pos_tbTop"].disabled = disabled;
	document.all["pos_ddlTopUnit"].disabled = disabled;
	document.all["pos_tbLeft"].disabled = disabled;
	document.all["pos_ddlLeftUnit"].disabled = disabled;
	//document.all["pos_tbWidth"].disabled = disabled;
	//document.all["pos_ddlWidthUnit"].disabled = disabled;
	//document.all["pos_tbHeight"].disabled = disabled;
	//document.all["pos_ddlHeightUnit"].disabled = disabled;
	
	document.all["pos_tbZIndex"].disabled = (item != "absolute");
	
	var image = "images/";
	switch(item)
	{
		case "":
			image += "pos_notset.gif";
			break;
		case "static":
			image += "pos_static.gif";
			break;
		case "relative":
			image += "pos_relative.gif";
			break;
		case "absolute":
			image += "pos_absolute.gif";
			break;
		default:
			throw "\"位置\"选项卡中的\"位置模式\"下拉列表中有不合法的项：" + item;
	}
	document.all["pos_imgPos"].src = image;
}
function pos_GetStyleString()
{
	var cssText = "";
	
	cssText += GetStyleItemFromControl("pos_ddlPos", "position", true);
	var ddlPos = document.all["pos_ddlPos"];
	if(ddlPos.selectedIndex >= 0)
	{
		var item = ddlPos.options[ddlPos.selectedIndex].value;
		if(item == "relative" || item == "absolute")
		{
			var top = GetSizeString(null, "pos_tbTop", "pos_ddlTopUnit");
			if(top.length > 0)
				cssText += "top:" + top + ";";
			var left = GetSizeString(null, "pos_tbLeft", "pos_ddlLeftUnit");
			if(left.length > 0)
				cssText += "left:" + left + ";";
			
			if(item == "absolute")
				cssText += GetStyleItemFromControl("pos_tbZIndex", "z-index", true);
		}
	}
	
	var width = GetSizeString(null, "pos_tbWidth", "pos_ddlWidthUnit");
	if(width.length > 0)
		cssText += "width:" + width + ";";
		
	var height = GetSizeString(null, "pos_tbHeight", "pos_ddlHeightUnit");
	if(height.length > 0)
		cssText += "height:" + height + ";";
		
	return cssText;
}

//=============================================== 布局 ==================================================
function lay_LoadMe()
{
	SetListBoxIndex("lay_ddlVisibility", GetStyleValue("visibility"), true);
	lay_ddlVisibility_onchange();
	SetListBoxIndex("lay_ddlDisplay", GetStyleValue("display"), true);
	lay_ddlDisplay_onchange();
	SetListBoxIndex("lay_ddlFloat", GetStyleValue("float"), true);
	lay_ddlFloat_onchange();
	SetListBoxIndex("lay_ddlClear", GetStyleValue("clear"), true);
	lay_ddlClear_onchange();
	SetListBoxIndex("lay_ddlOverflow", GetStyleValue("overflow"), true);
	
	lay_LoadClip();
	SetListBoxIndex("lay_ddlPbb", GetStyleValue("page-break-before"), true);
	SetListBoxIndex("lay_ddlPba", GetStyleValue("page-break-after"), true);
}
function lay_ddlVisibility_onchange()
{
	var ddl = document.all["lay_ddlVisibility"];
	var item = ddl.options[ddl.selectedIndex].value;
	var image = "images/layout_visibility_";
	switch(item)
	{
		case "":
			image += "notset.gif";
			break;
		case "hidden":
			image += "hidden.gif";
			break;
		case "visible":
			image += "visible.gif";
			break;
		default:
			throw "\"布局\"选项卡中的\"可见性\"下拉列表中有不合法的项：" + item;
	}
	document.all["lay_imgVisibility"].src = image;
}
function lay_ddlDisplay_onchange()
{
	var ddl = document.all["lay_ddlDisplay"];
	var item = ddl.options[ddl.selectedIndex].value;
	var image = "images/layout_display_";
	switch(item)
	{
		case "":
			image += "notset.gif";
			break;
		case "none":
			image += "none.gif";
			break;
		case "block":
			image += "block.gif";
			break;
		case "inline":
			image += "inline.gif";
			break;
		default:
			throw "\"布局\"选项卡中的\"显示\"下拉列表中有不合法的项：" + item;
	}
	document.all["lay_imgDisplay"].src = image;
}
function lay_ddlFloat_onchange()
{
	var ddl = document.all["lay_ddlFloat"];
	var item = ddl.options[ddl.selectedIndex].value;
	var image = "images/layout_float_";
	switch(item)
	{
		case "":
			image += "notset.gif";
			break;
		case "none":
			image += "none.gif";
			break;
		case "left":
			image += "left.gif";
			break;
		case "right":
			image += "right.gif";
			break;
		default:
			throw "\"布局\"选项卡中的\"允许文本流动\"下拉列表中有不合法的项：" + item;
	}
	document.all["lay_imgFloat"].src = image;
}
function lay_ddlClear_onchange()
{
	var ddl = document.all["lay_ddlClear"];
	var item = ddl.options[ddl.selectedIndex].value;
	var image = "images/layout_clear_";
	switch(item)
	{
		case "":
			image += "notset.gif";
			break;
		case "none":
			image += "none.gif";
			break;
		case "left":
			image += "left.gif";
			break;
		case "right":
			image += "right.gif";
			break;
		case "both":
			image += "both.gif";
			break;
		default:
			throw "\"布局\"选项卡中的\"允许浮动对象\"下拉列表中有不合法的项：" + item;
	}
	document.all["lay_imgClear"].src = image;
}
function lay_LoadClip()
{
	var rect = GetStyleValue("clip");
	if(rect == "")
		return;
	var startIndex = rect.indexOf("(");
	var endIndex = rect.indexOf(")");
	if(startIndex < 0 || endIndex < 0 || startIndex >= endIndex)
		return;
		
	var posArray = rect.substring(startIndex + 1, endIndex).toLowerCase().split(" ");
	if(posArray.length != 4)
		posArray = new Array("auto","auto","auto","auto");
	
	lay_ParseClipRectPart("Top", posArray[0]);	
	lay_ParseClipRectPart("Right", posArray[1]);
	lay_ParseClipRectPart("Bottom", posArray[2]);
	lay_ParseClipRectPart("Left", posArray[3]);
}

function lay_ParseClipRectPart(side, sizeString)
{
	if(sizeString != "auto")
	{
		var sizeObj = new Object();
		ParseCustomSize(sizeString, sizeObj);
		if(sizeObj.Type == 0)
		{
			document.all["lay_tbClip" + side].value = sizeObj.Value;
			SetListBoxIndex("lay_ddlClip" + side + "Unit", sizeObj.Unit, false);
		}
	}
}

function lay_GetStyleString()
{
	var cssText = "";
	
	cssText += GetStyleItemFromControl("lay_ddlVisibility", "visibility", true);
	cssText += GetStyleItemFromControl("lay_ddlDisplay", "display", true);
	cssText += GetStyleItemFromControl("lay_ddlFloat", "float", true);
	cssText += GetStyleItemFromControl("lay_ddlClear", "clear", true);
	cssText += GetStyleItemFromControl("lay_ddlOverflow", "overflow", true);
	
	var clipTop = GetSizeString(null, "lay_tbClipTop", "lay_ddlClipTopUnit");
	var clipRight = GetSizeString(null, "lay_tbClipRight", "lay_ddlClipRightUnit");
	var clipBottom = GetSizeString(null, "lay_tbClipBottom", "lay_ddlClipBottomUnit");
	var clipLeft = GetSizeString(null, "lay_tbClipLeft", "lay_ddlClipLeftUnit");
	
	if(clipTop.length > 0 || clipRight.length > 0 || clipBottom.length > 0 || clipLeft.length > 0)
	{
		cssText += "clip:rect(";
		
		cssText += ((clipTop.length > 0) ? clipTop : "auto") + " ";
		cssText += ((clipRight.length > 0) ? clipRight : "auto") + " ";
		cssText += ((clipBottom.length > 0) ? clipBottom : "auto") + " ";
		cssText += ((clipLeft.length > 0) ? clipLeft : "auto");
		
		cssText += ");";
	}
	
	cssText += GetStyleItemFromControl("lay_ddlPbb", "page-break-before", true);
	cssText += GetStyleItemFromControl("lay_ddlPba", "page-break-after", true);
	
	return cssText;
}

//=============================================== 边缘 ==================================================
function edg_LoadMe()
{
	edg_LoadMargin();
	edg_LoadPadding();
	edg_ParseBorderString();
}
function edg_LoadMargin()
{
	var margin = GetStyleValue("margin");
	var marginTop = GetStyleValue("margin-top");
	var marginRight = GetStyleValue("margin-right");
	var marginBottom = GetStyleValue("margin-bottom");
	var marginLeft = GetStyleValue("margin-left");
	if(margin == "")
	{
		if(marginTop == "" && marginRight == "" && marginBottom == "" && marginLeft == "")
			return;
	}
	else
	{
		var marginArray = margin.toLowerCase().split(" ");
		if(marginArray.length != 4)
			return;
		marginTop = marginArray[0];
		marginRight = marginArray[1];
		marginBottom = marginArray[2];
		marginLeft = marginArray[3];
	}
	
	edg_ParseMarginPart("Top", marginTop);
	edg_ParseMarginPart("Right", marginRight);
	edg_ParseMarginPart("Bottom", marginBottom);
	edg_ParseMarginPart("Left", marginLeft);
	
}
function edg_ParseMarginPart(side, sizeString)
{
	var sizeObj = new Object();
	ParseCustomSize(sizeString, sizeObj);
	if(sizeObj.Type == 0)
	{
		document.all["edg_tbMargin" + side].value = sizeObj.Value;
		SetListBoxIndex("edg_ddlMargin" + side + "Unit", sizeObj.Unit, false);
	}
}
function edg_LoadPadding()
{
	var paddingTop = GetStyleValue("padding-top");
	var paddingRight = GetStyleValue("padding-right");
	var paddingBottom = GetStyleValue("padding-bottom");
	var paddingLeft = GetStyleValue("padding-left");
	
	edg_ParsePaddingPart("Top", paddingTop);
	edg_ParsePaddingPart("Right", paddingRight);
	edg_ParsePaddingPart("Bottom", paddingBottom);
	edg_ParsePaddingPart("Left", paddingLeft);
	
}
function edg_ParsePaddingPart(side, sizeString)
{
	var sizeObj = new Object();
	ParseCustomSize(sizeString, sizeObj);
	if(sizeObj.Type == 0)
	{
		document.all["edg_tbPad" + side].value = sizeObj.Value;
		SetListBoxIndex("edg_ddlPad" + side + "Unit", sizeObj.Unit, false);
	}
}

function edg_ParseBorderString()
{
	borderObj = new Object();
	edg_ParseBorderPart("top", borderObj);
	edg_ParseBorderPart("right", borderObj);
	edg_ParseBorderPart("bottom", borderObj);
	edg_ParseBorderPart("left", borderObj);
	edg_ddlBorder_onchange();
}

function edg_ParseBorderPart(side, borderObj)
{
	/*
	 当 border-part （这里 part 通代 left、right、top、bottom）出现的时候，值的个数（空格分隔）有可能是 1个，2个，3个。
		a) 无论值有几个，最后一个一定是 style
		b) 如果值有两个，第一个有可能是 width，也有可能是 color，要具体根据第一个值来判断。
		c) 如果值有三个，出现顺序是 color width style。
		d) 如果值不满三个（一个或两个），表示 width 为 “中”；当 width 为 “未设置”的时候，border-part 不能作为整体出现，
		   只能按照三个部分分别出现。
	*/
	var part = GetStyleValue("border-" + side);
	var partStyle = GetStyleValue("border-" + side + "-style");
	var partColor = GetStyleValue("border-" + side + "-color");
	var partWidth = "";
	if(part != "")
	{
		var partArray = part.toLowerCase().split(" ");
		if(partArray.length == 1)
			partStyle = partArray[0];
		else if(partArray.length == 2)
		{
			var tempPart = partArray[0].toLowerCase();
			
			// 当 border-part 的值只有两个的时候，第一个有可能是宽度，也有可能是颜色，因此下面判断是不是宽度。
			var isWidth = false;
			var widthOptions = document.all["edg_ddlBorderWidth"].options;
			for(var i = 0; i < widthOptions.length; i++)
			{
				if(tempPart == widthOptions[i].value)
				{
					isWidth = true;
					break;
				}
			}
			var sizeObj = new Object();
			ParseCustomSize(tempPart, sizeObj);
			if(sizeObj.Type == 0)
				isWidth = true;
				
			if(isWidth)
				partWidth = tempPart;
			else
				partColor = partArray[0];
			partStyle = partArray[1];
		}
		else if(partArray.length == 3)
		{
			partColor = partArray[0];
			partWidth = partArray[1];
			partStyle = partArray[2];
		}
	}
	
	switch(side)
	{
		case "top":
			borderObj.topColor = partColor;
			borderObj.topWidth = partWidth;
			borderObj.topStyle = partStyle;
			break;
		case "right":
			borderObj.rightColor = partColor;
			borderObj.rightWidth = partWidth;
			borderObj.rightStyle = partStyle;
			break;
		case "bottom":
			borderObj.bottomColor = partColor;
			borderObj.bottomWidth = partWidth;
			borderObj.bottomStyle = partStyle;
			break;
		case "left":
			borderObj.leftColor = partColor;
			borderObj.leftWidth = partWidth;
			borderObj.leftStyle = partStyle;
			break;
	}
	//alert(borderObj.topStyle);
}

function edg_ddlBorder_onchange()
{
	var ddl = document.all["edg_ddlBorder"];
	var item = ddl.options[ddl.selectedIndex].value;
	var image = "images/edg_border_";
	var borderStyle = "", borderWidth = "",borderColor = "";
	switch(item)
	{
		case "top":
			image += "top.gif";
			borderStyle = borderObj.topStyle;
			borderWidth = borderObj.topWidth;
			borderColor = borderObj.topColor;
			break;
		case "left":
			image += "left.gif";
			borderStyle = borderObj.leftStyle;
			borderWidth = borderObj.leftWidth;
			borderColor = borderObj.leftColor;
			break;
		case "right":
			image += "right.gif";
			borderStyle = borderObj.rightStyle;
			borderWidth = borderObj.rightWidth;
			borderColor = borderObj.rightColor;
			break;
		case "bottom":
			image += "bottom.gif";
			borderStyle = borderObj.bottomStyle;
			borderWidth = borderObj.bottomWidth;
			borderColor = borderObj.bottomColor;
			break;
		case "all":
			if(borderObj.topStyle == borderObj.rightStyle && borderObj.rightStyle == borderObj.bottomStyle
				&& borderObj.bottomStyle == borderObj.leftStyle)
				borderStyle = borderObj.topStyle;
			if(borderObj.topWidth == borderObj.rightWidth && borderObj.rightWidth == borderObj.bottomWidth
				&& borderObj.bottomWidth == borderObj.leftWidth)
				borderWidth = borderObj.topWidth;
			if(borderObj.topColor == borderObj.rightColor && borderObj.rightColor == borderObj.bottomColor
				&& borderObj.bottomColor == borderObj.leftColor)
				borderColor = borderObj.topColor;
			image += "all.gif";
			break;
		default:
			throw "\"边缘\"选项卡中的\"选择要更改的边缘\"下拉列表中有不合法的项：" + item;
	}
	
	document.all["edg_imgBorder"].src = image;
	SetListBoxIndex("edg_ddlBorderStyle", borderStyle, true);
	document.all["edg_tbBorderColor"].value = borderColor;
	
	var sizeObj = new Object();
	ParseCustomSize(borderWidth, sizeObj)
	if(sizeObj.Type == 0)
	{
		document.all["edg_tbBorderWidth"].value = sizeObj.Value;
		SetListBoxIndex("edg_ddlBorderWidth", "custom", true);
		SetListBoxIndex("edg_ddlBorderWidthUnit", sizeObj.Unit,false);
	}
	else
		SetListBoxIndex("edg_ddlBorderWidth", borderWidth, true);
		
	edg_ddlBorderStyle_onchange();
}

function edg_ddlBorderStyle_onchange()
{
	var ddl = document.all["edg_ddlBorderStyle"];
	var item = ddl.options[ddl.selectedIndex].value;
	var disabled = (item == "" || item == "none");
	
	document.all["edg_tbBorderColor"].disabled = disabled;
	document.all["edg_btnBorderColor"].disabled = disabled;
	document.all["edg_ddlBorderWidth"].disabled = disabled;
	if(disabled)
	{
		document.all["edg_tbBorderWidth"].disabled = disabled;
		document.all["edg_ddlBorderWidthUnit"].disabled = disabled;
	}
	else
		edg_ddlBorderWidth_onchange();
}

function edg_ddlBorderWidth_onchange()
{
	var ddl = document.all["edg_ddlBorderWidth"];
	var item = ddl.options[ddl.selectedIndex].value;
	var disabled = (item != "custom");
	document.all["edg_tbBorderWidth"].disabled = disabled;
	document.all["edg_ddlBorderWidthUnit"].disabled = disabled;
	
	var ddlBorder = document.all["edg_ddlBorder"];
	var borderItem = ddlBorder.options[ddlBorder.selectedIndex].value;
	
	edg_ShowExample();
}

function edg_GetBorderPartFromControl()
{
	var tempObj = new Object();
	
	var ddlBorderStyle = document.all["edg_ddlBorderStyle"];
	tempObj.borderStyle = ddlBorderStyle.options[ddlBorderStyle.selectedIndex].value;
	
	if(tempObj.borderStyle == "" || tempObj.borderStyle == "none")
	{
		tempObj.borderWidth = "";
		tempObj.borderColor = "";
	}
	else
	{
		var ddlBorderWidth = document.all["edg_ddlBorderWidth"];
		tempObj.borderWidth = GetSizeString("edg_ddlBorderWidth", "edg_tbBorderWidth", "edg_ddlBorderWidthUnit");
		tempObj.borderColor = document.all["edg_tbBorderColor"].value;
	}
	
	return tempObj;
}

function edg_UpdateBorderObjTop(tempObj)
{
	borderObj.topStyle = tempObj.borderStyle;
	borderObj.topWidth = tempObj.borderWidth;
	borderObj.topColor = tempObj.borderColor;
}
function edg_UpdateBorderObjRight(tempObj)
{
	borderObj.rightStyle = tempObj.borderStyle;
	borderObj.rightWidth = tempObj.borderWidth;
	borderObj.rightColor = tempObj.borderColor;
}
function edg_UpdateBorderObjBottom(tempObj)
{
	borderObj.bottomStyle = tempObj.borderStyle;
	borderObj.bottomWidth = tempObj.borderWidth;
	borderObj.bottomColor = tempObj.borderColor;
}
function edg_UpdateBorderObjLeft(tempObj)
{
	borderObj.leftStyle = tempObj.borderStyle;
	borderObj.leftWidth = tempObj.borderWidth;
	borderObj.leftColor = tempObj.borderColor;
}

function edg_ShowExample()
{
	var style = edg_GetStyleString();
	document.all["tdExample"].style.cssText = "";
	document.all["tdExample"].innerHTML = 
	"<input id = \"tblOutput\" type=\"button\" value=\"示意文本\" style=\"" + style + "\"></input>";
	
	var ts = document.all["tblOutput"].style;
	if(ts.borderTopColor.length == 0)
		ts.borderTopColor = "silver";
	if(ts.borderRightColor.length == 0)
		ts.borderRightColor = "silver";
	if(ts.borderBottomColor.length == 0)
		ts.borderBottomColor = "silver";
	if(ts.borderLeftColor.length == 0)
		ts.borderLeftColor = "silver";
	if(ts.color.length == 0)
		ts.color = "White";
	ts.backgroundColor = "blue";
	
}
function edg_GetStyleString()
{
	var cssText = "";

	// margin
	var marginLeft = GetSizeString(null, "edg_tbMarginLeft", "edg_ddlMarginLeftUnit");
	var marginRight = GetSizeString(null, "edg_tbMarginRight", "edg_ddlMarginRightUnit");
	var marginTop = GetSizeString(null, "edg_tbMarginTop", "edg_ddlMarginTopUnit");
	var marginBottom = GetSizeString(null, "edg_tbMarginBottom", "edg_ddlMarginBottomUnit");
	if(marginLeft.length > 0 || marginRight.length > 0 || marginTop.length > 0 || marginBottom.length > 0)
	{
		if(marginLeft.length > 0 && marginRight.length > 0 && marginTop.length > 0 && marginBottom.length > 0)
			cssText += "margin:" + marginTop + " " + marginRight + " " + marginBottom + " " + marginLeft + ";";
		else
		{
			if(marginTop.length > 0)
				cssText += "margin-top:" + marginTop + ";";
			if(marginRight.length > 0)
				cssText += "margin-right:" + marginRight + ";";
			if(marginBottom.length > 0)
				cssText += "margin-bottom:" + marginBottom + ";";
			if(marginLeft.length > 0)
				cssText += "margin-left:" + marginLeft + ";";
		}
	}
	
	// padding
	cssText += edg_GetPaddingPartStyle("Top");
	cssText += edg_GetPaddingPartStyle("Right");
	cssText += edg_GetPaddingPartStyle("Bottom");
	cssText += edg_GetPaddingPartStyle("Left");
	
	// border-part
	cssText += edg_GetBorderStyle();
	return cssText;
}

// 根据一组 border 的控件返回 border-part 的 style 字符串。
function edg_GetBorderStyle()
{
	var cssText = "";
	
	var ddlBorder = document.all["edg_ddlBorder"];
	var border = ddlBorder.options[ddlBorder.selectedIndex].value;
	var tempObj = edg_GetBorderPartFromControl();
	
	switch(border)
	{
		case "top":
			edg_UpdateBorderObjTop(tempObj);
			break;
		case "right":
			edg_UpdateBorderObjRight(tempObj);
			break;
		case "bottom":
			edg_UpdateBorderObjBottom(tempObj);
			break;
		case "left":
			edg_UpdateBorderObjLeft(tempObj);
			break;
		case "all":
			if(tempObj.borderStyle != "")
			{
				edg_UpdateBorderObjTop(tempObj);
				edg_UpdateBorderObjRight(tempObj);
				edg_UpdateBorderObjBottom(tempObj);
				edg_UpdateBorderObjLeft(tempObj);
				break;
			}
	}
	
	// border-part
	cssText += edg_GetBorderPartStyle("top", borderObj.topColor, borderObj.topWidth, borderObj.topStyle);
	cssText += edg_GetBorderPartStyle("right", borderObj.rightColor, borderObj.rightWidth, borderObj.rightStyle);
	cssText += edg_GetBorderPartStyle("bottom", borderObj.bottomColor, borderObj.bottomWidth, borderObj.bottomStyle);
	cssText += edg_GetBorderPartStyle("left", borderObj.leftColor, borderObj.leftWidth, borderObj.leftStyle);
	
	return cssText;
}

function edg_GetBorderPartStyle(side, borderColor, borderWidth, borderStyle)
{
	var cssText = "";
	// border-part
	//当 width 为 “未设置”的时候，border-part 不能作为整体出现
	if(borderWidth == "notset")
	{
		if(borderStyle.length > 0)
			cssText += "border-" + side + "-style:" + borderStyle + ";";
		if(borderColor.length > 0)
			cssText += "border-" + side + "-color:" + borderColor + ";";
	}
	else if(borderStyle.length > 0 || borderWidth.length > 0 || borderColor.length > 0)
	{
		cssText += "border-" + side + ":";
		if(borderColor.length > 0)
			cssText += borderColor + " ";
		if(borderWidth.length > 0)
			cssText += borderWidth + " ";
		if(borderStyle.length > 0)
			cssText += borderStyle;
		
		cssText = TrimString(cssText);
		cssText += ";";
	}
	
	return cssText;
}
function edg_GetPaddingPartStyle(side)
{
	var pad = GetSizeString(null, "edg_tbPad" + side, "edg_ddlPad" + side + "Unit");
	if(pad.length > 0)
		return "padding-" + side.toLowerCase() + ":" + pad + ";";
	return "";
}


//=============================================== 列表 ==================================================
function lst_LoadMe()
{
	var type = "", pos = "", image = "";
	var all = GetStyleValue("list-style");
	if(all.length > 0)
	{
		var allArray = all.split(" ");
		if(allArray.length != 3)
			return;
		type = allArray[0];
		image = allArray[1];
		pos = allArray[2];
	}
	else
	{
		pos = GetStyleValue("list-style-position");
		type = GetStyleValue("list-style-type");
		image = GetStyleValue("list-style-image");
	}
	if(pos == "" && type == "" && image == "")
		return;
		
	if(type == "none")
		SetListBoxIndex("lst_ddlStyle", "none", true);
	else
	{
		SetListBoxIndex("lst_ddlStyleType", type, true);
		SetListBoxIndex("lst_ddlStylePos", pos, true);
		if(image != "")
		{
			if(image == "none")
				document.all["lst_rbStyleImageNone"].checked = true;
			else
			{
				var startIndex = image.indexOf("(");
				var endIndex = image.indexOf(")");
				if(image.substring(0,3) == "url" && startIndex >= 0 && endIndex > startIndex)
					document.all["lst_tbStyleImage"].value = image.substring(startIndex + 1, endIndex);
			}
			if(document.all["lst_tbStyleImage"].value != "" || document.all["lst_rbStyleImageNone"].checked)
				document.all["lst_cbCustomImage"].checked = true;
		}
	}
	if(document.all["lst_ddlStyleType"].selectedIndex > 0
	|| document.all["lst_ddlStylePos"].selectedIndex > 0
	|| document.all["lst_cbCustomImage"].checked)
		SetListBoxIndex("lst_ddlStyle", "display", true);
	
	lst_ddlStyle_onchange();
}			
function lst_ddlStyle_onchange()
{
	var ddl = document.all["lst_ddlStyle"];
	var item = ddl.options[ddl.selectedIndex].value;
	var disabled = (item != "display");
	document.all["lst_ddlStyleType"].disabled = disabled;
	document.all["lst_ddlStylePos"].disabled = disabled;
	
	document.all["lst_cbCustomImage"].disabled = disabled;
	if(disabled)
	{
		document.all["lst_rbStyleImage"].disabled = disabled;
		document.all["lst_rbStyleImageNone"].disabled = disabled;
		document.all["lst_tbStyleImage"].disabled = disabled;
		document.all["lst_btnStyleImage"].disabled = disabled;
	}
	else
		lst_cbCustomImage_onclick();
	
	lst_ShowExample();
}
function lst_cbCustomImage_onclick()
{
	var checked = document.all["lst_cbCustomImage"].checked;
	document.all["lst_rbStyleImage"].disabled = !checked;
	document.all["lst_rbStyleImageNone"].disabled = !checked;
	if(!checked)
	{
		document.all["lst_tbStyleImage"].disabled = !checked;
		document.all["lst_btnStyleImage"].disabled = !checked;
		lst_ShowExample();
	}
	else
		lst_rblStyleImage_onclick();
}
function lst_rblStyleImage_onclick()
{
	var checked = document.all["lst_rbStyleImage"].checked;
	document.all["lst_tbStyleImage"].disabled = !checked;
	document.all["lst_btnStyleImage"].disabled = !checked;
	lst_ShowExample();
}
function lst_ShowExample()
{
	var listObj = lst_GetListObject();
	var type = listObj.type;
	var pos = listObj.pos;
	var image = listObj.image;

	var style = lst_GetStyleString();
	//if(style.length > 0)
		document.all["tdExample"].innerHTML = "<ul style=\"" + style + "\"><li>列表项1<li>列表项2</ul>"	
	//else
	//	document.all["tdExample"].innerHTML = "列表项1<br>列表项2";
}
function lst_GetListObject()
{
	var listObj = new Object();
	
	var type = "", pos = "", image = "";
	var ddlStyle = document.all["lst_ddlStyle"];
	var displayList = ddlStyle.options[ddlStyle.selectedIndex].value;
	if(displayList == "none")
		type = "none";
	else if(displayList == "display")
	{
		// list-style-type
		var ddlType = document.all["lst_ddlStyleType"];
		if(ddlType.selectedIndex > 0)
			type = ddlType.options[ddlType.selectedIndex].value;
			
		// list-style-position
		var ddlPos = document.all["lst_ddlStylePos"];
		if(ddlPos.selectedIndex > 0)
			pos = ddlPos.options[ddlPos.selectedIndex].value;
		
		// list-style-image
		if(document.all["lst_cbCustomImage"].checked)
		{
			if(document.all["lst_rbStyleImage"].checked)
			{
				var tbImageValue = document.all["lst_tbStyleImage"].value;
				if(tbImageValue.length > 0)
					image = "url(" + tbImageValue + ")";
			}
			else if(document.all["lst_rbStyleImageNone"].checked)
				image = "none";
		}
	}
	
	listObj.type = type;
	listObj.pos = pos;
	listObj.image = image;
	
	return listObj;
}
function lst_GetStyleString()
{
	var cssText = "";
	
	var listObj = lst_GetListObject();
	var type = listObj.type;
	var pos = listObj.pos;
	var image = listObj.image;
	
	if(type == "none")
		cssText += "list-style-type:none";
	else
	{
		if(type.length > 0 && pos.length > 0 && image.length > 0)
			cssText += "list-style:" + type + " " + image + " " + pos + ";";
		else 
		{
		if(type.length > 0)
			cssText += "list-style-type:" + type + ";";
		if(pos.length > 0)
			cssText += "list-style-position:" + pos + ";";
		if(image.length > 0)
			cssText += "list-style-image:" + image + ";";
		}
	}
	
	return cssText;
}

//=============================================== 滤镜 ==================================================
function flt_LoadMe()
{
	flt_lbFilter_onchange();
	var filters = GetStyleValue("filter");
	if(filters == "")
		return;
	
	var filterArray = filters.split(")");
	for(var i = 0; i < filterArray.length; i++)
	{
		var filter = TrimString(filterArray[i]);
		var leftBracket = filter.indexOf("(");
		if(leftBracket <= 0)
			continue;
		var filterName = filter.substring(0, leftBracket);
		var filterIndex = GetListBoxIndex("flt_lbFilter", filterName, false);
		if(filterIndex >= 0)
		{
			var paraString = filter.substring(leftBracket + 1, filter.length);
			var paraArray = null;
			if(paraString.length > 0)
				paraArray = paraString.split(",");
			document.all["flt_lbFilter"].selectedIndex = filterIndex;
			flt_btnAddFilter_onclick(paraArray);
		}
	}
}

function flt_lbFilter_onchange()
{
	var lbFilter = document.all["flt_lbFilter"];
	var opt = lbFilter.options[lbFilter.selectedIndex];
	document.all["flt_lblFilterRemark"].innerText = opt.value;
	ShowTextExample();
}

function flt_btnAddFilter_onclick(valueArray)
{
	var lbFilter = document.all["flt_lbFilter"];
	var opt = lbFilter.options[lbFilter.selectedIndex];
	var lbSelected = document.all["flt_lbSelectedFilter"];
	var newOpt = document.createElement("option");
	lbSelected.options.add(newOpt);
	newOpt.innerText = opt.innerText;
	newOpt.value = "";
	
	var paraRemarkArray = opt.paraRemark.split(";");
	if(paraRemarkArray.length == 1 && paraRemarkArray[0] == "")
		paraRemarkArray = new Array();
		
	for(var i = 0; i < paraRemarkArray.length; i++)
	{
		var paraName = paraRemarkArray[i].split(":")[0];
		var paraValue = "";
		if(valueArray != null)
		{
			for(j = 0; j < valueArray.length; j++)
			{
				var nameValue = valueArray[j].split("=");
				if(nameValue.length != 2)
					continue;
				if(TrimString(nameValue[0].toLowerCase()) == paraName.toLowerCase())
				{
					paraValue = TrimString(nameValue[1]);
					break;
				}
			}
		}
		newOpt.value += paraName + "=" + paraValue + ",";
	}
	if(newOpt.value.length > 0)
		newOpt.value = newOpt.value.substring(0, newOpt.value.length - 1);
	
	lbSelected.selectedIndex = lbSelected.options.length - 1;
	flt_lbSelectedFilter_onchange();
}

function flt_lbSelectedFilter_onchange()
{
	var paraOptions = document.all["flt_lbPara"].options;
	while(paraOptions.length > 0)
		paraOptions.remove(0);
	
	var lbSelectedFilter = document.all["flt_lbSelectedFilter"];
	if(lbSelectedFilter.selectedIndex < 0)
	{
		document.all["flt_lbPara"].disabled = true;
		var tbParaValue = document.all["flt_tbParaValue"];
		tbParaValue.value = "";
		tbParaValue.disabled = true;
		document.all["flt_lblParaRemark"].innerText = "";
		flt_lbFilter_onchange();
		return;
	}
	var filterName = lbSelectedFilter.options[lbSelectedFilter.selectedIndex].innerText;
	var lbFilterOpt = GetListBoxOption("flt_lbFilter", filterName, false);
	document.all["flt_lblFilterRemark"].innerText = lbFilterOpt.value;
	var paraRemarkArray = lbFilterOpt.paraRemark.split(";");
	
	var valueArray = lbSelectedFilter.options[lbSelectedFilter.selectedIndex].value.split(",");
	if(valueArray.length == 1 && valueArray[0] == "")
		valueArray = new Array();
	for(var i = 0; i < valueArray.length; i++)
	{
		var paraName = valueArray[i].split("=")[0];
		var opt = document.createElement("option");
		paraOptions.add(opt);
		opt.innerText = paraName;
		opt.remark = paraRemarkArray[i].split(":")[1];
	}
	
	var disabled = (paraOptions.length == 0);
	document.all["flt_lbPara"].disabled = disabled;
	document.all["flt_tbParaValue"].disabled = disabled;
	if(paraOptions.length > 0)
	{
		document.all["flt_lbPara"].selectedIndex = 0;
		flt_lbPara_onchange();
	}
	else
	{
		var opt = document.createElement("option");
		paraOptions.add(opt);
		opt.innerText = "<无参数>";
		document.all["flt_tbParaValue"].value = "";
		ShowTextExample();
	}
}

function flt_lbPara_onchange()
{
	var lbSelectedFilter = document.all["flt_lbSelectedFilter"];
	var filterOpt = lbSelectedFilter.options[lbSelectedFilter.selectedIndex];
	var filterName = filterOpt.innerText;
	var valueArray = filterOpt.value.split(",");
	
	var lbPara = document.all["flt_lbPara"];
	var opt = lbPara.options[lbPara.selectedIndex];
	var paraName = opt.innerText;
	document.all["flt_lblParaRemark"].innerText = opt.remark;
	document.all["flt_lblPara"].innerText = paraName + " 的值：";
	
	var tbParaValue = document.all["flt_tbParaValue"];
	for(var i = 0; i < valueArray.length; i++)
	{
		var nameValue = valueArray[i].split("=");
		if(nameValue[0] == paraName)
		{
			tbParaValue.value = nameValue[1];
			break;
		}
	}
	try
	{
		tbParaValue.focus();
		tbParaValue.select(-1);
	}catch(e){}
}
function flt_tbParaValue_onchange()
{
	var lbSelectedFilter = document.all["flt_lbSelectedFilter"];
	var filterOpt = lbSelectedFilter.options[lbSelectedFilter.selectedIndex];
	var valueArray = filterOpt.value.split(",");

	var lbPara = document.all["flt_lbPara"];
	var paraName = lbPara.options[lbPara.selectedIndex].innerText;
	var paraValue = document.all["flt_tbParaValue"].value;
	for(var i = 0; i < valueArray.length; i++)
	{
		var nameValue = valueArray[i].split("=");
		if(nameValue[0] == paraName)
		{
			filterOpt.value = filterOpt.value.replace(valueArray[i], paraName + "=" + paraValue);
			break;
		}
	}
	ShowTextExample();
}
function flt_lbSelectedFilter_MoveUp()
{
	var lbSelectedFilter = document.all["flt_lbSelectedFilter"];
	if(lbSelectedFilter.selectedIndex < 0)
	{
		alert("请先选中一个滤镜。");
		return;
	}
	if(lbSelectedFilter.selectedIndex == 0)
		return;
	
	var curOption = lbSelectedFilter.options[lbSelectedFilter.selectedIndex];
	var upperOption = lbSelectedFilter.options[lbSelectedFilter.selectedIndex - 1];
	var innerText = curOption.innerText;
	var value = curOption.value;
	curOption.innerText = upperOption.innerText;
	curOption.value = upperOption.value;
	upperOption.innerText = innerText;
	upperOption.value = value;
	
	lbSelectedFilter.selectedIndex--;
	
	ShowTextExample();
}
function flt_lbSelectedFilter_MoveDown()
{
	var lbSelectedFilter = document.all["flt_lbSelectedFilter"];
	if(lbSelectedFilter.selectedIndex < 0)
	{
		alert("请先选中一个滤镜。");
		return;
	}
	if(lbSelectedFilter.selectedIndex == lbSelectedFilter.options.length - 1)
		return;
	
	var curOption = lbSelectedFilter.options[lbSelectedFilter.selectedIndex];
	var netherOption = lbSelectedFilter.options[lbSelectedFilter.selectedIndex + 1];
	var innerText = curOption.innerText;
	var value = curOption.value;
	curOption.innerText = netherOption.innerText;
	curOption.value = netherOption.value;
	netherOption.innerText = innerText;
	netherOption.value = value;
	
	lbSelectedFilter.selectedIndex++;
	
	ShowTextExample();
}
function flt_lbSelectedFilter_Remove()
{
	var lbSelectedFilter = document.all["flt_lbSelectedFilter"];
	if(lbSelectedFilter.selectedIndex < 0)
	{
		alert("请先选中一个滤镜。");
		return;
	}
	var selectedIndex = lbSelectedFilter.selectedIndex;
	lbSelectedFilter.options.remove(lbSelectedFilter.selectedIndex);
	if(selectedIndex == 0 && lbSelectedFilter.options.length > 0)
		lbSelectedFilter.selectedIndex = 0;
	else
		lbSelectedFilter.selectedIndex = selectedIndex - 1;
	flt_lbSelectedFilter_onchange();
	
	ShowTextExample();
}
function flt_GetStyleString()
{
	var cssText = "";
	var options = document.all["flt_lbSelectedFilter"].options;
	if(options.length == 0)
		return cssText;
	
	cssText += "filter:";
	for(var i = 0; i < options.length; i++)
	{
		var opt = options[i];
		var filterName = opt.innerText;
		var paraValue = options[i].value;
		cssText += filterName + "(" + paraValue + ") ";
	}
	cssText = TrimString(cssText) + ";";
	
	return cssText;
}

//=============================================== 其它 ==================================================
function oth_LoadMe()
{
	SetListBoxIndex("oth_ddlCursor", GetStyleValue("cursor"), true);
	oth_ddlCursor_onchange();
	SetListBoxIndex("oth_ddlBdrColl", GetStyleValue("border-collapse"), true);
	oth_ddlBdrColl_onchange();
	SetListBoxIndex("oth_ddlTblLay", GetStyleValue("table-layout"), true);
	
	var behavior = GetStyleValue("behavior");
	if(behavior != "")
	{
		var startIndex = behavior.indexOf("(");
		var endIndex = behavior.indexOf(")");
		if(startIndex >= 0 && endIndex >= 0 && startIndex < endIndex)
			document.all["oth_tbBehavior"].value = behavior.substring(startIndex + 1, endIndex);
	}
}
function oth_ddlCursor_onchange()
{
	var ddl = document.all["oth_ddlCursor"];
	var item = ddl.options[ddl.selectedIndex].value;
	var image = "images/oth_";
	switch(item)
	{
		case "":
			image += "notset.gif";
			break;
		case "auto":
		case "default":
			image += "auto_default.gif";
			break;
		case "crosshair":
		case "hand":
		case "move":
		case "text":
		case "wait":
		case "help":
			image += item + ".gif";
			break;
		case "n-resize":
		case "s-resize":
			image += "n_s_resize.gif";
			break;
		case "w-resize":
		case "e-resize":
			image += "w_e_resize.gif";
			break;
		case "nw-resize":
		case "se-resize":
			image += "nw_se_resize.gif";
			break;
		case "sw-resize":
		case "ne-resize":
			image += "sw_ne_resize.gif";
			break;
	}
	document.all["oth_imgCursor"].src = image;
}
function oth_ddlBdrColl_onchange()
{
	var ddl = document.all["oth_ddlBdrColl"];
	var item = ddl.options[ddl.selectedIndex].value;
	var image = "images/oth_borderCollapse_";
	if(item == "")
		image += "notset.gif";
	else
		image += item + ".gif";
	document.all["oth_imgBdrColl"].src = image;
}
function oth_GetStyleString()
{
	var cssText = "";
	
	cssText += GetStyleItemFromControl("oth_ddlCursor", "cursor", true);
	cssText += GetStyleItemFromControl("oth_ddlBdrColl", "border-collapse", true);
	cssText += GetStyleItemFromControl("oth_ddlTblLay", "table-layout", true);
	
	var behavior = document.all["oth_tbBehavior"].value;
	if(behavior.length > 0)
		cssText += "behavior:url(" + behavior + ");";
	
	return cssText;
}
