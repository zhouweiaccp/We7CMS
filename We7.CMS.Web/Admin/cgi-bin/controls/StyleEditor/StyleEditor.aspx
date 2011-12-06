<%@ Page Language="C#" AutoEventWireup="true" Codebehind="StyleEditor.aspx.cs" Inherits="We7.CMS.Web.Admin.StyleEditor" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>样式编辑器</title>
    <link rel="stylesheet" type="text/css" href="styleeditor.css" media="screen" />

    <script src="<%=AppPath%>/cgi-bin/DialogHelper.js" type="text/javascript" charset="gb2312"></script>

    <style type="text/css">
  
    .Active { BORDER-RIGHT: lightcyan thin ; BORDER-TOP: lightcyan thin inset; BORDER-LEFT: lightcyan thin inset; CURSOR: hand; BORDER-BOTTOM: lightcyan thin ; BACKGROUND-COLOR: lightcyan }
	.InActive { BORDER-RIGHT: medium none ; BORDER-TOP: powderblue thin solid ; BORDER-LEFT: powderblue thin solid ; CURSOR: hand; BORDER-BOTTOM: powderblue thin ; BACKGROUND-COLOR: powderblue }
	.Highlight { BORDER-RIGHT: medium none ; BORDER-TOP: lightcyan thin outset ; BORDER-LEFT: lightcyan thin outset ; CURSOR: hand; BORDER-BOTTOM: lightcyan thin ; BACKGROUND-COLOR: lightcyan }
		</style>

    <script src="StyleEditor.js" type="text/javascript" charset="gb2312"></script>

    <script type="text/javascript">
function selectColor(obj) {
    var s = window.showModalDialog("ColorList.aspx", window,"dialogWidth:640px;dialogHeight:480px");
    if(s != null) {
        var o = document.all[obj];
        if( o )
            o.value = s;
    }
}

function onBodyLoad() {
    var p = null;
    var s;
    
    if(window.parent != null ) {
       p = window.parent;
    }
    else if(window.opener != null) {
        p = window.opener;
    }
    if(p && p.getParameter != null)    {
        s = p.getParameter();    
    }
    else {
        s = "";
    }   
    
    LoadStyle(s);
}    

function onBodyKeyPress() {
    if(event.keyCode == 13) {
        SubmitMe();
    }
    else if(event.keyCode == 27) {
        CancelMe();
    }
}

function SubmitMe() {
    var s = GetStyleText();
    closeDialog(s, "");
}

function CancelMe() {
     closeDialog(null, null);
}
    </script>

