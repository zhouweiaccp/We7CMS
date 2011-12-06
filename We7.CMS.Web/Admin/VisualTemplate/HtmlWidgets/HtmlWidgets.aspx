<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HtmlWidgets.aspx.cs" Inherits="We7.CMS.Web.Admin.VisualTemplate.HtmlWidgets.HtmlWidgets" %>

<%@ Register Assembly="FCKeditor.net" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<META http-equiv="Content-Type" content="text/html;charset=UTF-8">
<head runat="server">
    <title></title>
    <style type="text/css">
        h2.title
        {
            margin-top: 5px;
            margin-bottom: 4px;
        }
        body
        {
            margin: 5px;
        }
        #NameLabel:hover
        {
            border: solid 1px #888;
            background-color: White;
            cursor: pointer;
        }
        
#fullbg{
background-color: Gray;
display:none;
z-index:3;
position:absolute;
left:0px;
top:0px;
filter:Alpha(Opacity=30);
/* IE */
-moz-opacity:0.4;
/* Moz + FF */
opacity: 0.4;
}

#dialog {
position:absolute;
width:200px;
height:200px;
display: none;
z-index: 5;
}
#main
{
height:1000px;    
}

    </style>
    <link rel="stylesheet" type="text/css" href="../theme/classic/css/main.css" media="screen" />
    <script src="<%=AppPath%>/cgi-bin/DialogHelper.js" type="text/javascript"></script>
    <script src="../Scripts/vdMain.js" type="text/javascript"></script>
    <script type="text/javascript" src="<%=AppPath%>/ajax/jquery/jquery.latest.min.js"></script>
    <script type="text/javascript" src="/Admin/cgi-bin/CheckBrowser.js"></script>
    <script type="text/javascript" charset="utf-8">
        $(document).ready(function () {
            $("#NameTextbox").hide();
            $('#NameLabel').show();
            $('#NameLabel').text($('#NameTextbox').val());
            $('#NameLabel').click(function (e) {
                $("#NameTextbox").val($('#NameLabel').text());
                $("#NameTextbox").show();
                $('#NameLabel').hide();
                var count = $("#NameLabel").text().length;
                $("#NameTextbox").css("width", count * 26);
            });
            $('#NameTextbox').blur(function (e) {
                $("#NameTextbox").hide();
                $('#NameLabel').show();
                $("#NameLabel").text($('#NameTextbox').val());
            });
        });
        function SaveTemplate() {
            var nameText = document.getElementById("NewCopyNameTextBox");
            nameText.value = ""; 
            var saveBtn = document.getElementById("<%=SaveButton.ClientID %>");
            if (saveBtn) saveBtn.click();
            showBg('dialog', 'dialog_content');
        }
        function Close(value) {
            value = value.replace(/\~/g, '/');
            window.returnValue = value;
            window.close();
            //document.getElementById("saveAricleButton").disabled = "true";
        }

        //显示灰色JS遮罩层
        function showBg(ct, content) {
            var bH = $("body").height();
            var bW = $("body").width() + 16;
            var objWH = getObjWh(ct);
            $("#fullbg").css({ width: bW, height: bH, display: "block" });
            var tbT = objWH.split("|")[0] + "px";
            var tbL = objWH.split("|")[1] + "px";
            $("#" + ct).css({ top: tbT, left: tbL, display: "block" });
            // $("#" + content).html("<div style='text-align:center'>正在加载，请稍后...</div>");
            $(window).scroll(function () { resetBg() });
            $(window).resize(function () { resetBg() });
        }
        function getObjWh(obj) {
            var st = document.documentElement.scrollTop; //滚动条距顶部的距离
            var sl = document.documentElement.scrollLeft; //滚动条距左边的距离
            var ch = document.documentElement.clientHeight; //屏幕的高度
            var cw = document.documentElement.clientWidth; //屏幕的宽度
            var objH = $("#" + obj).height(); //浮动对象的高度
            var objW = $("#" + obj).width(); //浮动对象的宽度
            var objT = Number(st) + (Number(ch) - Number(objH)) / 2;
            var objL = Number(sl) + (Number(cw) - Number(objW)) / 2;
            return objT + "|" + objL;
        }
        function resetBg() {
            var fullbg = $("#fullbg").css("display");
            if (fullbg == "block") {
                var bH2 = $("body").height();
                var bW2 = $("body").width() + 16;
                $("#fullbg").css({ width: bW2, height: bH2 });
                var objV = getObjWh("dialog");
                var tbT = objV.split("|")[0] + "px";
                var tbL = objV.split("|")[1] + "px";
                $("#dialog").css({ top: tbT, left: tbL });
            }
        }

        //关闭灰色JS遮罩层和操作窗口
        function closeBg() {
            $("#fullbg").css("display", "none");
            $("#dialog").css("display", "none");
        }
    </script>
</head>
<body class="we7-admin" onload="MaxTheWindow();" id="body">
    <form id="mainForm" runat="server">
    <div id="wrap">
        <h2 class="title">
            <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_look.gif" />
            <asp:Literal runat="server" ID="ActionLiteral"></asp:Literal>
            『<asp:TextBox runat="server" ID="NameTextbox" Text="我的部件"></asp:TextBox>
            <asp:Label ID="NameLabel" runat="server" Text="部件编辑">
            </asp:Label>』 <span class="summary" style="width: 300px">保存到文件
                <asp:Label ID="SummaryLabel" runat="server" Text="">
                </asp:Label>
                <asp:TextBox runat="server" Style="font-size: 12px; width: 150px" ID="FilenameTextBox"
                    Text="templateName"></asp:TextBox>
                <asp:Label ID="lblascx" Text=".ascx" runat="server" />
            </span>
        </h2>
        <WEC:MessagePanel ID="Messages" runat="server">
        </WEC:MessagePanel>
    </div>
    <div style="width: 100%; border: solid 0px black; position: absolute; z-index: 1"
        id="divEditor">
        <FCKeditorV2:FCKeditor ID="WidgetContentTextBox" runat="server" Height="450px"
            Width="100%" FullPage="true" ToolbarSet="composeHtmlWidget" BasePath="/admin/fckeditor/"
            FillEmptyBlocks="false" EnableSourceXHTML="true" EnableXHTML="true" HtmlEncodeOutput="false"
            UseBROnCarriageReturn="true" WorkPlace="compose">
        </FCKeditorV2:FCKeditor>
        <div class="editorFooter" style="width: 100%; margin-right: 5px">
        </div>
    </div>
    <div style="display:none">
       <asp:TextBox ID="NewCopyNameTextBox" runat="server"></asp:TextBox>
     <asp:Button ID="SaveButton" runat="server" Text="Save" OnClick="SaveButton_Click" />
       <input type="button" id="saveAricleButton" onclick="SaveTemplate()" runat="server" />
     </div>
     <asp:HiddenField ID="hfdValue" runat="server" />
    </form>
    
    <div id="main">
<div id="dialog_content">
</div>
</div>
<!-- JS遮罩层 -->
<div id="fullbg"></div>
<!-- end JS遮罩层 -->
<!-- 对话框 -->
<div id="dialog">
<div style="text-align: center;"><img src='../../Images/SaveLoding.gif' /><br /><b>正在保存中，请稍后...</b></div>
</div>
<!-- JS遮罩层上方的对话框 -->
</body>


</html>
