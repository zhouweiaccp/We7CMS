<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Compose.aspx.cs" Inherits="We7.CMS.Web.Admin.DataControlUI.Compose" ValidateRequest="false" %>
<%@ Register assembly="FCKeditor.net" namespace="FredCK.FCKeditorV2" tagprefix="FCKeditorV2" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WEC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
       </title>
   <link rel="stylesheet" type="text/css" href="../theme/classic/css/main.css" media="screen" />
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
        #NameLabel
        {
            display:inline-block;
            min-width:30px;
            min-height:25px;
        }
        #NameLabel:hover
        {
        	border:solid 1px #888;
        	background-color:White;
        	cursor:pointer;
        }
        #name-noticer
        {
            position:absolute;
            top:28px;
            left:170px;
            width:200px;
            height:50px;
            display:none;
            z-index:500;
        }
        .noticer-top
        {
            background-color:transparent;
            height:10px;
            background-image:url(/admin/images/noticertop.png);
       }
        .inner-noticer
        {
            
            background-color:#fafafa;
            border-width:1px;
            border-style:solid;
            border-color:#ccc;
            border-top:none;
            line-height:250%;
            font-size:13px;
            text-align:center;
            border-radius: 4px;
        }
        .light
        {
            background-color:#ddd;
        }
       </style>
    <script src="<%=AppPath%>/cgi-bin/DialogHelper.js" type="text/javascript"></script>
    <script type="text/javascript" src="<%=AppPath%>/ajax/jquery/jquery.latest.min.js"></script>
    <script type="text/javascript" src="/Admin/cgi-bin/CheckBrowser.js"></script>
    <script type="text/javascript">
var MSG_BLOCKER_IS_ON = "BLOCKER";
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
    var filter = "*.htm";
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
    var nameText=document.getElementById("NewCopyNameTextBox");
    nameText.value="";
    var saveBtn=document.getElementById("<%=SaveButton.ClientID %>");
    if(saveBtn)  saveBtn.click();
}
   
function saveTemplateAs()
{
    var nameText=document.getElementById("NewCopyNameTextBox");
    nameText.value=prompt("请输入模板副本名称",nameText.value);
    if(nameText.value!="")
    {
         var saveBtn=document.getElementById("<%=SaveButton.ClientID %>");
         if(saveBtn)  saveBtn.click();
    }
}

function publishThisCopy()
{
    var publishBtn=document.getElementById("<%=PublishButton.ClientID %>");
    if(publishBtn)  publishBtn.click();
    
}


</script>
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
            count = count ? count : 2;
            var t = $("#NameTextbox").css("width", count * 26)[0];
            t.select(); t.focus();
        });
        $('#NameTextbox').blur(function (e) {
            $("#NameTextbox").hide();
            $('#NameLabel').show();
            $("#NameLabel").text($('#NameTextbox').val());
        });
    });
</script>
</head>
<body class="we7-admin" onload="MaxTheWindow();">
    <form id="mainForm" runat="server">
    <div id="wrap">
        <h2 class="title">
            <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_look.gif" />
            <asp:Literal runat="server" ID="ActionLiteral"></asp:Literal>
                『<asp:TextBox runat="server"  ID="NameTextbox" Text="我的模板"   ></asp:TextBox>
                <asp:Label ID="NameLabel" title="点击更改名称" runat="server" Text="模板编辑">
                </asp:Label>』
            <span class="summary" style="width:300px">
            保存到文件
                <asp:Label ID="SummaryLabel" runat="server" Text="">
                </asp:Label>
                <asp:TextBox runat="server" style="font-size:12px;width:150px;height:20px;line-height:100%"  ID="FilenameTextBox" Text="templateName.ascx"   ></asp:TextBox>
            </span>
            <span class="copyTool" >
                  <asp:DropDownList ID="PrevewDropDownList" runat="server" style="font-size:12px;height:20px;margin:0px;vertical-align:inherit;*vertical-align:sub" onchange="MM_openMenu(this,1)" ></asp:DropDownList>
                    <span id="PublishSpan" runat="server" ><a href="javascript:publishThisCopy()"><img src="/admin/images/t-publish.png" alt="发布" /></a></span>
            </span>
             
        </h2>
        <WEC:MessagePanel ID="Messages" runat="server">
        </WEC:MessagePanel>
        <div style="width:100%;border: solid 0px black;position:absolute; z-index:1" id="divEditor" >
              <FCKeditorV2:FCKeditor ID="TemplateContentTextBox" runat="server" Height="450px" Width="100%" FullPage="true"  ToolbarSet="composenew"  BasePath="/admin/fckeditor/"   FillEmptyBlocks="false" EnableSourceXHTML="true" EnableXHTML="true"  HtmlEncodeOutput="false"  UseBROnCarriageReturn="true"  WorkPlace="compose"   >
            </FCKeditorV2:FCKeditor>
            <div class="editorFooter" style="width:100%;margin-right:5px"></div>
        </div>
    </div>

    <div style="display:none">
      <asp:Button ID="CopyButton" runat="Server" OnClick="CopyButton_Click" />
        <asp:Button ID="ImportButton" runat="Server" OnClick="ImportButton_Click" />
        <asp:TextBox ID="CopyIDTextBox" runat="server"></asp:TextBox>
        <asp:TextBox ID="ImportFileTextBox" runat="server"></asp:TextBox>
        <asp:TextBox ID="FileTextBox" runat="server"></asp:TextBox>
        <asp:TextBox ID="TemplatePathTextBox" runat="server"></asp:TextBox>
         <asp:TextBox ID="NewCopyNameTextBox" runat="server"></asp:TextBox>
        <asp:Button ID="SaveButton" runat="server" Text="Save" OnClick="SaveButton_Click" />
        <asp:Button ID="PublishButton" runat="server" Text="Publish" OnClick="PublishButton_Click" />
        <input type="hidden" id="ControlFileHidden" />
        <input type="button" id="saveAricleButton" onclick="SaveTemplate()" />
         <asp:TextBox ID="HeaderTextBox" runat="server" ></asp:TextBox>
     </div>
    </form>
</body>
<script type="text/javascript">
    function shake(ele) {
        var i = 0, cls = "light", times = 3, t;
        t = setInterval(function () {
            if (i % 2 === 0) {
                ele.addClass(cls);
            } else {
                ele.removeClass(cls);
            }
            if (++i >= times * 2) {
                ele.removeClass(cls);
                clearInterval(t);
            }
        }, 160);
    }
    $(document).ready(function () {
        var noticer = $('<div id="name-noticer"><div class="noticer-top"></div><div id="inner-noticer" class="inner-noticer">点击可更改名称</div></div>');
        setTimeout(function () {
            noticer.appendTo("#wrap").fadeIn(function () {
                shake($('#inner-noticer'));
            });
        }, 1000);
        setTimeout(function () {
            noticer.fadeOut("slow");
        }, 5000);
    });

</script>
</html>

