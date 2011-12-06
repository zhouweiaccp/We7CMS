<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectChannel.aspx.cs" Inherits="We7.CMS.Web.Admin.cgi_bin.controls.SelectChannel" %>
<html xmlns="http://www.WestEngine.com" >
<head runat="server">
    <title>选择栏目与文章</title>
     <link href="/Cutesoft_Client/Dialogs/Load.ashx?type=style&file=dialog.css" type="text/css" rel="stylesheet" />
         <style type="text/css">
		.row { HEIGHT: 22px }
		.cb { VERTICAL-ALIGN: middle }
		.itemimg { VERTICAL-ALIGN: middle }
		.editimg { VERTICAL-ALIGN: middle }
		.cell1 { VERTICAL-ALIGN: middle }
		.cell2 { VERTICAL-ALIGN: middle }
		.cell3 { PADDING-RIGHT: 4px; VERTICAL-ALIGN: middle; TEXT-ALIGN: right }
		.cb { }
		body {
	background-color: #D4D0C8;
}
body,td,th {
	font-size: 12px;
}
</style>
 <script language="javascript">
	function do_insert()
	{
		returnValue=document.getElementById('TargetUrl').value;
		window.close();
	}
	
	function do_cancel()
	{
	    returnValue='';
        window.close();
	}
 </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table border="0" cellpadding="0" cellspacing="2" width="100%">
            <tr>
                <td style="width: 10px" >
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/admin/Images/openfolder.gif" />
                </td>
                <td class="normal"  align="left">
                    <div id="SummaryLabel">根栏目</div></td>
                <td>
                <!--
                    <asp:DropDownList ID="FieldDropDownList" runat="server">
                        <asp:ListItem Value="Name">按名称查找</asp:ListItem>
                        <asp:ListItem Value="Description">按描述查找</asp:ListItem>
                    </asp:DropDownList><asp:TextBox ID="SearchTextBox" runat="server" Columns="20" MaxLength="64"></asp:TextBox><asp:HyperLink
                        ID="SearchHyperLink" runat="server" NavigateUrl="javascript:document.mainForm.QueryButton.click();">
                        <asp:Image ID="SearchImage1" runat="server" ImageUrl="~/admin/Images/icon_search.gif" />
                        执行</asp:HyperLink>
                  -->
                  </td>
            </tr>
        </table>
        <table border="0" cellpadding="2" cellspacing="0" width="100%">
            <tr>
                <td style="width: 280px; white-space: nowrap" valign="top">
                    <div style="border-right: 1.5pt inset; padding-right: 0px; border-top: 1.5pt inset;
                        padding-left: 0px; padding-bottom: 0px; vertical-align: middle; overflow: auto;
                        border-left: 1.5pt inset; width: 280px; padding-top: 0px; border-bottom: 1.5pt inset;
                        height: 250px; background-color: white">
                        <!--
                        <asp:Table ID="FoldersAndFiles" runat="server" CellPadding="0" CellSpacing="1" CssClass="sortable"
                            Width="100%">
                            <asp:TableRow BackColor="#f0f0f0">
                                <asp:TableHeaderCell Width="16px">
                                    <asp:ImageButton ID="Select" runat="server" AlternateText="选择此栏目" ImageUrl="~/admin/Images/icon_ok.gif"
                                        OnClick="Select_Click" Visible="true" />
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="16px">
                                    <asp:ImageButton ID="DoRefresh" runat="server" AlternateText="刷新" ImageUrl="~/admin/Images/icon_refresh.gif"
                                        OnClick="DoRefresh_Click" Visible="true" />
                                </asp:TableHeaderCell>
                                <asp:TableHeaderCell ID="name_Cell" CssClass="filelistHeadCol" Font-Bold="False"
                                    Width="145px" Wrap="True">标题</asp:TableHeaderCell>
                                <asp:TableHeaderCell ID="date_Cell" CssClass="filelistHeadCol" Font-Bold="False"
                                    Width="62px">提交日期</asp:TableHeaderCell>
                                <asp:TableHeaderCell ID="op_Cell" Width="16px">&nbsp;</asp:TableHeaderCell>
                                <asp:TableHeaderCell ID="op_space" Width="1px"></asp:TableHeaderCell>
                            </asp:TableRow>
                        </asp:Table>
                        -->
                        &nbsp;
                        <iframe src="iChannels.aspx" width="250" height="100%" frameborder="0" scrolling="no"  ></iframe>
                    </div>
                </td>
                <td style="width: 328px; white-space: nowrap" valign="top">
                    <div style="border-right: 1.5pt inset; padding-right: 0px; border-top: 1.5pt inset;
                        padding-left: 0px; padding-bottom: 0px; vertical-align: top; overflow: auto;
                        border-left: 1.5pt inset; width: 100%; padding-top: 0px; border-bottom: 1.5pt inset;
                        height: 250px; background-color: white">
                        <div id="divpreview" style="width: 100%; height: 100%; background-color: white">
                          <iframe id="ifArticles" src="iArticles.aspx" width="300" height="100%" frameborder="0" scrolling="no"  ></iframe>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 2px">
                <!--
                    <asp:Image ID="MessageImage" runat="server" ImageUrl="~/admin/Images/icon_info.gif" /><asp:Label
                        ID="MessageLabel" runat="server" Text="">
                            </asp:Label>
                 -->
               </td>
            </tr>
        </table>
    
    </div>
        <table border="0" cellpadding="2" cellspacing="0" width="100%">
            <tr>
                <td style="width: 2px">
                </td>
                <td valign="top">
                    <fieldset>
                        <legend>插入</legend>
                        <table border="0" cellpadding="4" cellspacing="0">
                            <tr>
                                <td style="height: 10px">
                                    <table border="0" cellpadding="1" cellspacing="0" class="normal">
                                        <tr>
                                            <td valign="middle">
                                                Url地址:</td>
                                            <td colspan="3">
                                                <input id="TargetUrl" runat="server" name="TargetUrl" 
                                                    size="63" type="text" /></td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    &nbsp;
                    <div style="padding-top: 4px">
                        <input id="Button1" class="inputbuttoninsert" onclick="do_insert()" type="button"
                            value="确定" style="width: 80px;height:22px" />
                        &nbsp; &nbsp;
                        <input id="Button2" class="inputbuttoncancel" onclick="do_cancel()" type="button"
                            value="取消" style="width: 80px;height:22px" />
                    </div>
                </td>
            </tr>
        </table>
         <div style="display: none">
        <asp:TextBox ID="IDTextBox" runat="server" Text=""></asp:TextBox>
        <asp:TextBox ID="FullPathTextBox"
            runat="server" Text=""></asp:TextBox>
        <input id="ChannelFullUrl" type="hidden" />
       </div>
    </form>
</body>
</html>
