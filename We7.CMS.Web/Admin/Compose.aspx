<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Compose.aspx.cs" Inherits="We7.CMS.Web.Admin.Compose"
    ValidateRequest="false" %>
<%@ Register assembly="FCKeditor.net" namespace="FredCK.FCKeditorV2" tagprefix="FCKeditorV2" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WEC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>
        <%=MyPageTitle%></title>
   <link rel="stylesheet" type="text/css" href="theme/classic/css/main.css" media="screen" />
   <style type="text/css">
        h2.title
        {
        	margin-top:5px;
        	margin-bottom:4px;
        }
        body
        {
        	margin:5px;
        }
        </style>
    <script src="<%=AppPath%>/cgi-bin/DialogHelper.js" type="text/javascript"></script>
    <script type="text/javascript">
var MSG_BLOCKER_IS_ON = "BLOCKER";
function showErrorMessage(m) {
}

function doCommand(cmd)
{
     switch(cmd) {
        case "importhtml":
           importTemplate();
           break;
        case "copytemplate":
           copyTemplate();
           break;
      }
}

function importTemplate() {
    var pathText =document.getElementById('<%=TemplatePathTextBox.ClientID%>'); 
    var dir = pathText.value;
    var filter = "*.html";
    weShowModelDialog("/admin/Folder.aspx?noSelect=0&folder=" + dir + "&filter=" + filter, handleImportTemplate);
}

function handleImportTemplate(v, t) {
    if(v) {
        document.getElementById("ImportFileTextBox").value = v;
        document.getElementById("ImportButton").click();
    }
}

function copyTemplate() {
    weShowModelDialog("/admin/TemplateList.aspx", onCopyTemplateListCallback);
}

function onCopyTemplateListCallback(v, t) {
    if(v) {
        document.getElementById("CopyIDTextBox").value = v;
        document.getElementById("CopyButton").click();
    }
}

function SaveTemplate()
{
    var saveBtn=document.getElementById("<%=SaveButton.ClientID %>");
    if(saveBtn)  saveBtn.click();
}

function getHeaderText()
{
    var txtHeader=document.getElementById("<%=HeaderTextBox.ClientID %>");
    if(txtHeader)
        return txtHeader.value;
    else
        return "";
}

function setHeaderText(v)
{
     var txtHeader=document.getElementById("<%=HeaderTextBox.ClientID %>");
     if(txtHeader) txtHeader.value=v;
}
    
    </script>

</head>
<body class="we7-admin" onload="MaxTheWindow();">
    <form id="mainForm" runat="server">
    <div id="wrap">
        <h2 class="title">
            <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_look.gif" />
            <asp:Label ID="NameLabel" runat="server" Text="模板编辑">
            </asp:Label>
            <span class="summary">
                <asp:Label ID="SummaryLabel" runat="server" Text="">
                </asp:Label>
            </span>
              <div id="buttonShow" style="padding-top:20px;padding-right:10px">
                  <a href="javascript:;" onclick="importTemplate();"><u>当前模板所在文件夹</u></a>
             </div>
        </h2>
         <WEC:MessagePanel ID="Messages" runat="server">
    </WEC:MessagePanel>
        <div style="border: solid 0px black" id="divEditor">
              <FCKeditorV2:FCKeditor ID="TemplateContentTextBox" runat="server" Height="450px" Width="100%"  ToolbarSet="compose"  BasePath="/admin/fckeditor/"  FillEmptyBlocks="false" FormatSource="true"  UseBROnCarriageReturn="true"  WorkPlace="compose"   >
            </FCKeditorV2:FCKeditor>
            <div class="editorFooter" style="width:100%;margin-right:5px"></div>
        </div>

        <div style="display: none">
            <asp:Button ID="CopyButton" runat="Server" OnClick="CopyButton_Click" />
            <asp:Button ID="ImportButton" runat="Server" OnClick="ImportButton_Click" />
            <asp:TextBox ID="CopyIDTextBox" runat="server"></asp:TextBox>
            <asp:TextBox ID="ImportFileTextBox" runat="server"></asp:TextBox>
            <asp:TextBox ID="FileTextBox" runat="server"></asp:TextBox>
            <asp:TextBox ID="TemplatePathTextBox" runat="server"></asp:TextBox>
            <asp:Button ID="SaveButton" runat="server" Text="Save" OnClick="SaveButton_Click" />
            <input type="hidden" id="ControlFileHidden" />
        </div>
    </div>
    <div id="mask" class="mask">
    </div>
    <div style="display: none; position: absolute; z-index: 999; height: 400px; width: 670px;
        left: 100px; top: 100px; background-color: #fff; border: 3px solid #ccc;" id="divHeader"
        class="MessageBox">
        <a href="javascript:closeHeader()" style="background: url(/images/x.png) no-repeat;
            width: 25px; height: 29px; display: inline; z-index: 3200; position: absolute;
            top: -15px; right: -18px; cursor: pointer;"></a>
        <h4>
            <asp:Label ID="HeaderLabel" runat="server" Text="&nbsp;编辑模板的HEAD信息。这部分的内容，在应用模板的时候，添加到HTML页面的HEAD部分。">
            </asp:Label>
        </h4>
        <div style="margin: 5px">
            <asp:TextBox ID="HeaderTextBox" runat="server" Rows="22" Columns="80" TextMode="MultiLine"></asp:TextBox>
        </div>
    </div>
    <div style="display:none">
    <input type="button" id="saveAricleButton" onclick="SaveTemplate()" />
</div>
    </form>
</body>
</html>
