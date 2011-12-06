<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WidgetEditor.aspx.cs"
 Inherits="We7.CMS.Web.Admin.VisualTemplate.WidgetEditor" ValidateRequest="false" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>部件编辑</title>
    <base target="_self" />
    <style type="text/css">
        input, selectd
        {
            margin: 0;
        }
        td
        {
            font-size: 14px;
        }        
        body{
            margin:0px auto;  
        }    
        .CodeMirror-wrapping{
            border:1px solid #aaa;
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

#main {
height: 1000px;
} 
    </style>
    <link rel="stylesheet" type="text/css" href="/admin/ajax/CodeMirror/css/docs.css"/>
    <script src="/admin/ajax/CodeMirror/js/codemirror.js" type="text/javascript"></script>
   <%-- <link href="/admin/ajax/CodeMirror2/lib/codemirror.css" type="text/css" rel="Stylesheet" />
    <link href="/admin/ajax/CodeMirror2/css/docs.css" type="text/css" rel="Stylesheet" />
    <link href="/admin/ajax/CodeMirror2/mode/css/css.css" type="text/css" rel="Stylesheet" />
    <link href="/admin/ajax/CodeMirror2/mode/xml/xml.css" type="text/css" rel="Stylesheet" />
    <link href="/admin/ajax/CodeMirror2/mode/javascript/javascript.css" type="text/css" rel="Stylesheet" />
   
    <script src="/admin/ajax/CodeMirror2/lib/codemirror.js" type="text/javascript"></script>
    <script src="/admin/ajax/CodeMirror2/mode/xml/xml.js" type="text/javascript"></script>
    <script src="/admin/ajax/CodeMirror2/mode/htmlmixed/htmlmixed.js" type="text/javascript"></script>
    <script src="/admin/ajax/CodeMirror2/mode/javascript/javascript.js" type="text/javascript"></script>--%>
     <script type="text/javascript" src="<%=AppPath%>/ajax/jquery/jquery.latest.min.js"></script>
    <script type="text/javascript">
        function store() {
            document.getElementById("hdFileName").value = "";
             var r = window.showModalDialog('WidgetSaveAs.htm?20110418011', '<%=Request["ctr"] %>', 'scrollbars=no;resizable=no;help=no;status=no;center=1; dialogHeight=160px;dialogwidth=430px;');
             if (r) {
                 showBg('dialog', 'dialog_content');
                document.getElementById("hdFileName").value = r.name;
                return true;
            }
            return false;
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
        function Close(value) {
            value = value.replace(/\~/g, '/');
            window.returnValue = value;
            window.close();
        }
    </script>

</head>
<body style="Margin:0 auto;Padding:0;">
    <form id="form1" runat="server">
    <div>
        <table width="100%">
            <tr>
                <td>
                    <asp:TextBox ID="CtrCodeTextBox" runat="server" TextMode="MultiLine" Width="700px"
                        style="border: 2px solid #aaa;"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="SaveButton" runat="server" Text="　　保存　　" OnClick="SaveButton_Click" />
                            </td>
                            <td>
                                <asp:Button ID="StoreButton" OnClientClick="return store();" runat="server" Text="　　另存为　　" OnClick="StoreButton_Click" />
                                <asp:HiddenField ID="hdFileName" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="msgLabel" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
      <script type="text/javascript">
//          var editor =
//            CodeMirror.fromTextArea(document.getElementById('<%=CtrCodeTextBox.ClientID %>')
//            , {
//                mode : "text/html",
          //              lineNumbers: "true" });
          var editor = CodeMirror.fromTextArea('<%=CtrCodeTextBox.ClientID %>', {
              parserfile: ["parsexml.js?" + new Date().getTime(), "parsecss.js?" + new Date().getTime(), "tokenizejavascript.js?" + new Date().getTime(), "parsejavascript.js?"+new Date().getTime(), "parsehtmlmixed.js?"+new Date().getTime()],
//              stylesheet: "/admin/Ajax/CodeMirror/contrib/csharp/css/csharpcolors.css",
              stylesheet: ["/admin/Ajax/CodeMirror/css/xmlcolors.css"
                    , "/admin/Ajax/CodeMirror/css/jscolors.css", "/admin/Ajax/CodeMirror/css/csscolors.css"],
              path: "/admin/Ajax/CodeMirror/js/",
              height: "400px",
              width: "780px",
              indentUnit: 4,
              lineNumbers: "true"
          });
    </script>
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