</head>
<body onkeypress="onBodyKeyPress()" onload="onBodyLoad();">
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
                                选择</asp:HyperLink>
                            <span>| </span>
                            <asp:HyperLink ID="CancelHyperLink" runat="server" NavigateUrl="javascript:CancelMe();">
                                <asp:Image ID="CancelImage" runat="server" ImageUrl="~/admin/Images/icon_cancel.gif" />
                                取消</asp:HyperLink>
                            <span>| </span>
                        </div>
                        <div id="plContainer" style="z-index: 102; left: 0px; width: 720px; position: relative;
                            top: 0px; height: 500px; background-color: Gray">
                            <table id="tblTab" style="border-right: lightcyan thin; border-top: thin; font-size: 12px;
                                z-index: 102; left: 8px; border-left: thin; width: 55px; border-bottom: thin;
                                position: absolute; top: 8px; height: 272px; text-align: center" cellspacing="0"
                                cellpadding="0" border="0">
                                <tr>
                                    <td id="tabFont" onmouseover="HighlightTab(this.id);" style="height: 29px" onclick="SwitchTab(this.id);"
                                        onmouseout="UnHighlightTab(this.id);" valign="middle" align="center">
                                        <table style="font-size: 12px" cellspacing="0" cellpadding="0" border="0">
                                            <tr>
                                                <td>
                                                    <img alt="" src="images/ico_font.gif"></td>
                                                <td>
                                                    &nbsp;字体</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="tabBg" onmouseover="HighlightTab(this.id);" onclick="SwitchTab(this.id);"
                                        onmouseout="UnHighlightTab(this.id);" align="center">
                                        <table style="font-size: 12px" cellspacing="0" cellpadding="0" border="0">
                                            <tr>
                                                <td>
                                                    <img alt="" src="images/ico_bk.gif" /></td>
                                                <td>
                                                    &nbsp;背景</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="tabText" onmouseover="HighlightTab(this.id);" style="height: 25px" onclick="SwitchTab(this.id);"
                                        onmouseout="UnHighlightTab(this.id);" align="center">
                                        <table style="font-size: 12px" cellspacing="0" cellpadding="0" border="0">
                                            <tr>
                                                <td>
                                                    <img alt="" src="images/ico_text.gif" /></td>
                                                <td>
                                                    &nbsp;文本</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="tabPos" onmouseover="HighlightTab(this.id);" onclick="SwitchTab(this.id);"
                                        onmouseout="UnHighlightTab(this.id);" align="center">
                                        <table style="font-size: 12px" cellspacing="0" cellpadding="0" border="0">
                                            <tr>
                                                <td>
                                                    <img alt="" src="images/ico_pos.gif"></td>
                                                <td>
                                                    &nbsp;位置</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="tabLayout" onmouseover="HighlightTab(this.id);" style="height: 29px" onclick="SwitchTab(this.id);"
                                        onmouseout="UnHighlightTab(this.id);" align="center">
                                        <table style="font-size: 12px" cellspacing="0" cellpadding="0" border="0">
                                            <tr>
                                                <td>
                                                    <img alt="" src="images/ico_page.gif"></td>
                                                <td>
                                                    &nbsp;布局</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="tabEdge" onmouseover="HighlightTab(this.id);" onclick="SwitchTab(this.id);"
                                        onmouseout="UnHighlightTab(this.id);" align="center">
                                        <table style="font-size: 12px" cellspacing="0" cellpadding="0" border="0">
                                            <tr>
                                                <td>
                                                    <img alt="" src="images/ico_border.gif"></td>
                                                <td>
                                                    &nbsp;边缘</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="tabList" onmouseover="HighlightTab(this.id);" style="height: 24px" onclick="SwitchTab(this.id);"
                                        onmouseout="UnHighlightTab(this.id);" align="center">
                                        <table style="font-size: 12px" cellspacing="0" cellpadding="0" border="0">
                                            <tr>
                                                <td>
                                                    <img alt="" src="images/ico_list.gif"></td>
                                                <td>
                                                    &nbsp;列表</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="tabFilter" onmouseover="HighlightTab(this.id);" onclick="SwitchTab(this.id);"
                                        onmouseout="UnHighlightTab(this.id);" align="center">
                                        <table style="font-size: 12px" cellspacing="0" cellpadding="0" border="0">
                                            <tr>
                                                <td>
                                                    <img alt="" src="images/ico_filter.gif"></td>
                                                <td>
                                                    &nbsp;滤镜</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="tabOther" onmouseover="HighlightTab(this.id);" onclick="SwitchTab(this.id);"
                                        onmouseout="UnHighlightTab(this.id);" align="center">
                                        <table style="font-size: 12px" cellspacing="0" cellpadding="0" border="0">
                                            <tr>
                                                <td>
                                                    <img alt="" src="images/ico_other.gif"></td>
                                                <td>
                                                    其它</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table id="tblContent" style="border-right: lightcyan thin inset; border-top: lightcyan thin inset;
                                font-size: 12px; z-index: 101; left: 65px; border-left: lightcyan thin inset;
                                width: 642px; border-bottom: lightcyan thin inset; position: absolute; top: 8px;
                                height: 456px; background-color: lightcyan" cellspacing="1" cellpadding="1" width="642"
                                border="1">
                                <tr>
                                    <td>
                                        <!-- 
								  
								                                               *************                                                         
									                                         ****************                                                        
																			******  字体 ***** 
									                                         ****************
									                                           *************
									                                           
								-->
                                        <div id="plExampleContainer" style="border-right: thin solid; border-top: thin solid;
                                            z-index: 500; left: 0px; overflow: auto; border-left: thin solid; width: 632px;
                                            border-bottom: thin solid; position: absolute; top: 368px; height: 80px">
                                            <table id="tblExample" style="z-index: 500; left: 0px; overflow: scroll; width: 100%;
                                                position: absolute; top: 0px; height: 100%" cellspacing="1" cellpadding="1" border="0">
                                                <tr>
                                                    <td id="tdExample" style="overflow: auto" valign="middle" align="center">
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="tabFontContent" style="width: 634px; position: absolute; top: 8px; height: 448px">
                                            <div style="display: inline; z-index: 101; left: 16px; width: 56px; position: absolute;
                                                top: 8px; height: 14px">
                                                字体名称</div>
                                            <hr style="z-index: 102; left: 80px; width: 526px; position: absolute; top: 16px;
                                                height: 1px" width="526" size="1">
                                            <input id="font_tbFontFamily" style="font-size: 12px; z-index: 103; left: 32px; width: 264px;
                                                position: absolute; top: 24px; height: 22px" readonly type="text" size="38">
                                            <input id="font_btnfont_OpenSelectFontWindow" style="font-size: 12px; z-index: 104;
                                                left: 304px; width: 24px; position: absolute; top: 24px; height: 20px" onclick="font_OpenSelectFontWindow();"
                                                type="button" value="...">
                                            <div style="display: inline; z-index: 105; left: 16px; width: 56px; position: absolute;
                                                top: 56px; height: 15px">
                                                字体属性</div>
                                            <hr style="z-index: 106; left: 80px; width: 524px; position: absolute; top: 64px;
                                                height: 1px" width="524" size="1">
                                            <div style="display: inline; z-index: 107; left: 32px; width: 32px; position: absolute;
                                                top: 80px; height: 15px">
                                                颜色</div>
                                            <input language="javascript" id="font_tbColor" style="font-size: 12px; z-index: 108;
                                                left: 72px; width: 88px; position: absolute; top: 73px; height: 23px" type="text"
                                                onchange="ShowTextExample();" size="9">
                                            <input id="font_ColorSelect" style="z-index: 109; left: 168px; width: 24px; position: absolute;
                                                top: 72px; height: 24px" onclick="selectColor('font_tbColor');" type="button"
                                                value="...">
                                            <div style="display: inline; z-index: 110; left: 208px; width: 32px; position: absolute;
                                                top: 80px; height: 15px">
                                                斜体</div>
                                            <select id="font_ddlFontStyle" style="font-size: 12px; z-index: 111; left: 248px;
                                                width: 144px; position: absolute; top: 75px" onchange="ShowTextExample();">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="normal">正常</option>
                                                <option value="italic">斜体</option>
                                            </select>
                                            <div style="display: none; z-index: 112; left: 408px; width: 56px; position: absolute;
                                                top: 80px; height: 15px">
                                                <p>
                                                    小写字母</p>
                                            </div>
                                            <select id="font_ddlFontVariant" style="display: none; font-size: 12px; z-index: 113;
                                                left: 472px; width: 144px; position: absolute; top: 76px" onchange="ShowTextExample();">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="normal">正常</option>
                                                <option value="small-caps">小写字母</option>
                                            </select>
                                            <div style="display: inline; z-index: 114; left: 32px; width: 29px; position: absolute;
                                                top: 104px; height: 15px">
                                                大小</div>
                                            <hr style="z-index: 115; left: 64px; width: 28%; position: absolute; top: 112px;
                                                height: 1px" width="28.06%" size="1">
                                            <table id="Table2" style="font-size: 12px; z-index: 116; left: 304px; width: 208px;
                                                position: absolute; top: 126px; height: 103px" cellspacing="1" cellpadding="1"
                                                width="208" border="0">
                                                <tr valign="bottom">
                                                    <td style="width: 22px">
                                                        <input language="javascript" id="font_cbTxtDec_None" onclick="font_cbTxtDec_None_onclick(this.checked);"
                                                            type="checkbox" value="none"></td>
                                                    <td>
                                                        <label for="font_cbTxtDec_None">
                                                            无</label>
                                                    </td>
                                                </tr>
                                                <tr valign="bottom">
                                                    <td style="width: 22px">
                                                        <input id="font_cbTxtDec_Underline" onclick="ShowTextExample();" type="checkbox"
                                                            value="underline"></td>
                                                    <td>
                                                        <label for="font_cbTxtDec_Underline">
                                                            下划线</label>
                                                    </td>
                                                </tr>
                                                <tr valign="bottom">
                                                    <td style="width: 22px">
                                                        <input id="font_cbTxtDec_LineThrough" onclick="ShowTextExample();" type="checkbox"
                                                            value="line-through"></td>
                                                    <td>
                                                        <label for="font_cbTxtDec_LineThrough">
                                                            删除线</label>
                                                    </td>
                                                </tr>
                                                <tr valign="bottom">
                                                    <td style="width: 22px">
                                                        <input id="font_cbTxtDec_Overline" onclick="ShowTextExample();" type="checkbox" value="overline"></td>
                                                    <td>
                                                        <label for="font_cbTxtDec_Overline">
                                                            上划线</label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <hr style="z-index: 117; left: 336px; width: 36.42%; position: absolute; top: 112px;
                                                height: 1px" width="36.42%" size="1">
                                            <div style="display: inline; z-index: 118; left: 304px; width: 29px; position: absolute;
                                                top: 104px; height: 15px">
                                                效果</div>
                                            <hr style="z-index: 119; left: 72px; width: 28%; position: absolute; top: 248px;
                                                height: 1px" width="24.03%" size="1">
                                            <div style="display: inline; z-index: 120; left: 32px; width: 29px; position: absolute;
                                                top: 240px; height: 15px">
                                                粗体</div>
                                            <table id="Table3" style="font-size: 12px; z-index: 121; left: 32px; width: 216px;
                                                position: absolute; top: 256px; height: 45px" cellspacing="1" cellpadding="0"
                                                width="216" align="left" border="0">
                                                <tr valign="bottom">
                                                    <td style="width: 20px">
                                                        <input id="font_rbFontWeight0" onclick="font_ChangeFontWeightType(0);" type="radio"
                                                            checked name="font_rblFontWeight"></td>
                                                    <td style="width: 80px">
                                                        <label for="font_rbFontWeight0">
                                                            绝对</label></td>
                                                    <td colspan="2">
                                                        <select id="font_ddlAbsFontWeight" style="font-size: 12px; width: 100%" onchange="ShowTextExample();">
                                                            <option value="" selected>&lt;未设置&gt;</option>
                                                            <option value="normal">正常</option>
                                                            <option value="bold">粗体</option>
                                                        </select>
                                                    </td>
                                                </tr>
                                                <tr valign="bottom">
                                                    <td style="width: 20px">
                                                        <input id="font_rbFontWeight1" onclick="font_ChangeFontWeightType(1);" type="radio"
                                                            name="font_rblFontWeight"></td>
                                                    <td style="width: 80px">
                                                        <label for="font_rbFontWeight1">
                                                            相对</label></td>
                                                    <td colspan="2">
                                                        <select id="font_ddlRevFontWeight" style="font-size: 12px; width: 100%" onchange="ShowTextExample();">
                                                            <option value="">&lt;未设置&gt;</option>
                                                            <option value="lighter" selected>较淡</option>
                                                            <option value="bolder">较深</option>
                                                        </select>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div style="display: none; z-index: 122; left: 304px; width: 29px; position: absolute;
                                                top: 240px; height: 15px">
                                                大写</div>
                                            <select id="font_ddlTxtTrans" style="display: none; font-size: 12px; z-index: 123;
                                                left: 304px; width: 208px; position: absolute; top: 256px" onchange="ShowTextExample();">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="none">无</option>
                                                <option value="capitalize">首字母大写</option>
                                                <option value="lowercase">小写</option>
                                                <option value="uppercase">大写</option>
                                            </select>
                                            <input language="javascript" id="font_rbFontSize0" style="z-index: 124; left: 32px;
                                                position: absolute; top: 128px" onclick="font_ChangeSizeType(0);" type="radio"
                                                checked name="font_rblFontSize">
                                            <input id="font_tbFontSize" style="z-index: 125; left: 120px; width: 72px; position: absolute;
                                                top: 128px; height: 22px" type="text" onchange="ShowTextExample();" size="6">
                                            <select id="font_ddlFontUnit" style="font-size: 12px; z-index: 126; left: 200px;
                                                position: absolute; top: 128px" onchange="ShowTextExample();" size="1">
                                                <option value="px" selected>px</option>
                                                <option value="pt">pt</option>
                                                <option value="pc">pc</option>
                                                <option value="mm">mm</option>
                                                <option value="cm">cm</option>
                                                <option value="in">in</option>
                                                <option value="em">em</option>
                                                <option value="ex">ex</option>
                                                <option>%</option>
                                            </select>
                                            <input language="javascript" id="font_rbFontSize1" style="z-index: 127; left: 32px;
                                                position: absolute; top: 160px" onclick="font_ChangeSizeType(1);" type="radio"
                                                name="font_rblFontSize">
                                            <select id="font_ddlAbsFontSize" style="font-size: 12px; z-index: 128; left: 120px;
                                                width: 128px; position: absolute; top: 160px" onchange="ShowTextExample();" size="1">
                                                <option value="">&lt;未设置&gt;</option>
                                                <option value="xx-small" selected>XX-Small</option>
                                                <option value="x-small">X-Small</option>
                                                <option value="small">Small</option>
                                                <option value="medium">Medium</option>
                                                <option value="large">Large</option>
                                                <option value="x-large">X-Large</option>
                                                <option value="xx-large">XX-Large</option>
                                            </select>
                                            <input id="font_rbFontSize2" style="z-index: 129; left: 32px; position: absolute;
                                                top: 192px" onclick="font_ChangeSizeType(2);" type="radio" name="font_rblFontSize">
                                            <select id="font_ddlRevFontSize" style="font-size: 12px; z-index: 130; left: 120px;
                                                width: 128px; position: absolute; top: 192px" onchange="ShowTextExample();" size="1">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="smaller">较小</option>
                                                <option value="larger">较大</option>
                                            </select>
                                            <div style="display: inline; z-index: 131; left: 64px; width: 32px; position: absolute;
                                                top: 132px; height: 15px">
                                                特定</div>
                                            <div style="display: inline; z-index: 132; left: 64px; width: 32px; position: absolute;
                                                top: 164px; height: 15px">
                                                绝对</div>
                                            <div style="display: inline; z-index: 133; left: 64px; width: 32px; position: absolute;
                                                top: 196px; height: 15px">
                                                相对</div>
                                        </div>
                                        <!-- 
								  
								                                               *************                                                         
									                                         ****************                                                        
																			******  背景 ***** 
									                                         ****************
									                                           *************
									                                           
								-->
                                        <div id="tabBgContent" style="width: 634px; position: absolute; top: 500px; height: 440px">
                                            <div style="display: inline; z-index: 100; left: 16px; width: 56px; position: absolute;
                                                top: 8px; height: 14px">
                                                背景色</div>
                                            <hr style="z-index: 101; left: 80px; width: 526px; position: absolute; top: 16px;
                                                height: 1px" width="595" size="1">
                                            <input id="bg_tbBgColor" style="font-size: 12px; z-index: 102; left: 80px; width: 176px;
                                                position: absolute; top: 32px; height: 22px" type="text" onchange="ShowTextExample();"
                                                size="24">
                                            <input id="bg_btnSelectColor" style="font-size: 12px; z-index: 104; left: 264px;
                                                width: 24px; position: absolute; top: 32px; height: 24px" onclick="selectColor('bg_tbBgColor');"
                                                type="button" value="...">
                                            <div style="display: inline; z-index: 105; left: 32px; width: 48px; position: absolute;
                                                top: 36px; height: 10px">
                                                颜色</div>
                                            <input id="bg_cbBgTrans" style="z-index: 106; left: 344px; position: absolute; top: 32px"
                                                onclick="bg_BgTrans_change();ShowTextExample();" type="checkbox">
                                            <div style="display: inline; z-index: 107; left: 368px; width: 32px; position: absolute;
                                                top: 35px; height: 15px">
                                                <label for="bg_cbBgTrans">
                                                    透明</label>
                                            </div>
                                            <div style="display: inline; z-index: 108; left: 16px; width: 56px; position: absolute;
                                                top: 72px; height: 14px">
                                                背景图像</div>
                                            <hr style="z-index: 109; left: 80px; width: 526px; position: absolute; top: 80px;
                                                height: 1px" width="595" size="1">
                                            <div style="display: inline; z-index: 110; left: 32px; width: 32px; position: absolute;
                                                top: 92px; height: 15px">
                                                图像</div>
                                            <input language="javascript" id="bg_tbBgImage" style="font-size: 12px; z-index: 111;
                                                left: 72px; width: 432px; position: absolute; top: 88px; height: 22px" type="text"
                                                onchange="return bg_tbBgImage_onchange()" size="66">
                                            <div style="display: inline; z-index: 112; left: 48px; width: 32px; position: absolute;
                                                top: 116px; height: 15px">
                                                平铺</div>
                                            <select id="bg_ddlRepeat" style="font-size: 12px; z-index: 113; left: 88px; width: 168px;
                                                position: absolute; top: 112px" disabled onchange="ShowTextExample();">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="repeat-x">水平平铺</option>
                                                <option value="repeat-y">垂直平铺</option>
                                                <option value="repeat">水平和垂直平铺</option>
                                                <option value="no-repeat">不平铺</option>
                                            </select>
                                            <div style="display: inline; z-index: 114; left: 272px; width: 32px; position: absolute;
                                                top: 116px; height: 15px">
                                                滚动</div>
                                            <select id="bg_ddlAttach" style="font-size: 12px; z-index: 115; left: 312px; width: 192px;
                                                position: absolute; top: 112px" disabled onchange="ShowTextExample();">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="scroll">滚动背景</option>
                                                <option value="fixed">固定背景</option>
                                            </select>
                                            <div style="display: inline; z-index: 116; left: 48px; width: 32px; position: absolute;
                                                top: 152px; height: 15px">
                                                位置</div>
                                            <hr style="z-index: 117; left: 88px; width: 52.33%; position: absolute; top: 160px;
                                                height: 1px" width="52.33%" size="1">
                                            <div style="display: inline; z-index: 118; left: 72px; width: 32px; position: absolute;
                                                top: 180px; height: 15px">
                                                水平</div>
                                            <select id="bg_ddlPosX" style="font-size: 12px; z-index: 119; left: 112px; width: 152px;
                                                position: absolute; top: 176px" disabled onchange="bg_ddlPosX_onchange();">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="left">左</option>
                                                <option value="center">居中</option>
                                                <option value="right">右</option>
                                                <option value="custom">自定义</option>
                                            </select>
                                            <input id="bg_tbPosX" style="font-size: 12px; z-index: 120; left: 288px; width: 48px;
                                                position: absolute; top: 176px; height: 22px" disabled type="text" onchange="ShowTextExample();"
                                                size="2">
                                            <select id="bg_ddlPosXUnit" style="font-size: 12px; z-index: 121; left: 344px; width: 80px;
                                                position: absolute; top: 176px" disabled onchange="ShowTextExample();">
                                                <option value="px" selected>px</option>
                                                <option value="pt">pt</option>
                                                <option value="pc">pc</option>
                                                <option value="mm">mm</option>
                                                <option value="cm">cm</option>
                                                <option value="in">in</option>
                                                <option value="em">em</option>
                                                <option value="ex">ex</option>
                                                <option>%</option>
                                            </select>
                                            <div style="display: inline; z-index: 122; left: 72px; width: 32px; position: absolute;
                                                top: 210px; height: 15px">
                                                垂直</div>
                                            <select id="bg_ddlPosY" style="font-size: 12px; z-index: 123; left: 112px; width: 152px;
                                                position: absolute; top: 208px" disabled onchange="bg_ddlPosY_onchange();">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="top">顶</option>
                                                <option value="center">居中</option>
                                                <option value="bottom">底</option>
                                                <option value="custom">自定义</option>
                                            </select>
                                            <input id="bg_tbPosY" style="font-size: 12px; z-index: 124; left: 288px; width: 48px;
                                                position: absolute; top: 208px; height: 22px" disabled type="text" onchange="ShowTextExample();"
                                                size="2">
                                            <select id="bg_ddlPosYUnit" style="font-size: 12px; z-index: 125; left: 344px; width: 80px;
                                                position: absolute; top: 208px" disabled onchange="ShowTextExample();">
                                                <option value="px" selected>px</option>
                                                <option value="pt">pt</option>
                                                <option value="pc">pc</option>
                                                <option value="mm">mm</option>
                                                <option value="cm">cm</option>
                                                <option value="in">in</option>
                                                <option value="em">em</option>
                                                <option value="ex">ex</option>
                                                <option>%</option>
                                            </select>
                                            <input language="javascript" id="bg_cbNoneImage" style="z-index: 126; left: 32px;
                                                position: absolute; top: 240px" onclick="return bg_cbNoneImage_onclick()" type="checkbox">
                                            <div style="display: inline; z-index: 127; left: 56px; width: 88px; position: absolute;
                                                top: 243px; height: 16px">
                                                <label for="bg_cbNoneImage">
                                                    不使用背景图像</label></div>
                                            <input id="bg_btnSelectImg" style="font-size: 12px; z-index: 128; left: 512px; width: 24px;
                                                position: absolute; top: 88px; height: 24px" onclick="javascript:document.all['bg_tbBgImage'].value=window.showModalDialog('../ftp/default.aspx?obj=0','','dialogHeight:525px;dialogWidth:730px;center:yes;resizable:no;scroll:yes;status:no');"
                                                type="button" value="..." name="Button1">
                                        </div>
                                        <!-- 
								  
								                                               *************                                                         
									                                         ****************                                                        
																			******  文本 ***** 
									                                         ****************
									                                           *************
									                                           
								-->
                                        <div id="tabTextContent" style="width: 634px; position: absolute; top: 1000px; height: 440px">
                                            <div style="display: inline; z-index: 100; left: 16px; width: 56px; position: absolute;
                                                top: 8px; height: 14px">
                                                对齐方式</div>
                                            <hr style="z-index: 101; left: 80px; width: 526px; position: absolute; top: 16px;
                                                height: 1px" width="595" size="1">
                                            <div style="display: inline; z-index: 103; left: 16px; width: 40px; position: absolute;
                                                top: 36px; height: 12px">
                                                水平：</div>
                                            <select language="javascript" id="txt_ddlAlign" style="font-size: 12px; z-index: 104;
                                                left: 64px; width: 120px; position: absolute; top: 32px" onchange="return txt_ddlAlign_onchange()">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="left">左</option>
                                                <option value="center">居中</option>
                                                <option value="right">右</option>
                                                <option value="justify">两端对齐</option>
                                            </select>
                                            <select id="txt_ddlVAlign" style="font-size: 12px; z-index: 105; left: 432px; width: 120px;
                                                position: absolute; top: 32px" onchange="ShowTextExample();">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="sub">下标</option>
                                                <option value="super">上标</option>
                                                <option value="baseline">正常</option>
                                            </select>
                                            <div style="display: inline; z-index: 106; left: 384px; width: 40px; position: absolute;
                                                top: 36px; height: 12px">
                                                垂直：</div>
                                            <div style="display: inline; z-index: 107; left: 200px; width: 40px; position: absolute;
                                                top: 36px; height: 12px">
                                                调整：</div>
                                            <select id="txt_ddlJustify" style="font-size: 12px; z-index: 108; left: 248px; width: 120px;
                                                position: absolute; top: 32px" onchange="ShowTextExample();">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="auto">自动</option>
                                                <option value="inter-word">设置字距</option>
                                                <option value="newspaper">报纸样式</option>
                                                <option value="distribute">分布间距</option>
                                                <option value="distribute-all-lines">分布所有行</option>
                                            </select>
                                            <div style="display: inline; z-index: 109; left: 16px; width: 32px; position: absolute;
                                                top: 64px; height: 14px">
                                                间距</div>
                                            <div style="display: inline; z-index: 110; left: 32px; width: 32px; position: absolute;
                                                top: 84px; height: 14px">
                                                字母</div>
                                            <select language="javascript" id="txt_ddlLetSpa" style="font-size: 12px; z-index: 111;
                                                left: 64px; width: 120px; position: absolute; top: 80px" onchange="return txt_ddlLetSpa_onchange()">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="normal">正常</option>
                                                <option value="custom">自定义</option>
                                            </select>
                                            <input id="txt_tbLetSpa" style="font-size: 12px; z-index: 112; left: 200px; width: 56px;
                                                position: absolute; top: 80px; height: 22px" disabled type="text" onchange="ShowTextExample();"
                                                size="4">
                                            <select id="txt_ddlLetSpaUnit" style="font-size: 12px; z-index: 113; left: 264px;
                                                width: 56px; position: absolute; top: 80px" disabled onchange="ShowTextExample();">
                                                <option value="px" selected>px</option>
                                                <option value="pt">pt</option>
                                                <option value="pc">pc</option>
                                                <option value="mm">mm</option>
                                                <option value="cm">cm</option>
                                                <option value="in">in</option>
                                                <option value="em">em</option>
                                                <option value="ex">ex</option>
                                                <option>%</option>
                                            </select>
                                            <div style="display: inline; z-index: 114; left: 32px; width: 16px; position: absolute;
                                                top: 116px; height: 14px">
                                                行</div>
                                            <select language="javascript" id="txt_ddlLineHeight" style="font-size: 12px; z-index: 115;
                                                left: 64px; width: 120px; position: absolute; top: 112px" onchange="return txt_ddlLineHeight_onchange()">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="normal">正常</option>
                                                <option value="custom">自定义</option>
                                            </select>
                                            <input id="txt_tbLineHeight" style="font-size: 12px; z-index: 121; left: 200px; width: 56px;
                                                position: absolute; top: 112px; height: 22px" disabled type="text" onchange="ShowTextExample();"
                                                size="4">
                                            <select id="txt_ddlLineHeightUnit" style="font-size: 12px; z-index: 130; left: 264px;
                                                width: 56px; position: absolute; top: 112px" disabled onchange="ShowTextExample();">
                                                <option value="px" selected>px</option>
                                                <option value="pt">pt</option>
                                                <option value="pc">pc</option>
                                                <option value="mm">mm</option>
                                                <option value="cm">cm</option>
                                                <option value="in">in</option>
                                                <option value="em">em</option>
                                                <option value="ex">ex</option>
                                                <option>%</option>
                                            </select>
                                            <div style="display: inline; z-index: 131; left: 360px; width: 48px; position: absolute;
                                                top: 64px; height: 12px">
                                                文本流</div>
                                            <div style="display: inline; z-index: 132; left: 376px; width: 32px; position: absolute;
                                                top: 84px; height: 14px">
                                                缩进</div>
                                            <hr style="z-index: 133; left: 56px; width: 41.37%; position: absolute; top: 72px;
                                                height: 1px" width="41.37%" size="1">
                                            <hr style="z-index: 134; left: 416px; width: 30%; position: absolute; top: 72px;
                                                height: 1px" width="30.66%" size="1">
                                            <input id="txt_tbIndent" style="font-size: 12px; z-index: 135; left: 432px; width: 56px;
                                                position: absolute; top: 80px; height: 22px" type="text" onchange="ShowTextExample();"
                                                size="4">
                                            <select id="txt_ddlIndentUnit" style="font-size: 12px; z-index: 136; left: 496px;
                                                width: 56px; position: absolute; top: 80px" onchange="ShowTextExample();">
                                                <option value="px" selected>px</option>
                                                <option value="pt">pt</option>
                                                <option value="pc">pc</option>
                                                <option value="mm">mm</option>
                                                <option value="cm">cm</option>
                                                <option value="in">in</option>
                                                <option value="em">em</option>
                                                <option value="ex">ex</option>
                                                <option>%</option>
                                            </select>
                                            <div style="display: inline; z-index: 137; left: 376px; width: 56px; position: absolute;
                                                top: 116px; height: 6px">
                                                文本方向</div>
                                            <select id="txt_ddlDirection" style="font-size: 12px; z-index: 138; left: 432px;
                                                width: 120px; position: absolute; top: 112px" onchange="ShowTextExample();">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="ltr">从左到右</option>
                                                <option value="rtl">从右到左</option>
                                            </select>
                                        </div>
                                        <!-- 
								  
								                                               *************                                                         
									                                         ****************                                                        
																			******  位置 ***** 
									                                         ****************
									                                           *************
									                                           
								-->
                                        <div id="tabPosContent" style="width: 634px; position: absolute; top: 1500px; height: 440px">
                                            &nbsp;
                                            <img id="pos_imgPos" style="z-index: 100; left: 24px; position: absolute; top: 16px"
                                                alt="" src="images/pos_notset.gif">
                                            <div style="display: inline; z-index: 101; left: 64px; width: 56px; position: absolute;
                                                top: 16px; height: 15px">
                                                位置模式</div>
                                            <select language="javascript" id="pos_ddlPos" style="font-size: 12px; z-index: 102;
                                                left: 64px; width: 176px; position: absolute; top: 32px" onchange="return pos_ddlPos_onchange()">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="static">正常流中的位置</option>
                                                <option value="relative">正常流的偏移量</option>
                                                <option value="absolute">绝对位置</option>
                                            </select>
                                            <div style="display: inline; z-index: 103; left: 64px; width: 16px; position: absolute;
                                                top: 68px; height: 15px">
                                                顶</div>
                                            <input id="pos_tbTop" style="font-size: 12px; z-index: 104; left: 88px; width: 48px;
                                                position: absolute; top: 64px; height: 22px" type="text" size="2">
                                            <select id="pos_ddlTopUnit" style="font-size: 12px; z-index: 105; left: 144px; width: 56px;
                                                position: absolute; top: 64px">
                                                <option value="px" selected>px</option>
                                                <option value="pt">pt</option>
                                                <option value="pc">pc</option>
                                                <option value="mm">mm</option>
                                                <option value="cm">cm</option>
                                                <option value="in">in</option>
                                                <option value="em">em</option>
                                                <option value="ex">ex</option>
                                                <option>%</option>
                                            </select>
                                            <div style="display: inline; z-index: 106; left: 64px; width: 16px; position: absolute;
                                                top: 100px; height: 15px">
                                                左</div>
                                            <input id="pos_tbLeft" style="font-size: 12px; z-index: 107; left: 88px; width: 48px;
                                                position: absolute; top: 96px; height: 22px" type="text" size="2">
                                            <select id="pos_ddlLeftUnit" style="font-size: 12px; z-index: 108; left: 144px; width: 56px;
                                                position: absolute; top: 96px">
                                                <option value="px" selected>px</option>
                                                <option value="pt">pt</option>
                                                <option value="pc">pc</option>
                                                <option value="mm">mm</option>
                                                <option value="cm">cm</option>
                                                <option value="in">in</option>
                                                <option value="em">em</option>
                                                <option value="ex">ex</option>
                                                <option>%</option>
                                            </select>
                                            <div style="display: inline; z-index: 109; left: 232px; width: 32px; position: absolute;
                                                top: 68px; height: 12px">
                                                高度</div>
                                            <input id="pos_tbHeight" style="font-size: 12px; z-index: 110; left: 272px; width: 48px;
                                                position: absolute; top: 64px; height: 22px" type="text" size="2">
                                            <select id="pos_ddlHeightUnit" style="font-size: 12px; z-index: 111; left: 328px;
                                                width: 56px; position: absolute; top: 64px">
                                                <option value="px" selected>px</option>
                                                <option value="pt">pt</option>
                                                <option value="pc">pc</option>
                                                <option value="mm">mm</option>
                                                <option value="cm">cm</option>
                                                <option value="in">in</option>
                                                <option value="em">em</option>
                                                <option value="ex">ex</option>
                                                <option>%</option>
                                            </select>
                                            <div style="display: inline; z-index: 112; left: 232px; width: 32px; position: absolute;
                                                top: 100px; height: 12px">
                                                宽度</div>
                                            <input id="pos_tbWidth" style="font-size: 12px; z-index: 114; left: 272px; width: 48px;
                                                position: absolute; top: 96px; height: 22px" type="text" size="2">
                                            <select id="pos_ddlWidthUnit" style="font-size: 12px; z-index: 115; left: 328px;
                                                width: 56px; position: absolute; top: 96px">
                                                <option value="px" selected>px</option>
                                                <option value="pt">pt</option>
                                                <option value="pc">pc</option>
                                                <option value="mm">mm</option>
                                                <option value="cm">cm</option>
                                                <option value="in">in</option>
                                                <option value="em">em</option>
                                                <option value="ex">ex</option>
                                                <option>%</option>
                                            </select>
                                            <div style="display: inline; z-index: 113; left: 64px; width: 40px; position: absolute;
                                                top: 132px; height: 15px">
                                                Z 索引</div>
                                            <input id="pos_tbZIndex" style="font-size: 12px; z-index: 116; left: 112px; width: 72px;
                                                position: absolute; top: 128px; height: 22px" type="text" size="6">
                                        </div>
                                        <!-- 
								  
								                                               *************                                                         
									                                         ****************                                                        
																			******  布局 ***** 
									                                         ****************
									                                           *************
									                                           
								-->
                                        <div id="tabLayoutContent" style="width: 634px; position: absolute; top: 2000px;
                                            height: 440px">
                                            <div style="display: inline; z-index: 100; left: 16px; width: 56px; position: absolute;
                                                top: 8px; height: 12px">
                                                流控制&nbsp;
                                            </div>
                                            <hr style="z-index: 101; left: 72px; width: 500px; position: absolute; top: 16px;
                                                height: 1px" width="561" size="1">
                                            <img id="lay_imgVisibility" style="z-index: 107; left: 24px; position: absolute;
                                                top: 32px" alt="" src="images/layout_visibility_notset.gif">
                                            <div style="display: inline; z-index: 102; left: 72px; width: 56px; position: absolute;
                                                top: 32px; height: 14px">
                                                可见性：</div>
                                            <select language="javascript" id="lay_ddlVisibility" style="font-size: 12px; z-index: 104;
                                                left: 72px; width: 144px; position: absolute; top: 48px" onchange="return lay_ddlVisibility_onchange()">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="hidden">隐藏</option>
                                                <option value="visible">可见</option>
                                            </select>
                                            <img id="lay_imgDisplay" style="z-index: 111; left: 24px; position: absolute; top: 80px"
                                                alt="" src="images/layout_display_notset.gif">
                                            <div style="display: inline; z-index: 106; left: 72px; width: 40px; position: absolute;
                                                top: 80px; height: 12px">
                                                显示：</div>
                                            <select language="javascript" id="lay_ddlDisplay" style="font-size: 12px; z-index: 112;
                                                left: 72px; width: 144px; position: absolute; top: 96px" onchange="return lay_ddlDisplay_onchange()">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="none">不显示</option>
                                                <option value="block">作为块元素</option>
                                                <option value="inline">作为内流元素</option>
                                            </select>
                                            <div style="display: inline; z-index: 109; left: 304px; width: 88px; position: absolute;
                                                top: 32px; height: 14px">
                                                允许文本流动：</div>
                                            <img id="lay_imgFloat" style="z-index: 108; left: 256px; position: absolute; top: 32px"
                                                alt="" src="images/layout_float_notset.gif">
                                            <select language="javascript" id="lay_ddlFloat" style="font-size: 12px; z-index: 110;
                                                left: 304px; width: 144px; position: absolute; top: 48px" onchange="return lay_ddlFloat_onchange()">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="none">边上不允许文本</option>
                                                <option value="right">靠右</option>
                                                <option value="left">靠左</option>
                                            </select>
                                            <img id="lay_imgClear" style="z-index: 113; left: 256px; position: absolute; top: 80px"
                                                alt="" src="images/layout_clear_notset.gif">
                                            <div style="display: inline; z-index: 114; left: 304px; width: 88px; position: absolute;
                                                top: 80px; height: 14px">
                                                允许浮动对象：</div>
                                            <select language="javascript" id="lay_ddlClear" style="font-size: 12px; z-index: 115;
                                                left: 304px; width: 144px; position: absolute; top: 96px" onchange="return lay_ddlClear_onchange()">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="both">任何一边</option>
                                                <option value="right">仅右边</option>
                                                <option value="left">仅左边</option>
                                                <option value="none">不允许</option>
                                            </select>
                                            <div style="display: inline; z-index: 117; left: 24px; width: 32px; position: absolute;
                                                top: 128px; height: 14px">
                                                内容&nbsp;</div>
                                            <hr style="z-index: 122; left: 64px; width: 508px; position: absolute; top: 136px;
                                                height: 1px" width="504" size="1">
                                            <div style="display: inline; z-index: 125; left: 40px; width: 32px; position: absolute;
                                                top: 152px; height: 14px">
                                                溢出&nbsp;</div>
                                            <select id="lay_ddlOverflow" style="font-size: 12px; z-index: 128; left: 104px; width: 200px;
                                                position: absolute; top: 152px">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="auto">需要时使用滚动条</option>
                                                <option value="scroll">始终使用滚动条</option>
                                                <option value="visible">不剪辑内容</option>
                                                <option value="hidden">剪辑内容</option>
                                            </select>
                                            <div style="display: inline; z-index: 132; left: 24px; width: 32px; position: absolute;
                                                top: 184px; height: 14px">
                                                剪辑</div>
                                            <hr style="z-index: 133; left: 64px; width: 508px; position: absolute; top: 192px;
                                                height: 1px" width="506" size="1">
                                            <div style="display: inline; z-index: 134; left: 56px; width: 16px; position: absolute;
                                                top: 212px; height: 15px">
                                                顶</div>
                                            <input id="lay_tbClipTop" style="font-size: 12px; z-index: 135; left: 80px; width: 48px;
                                                position: absolute; top: 208px; height: 22px" type="text" size="2">
                                            <select id="lay_ddlClipTopUnit" style="font-size: 12px; z-index: 136; left: 136px;
                                                width: 56px; position: absolute; top: 208px">
                                                <option value="px" selected>px</option>
                                                <option value="pt">pt</option>
                                                <option value="pc">pc</option>
                                                <option value="mm">mm</option>
                                                <option value="cm">cm</option>
                                                <option value="in">in</option>
                                                <option value="em">em</option>
                                                <option value="ex">ex</option>
                                                <option>%</option>
                                            </select>
                                            <div style="display: inline; z-index: 137; left: 56px; width: 16px; position: absolute;
                                                top: 244px; height: 15px">
                                                左</div>
                                            <input id="lay_tbClipLeft" style="font-size: 12px; z-index: 138; left: 80px; width: 48px;
                                                position: absolute; top: 240px; height: 22px" type="text" size="2">
                                            <select id="lay_ddlClipLeftUnit" style="font-size: 12px; z-index: 139; left: 136px;
                                                width: 56px; position: absolute; top: 240px">
                                                <option value="px" selected>px</option>
                                                <option value="pt">pt</option>
                                                <option value="pc">pc</option>
                                                <option value="mm">mm</option>
                                                <option value="cm">cm</option>
                                                <option value="in">in</option>
                                                <option value="em">em</option>
                                                <option value="ex">ex</option>
                                                <option>%</option>
                                            </select>
                                            <div style="display: inline; z-index: 140; left: 224px; width: 32px; position: absolute;
                                                top: 212px; height: 12px">
                                                底</div>
                                            <input id="lay_tbClipBottom" style="font-size: 12px; z-index: 141; left: 264px; width: 48px;
                                                position: absolute; top: 208px; height: 22px" type="text" size="2">
                                            <select id="lay_ddlClipBottomUnit" style="font-size: 12px; z-index: 142; left: 320px;
                                                width: 56px; position: absolute; top: 208px">
                                                <option value="px" selected>px</option>
                                                <option value="pt">pt</option>
                                                <option value="pc">pc</option>
                                                <option value="mm">mm</option>
                                                <option value="cm">cm</option>
                                                <option value="in">in</option>
                                                <option value="em">em</option>
                                                <option value="ex">ex</option>
                                                <option>%</option>
                                            </select>
                                            <div style="display: inline; z-index: 143; left: 224px; width: 32px; position: absolute;
                                                top: 244px; height: 12px">
                                                右</div>
                                            <input id="lay_tbClipRight" style="font-size: 12px; z-index: 144; left: 264px; width: 48px;
                                                position: absolute; top: 240px; height: 22px" type="text" size="2">
                                            <select id="lay_ddlClipRightUnit" style="font-size: 12px; z-index: 145; left: 320px;
                                                width: 56px; position: absolute; top: 240px">
                                                <option value="px" selected>px</option>
                                                <option value="pt">pt</option>
                                                <option value="pc">pc</option>
                                                <option value="mm">mm</option>
                                                <option value="cm">cm</option>
                                                <option value="in">in</option>
                                                <option value="em">em</option>
                                                <option value="ex">ex</option>
                                                <option>%</option>
                                            </select>
                                            <div style="display: inline; z-index: 146; left: 24px; width: 64px; position: absolute;
                                                top: 272px; height: 14px">
                                                打印分页符</div>
                                            <hr style="z-index: 147; left: 88px; width: 487px; position: absolute; top: 282px;
                                                height: 1px" width="489" size="1">
                                            <div style="display: inline; z-index: 148; left: 48px; width: 32px; position: absolute;
                                                top: 300px; height: 15px">
                                                段前</div>
                                            <select id="lay_ddlPbb" style="font-size: 12px; z-index: 149; left: 88px; width: 176px;
                                                position: absolute; top: 296px">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="auto">自动</option>
                                                <option value="always">强制分页</option>
                                            </select>
                                            <div style="display: inline; z-index: 150; left: 288px; width: 32px; position: absolute;
                                                top: 300px">
                                                段后</div>
                                            <select id="lay_ddlPba" style="font-size: 12px; z-index: 151; left: 328px; width: 176px;
                                                position: absolute; top: 296px">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="auto">自动</option>
                                                <option value="always">强制分页</option>
                                            </select>
                                        </div>
                                        <!-- 
								  
								                                               *************                                                         
									                                         ****************                                                        
																			******  边缘 ***** 
									                                         ****************
									                                           *************
									                                           
								-->
                                        <div id="tabEdgeContent" style="width: 634px; position: absolute; top: 2500px; height: 440px">
                                            <div style="display: inline; z-index: 101; left: 16px; width: 32px; position: absolute;
                                                top: 8px; height: 14px">
                                                边距&nbsp;
                                            </div>
                                            <hr style="z-index: 102; left: 56px; width: 129px; position: absolute; top: 16px;
                                                height: 1px" width="129" size="1">
                                            <div style="display: inline; z-index: 103; left: 40px; width: 24px; position: absolute;
                                                top: 36px; height: 14px">
                                                顶：</div>
                                            <select id="edg_ddlMarginTopUnit" style="font-size: 12px; z-index: 104; left: 128px;
                                                width: 56px; position: absolute; top: 32px" onchange="edg_ShowExample();">
                                                <option value="px" selected>px</option>
                                                <option value="pt">pt</option>
                                                <option value="pc">pc</option>
                                                <option value="mm">mm</option>
                                                <option value="cm">cm</option>
                                                <option value="in">in</option>
                                                <option value="em">em</option>
                                                <option value="ex">ex</option>
                                                <option>%</option>
                                            </select>
                                            <input id="edg_tbMarginTop" style="font-size: 12px; z-index: 105; left: 72px; width: 48px;
                                                position: absolute; top: 32px; height: 22px" type="text" onchange="edg_ShowExample();"
                                                size="2">
                                            <div style="display: inline; z-index: 106; left: 40px; width: 24px; position: absolute;
                                                top: 60px; height: 14px">
                                                底：</div>
                                            <input id="edg_tbMarginBottom" style="font-size: 12px; z-index: 107; left: 72px;
                                                width: 48px; position: absolute; top: 56px; height: 22px" type="text" onchange="edg_ShowExample();"
                                                size="2">
                                            <select id="edg_ddlMarginBottomUnit" style="font-size: 12px; z-index: 108; left: 128px;
                                                width: 56px; position: absolute; top: 56px" onchange="edg_ShowExample();">
                                                <option value="px" selected>px</option>
                                                <option value="pt">pt</option>
                                                <option value="pc">pc</option>
                                                <option value="mm">mm</option>
                                                <option value="cm">cm</option>
                                                <option value="in">in</option>
                                                <option value="em">em</option>
                                                <option value="ex">ex</option>
                                                <option>%</option>
                                            </select>
                                            <div style="display: inline; z-index: 109; left: 40px; width: 24px; position: absolute;
                                                top: 84px; height: 14px">
                                                左：</div>
                                            <input id="edg_tbMarginLeft" style="font-size: 12px; z-index: 110; left: 72px; width: 48px;
                                                position: absolute; top: 80px; height: 22px" type="text" onchange="edg_ShowExample();"
                                                size="2">
                                            <select id="edg_ddlMarginLeftUnit" style="font-size: 12px; z-index: 111; left: 128px;
                                                width: 56px; position: absolute; top: 80px" onchange="edg_ShowExample();">
                                                <option value="px" selected>px</option>
                                                <option value="pt">pt</option>
                                                <option value="pc">pc</option>
                                                <option value="mm">mm</option>
                                                <option value="cm">cm</option>
                                                <option value="in">in</option>
                                                <option value="em">em</option>
                                                <option value="ex">ex</option>
                                                <option>%</option>
                                            </select>
                                            <div style="display: inline; z-index: 112; left: 40px; width: 24px; position: absolute;
                                                top: 108px; height: 14px">
                                                右：</div>
                                            <input id="edg_tbMarginRight" style="font-size: 12px; z-index: 113; left: 72px; width: 48px;
                                                position: absolute; top: 104px; height: 22px" type="text" onchange="edg_ShowExample();"
                                                size="2">
                                            <select id="edg_ddlMarginRightUnit" style="font-size: 12px; z-index: 114; left: 128px;
                                                width: 56px; position: absolute; top: 104px" onchange="edg_ShowExample();">
                                                <option value="px" selected>px</option>
                                                <option value="pt">pt</option>
                                                <option value="pc">pc</option>
                                                <option value="mm">mm</option>
                                                <option value="cm">cm</option>
                                                <option value="in">in</option>
                                                <option value="em">em</option>
                                                <option value="ex">ex</option>
                                                <option>%</option>
                                            </select>
                                            <div style="display: inline; z-index: 116; left: 224px; width: 32px; position: absolute;
                                                top: 8px; height: 14px">
                                                空白&nbsp;</div>
                                            <hr style="z-index: 118; left: 264px; width: 129px; position: absolute; top: 16px;
                                                height: 1px" width="129" size="1">
                                            <div style="display: inline; z-index: 122; left: 248px; width: 24px; position: absolute;
                                                top: 36px; height: 14px">
                                                顶：</div>
                                            <select id="edg_ddlPadTopUnit" style="font-size: 12px; z-index: 120; left: 336px;
                                                width: 56px; position: absolute; top: 32px" onchange="edg_ShowExample();">
                                                <option value="px" selected>px</option>
                                                <option value="pt">pt</option>
                                                <option value="pc">pc</option>
                                                <option value="mm">mm</option>
                                                <option value="cm">cm</option>
                                                <option value="in">in</option>
                                                <option value="em">em</option>
                                                <option value="ex">ex</option>
                                                <option>%</option>
                                            </select>
                                            <input id="edg_tbPadTop" style="font-size: 12px; z-index: 123; left: 280px; width: 48px;
                                                position: absolute; top: 32px; height: 22px" type="text" onchange="edg_ShowExample();"
                                                size="2">
                                            <div style="display: inline; z-index: 128; left: 248px; width: 24px; position: absolute;
                                                top: 60px; height: 14px">
                                                底：</div>
                                            <input id="edg_tbPadBottom" style="font-size: 12px; z-index: 124; left: 280px; width: 48px;
                                                position: absolute; top: 56px; height: 22px" type="text" onchange="edg_ShowExample();"
                                                size="2">
                                            <select id="edg_ddlPadBottomUnit" style="font-size: 12px; z-index: 117; left: 336px;
                                                width: 56px; position: absolute; top: 56px" onchange="edg_ShowExample();">
                                                <option value="px" selected>px</option>
                                                <option value="pt">pt</option>
                                                <option value="pc">pc</option>
                                                <option value="mm">mm</option>
                                                <option value="cm">cm</option>
                                                <option value="in">in</option>
                                                <option value="em">em</option>
                                                <option value="ex">ex</option>
                                                <option>%</option>
                                            </select>
                                            <div style="display: inline; z-index: 119; left: 248px; width: 24px; position: absolute;
                                                top: 84px; height: 14px">
                                                左：</div>
                                            <input id="edg_tbPadLeft" style="font-size: 12px; z-index: 121; left: 280px; width: 48px;
                                                position: absolute; top: 80px; height: 22px" type="text" onchange="edg_ShowExample();"
                                                size="2">
                                            <select id="edg_ddlPadLeftUnit" style="font-size: 12px; z-index: 125; left: 336px;
                                                width: 56px; position: absolute; top: 80px" onchange="edg_ShowExample();">
                                                <option value="px" selected>px</option>
                                                <option value="pt">pt</option>
                                                <option value="pc">pc</option>
                                                <option value="mm">mm</option>
                                                <option value="cm">cm</option>
                                                <option value="in">in</option>
                                                <option value="em">em</option>
                                                <option value="ex">ex</option>
                                                <option>%</option>
                                            </select>
                                            <div style="display: inline; z-index: 126; left: 248px; width: 24px; position: absolute;
                                                top: 108px; height: 14px">
                                                右：</div>
                                            <input id="edg_tbPadRight" style="font-size: 12px; z-index: 127; left: 280px; width: 48px;
                                                position: absolute; top: 104px; height: 22px" type="text" onchange="edg_ShowExample();"
                                                size="2">
                                            <select id="edg_ddlPadRightUnit" style="font-size: 12px; z-index: 115; left: 336px;
                                                width: 56px; position: absolute; top: 104px" onchange="edg_ShowExample();">
                                                <option value="px" selected>px</option>
                                                <option value="pt">pt</option>
                                                <option value="pc">pc</option>
                                                <option value="mm">mm</option>
                                                <option value="cm">cm</option>
                                                <option value="in">in</option>
                                                <option value="em">em</option>
                                                <option value="ex">ex</option>
                                                <option>%</option>
                                            </select>
                                            <div style="display: inline; z-index: 129; left: 16px; width: 32px; position: absolute;
                                                top: 136px; height: 15px">
                                                边框</div>
                                            <hr style="z-index: 130; left: 48px; width: 67.4%; position: absolute; top: 144px;
                                                height: 1px" width="67.4%" size="1">
                                            <img id="edg_imgBorder" style="z-index: 131; left: 40px; position: absolute; top: 160px"
                                                height="32" alt="" src="images/edg_border_all.gif" width="32">
                                            <div style="display: inline; z-index: 132; left: 80px; width: 104px; position: absolute;
                                                top: 160px; height: 16px">
                                                选择要更改的边缘</div>
                                            <select language="javascript" id="edg_ddlBorder" style="font-size: 12px; z-index: 133;
                                                left: 80px; width: 112px; position: absolute; top: 176px" onchange="return edg_ddlBorder_onchange()">
                                                <option value="top">顶</option>
                                                <option value="bottom">底</option>
                                                <option value="left">左</option>
                                                <option value="right">右</option>
                                                <option value="all" selected>全部</option>
                                            </select>
                                            <div style="display: inline; z-index: 134; left: 72px; width: 32px; position: absolute;
                                                top: 210px; height: 15px">
                                                样式</div>
                                            <select language="javascript" id="edg_ddlBorderStyle" style="font-size: 12px; z-index: 135;
                                                left: 112px; width: 136px; position: absolute; top: 208px" onchange="return edg_ddlBorderStyle_onchange()">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="none">无</option>
                                                <option value="solid">实线</option>
                                                <option value="double">双线</option>
                                                <option value="groove">凹线</option>
                                                <option value="ridge">凸线</option>
                                                <option value="inset">內嵌</option>
                                                <option value="outset">外嵌</option>
                                            </select>
                                            <div style="display: inline; z-index: 136; left: 72px; width: 32px; position: absolute;
                                                top: 244px; height: 15px">
                                                宽度</div>
                                            <select language="javascript" id="edg_ddlBorderWidth" style="font-size: 12px; z-index: 137;
                                                left: 112px; width: 136px; position: absolute; top: 240px" onchange="return edg_ddlBorderWidth_onchange()">
                                                <option value="notset" selected>&lt;未设置&gt;</option>
                                                <option value="thin">细</option>
                                                <option value="">中</option>
                                                <option value="thick">粗</option>
                                                <option value="custom">自定义</option>
                                            </select>
                                            <select id="edg_ddlBorderWidthUnit" style="font-size: 12px; z-index: 138; left: 312px;
                                                width: 56px; position: absolute; top: 240px" disabled onchange="edg_ShowExample();">
                                                <option value="px" selected>px</option>
                                                <option value="pt">pt</option>
                                                <option value="pc">pc</option>
                                                <option value="mm">mm</option>
                                                <option value="cm">cm</option>
                                                <option value="in">in</option>
                                                <option value="em">em</option>
                                                <option value="ex">ex</option>
                                                <option>%</option>
                                            </select>
                                            <input id="edg_tbBorderWidth" style="font-size: 12px; z-index: 139; left: 256px;
                                                width: 48px; position: absolute; top: 240px; height: 22px" disabled type="text"
                                                onchange="edg_ShowExample();" size="2">
                                            <div style="display: inline; z-index: 140; left: 72px; width: 32px; position: absolute;
                                                top: 276px; height: 15px">
                                                颜色</div>
                                            <input language="javascript" id="edg_tbBorderColor" style="font-size: 12px; z-index: 141;
                                                left: 112px; width: 88px; position: absolute; top: 272px; height: 22px" type="text"
                                                onchange="edg_ShowExample();" size="9">
                                            <input id="edg_btnBorderColor" style="font-size: 12px; z-index: 142; left: 208px;
                                                position: absolute; top: 272px" onclick="selectColor('edg_tbBorderColor');" type="button"
                                                value="...">
                                        </div>
                                        <!-- 
								  
								                                               *************                                                         
									                                         ****************                                                        
																			******  列表 ***** 
									                                         ****************
									                                           *************
									                                           
								-->
                                        <div id="tabListContent" style="width: 634px; position: absolute; top: 3000px; height: 440px">
                                            <div style="display: inline; z-index: 101; left: 16px; width: 56px; position: absolute;
                                                top: 40px; height: 12px">
                                                项目符号
                                            </div>
                                            <hr style="z-index: 102; left: 72px; width: 500px; position: absolute; top: 48px;
                                                height: 1px" width="561" size="1">
                                            <div style="display: inline; z-index: 103; left: 16px; width: 40px; position: absolute;
                                                top: 12px; height: 12px">
                                                列表：</div>
                                            <select language="javascript" id="lst_ddlStyle" style="font-size: 12px; z-index: 104;
                                                left: 64px; width: 160px; position: absolute; top: 8px" onchange="return lst_ddlStyle_onchange()">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="display">显示项目符号</option>
                                                <option value="none">取消项目符号</option>
                                            </select>
                                            <select id="lst_ddlStyleType" style="font-size: 12px; z-index: 105; left: 88px; width: 208px;
                                                position: absolute; top: 64px" disabled onchange="lst_ShowExample();">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="circle">圆形</option>
                                                <option value="disk">圆盘</option>
                                                <option value="square">正方形</option>
                                                <option value="decimal">数字（1 2 3 4...）</option>
                                                <option value="lower-roman">小写罗马数字（i ii iii iv...）</option>
                                                <option value="upper-roman">大写罗马数字（I II III IV...）</option>
                                                <option value="lower-alpha">小写字母（a b c d...）</option>
                                                <option value="upper-alpha">大写字母（A B C D...）</option>
                                            </select>
                                            <div style="display: inline; z-index: 106; left: 40px; width: 40px; position: absolute;
                                                top: 68px; height: 12px">
                                                样式：</div>
                                            <div style="display: inline; z-index: 107; left: 40px; width: 40px; position: absolute;
                                                top: 100px; height: 14px">
                                                位置：</div>
                                            <select id="lst_ddlStylePos" style="font-size: 12px; z-index: 108; left: 88px; width: 208px;
                                                position: absolute; top: 96px" disabled onchange="lst_ShowExample();">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="outside">外侧（文本缩进）</option>
                                                <option value="inside">内侧（文本不缩进）</option>
                                            </select>
                                            <input language="javascript" id="lst_cbCustomImage" style="z-index: 109; left: 40px;
                                                position: absolute; top: 128px" disabled onclick="return lst_cbCustomImage_onclick()"
                                                type="checkbox">
                                            <div style="display: inline; z-index: 110; left: 64px; width: 88px; position: absolute;
                                                top: 132px; height: 14px">
                                                <label for="lst_cbCustomImage">
                                                    自定义项目编号</label></div>
                                            <input language="javascript" id="lst_rbStyleImage" style="z-index: 111; left: 56px;
                                                position: absolute; top: 152px" disabled onclick="return lst_rblStyleImage_onclick()"
                                                type="radio" checked name="lst_rblStyleImage">
                                            <div style="display: inline; z-index: 112; left: 80px; width: 32px; position: absolute;
                                                top: 156px; height: 15px">
                                                <label for="lst_rblStyleImage">
                                                    图像</label></div>
                                            <input id="lst_tbStyleImage" style="font-size: 12px; z-index: 113; left: 120px; width: 336px;
                                                position: absolute; top: 152px; height: 22px" disabled type="text" onchange="lst_ShowExample();"
                                                size="50">
                                            <input id="lst_btnStyleImage" style="font-size: 12px; z-index: 114; left: 464px;
                                                width: 24px; position: absolute; top: 150px; height: 24px" onclick="javascript:document.all['lst_tbStyleImage'].value=window.showModalDialog('../ftp/default.aspx?obj=0','','dialogHeight:525px;dialogWidth:730px;center:yes;resizable:no;scroll:yes;status:no');"
                                                disabled type="button" value="...">
                                            <input language="javascript" id="lst_rbStyleImageNone" style="z-index: 115; left: 56px;
                                                position: absolute; top: 176px" disabled onclick="return lst_rblStyleImage_onclick()"
                                                type="radio" name="lst_rblStyleImage">
                                            <div style="display: inline; z-index: 116; left: 80px; width: 16px; position: absolute;
                                                top: 180px; height: 15px">
                                                无</div>
                                        </div>
                                        <!-- 
								  
								                                               *************                                                         
									                                         ****************                                                        
																			******  滤镜 ***** 
									                                         ****************
									                                           *************
									                                           
								-->
                                        <div id="tabFilterContent" style="width: 640px; position: absolute; top: 3500px;
                                            height: 440px">
                                            <div style="display: inline; z-index: 101; left: 16px; width: 56px; position: absolute;
                                                top: 8px; height: 12px">
                                                滤镜属性&nbsp;
                                            </div>
                                            <select language="javascript" id="flt_lbFilter" style="font-size: 12px; z-index: 102;
                                                left: 16px; width: 144px; position: absolute; top: 24px; height: 228px" onchange="return flt_lbFilter_onchange()"
                                                size="14">
                                                <option value="让HTML元件呈现出透明的渐进效果。" selected pararemark="Opacity:不透明的程度，百分比。从0到100，0表是完全透明，100表示完全不透明。;FinishOpacit:设置渐变的透明效果时，用来指定结束时的透明度，范围是0 到 100。;Style:设置渐变透明的样式，值为0代表统一形状、1代表线形、2代表放射状、3代表长方形。;StartX:代表渐变透明效果的开始X坐标。;StartY:代表渐变透明效果的开始Y坐标。;FinishX:代表渐变透明效果结束 X 的坐标。;FinishY:代表渐变透明效果结束 Y 的坐标。;">
                                                    Alpha</option>
                                                <option value="让HTML元件产生风吹模糊的效果" pararemark="Add:是否单方向模糊，此参数是一个布尔值，true（非0）或false（0）。;Direction:设置模糊的方向，其中0度代表垂直向上，然后每45度为一个单位。;Strength:代表模糊的象素值。;">
                                                    Blur</option>
                                                <option value="图像之间的淡入和淡出的效果" pararemark="Duration:淡入或淡出的时间;">BlendTrans</option>
                                                <option value="让图像中的某一颜色变成透明色" pararemark="Color:是指要设置为透明的颜色。;">Chroma</option>
                                                <option value="让HTML元件有一个下落式的阴影" pararemark="Color:指定阴影的颜色。;OffX:指定阴影相对于元素在水平方向偏移量，整数。;OffY:指定阴影相对于元素在垂直方向偏移量，整数。;Positive:是一个布尔值，值为true（非0）时，表示为建立外阴影；为false(0)，表示为建立内阴影。;">
                                                    DropShadow</option>
                                                <option value="让HTML元件水平翻转" pararemark="">FlipH</option>
                                                <option value="让HTML元件垂直翻转" pararemark="">FlipV</option>
                                                <option value="在元件的周围产生光晕而模糊的效果" pararemark="Color:指定发光的颜色。;Strength:光的强度，可以是1到255之间的任何整数，数字越大，发光的范围就越大。;">
                                                    Glow</option>
                                                <option value="把一个彩色的图片变成黑白色" pararemark="">Gray</option>
                                                <option value="产生图片的照片底片的效果" pararemark="">Invert</option>
                                                <option value="在HTML元件上放置一个光影" pararemark="">Light</option>
                                                <option value="利用另一个HTML元件在另一个元件上产生图像的遮罩" pararemark="Color:设置底色，让对象遮住底色的部分透明。;">Mask</option>
                                                <option value="建立切换效果" pararemark="Duration:切换时间，以秒为单位。;Transtition:切换方式，可设置为从0到23。;">
                                                    RevealTrans</option>
                                                <option value="产生一个比较立体的阴影" pararemark="Color:指阴影的颜色。;Direction:设置投影的方向，0度代表垂直向上，然后每45度为一个单位。;">
                                                    Shadow</option>
                                                <option value="让HTML元件产生水平或是垂直方向上的波浪变形" pararemark="Add:表示是否显示原对象，0表示不显示，非0表示要显示原对象。;Freq:设置波动的个数。;LightStrength:设置波浪效果的光照强度，从0到100。0表示最弱，100表示最强。;Phase:波浪的起始相角。从0到100的百分数值。（例如：25相当于90度，而50相当于180度。）;Strength:设置波浪摇摆的幅度。;">
                                                    Wave</option>
                                                <option value="产生HTML元件的轮廓，就像是照X光一样" pararemark="">XRay</option>
                                            </select>
                                            <div style="display: inline; z-index: 103; left: 192px; width: 72px; position: absolute;
                                                top: 8px; height: 14px">
                                                选定的滤镜</div>
                                            <div id="flt_lblFilterRemark" style="display: inline; z-index: 104; left: 16px; width: 320px;
                                                position: absolute; top: 256px; height: 42px">
                                            </div>
                                            <div id="flt_lblParaRemark" style="display: inline; z-index: 105; left: 376px; width: 240px;
                                                position: absolute; top: 216px; height: 82px">
                                            </div>
                                            <input language="javascript" id="flt_tbParaValue" style="font-size: 12px; z-index: 106;
                                                left: 376px; width: 180px; position: absolute; top: 192px; height: 21px" type="text"
                                                onchange="return flt_tbParaValue_onchange()" size="25">
                                            <div id="flt_lblPara" style="display: inline; z-index: 107; left: 376px; width: 176px;
                                                position: absolute; top: 176px; height: 15px" nowrap>
                                                参数值</div>
                                            <select language="javascript" id="flt_lbSelectedFilter" style="z-index: 108; left: 192px;
                                                width: 144px; position: absolute; top: 24px; height: 222px" onchange="return flt_lbSelectedFilter_onchange()"
                                                size="14" name="lbSelectedFont">
                                            </select>
                                            <img id="flt_imgMoveUp" style="z-index: 109; left: 344px; cursor: hand; position: absolute;
                                                top: 24px" onclick="flt_lbSelectedFilter_MoveUp();" alt="上移" src="images/font_up.GIF">
                                            <img id="flt_imgMoveDown" style="z-index: 110; left: 344px; cursor: hand; position: absolute;
                                                top: 56px" onclick="flt_lbSelectedFilter_MoveDown();" alt="下移" src="images/font_down.GIF">
                                            <img id="flt_imgRemove" style="z-index: 111; left: 344px; width: 16px; cursor: hand;
                                                position: absolute; top: 224px; height: 16px" onclick="flt_lbSelectedFilter_Remove();"
                                                height="16" alt="移除" src="images/font_remove.ico" width="16">
                                            <input language="javascript" id="flt_btnAddFilter" style="z-index: 112; left: 168px;
                                                position: absolute; top: 120px" onclick="return flt_btnAddFilter_onclick(null)"
                                                type="button" value="->">
                                            <select language="javascript" id="flt_lbPara" style="z-index: 113; left: 376px; width: 144px;
                                                position: absolute; top: 24px; height: 146px" onchange="return flt_lbPara_onchange()"
                                                size="9">
                                            </select>
                                            <div style="display: inline; z-index: 114; left: 376px; width: 56px; position: absolute;
                                                top: 8px; height: 15px">
                                                参数列表</div>
                                        </div>
                                        <!-- 
								  
								                                               *************                                                         
									                                         ****************                                                        
																			******  其它 ***** 
									                                         ****************
									                                           *************
									                                           
								-->
                                        <div id="tabOtherContent" style="width: 634px; position: absolute; top: 4000px; height: 440px">
                                            <div style="display: inline; z-index: 101; left: 16px; width: 56px; position: absolute;
                                                top: 8px; height: 12px">
                                                用户界面
                                            </div>
                                            <hr style="z-index: 102; left: 72px; width: 500px; position: absolute; top: 16px;
                                                height: 1px" width="561" size="1">
                                            <div style="display: inline; z-index: 103; left: 80px; width: 40px; position: absolute;
                                                top: 24px; height: 12px">
                                                光标：</div>
                                            <img id="oth_imgCursor" style="z-index: 107; left: 40px; position: absolute; top: 24px"
                                                height="32" alt="" src="images/oth_notset.gif" width="32">
                                            <select language="javascript" id="oth_ddlCursor" style="font-size: 12px; z-index: 104;
                                                left: 80px; width: 136px; position: absolute; top: 40px" onchange="return oth_ddlCursor_onchange()">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="auto">自动</option>
                                                <option value="default">默认</option>
                                                <option value="crosshair">十字线</option>
                                                <option value="hand">手形</option>
                                                <option value="move">移动</option>
                                                <option value="n-resize">向上调整大小</option>
                                                <option value="s-resize">向下调整大小</option>
                                                <option value="w-resize">向左调整大小</option>
                                                <option value="e-resize">向右调整大小</option>
                                                <option value="nw-resize">向左上角调整大小</option>
                                                <option value="sw-resize">向左下角调整大小</option>
                                                <option value="ne-resize">向右上角调整大小</option>
                                                <option value="se-resize">向右下角调整大小</option>
                                                <option value="text">文本</option>
                                                <option value="wait">沙漏</option>
                                                <option value="help">帮助</option>
                                            </select>
                                            <div style="display: inline; z-index: 106; left: 80px; width: 40px; position: absolute;
                                                top: 80px; height: 12px">
                                                边框：</div>
                                            <img id="oth_imgBdrColl" style="z-index: 110; left: 40px; position: absolute; top: 80px"
                                                height="32" alt="" src="images/oth_borderCollapse_notset.gif" width="32">
                                            <select language="javascript" id="oth_ddlBdrColl" style="font-size: 12px; z-index: 105;
                                                left: 80px; width: 136px; position: absolute; top: 96px" onchange="return oth_ddlBdrColl_onchange()">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="separate">分隔单元格边框</option>
                                                <option value="collapse">折叠单元格边框</option>
                                            </select>
                                            <div style="display: inline; z-index: 108; left: 16px; width: 16px; position: absolute;
                                                top: 64px; height: 15px">
                                                表</div>
                                            <hr style="z-index: 109; left: 32px; width: 538px; position: absolute; top: 72px;
                                                height: 1px" width="538" size="1">
                                            <div style="display: inline; z-index: 111; left: 240px; width: 40px; position: absolute;
                                                top: 80px; height: 12px">
                                                布局：</div>
                                            <select id="oth_ddlTblLay" style="font-size: 12px; z-index: 112; left: 240px; width: 136px;
                                                position: absolute; top: 96px">
                                                <option value="" selected>&lt;未设置&gt;</option>
                                                <option value="auto">自动</option>
                                                <option value="fixed">固定布局</option>
                                            </select>
                                            <div style="display: inline; z-index: 113; left: 16px; width: 24px; position: absolute;
                                                top: 132px; height: 15px">
                                                URL</div>
                                            <input id="oth_tbBehavior" style="font-size: 12px; z-index: 114; left: 48px; width: 424px;
                                                position: absolute; top: 128px; height: 22px" type="text" size="65">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <hr style="z-index: 101; left: 80px; width: 526px; position: absolute; top: 16px;
                                height: 1px" width="595" size="1">
                            <hr style="z-index: 100; left: 80px; width: 526px; position: absolute; top: 16px;
                                height: 1px" width="595" size="1">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
