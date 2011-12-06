<%@ Page Language="C#" AutoEventWireup="true" Codebehind="DataControlBuilder.aspx.cs"
    Inherits="We7.CMS.Web.Admin.DataControlBuilder" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>数据控件</title>
    <base target="_self" />
    <link rel="stylesheet" type="text/css" href="theme/classic/css/main.css" media="screen" />
    <meta content="noindex, nofollow" name="robots" />
    <script src="<%=AppPath%>/ajax/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
	<script src="fckeditor/editor/dialog/common/fck_dialog_common.js" type="text/javascript"></script>
	
	<script src="<%=AppPath%>/cgi-bin/DialogHelper.js" type="text/javascript"></script>
    <script src="cgi-bin/controls/datacontrol.js" type="text/javascript" charset="gb2312"></script>
    <script src="cgi-bin/controls/datacontrol/Common.js" type="text/javascript" charset="gb2312"></script>
    
   <script src="<%=AppPath%>/ajax/jquery/jquery.simplemodal-1.1.1.js" type="text/javascript"></script> 
   
	<script type="text/javascript">
//debugger;
var dialog	= window.parent ;
var oEditor = dialog.InnerDialogLoaded() ;
var FCKDomTools = oEditor.FCKDomTools ;
var FCKDomRange = oEditor.FCKDomRange ;
var FCK = oEditor.FCK ;
// Gets the document DOM
var oDOM = oEditor.FCK.EditorDocument ;
var oActiveEl = dialog.Selection.GetSelectedElement() ;

function onDocumentLoad() {
    oEditor.FCKLanguageManager.TranslatePage(document) ;
    var node = oActiveEl;
    while(node)
    {
            if(node.tagName == 'DIV' && ( node.className == 'wec' || node.tag == 'wec' ) )
                break;
            node=node.parentNode;
    }
    oActiveEl=node;

    dialog.SetOkButton( true ) ;
}
function Ok()
{
	oEditor.FCKUndo.SaveUndoStep() ;

//    debugger;
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
	        FCKDomTools.RemoveNode(node, false ) ;
	        
	    // Restore selection.
		range.MoveToBookmark( bookmark ) ;
		range.Select() ;
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

function onBodyLoad()
{
    onDocumentLoad();
    var txt = document.mainForm.FieldsTextBox.value;
    var table = document.getElementById("mainTable");
    BUILDER.loadXML(txt);
    BUILDER.createHtmlObject(table);

    if(oActiveEl!= null ) {
        values =oActiveEl;
        BUILDER.loadValue(values);
        var idv = BUILDER.getID();
        if(idv == null) {
            var key=QueryString("file");
            if(key && key!="null")
            {
                var   sarray=new   Array();   
                sarray=key.split('.');   
                idv=sarray[0]+"_01";  
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
        var key=QueryString("file");
        if(key && key!="null")
        {
            var   sarray=new   Array();   
            sarray=key.split('.');   
            idName=sarray[0]+"_01";  
        }
         document.mainForm.IDTextBox.value = idName;
    }
    document.mainForm.IDTextBox.focus();
}

function showErrorMessage(m) {
    var ml = document.getElementById("MessageLabel");
    ml.innerHTML = m;
}

    </script>
</head>

<body onload="onBodyLoad()">
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
            <div >
                    控件名称：
                    <asp:TextBox ID="IDTextBox" runat="server" Text=""></asp:TextBox><asp:HyperLink ID="EditCssHyperLink" runat="server" Target="_blank" Visible="false">
                        编辑css</asp:HyperLink>
                </div>
                <br />
                <div>
                    <table id="mainTable">
                        <tr>
                            <th width="130" align="center" >
                                项目</th>
                            <th >
                                内容</th>
                            <th width="400">
                                描述</th>
                        </tr>
                    </table>
                    <br />
                    <div id="messageLayer">
                        <asp:Image ID="MessageImage" runat="server" ImageUrl="~/admin/Images/icon_info.gif" />
                        <asp:Label ID="MessageLabel" runat="server" Text="">
                        </asp:Label>
                    </div>
                </div>
                <br />
                <div style="display: none">
                    <asp:TextBox ID="FieldsTextBox" runat="server" Text=""></asp:TextBox>
                    <asp:TextBox ID="IsFirstTextBox" runat="server" Text=""></asp:TextBox>
                   <asp:TextBox ID="CssFileTextBox" runat="server" Text=""></asp:TextBox> 
                    <asp:TextBox ID="StylePathTextBox" runat="server" Text=""></asp:TextBox> 
                </div>

        <div id="mask"  class="mask"></div>
    </form>
</body>
</html>
