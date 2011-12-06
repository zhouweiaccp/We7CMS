<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/admin/theme/classic/content.Master"
    CodeBehind="AdviceTypes.aspx.cs" Inherits="We7.CMS.Web.Admin.AdviceTypes" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <base target="_self" />
    <script src="<%=AppPath%>/cgi-bin/DialogHelper.js" type="text/javascript"></script>
    <script type="text/javascript">
        function closeWindow(v, t) {
            weCloseDialog(v, t);
        }
        function deleteArticle() {
            if (confirm('您确认要把此留言类型删除删除吗？删除的同时把此类型的菜单也同时删除了，此操作不可恢复，请确认！')) {
                var submitBtn = document.getElementById("<%=DeleteButton.ClientID %>");
                submitBtn.click();
            }
        }
        function onCancelHyperLinkClick() {
            closeWindow(null, null);
        }
        function onSubmitHyperLinkClick() { 
            var submitBtn = document.getElementById("<%=SubmitButton.ClientID %>");
            submitBtn.click();
        }
        function SelectAll(tempControl) {
            var theBox = tempControl;
            xState = theBox.checked;

            elem = theBox.form.elements;
            for (i = 0; i < elem.length; i++) {
                if (elem[i].type == "checkbox" && elem[i].id != theBox.id) {
                    if (elem[i].checked != xState)
                        elem[i].click();
                }
            }
        }
//        function document.onkeydown() {
//            if (event.keyCode == 13) {
//                var btn = document.getElementById("<%=QueryButton.ClientID %>");
//                btn.focus();
//                btn.click();
//            }
//        }
        $(document).keydown(function (event) {
            if (event.keyCode == 13) {
                var btn = document.getElementById("<%=QueryButton.ClientID %>");
                btn.focus();
                btn.click();
            }
        });
    </script>
    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icon_settings.png" />
        <asp:Label ID="NameLabel" runat="server" Text="互动反馈类型管理">
        </asp:Label>
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server" Text="系统允许自定义多种反馈模型，反馈模型同时生成对应菜单，位于<反馈>主菜单下。"><a href="http://help.we7.cn/library/58.html" target="_blank">如何新建反馈类型？</a></
            </asp:Label>
        </span>
    </h2>
    <div class="toolbar">
        <li>
            <asp:HyperLink ID="DeleteHyperLink" NavigateUrl="javascript:deleteArticle();" runat="server">
          删除
            </asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="AddAdviceTypeHyperLink" runat="server" NavigateUrl="AdviceTypeEdit.aspx"
                Target="_parent">
            创建新类型</asp:HyperLink>
        </li>
        <li>
            <a href="/admin/ContentModel/Models.aspx?modelType=ADVICE">模型管理</a>
        </li>
    </div>
    <div style="display: table; width: 100%">
        <p class="search-box">
            模型名称：
            <asp:TextBox ID="SearchTextBox" ToolTip="  按模型名称查询  " Text="" runat="server"></asp:TextBox>
            创始人：
            <asp:TextBox ID="AccountTextBox" ToolTip="  按创始人名称查询  " runat="server"></asp:TextBox>
            <input type="button" class="button" id="QueryButton" runat="server" value=" 查询" onserverclick="QueryButton_ServerClick" />
        </p>
    </div>
    <WEC:MessagePanel runat="Server" ID="Messages">
    </WEC:MessagePanel>
    <div>
        <asp:GridView ID="DataGridView" runat="server" AutoGenerateColumns="False" Width="100%"
            ShowFooter="True" CssClass="List" GridLines="Horizontal">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkHeader" runat="server" onclick="javascript:SelectAll(this);"
                            AutoPostBack="false" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkItem" runat="server" />
                        <asp:TextBox ID="MenuNameTextBox" runat="server" Text='<%# Eval("Title") %>' Visible="False"></asp:TextBox>
                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="False"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="25px" />
                </asp:TemplateField>
                <asp:HyperLinkField DataNavigateUrlFields="ID" HeaderText="类型名称" DataNavigateUrlFormatString="AdviceTypeEdit.aspx?adviceTypeID={0}"
                    DataTextField="Title" DataTextFormatString="{0}" Target="_parent" />
                <asp:TemplateField HeaderText="创建者">
                    <ItemTemplate>
                        <%# GetAccountNameByAdviceTypeID(Eval("ID").ToString())%>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                </asp:TemplateField>
                <asp:BoundField HeaderText="操作类别" DataField="StateText"></asp:BoundField>
                <asp:BoundField HeaderText="创建日期" DataField="CreateDate"></asp:BoundField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <a href="../manage/AddFeedbackMenu.aspx?menuTypeID=<%# Eval("ID") %>&menuName=<%# Eval("Title")%>&returnURL=<%= Request.RawUrl %>"
                            class="addMenu">将此类型加入到菜单</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div class="pagination">
        <p>
            <WEC:Pager ID="AdvicePager" PageSize="15" PageIndex="0" runat="server" OnFired="Pager_Fired" />
        </p>
    </div>
    <br />
    <div style="display: none">
        <asp:Button ID="SubmitButton" runat="server" Text="" OnClick="SubmitButton_Click" />
        <asp:Button ID="DeleteButton" runat="server" Text="" OnClick="DeleteButton_Click" />
        <asp:TextBox ID="AttributeValueTextBox" runat="server"></asp:TextBox>
        <input type="hidden" id="hdDemoSite" value='<%= We7.Framework.Config.GeneralConfigs.GetConfig().IsDemoSite.ToString().ToLower() %>' />
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $("a.addMenu").click(function () {
                var isDemoSite = document.getElementById("hdDemoSite").value;
                if (isDemoSite === "true") {
                    alert("对不起，此演示站点您没有该操作权限！");
                    return false;
                }
            });            
        });
    </script>
</asp:Content>
