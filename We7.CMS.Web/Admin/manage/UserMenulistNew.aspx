<%@ Page Language="C#" MasterPageFile="~/admin/theme/classic/content.Master" AutoEventWireup="true"
    CodeBehind="UserMenulistNew.aspx.cs" Inherits="We7.CMS.Web.Admin.manage.UserMenulistNew" %>

<%@ Register TagPrefix="WEC" Namespace="We7.CMS.Controls" Assembly="We7.CMS.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <script language="javascript" type="text/javascript">
        function DeleteConfirm(id, name) {
            var messages = "删除菜单：【" + name + "】\r\n" +
                "如果为第一级菜单，它所属的二级菜单也同时删除。\r\n" +
                "菜单一旦被删除，将不能够恢复！您是否确定删除？";
            var ifConfirm = window.confirm(messages);
            if (ifConfirm) {
                var IDTextBox = document.getElementById("<%=IDTextBox.ClientID %>");
                IDTextBox.value = id;
                var DeleteButton = document.getElementById("<%=DeleteMenuButton.ClientID %>");
                DeleteButton.click();
            }
        }
        function ShowConfirm(id, name) {
            var messages = "显示菜单：【" + name + "】\r\n" +
                "把此菜单恢复显示，此操作只正对系统菜单！您是否确定恢复？";
            var ifConfirm = window.confirm(messages);
            if (ifConfirm) {
                var IDTextBox = document.getElementById("<%=IDTextBox.ClientID %>");
                IDTextBox.value = id;
                var ShowButton = document.getElementById("<%=ShowMenuButton.ClientID %>");
                ShowButton.click();
            }
        }


        function HideConfirm(id, name) {
            var messages = "隐藏菜单：【" + name + "】\r\n" +
                "如果为第一级菜单，它所属的二级菜单也同时隐藏。\r\n" +
                "您是否确定隐藏菜单？";
            var ifConfirm = window.confirm(messages);
            if (ifConfirm) {
                var IDTextBox = document.getElementById("<%=IDTextBox.ClientID %>");
                IDTextBox.value = id;
                var HideButton = document.getElementById("<%=HideMenuButton.ClientID %>");
                HideButton.click();
            }
        }
    </script>
    <script type="text/javascript" src="<%=AppPath%>/cgi-bin/search.js"></script>
    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icon_settings.png" />
        <asp:Label ID="NameLabel" runat="server" Text="网站管理后台菜单设置">
        </asp:Label>
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server" Text="本站点后台菜单设置。">
            </asp:Label>
        </span>
    </h2>
    <div class="toolbar">
        <li class="smallButton4">
            <asp:HyperLink ID="AddNewHyperLink" NavigateUrl="AddNewMenuUser.aspx" runat="server">
          新建菜单
            </asp:HyperLink></li>
        <li class="smallButton8">
            <asp:HyperLink ID="AddMenuHyperLink" NavigateUrl="AddMenuUserNew.aspx?type=1" runat="server">
            从内容模型新建菜单
            </asp:HyperLink>
        </li>
        <li class="smallButton8">
            <asp:HyperLink ID="AddFeedbackMenuHyperLink" NavigateUrl="AddFeedbackMenu.aspx" runat="server">
            从反馈模型新建菜单
            </asp:HyperLink>
        </li>
    </div>
    <br />
    <WEC:MessagePanel ID="Messages" runat="server">
    </WEC:MessagePanel>
    <div style="display: table; width: 100%">
        <ul class="subsubsub">
            <asp:Literal ID="StateLiteral" runat="server"></asp:Literal>
        </ul>
        <p class="search-box">
            <label class="hidden" for="user-search-input">
                搜索菜单:</label>
            <input type="text" class="search-input" id="KeyWord" name="KeyWord" value="" onkeydown="javascript:KeyPressSearch('Menulist.aspx',event);" />
            <input type="button" value="搜索菜单" class="button" id="SearchButton" onclick="javascript:doSearch('Menulist.aspx');" />
        </p>
    </div>
    <div>
        系统菜单可以隐藏和显示，用户自定义菜单可以删除</div>
    <div style="min-height: 300px">
        <asp:GridView ID="DataGridView" runat="server" AutoGenerateColumns="False" ShowFooter="True"
            CssClass="List">
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="ID" DataTextField="Title" DataTextFormatString="{0}"
                    DataNavigateUrlFormatString="AddNewMenuUser.aspx?id={0}" HeaderText="名称" />
                <asp:BoundField DataField="MenuType" DataFormatString="{0}" HeaderText="菜单类型" />
                <asp:BoundField DataField="Index" DataFormatString="{0}" HeaderText="序列号" />
                <asp:BoundField DataField="Group" DataFormatString="{0}" HeaderText="编组组号" />
                <asp:BoundField DataField="Created" DataFormatString="{0:yyyy-MM-dd}" HeaderText="创建时间" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <a href="<%# Eval("MenuDelUrl") %>" style="display: <%# Eval("MenuDelVisble") %>">删除</a>
                        <a href="<%# Eval("MenuSystemUrl") %>" style="display: <%# Eval("MenuSystemVisble") %>">
                            隐藏</a> <a href="<%# Eval("MenuSystemShowUrl") %>" style="display: <%# Eval("MenuSystemShowVisble") %>">
                                显示</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div class="pagination">
        <p>
            <WEC:Pager ID="Pager" PageSize="15" PageIndex="0" runat="server" OnFired="Pager_Fired" />
        </p>
    </div>
    <div style="display: none">
        <asp:Button ID="DeleteMenuButton" runat="server" Text="Save" OnClick="DeleteMenuButton_Click" />
        <asp:Button ID="ShowMenuButton" runat="server" Text="Save" OnClick="ShowMenuButton_Click" />
        <asp:Button ID="HideMenuButton" runat="server" Text="Save" OnClick="HideButton_Click" />
        <asp:TextBox ID="IDTextBox" runat="server"></asp:TextBox>
    </div>
</asp:Content>
