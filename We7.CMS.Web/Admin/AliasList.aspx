<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AliasList.aspx.cs" Inherits="We7.CMS.Web.Admin.AliasList" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>选择一个别名</title>
   <base target="_self"></base> 
    <link rel="stylesheet" type="text/css" href="styles/list.css" media="screen" />
    <script src="<%=AppPath%>/cgi-bin/DialogHelper.js" type="text/javascript"></script>
    <script type="text/javascript">
var MSG_OPENER_WAS_CLOSED = "The callee was be closed. The process cannot be processed.  Please close the window and reopen it to fix this problem.";
var MSG_ERROR_OCCUR = "The function returns a error. ";

function showErrorMessage(s)
{
    var p = document.all["MessageLabel"];
    if(p)
    {
        p.style.color = "red";
        p.innerText = s;
    }
}

function closeWindow(v, t) {
    try 
    {
        window.close();
    }
    catch(e)
    {
        if(e.number == -2147418094)
        {
            showErrorMessage(MSG_OPENER_WAS_CLOSED);
        }
        else 
        {
            showErrorMessage(MSG_ERROR_OCCUR + e.message);
        }
    }
}

function onSelectHyperLinkClick() {
    var cs = mainForm.Radios;
    if(cs) {
        if(cs.length || cs.checked == false) {
            for(var i=0; i<cs.length; i++) {
                if(cs[i].checked) {
                    window.returnValue =cs[i].value+","+ cs[i].title;
                    window.close();
                    return;
                }
            }
            showErrorMessage("您还没有选择别名！");
        }
        else {
                window.returnValue =cs.value+","+ cs.title;
                window.close();
        }
    }
    else {
        showErrorMessage("列表中没有别名。");
    }
}


function onCancelHyperLinkClick() {
  closeWindow(null, null);
}

function onDocumentLoad() {
}
    </script>
</head>
<body id="classic" onload="onDocumentLoad()"  style="background-color:ButtonFace" leftmargin=10 rightmargin=10>
    <form id="mainForm" runat="server">
        <div id="wrap">
            <div id="content-wrap">
                <div id="content">
                    <div id="breadcrumb">
                        <br />
                        <div>
                            <span>快速搜索：</span>
                            <asp:DropDownList ID="FieldDropDownList" runat="server" Enabled="false">
                                <asp:ListItem Value="Words">按名称查找</asp:ListItem>
                                <asp:ListItem Value="AliasType">按类别查找</asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="SearchTextBox" runat="server" Columns="20" MaxLength="64"></asp:TextBox>
                            <asp:HyperLink ID="SearchHyperLink" runat="server" NavigateUrl="javascript:document.mainForm.QueryButton.click();">
                                <asp:Image ID="SearchImage1" runat="server" ImageUrl="~/admin/Images/icon_search.gif" />
                                执行</asp:HyperLink>
                        </div>
                         <div class="toolbar">
                            <asp:HyperLink ID="SelectHyperLink" runat="server" NavigateUrl="javascript:onSelectHyperLinkClick();">
                                <asp:Image ID="SelectImage" runat="server" ImageUrl="~/admin/Images/icon_ok.gif" />
                                选择</asp:HyperLink>
                            <span>| </span>
                            <asp:HyperLink ID="RefreshHyperLink" runat="server" NavigateUrl="AliasList.aspx">
                                <asp:Image ID="RefreshImage" runat="server" ImageUrl="~/admin/Images/icon_refresh.gif" />
                                刷新</asp:HyperLink>
                            <span>|</span>
                            <asp:HyperLink ID="CancelHyperLink" runat="server" NavigateUrl="javascript:onCancelHyperLinkClick();">
                                <asp:Image ID="CancelImage" runat="server" ImageUrl="~/admin/Images/icon_cancel.gif" />
                                取消</asp:HyperLink>
                        </div>
                        <div id="messageLayer">
                            <asp:Image ID="MessageImage" runat="server" ImageUrl="~/admin/Images/icon_info.gif" />
                            <asp:Label ID="MessageLabel" runat="server" Text="">
                            </asp:Label>
                        </div>
                        <div>
                            <asp:GridView ID="DetailGridView" runat="server" AutoGenerateColumns="False" ShowFooter="True">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemStyle Width="10px" />
                                        <ItemTemplate>
                                            <input type="radio" name="Radios" value="<%#DataBinder.Eval(Container.DataItem, "Words") %>"
                                                title="<%#DataBinder.Eval(Container.DataItem, "AliasType") %>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Words" DataFormatString="{0}" HeaderText="名称" />
                                    <asp:BoundField DataField="AliasType" DataFormatString="{0}" HeaderText="类别" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <br />
                        <div style="display: none">
                            <asp:Button ID="QueryButton" runat="server" Text="Query" OnClick="QueryButton_Click" />
                            <asp:TextBox ID="IDTextBox" runat="server" Text=""></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
