<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataControlListEx.aspx.cs"
    Inherits="We7.CMS.Web.Admin.DataControlUI.DataControlListEx" %>

<%@ Import Namespace="We7.CMS" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register TagPrefix="WEC" Namespace="We7.CMS.Controls" Assembly="We7.CMS.UI" %>
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
    <WEC:MessagePanel ID="Messages" runat="server">
    </WEC:MessagePanel>
    <div>
        <table>
            <tr>
                <td>
                    控件类别
                </td>
                <td>
                    <asp:DropDownList ID="ddlType" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    关键字
                </td>
                <td>
                    <asp:TextBox ID="txtKeyWord" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnQuery" runat="server" Text="&nbsp;查&nbsp;&nbsp;询&nbsp;" OnClick="btnQuery_Click" />
                    <asp:Button ID="btnCreateIndex" runat="server" Text="重建索引" OnClick="btnCreateIndex_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div>
        <asp:DataList ID="ModeDataList" runat="server" AutoGenerateColumns="False" ShowFooter="True">
            <ItemTemplate>
          
                <div style="width: 100%; padding: 10px;">
                    <div style="float: left;">
                        <a href="javascript:onSelectThis('<%# GetControlFileName(Container.DataItem) %>');">
                            <img alt="<%# Eval("Desc") %>" id="ModeImage" style="width: 176px; height: 120px;"
                                src="/<%# GetDemoUrl(Container.DataItem) %>" title="点击选择控件 <%# Eval("Desc") %>" onerror="this.src='/Admin/images/s.jpg'" /></a>
                    </div>
                    <div style="margin-left: 196px">
                        <div>
                            <h2>
                           <%#GetTitle(Container.DataItem)%>
                            </h2>
                        </div>
                        <div>
                   
                            <%# GetControls(Container.DataItem) %><br />
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
        <webdiyer:AspNetPager ID="Pager" AlwaysShow="true" runat="server" OnPageChanging="Pager_PageChanging"
            Style="color: #077ac7; font-size: 12px;" PageIndexBoxType="TextBox" Width="100%"
            PageIndexBoxStyle="width:19px" FirstPageText="首页" LastPageText="尾页" NextPageText="后页"
            PrevPageText="前页" NumericButtonTextFormatString="{0}" HorizontalAlign="right"
            CustomInfoStyle="text-align:left;" PageSize="10" ShowCustomInfoSection="Left"
            TextAfterPageIndexBox="页" TextBeforePageIndexBox="转到第" CustomInfoHTML="第 <font color='red'><b>%CurrentPageIndex%</b></font> 页 共 %PageCount% 页 显示 %StartRecordIndex%-%EndRecordIndex% 条">
        </webdiyer:AspNetPager>
    </div>
    </form>
</body>
</html>
