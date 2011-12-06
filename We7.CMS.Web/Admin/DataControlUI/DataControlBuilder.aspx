<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataControlBuilder.aspx.cs"
    Inherits="We7.CMS.Web.Admin.DataControlUI.DataControlBuilder" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>数据控件</title>
    <base target="_self" />
    <link rel="stylesheet" type="text/css" href="../theme/classic/css/main.css" media="screen" />
    <meta content="noindex, nofollow" name="robots" />

    <script src="../fckeditor/editor/dialog/common/fck_dialog_common.js" type="text/javascript"></script>

    <script src="<%=AppPath%>/cgi-bin/DialogHelper.js" type="text/javascript"></script>

    <script src="../cgi-bin/base64.js" type="text/javascript"></script>

    <script src="../cgi-bin/datacontrol/datacontrol.new.js" type="text/javascript"></script>

    <script src="../cgi-bin/datacontrol/Common.new.js" type="text/javascript"></script>

    <script src="<%=AppPath%>/ajax/jquery/jquery1.2.6.pack.js" type="text/javascript"></script>

    <script src="<%=AppPath%>/ajax/jquery/jquery.simplemodal-1.1.1.js" type="text/javascript"></script>

    <script type="text/javascript">
var dialog	= window.parent ;
var oEditor = dialog.InnerDialogLoaded() ;
var FCKDomTools = oEditor.FCKDomTools ;
var FCKDomRange = oEditor.FCKDomRange ;
var FCK = oEditor.FCK ;
// Gets the document DOM
var oDOM = oEditor.FCK.EditorDocument ;
var oActiveEl = dialog.Selection.GetSelectedElement() ;
var CurrentDC=null;

function onDocumentLoad() {
    oEditor.FCKLanguageManager.TranslatePage(document) ;
    var node = oActiveEl;
    if(node && node.tagName=="IMG")
    {
         var ct = node.getAttribute("control");
	     var strControl=HtmlDecode(ct);
	     node=parseDom(strControl);
    }
    oActiveEl=node;
    dialog.SetOkButton( true ) ;
}
function Ok()
{
	oEditor.FCKUndo.SaveUndoStep() ;

    BUILDER.setID(document.mainForm.IDTextBox.value);
     if(BUILDER.validate()) {
        var c=BUILDER.getControl();
        DeleteDiv();
        oEditor.FCK.InsertHtml(c);
        return true ;
      }
    return false;
}

function DeleteDiv()
{
    	var node = oActiveEl;

        // Remember the current selection position.
		var range = new FCKDomRange( FCK.EditorWindow ) ;
		range.MoveToSelection() ;
		var bookmark = range.CreateBookmark() ;
		
        while(node)
        {
            if(node.tagName == 'DIV' && ( node.className == 'wec' || node.tag == 'wec' ) )
                break;
            node=node.parentNode;
        }

		if(node)
		{
	        FCKDomTools.RemoveNode(node, false);
	    }
	        
	    // Restore selection.
		range.MoveToBookmark( bookmark ) ;
		range.Select();
}
    </script>

    <script type="text/javascript">
function QueryString(name)
{
    var qs=name+"=";
    var str=location.search;
    if(str.length>0)
    {
        begin=str.indexOf(qs);
        if(begin!=-1)
        {
            begin+=qs.length;
            end=str.indexOf("&",begin);
            if(end==-1)end=str.length;
            return(str.substring(begin,end));
        }
    }
    return null;
}

var datamode;
function reloadData(fn)
{
    $.ajax({
        url:'../Ajax/TemplateCheck.aspx',
        data:{cmd:'reload',ctr:$("#ControlSelect").val()},
        success:function(result,op)
        {
            datamode=eval("("+result+")");
            if(fn)fn();
        },
        error:function(xh,st,op){
            alert("加载数据出错");
        }
    });
}

function createOp(txt, value) {
	    var op = document.createElement("OPTION");
        op.text = txt;
        op.value = value;
        return op;
}

function setInfo(dcc)
{
       document.getElementById("brief").innerHTML=dcc.description;
       document.getElementById("thumbnail").src="/"+dcc.demoUrl;           
}

