<%@ Page Language="C#" AutoEventWireup="true" Codebehind="TemplateList.aspx.cs" Inherits="We7.CMS.Web.Admin.TemplateList" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>选择一个模板</title>
   <base target="_self"></base> 
   <link rel="stylesheet" type="text/css" href="theme/classic/css/main.css" media="screen" />
    <script src="<%=AppPath%>/cgi-bin/DialogHelper.js" type="text/javascript"></script>

    <script type="text/javascript">
        var MSG_OPENER_WAS_CLOSED = "The callee was be closed. The process cannot be processed.  Please close the window and reopen it to fix this problem.";
        var MSG_ERROR_OCCUR = "The function returns a error. ";

        function showErrorMessage(s) {
            var p = document.all["MessageLabel"];
            if (p) {
                p.style.color = "red";
                p.innerText = s;
            }
        }

        function closeWindow(v, t) {
            try {
                //       closeDialog(v, t);
                window.close();
            }
            catch (e) {
                if (e.number == -2147418094) {
                    showErrorMessage(MSG_OPENER_WAS_CLOSED);
                }
                else {
                    showErrorMessage(MSG_ERROR_OCCUR + e.message);
                }
            }
        }

        function onSelectHyperLinkClick() {
            var cs = mainForm.Radios;
            if (cs) {
                if (cs.length || cs.checked == false) {
                    for (var i = 0; i < cs.length; i++) {
                        if (cs[i].checked) {
                            //                    closeWindow(cs[i].value, cs[i].title);

                            weCloseDialog(cs[i].value, cs[i].title);
                            //                    window.returnValue =cs[i].value+","+ cs[i].title;
                            //                    window.close();
                            return;
                        }
                    }
                    showErrorMessage("您还没有选择模板！");
                }
                else {
                    //            closeWindow(cs.value, cs.title);
                    //                window.returnValue =cs.value+","+ cs.title;
                    //                window.close();
                    weCloseDialog(cs[i].value, cs[i].title);
                }
            }
            else {
                showErrorMessage("列表中没有模板。");
            }
        }

        function onSelectThisClick(v, t) {
            weCloseDialog(v, t);
        }

        function onCancelHyperLinkClick() {
            //  closeWindow(null, null);
            weCloseDialog(null, null);
        }

        function onDocumentLoad() {
        }
    </script>
</head>
<body  onload="onDocumentLoad()"  style="padding-left:10px;padding-right:10px" >
    <form id="mainForm" runat="server">

                            <h2>
                                <asp:Label ID="selectLabel" runat="server" Text="选择一个模板：">
                                </asp:Label>
                            </h2>    
                          <p class="search-box">
                            <span>快速搜索：</span>
                            <asp:DropDownList ID="FieldDropDownList" class="search-input"  runat="server" Enabled="false">
                                <asp:ListItem Value="Name">按名称查找</asp:ListItem>
                                <asp:ListItem Value="Author">按作者查找</asp:ListItem>
                                <asp:ListItem Value="Description">按描述查找</asp:ListItem>
                                <asp:ListItem Value="FileName">按文件名查找</asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="SearchTextBox" class="search-input" runat="server" Columns="20" MaxLength="64"></asp:TextBox>
                            <asp:HyperLink ID="SearchHyperLink" runat="server"  NavigateUrl="javascript:document.mainForm.QueryButton.click();">
                                <asp:Image ID="SearchImage1" runat="server" ImageUrl="~/admin/Images/icon_search.gif" />
                                搜索</asp:HyperLink>
                        </p>

                        <div style="display:table;min-height:35px;width:100%">
                            <asp:GridView ID="DetailGridView" runat="server" AutoGenerateColumns="False" GridLines="Horizontal"  ShowFooter="false" CssClass="List" >
                                <Columns>
                                    <asp:TemplateField  >
                                    <HeaderTemplate>名称</HeaderTemplate>
                                        <ItemStyle Width="100px" />
                                        <ItemTemplate>
                                                <a href="javascript:onSelectThisClick('<%#DataBinder.Eval(Container.DataItem, "FileName") %>','<%#DataBinder.Eval(Container.DataItem, "Name") %>')" title="点击选择">
                                                <%#DataBinder.Eval(Container.DataItem, "Name") %></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Description" DataFormatString="{0}" HeaderText="描述" />
                                    <asp:BoundField DataField="FileName" HeaderText="文件" />
                                   <%-- <asp:BoundField DataField="TypeText" HeaderText="类型" />--%>
                                    <asp:BoundField DataField="Created"  HeaderText="创建日期" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <br />
                      <div id="messageLayer">
                            <asp:Image ID="MessageImage" runat="server" ImageUrl="~/admin/Images/icon_info.gif" />
                            <asp:Label ID="MessageLabel" runat="server" Text="">
                            </asp:Label>
                        </div>

                        <div style="display: none">
                            <asp:Button ID="QueryButton" runat="server" Text="Query" OnClick="QueryButton_Click" />
                            <asp:TextBox ID="IDTextBox" runat="server" Text=""></asp:TextBox>
                        </div>
    </form>
</body>
</html>
