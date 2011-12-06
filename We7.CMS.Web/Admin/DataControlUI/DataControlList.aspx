<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataControlList.aspx.cs"
    Inherits="We7.CMS.Web.Admin.DataControlUI.DataControlList" %>

<%@ Import Namespace="We7.CMS" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WEC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>选择一个数据控件</title>
    <base target="_self" />
    <link rel="stylesheet" type="text/css" href="../theme/classic/css/main.css" media="screen" />
    <meta content="noindex, nofollow" name="robots" />

    <script src="../fckeditor/editor/dialog/common/fck_dialog_common.js" type="text/javascript"></script>

    <script type="text/javascript">
var dialog	= window.parent ;
var oEditor = dialog.InnerDialogLoaded() ;

// Gets the document DOM
var oDOM = oEditor.FCK.EditorDocument ;
var oActiveEl = dialog.Selection.GetSelectedElement() ;

    </script>

    <script src="<%=AppPath%>/cgi-bin/DialogHelper.js" type="text/javascript"></script>

    <script src="<%=AppPath%>/cgi-bin/search.js" type="text/javascript"></script>

    <style type="text/css">
        #site-mode
        {
            position: absolute;
            right: 0px;
            top: 0;
            width: 70px;
            height: 31px;
        }
    </style>

    <script type="text/javascript">

        function onSelectThis(v)
        {
             var tempFile=QueryStringFromUrl("file",top.location.search);
             var folder=QueryStringFromUrl("folder",top.location.search);
             var url= "DataControlBuilder.aspx?isFirst=1&file=" +  v+"&template=" + tempFile+"&folder="+folder;
             window.location=url;
        }

        function onDocumentLoad() {
        }

        function onModeTypeClick(stylelist) {
            if (stylelist == "list") {
                ListControl.style.display = "";
                ImageControl.style.display = "none";
            }
            if (stylelist == "icon") {
                ListControl.style.display = "none";
                ImageControl.style.display = "";
            }
        }
    </script>

</head>
<body onload="onDocumentLoad()">
    <form id="mainForm" runat="server">
    <div>
        <span>快速搜索：</span>
        <asp:DropDownList ID="FieldDropDownList" runat="server">
            <asp:ListItem Value="Name">按关键字查找</asp:ListItem>
            <asp:ListItem Value="FileName">按文件名查找</asp:ListItem>
        </asp:DropDownList>
        <asp:TextBox ID="SearchTextBox" runat="server" Columns="20" MaxLength="64"></asp:TextBox>
        <asp:HyperLink ID="SearchHyperLink" runat="server" NavigateUrl="javascript:document.mainForm.QueryButton.click();">
            <asp:Image ID="SearchImage1" runat="server" ImageUrl="~/admin/Images/icon_search.gif" />
            执行</asp:HyperLink>
    </div>
    <br />
    <div id="messageLayer">
        <asp:Image ID="MessageImage" runat="server" ImageUrl="~/admin/Images/icon_info.gif" />
        <asp:Label ID="MessageLabel" runat="server" Text="">
        </asp:Label>
    </div>
    <br />
    <asp:DataList ID="ModeDataList" runat="server" AutoGenerateColumns="False" ShowFooter="True">
        <ItemTemplate>
            <div style="width: 100%; padding: 10px;">
                <div style="float: left;">
                
                    <a href="javascript:onSelectThis('<%# GetControlFileName(Container.DataItem) %>');">
                        <img alt="<%# Eval("Desc") %>" id="ModeImage" style="width:176px; height:120px;"
                            src="/<%# GetDemoUrl(Container.DataItem) %>"
                            title="点击选择控件 <%# Eval("Desc") %>" /></a>
                </div>
                <div style="margin-left: 196px">
                    <div>
                        <h2>
                            <a href="javascript:onSelectThis('<%# GetControlFileName(Container.DataItem) %>');">
                                <%#Eval("Desc")%></a>
                        </h2>
                    </div>
                    <div>
                        外观：<%# GetControls(Container.DataItem) %><br />
                        作者：<%#Eval("Author")%><br />
                        创建时间：<%#Eval("Created")%>
                    </div>
                    <div style="font-weight: lighter">
                        描述：
                        <%#Eval("Desc")%>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:DataList>
    </div>
    <br />
    <div style="display: none">
        <asp:TextBox ID="IDTextBox" runat="server" Text=""></asp:TextBox>
    </div>
    </form>
</body>
</html>