function onBodyLoad()
{    
    onDocumentLoad();
    
    var txt = document.mainForm.FieldsTextBox.value;
    datamode=eval("("+txt+")");
    

    var dc;
    var ctrsl=document.getElementById("ControlSelect");
    var ctr=document.getElementById("ControlHidden").value;
    var reg=new RegExp("^"+ctr+"$",'ig');
    for(var i=0;i<datamode.items.length;i++)
    {
        var name=datamode.items[i].name;
        var control=datamode.items[i].control;
        var fileName=datamode.items[i].fileName;
        ctrsl.options[i]=createOp(name,fileName);
        if(reg.test(fileName))
        {
            ctrsl.options[i].selected=true;
            CurrentDC=dc=datamode.items[i];
            dc.template = '<%=We7Helper.Base64Decode(Template) %>';
            setInfo(dc);
        }
    }
    if(!dc)
    {
        alert('当前控件不存在');
        return;
    }
    dc.gp=$("#GroupHidden").val();
    
    ctrsl.onchange=function()
    {
        var dcc={};
        document.getElementById("ControlHidden").value=this.value;
        for(var i=0;i<datamode.items.length;i++)
        {
            //var control=datamode.items[i].control;
            var control=datamode.items[i].fileName;
            if(this.value==control)
            {
                CurrentDC=dcc=datamode.items[i];
                setInfo(dcc);
                BUILDER.refresh(dcc);   
                break;
            }
        }
        
    }
    
    var table = document.getElementById("mainTable");
    BUILDER.loadData(dc);
    BUILDER.createHtmlObject(table);
    BUILDER.Event.add("StyleSelector",function(obj){
         reloadData();
    });

    if(oActiveEl!= null ) {
        values =oActiveEl;
        BUILDER.loadValue(values);
        var idv = BUILDER.getID();
        if(idv == null) {
         
            if(CurrentDC)
            {
                var dt=new Date();
                var key=CurrentDC.control+dt.getTime()+Math.round((Math.random()*100));//QueryString("file");
                if(key && key!="null")
                {
                      idv=key.replace('.','_');
                }
            }
            else
            {
                idv = "WECDControl_01";
            }
        }
        document.mainForm.IDTextBox.value = idv;
    }
    
    var isFirst = document.getElementById("IsFirstTextBox").value;
    if(isFirst == "1") {
        BUILDER.loadDefaultValues();
        var idName = "WECDControl_01";
        if(CurrentDC)
        {
            var dt=new Date();
            var key=CurrentDC.control+"_"+dt.getTime()+Math.round((Math.random()*100));//QueryString("file");
            if(key && key!="null")
            {
                  idName=key.replace('.','_');
            }
        }
        document.mainForm.IDTextBox.value = idName;
    }
    document.mainForm.IDTextBox.focus();

    $("#CssClass").html("");
            $.ajax({
                contentType: "application/json",
                url: "/Admin/VisualTemplate/VisualService.asmx/GetStyleByControl",
                data: '{"controlPath":"' + ctr + '"}',
                type: "POST",
                success: function (_result_) {
                    var array = eval(_result_);
                    for (var i in array) {
                        $("#CssClass").append("<option value='" + array[i].replace(".","_") + "'>" + array[i] + "</option>");
                    }
                }
            });
}

function showErrorMessage(m) {
    var ml = document.getElementById("MessageLabel");
    ml.innerHTML = m;
}

