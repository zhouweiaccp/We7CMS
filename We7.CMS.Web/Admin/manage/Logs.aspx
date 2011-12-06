<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logs.aspx.cs" Inherits="We7.CMS.Web.Admin.Logs" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:content id="We7Content" contentplaceholderid="MyContentPlaceHolder" runat="server">
    <script type="text/javascript" language="javascript" src="/Scripts/we7/we7.loader.js">
    we7("#<%=starttime2.ClientID %>").pickDate();
    we7("#<%=endtime2.ClientID %>").pickDate();
    </script>

    <script type="text/javascript">
        function SelectAll(tempControl) {
            var theBox = tempControl;
            xState = theBox.checked;

            elem = theBox.form.elements;
            for (i = 0; i < elem.length; i++)
                if (elem[i].type == "checkbox" && elem[i].id != theBox.id) {
                    if (elem[i].checked != xState)
                        elem[i].click();
                }
        }
        function DeleteBtn() {
            var DeleteBtn = document.getElementById("<%=DeleteBtn.ClientID %>");
            DeleteBtn.click();
        }
        function OutPutButton() {
            var OutPutButton = document.getElementById("<%=OutPutButton.ClientID %>");
            OutPutButton.click();
        }
        function QueryButton() {
            var QueryButton = document.getElementById("<%=QueryButton.ClientID %>");
            QueryButton.click();
        }
    </script>
                        <h2 class="title" >
                            <asp:Image ID="LogImage" runat="server" ImageUrl="~/admin/Images/icons_tools.gif" />
                            <asp:Label ID="LogLabel" runat="server" Text="日志管理">
                            </asp:Label>
                            <span class="summary">
                                <asp:Label ID="LogSummaryLabel" runat="server" Text="">
                                </asp:Label>
                            </span>
                        </h2>
                        <div class="toolbar">
                            <asp:HyperLink ID="RefreshHyperLink" NavigateUrl="Logs.aspx" runat="server">
                                刷新</asp:HyperLink>
                            <asp:HyperLink ID="DeleteHyperLink" NavigateUrl="javascript:DeleteBtn();"
                                runat="server">
                                删除
                            </asp:HyperLink>
                            <asp:HyperLink ID="OutPutHyperLink" NavigateUrl="javascript:OutPutButton();" runat="server">
                                导出日志</asp:HyperLink>
                            <asp:HyperLink ID="QueryHyperLink" NavigateUrl="javascript:QueryButton();"
                                runat="server">
                                查询
                            </asp:HyperLink>
                        </div>
                        <asp:Panel ID="QueryPanel" runat="server" Width="100%">
                            <table class='tree'>
                                <tr valign="top">
                                    <td style="height: 30px">
                                        时间：从<input id="starttime2"  size="12" name="starttime" runat="server" />
                                        到<input id="endtime2"  size="12" name="endtime" runat="server" />
                                        的日志
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        根据用户名称查找：
                                        <asp:TextBox ID="UserTextBox" runat="server"></asp:TextBox></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="MessagePanel" runat="Server" Visible="false">
                            <asp:Image ID="MessageImage" runat="server" ImageUrl="~/admin/Images/icon_warning.gif" />
                            <asp:Label ID="MessageLabel" runat="server" Text=""></asp:Label>
                        </asp:Panel>
                        <asp:Panel ID="LogListPanel" runat="server" Width="100%" Visible="False">
                            <div style="min-height: 350px;">
                                <asp:GridView ID="DataGridView" runat="server" AutoGenerateColumns="False" Width="100%"
                                    ShowFooter="True">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkHeader" runat="server" onclick="javascript:SelectAll(this);"
                                                    AutoPostBack="false" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkItem" runat="server" />
                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="False"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="LogDetail.aspx?id={0}"
                                            DataTextField="Content" DataTextFormatString="{0}" HeaderText="操作描述">
                                            <HeaderStyle Width="40%" />
                                        </asp:HyperLinkField>
                                        <asp:BoundField HeaderText="用户" DataField="UserID"></asp:BoundField>
                                        <asp:BoundField HeaderText="页面" DataField="Page"></asp:BoundField>
                                        <asp:BoundField HeaderText="时间" DataField="Created"></asp:BoundField>
                                        <asp:BoundField HeaderText="备注" DataField="Remark"></asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="pagination">
                                <p>
                                    <WEC:Pager ID="LogPager" PageSize="15" PageIndex="0" runat="server" OnFired="Pager_Fired" />
                                </p>
                            </div>
                        </asp:Panel>
                        <div style="display: none">
                            <asp:Button ID="QueryButton" runat="server" OnClick="QueryButton_Click" Text="Query" />
                            <asp:Button ID="DeleteBtn" runat="server" Text="Delete" OnClick="DeleteBtn_Click" />
                           <asp:Button ID="OutPutButton" runat="server" Text="OutPut" OnClick="OutPutButton_Click" /> 
                        </div>
</asp:content>
