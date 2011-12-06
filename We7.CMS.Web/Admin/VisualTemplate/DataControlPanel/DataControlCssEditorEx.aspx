<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataControlCssEditorEx.aspx.cs" Inherits="We7.CMS.Web.Admin.VisualTemplate.DataControlCssEditorEx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title>样式编辑</title>
<base target="_self" />
<style type="text/css">
input, selectd {
	margin: 0;
}
body, td {
	font-size: 12px;
	padding:3px;
}
body {
	padding: 0;
	margin: 0;
	font-family: 宋体 Arial;
	background-color: #F7F7F7;
}
</style>
<script src="/Scripts/jQuery/jquery-1.4.2.js" type="text/javascript"></script>
<script src="/Scripts/Common.js" type="text/javascript"></script>
<script src="/Scripts/jQuery/plugin/colorpicker/colorpicker.js" type="text/javascript"></script>
<script src="/Scripts/ckfinder/ckfinder.js?20110422004" type="text/javascript"></script>
<script src="/admin/ajax/CodeMirror/js/codemirror.js" type="text/javascript"></script>
<script src="/Admin/VisualTemplate/Scripts/adminpanel.js" type="text/javascript"></script>
</head>
<body>
<form id="form1" runat="server">
  <div style="padding:10px;">
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td bgcolor="#CCCACC">
          <span>插入图片:</span> 
          <span><input type="text" id="cssImageUrl" name="cssImageUrl" /></span> 
          <span>[<a href="#" title="点击插入图片" onclick="insertStr(document.getElementById('<%=CssTextBox.ID %>'))">复制</a>]</span> 
          <span>[<a href="#" title="点击上传或选择图片" id="btnSelectImage">选择</a>]</span> <span>样式名称：</span> 
          <span>
            <select id="StyleList">
            </select>
          </span> 
          <span>[<asp:LinkButton ID="edtSave" runat="server" onclick="edtSave_Click">保存</asp:LinkButton>]</span> 
          <span>[<asp:LinkButton ID="edtSaveAs" runat="server" onclick="edtSaveAs_Click" OnClientClick="SetStyleName()">另存为</asp:LinkButton>]</span>
            <input id="NewStyleName" name="NewStyleName" type="hidden" value="" />
        </td>
      </tr>
      <tr>
        <td><asp:TextBox ID="CssTextBox" runat="server" TextMode="MultiLine" Height="415px" Width="100%"></asp:TextBox></td>
      </tr>
    </table>
  </div>
<script type="text/javascript">
    var arrayCache = null;
    ///
    ///参数ctrl为input或者textarea对象，pos为光标要移动到的位置
    ///
    function getCursortPosition(ctrl) {//获取光标位置函数
        var CaretPos = 0; // IE Support
        if (document.selection) {
            ctrl.focus();
            var Sel = document.selection.createRange();
            Sel.moveStart('character', -ctrl.value.length);
            CaretPos = Sel.text.length;
        }
        // Firefox support
        else if (ctrl.selectionStart || ctrl.selectionStart == '0')
            CaretPos = ctrl.selectionStart;
        return (CaretPos);
    }

    function insertStr(ctrl) {//设置光标位置函数
        /*if (document.selection) {
            ctrl.focus();
            var sel = document.selection.createRange();
            document.selection.empty();
            sel.text = document.getElementById("cssImageUrl").value;
        }
        else {
            var prefix, main, suffix;
            prefix = ctrl.value.substring(0, ctrl.selectionStart);
            main = ctrl.value.substring(ctrl.selectionStart, ctrl.selectionEnd);
            suffix = ctrl.value.substring(ctrl.selectionEnd);
            ctrl.value = prefix + document.getElementById("cssImageUrl").value + suffix;
        }
        */
        if (window.clipboardData) {
            window.clipboardData.clearData();
            window.clipboardData.setData("Text", document.getElementById("cssImageUrl").value);
        } else if (navigator.userAgent.indexOf("Opera") != -1) {
            window.location = document.getElementById("cssImageUrl").value;
        } else if (window.netscape) {
            try {
                netscape.security.PrivilegeManager.enablePrivilege("UniversalXPConnect");
            } catch (e) {
                alert("您的当前浏览器设置已关闭此功能！请按以下步骤开启此功能!新开一个浏览器，在浏览器地址栏输入'about:config'并回车.然后找到'signed.applets.codebase_principal_support'项，双击后设置为'true'。声明：本功能不会危极您计算机或数据的安全！");
            }
            var clip = Components.classes['@mozilla.org/widget/clipboard;1'].createInstance(Components.interfaces.nsIClipboard);
            if (!clip) return;
            var trans = Components.classes['@mozilla.org/widget/transferable;1'].createInstance(Components.interfaces.nsITransferable);
            if (!trans) return;
            trans.addDataFlavor('text/unicode');
            var str = new Object();
            var len = new Object();
            var str = Components.classes["@mozilla.org/supports-string;1"].createInstance(Components.interfaces.nsISupportsString);
            var copytext = document.getElementById("cssImageUrl").value;
            str.data = copytext;
            trans.setTransferData("text/unicode", str, copytext.length * 2);
            var clipid = Components.interfaces.nsIClipboard;
            if (!clip) return false;
            clip.setData(trans, null, clipid.kGlobalClipboard);
        }        
        alert("你现在可以粘贴了！");
        //document.getElementById("cssImageUrl").value = "";
    }
    ///
    ///End
    ///

    $.ajax({
        contentType: "application/json",
        url: "/Admin/VisualTemplate/VisualService.asmx/GetStyleByControl",
        data: '{"controlPath":"<%=ControlFile %>"}',
        type: "POST",
        success: function (_result_) {
            var array = eval(_result_);
            arrayCache = array;

            for (var i in array) {
                $("#StyleList").append("<option value='" + array[i] + "'>" + array[i] + "</option>");
            }

            var count = $("#StyleList option").length;

            for (var i = 0; i < count; i++) {
                if ($("#StyleList ").get(0).options[i].text.replace(".","_") == '<%=Style %>') {
                    $("#StyleList ").get(0).options[i].selected = true;
                    break;
                }
            } 
        }
    });

    $("#StyleList").bind("change", function () {
        window.location.href = "DataControlCssEditorEx.aspx?cmd=edit&ctr=<%=ControlFile %>&style=" + $(this).val() + "&gp=<%=Group %>";
    });

    $("#btnSelectImage").bind("click", function () {
        BrowseServer("cssImageUrl");
    });

    function SetStyleName(style) {
        var c = window.prompt("请输入新样式名称！", "newStyle");
        var isChecked=true;
        for (var i in arrayCache) {
            if (arrayCache[i] == c)
                isChecked = false;
        }
        if (isChecked)
            $("#NewStyleName").val(c);
        else
            alert("你输入的样式表名称已存在！");
    }

    var editor = CodeMirror.fromTextArea('<%=CssTextBox.ID %>', {
        parserfile: ["parsecss.js"],
        stylesheet: ["/admin/Ajax/CodeMirror/css/csscolors.css"],
        path: "/admin/Ajax/CodeMirror/js/",
        height: "460px",
        width: "740px",
        indentUnit: 4,
        lineNumbers: "true"
    });

    </script>
</form>
</body>
</html>