function InitCtrEditor(editctr)
{
    $(editctr).bind("click",function(){
        <% if(IsModel){ %>
	    var result=window.showModalDialog("DataControlEditor.aspx?cmd=edit&ctr="+$("#ControlSelect").val(),"修改控件",'scrollbars=no;resizable=no;help=no;status=no;dialogHeight=510px;dialogwidth=750px;');
	    <% }else{ %>
	    var result=window.showModalDialog("DataControlModelEditor.aspx?cmd=edit&ctr="+$("#ControlSelect").val(),"修改控件",'scrollbars=no;resizable=no;help=no;status=no;dialogHeight=510px;dialogwidth=750px;');
	    <%} %>
	    if(result&&result.key&& result.value)
	    {
	        var cdc={};
	        var ops=$("#ControlSelect")[0].options;
	        var op=ops[ops.length]=createOp(result.key,result.value);
	        op.selected=true;
	        reloadData(function(obj){
	            $("#ControlSelect")[0].onchange();
	        });	        
	    }
	    return false;
    });
}
function InitWidgetEdit(editwidget){
    if($(editwidget)){
        $(editwidget).bind("click",function(){
            var filename = '<%=Request["file"] %>';
            var r = window.showModalDialog(
            '/Admin/VisualTemplate/WidgetEditor/WidgetEditor.aspx?ctr=' + filename + "&t=" + (new Date()).valueOf()
            , filename
            , 'scrollbars=no;resizable=no;help=no;status=no;center=1; dialogHeight=400px;dialogwidth=880px;');
            if (r) {
                //刷新
            }
        });
    }
}
$(document).ready(function(){
    InitCtrEditor("#editctr");

    InitWidgetEdit("#editwidget");
});

    </script>

    <style>
        fieldset
        {
            border: 1px solid;
            padding-left: 10px;
            padding-right: 10px;
        }
        hr
        {
            height: 2px;
        }
        input
        {
            font-size: 12px;
        }
    </style>
</head>
<body onload="onBodyLoad()" style="padding: 0; margin: 0; x-overflow: hidden; width: 630px;
    padding: 0 10px">
    <form id="mainForm" runat="server">
    <h2>
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/logo_listset.gif" />
        <asp:Label ID="NameLabel" runat="server" Text="设置数据控件的参数">
        </asp:Label>
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server" Text="">
            </asp:Label>
        </span>

    </h2>
    <div style="border-top: solid 1px #ddd; margin: 0px auto;">
        <table cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td valign="top" style="width: 200px;">
                    <fieldset style="text-align: center; padding: 10px;">
                        <legend>缩略图</legend>
                        <img id="thumbnail" width="176" height="120" onerror="this.src='/admin/images/s.jpg'" />
                    </fieldset>
                    <fieldset>
                        <legend>简介</legend>
                        <label id="brief">
                        </label>
                        <br />
                    </fieldset>
                    <fieldset>
                        <legend>提示</legend>鼠标属性标题上悬停1秒,可以看到属性的详细信息。<br />
                        <br />
                    </fieldset>
                </td>
                <td valign="top">
                    <fieldset style="minheight: 340px; height: auto !important; height: 340px;">
                        <legend>属性设置</legend>
                        <table id="mainTable">
                            <tr>
                                <th style="width: 100px;">
                                    项目
                                </th>
                                <th>
                                    内容
                                </th>
                            </tr>
                            <tr>
                                <td title="控件外观" style="cursor: pointer;">
                                    控件外观：
                                </td>
                                <td>
                                    <asp:DropDownList ID="ControlSelect" runat="server">
                                    </asp:DropDownList>
                                    <%
                                        if (IsControl())
                                        { 
                                    %>
                                    <a id="editctr" href="#">【修改】</a>
                                    <%}
                                        else if (IsWidget())
                                        {
                                        %>
                                    <a id="editwidget" href="#">【修改】</a>
                                        <%
                                        }
                                    %>
                                </td>
                            </tr>
                            <tr title="控件的标识符" style="cursor: pointer">
                                <td>
                                    控件名称：
                                </td>
                                <td>
                                    <asp:TextBox ID="IDTextBox" runat="server" Text=""></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
        <div id="messageLayer" style="display: none">
            <asp:Image ID="MessageImage" runat="server" ImageUrl="~/admin/Images/icon_info.gif" />
            <asp:Label ID="MessageLabel" runat="server" Text="">
            </asp:Label>
        </div>
    </div>
    <div style="display: none">
        <asp:TextBox ID="FieldsTextBox" runat="server" Text=""></asp:TextBox>
        <asp:TextBox ID="IsFirstTextBox" runat="server" Text=""></asp:TextBox>
        <asp:TextBox ID="CssFileTextBox" runat="server" Text=""></asp:TextBox>
        <asp:TextBox ID="StylePathTextBox" runat="server" Text=""></asp:TextBox>
        <asp:HiddenField ID="GroupHidden" runat="server" />
        <asp:HiddenField ID="ControlHidden" runat="server" />
    </div>
    <div id="mask" class="mask">
    </div>
    </form>
</body>
</html>
