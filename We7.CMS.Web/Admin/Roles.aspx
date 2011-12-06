<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Roles.aspx.cs" Inherits="We7.CMS.Web.Admin.Roles"
    MasterPageFile="~/admin/theme/classic/content.Master" %>

<%@ Register TagPrefix="WEC" Namespace="We7.CMS.Controls" Assembly="We7.CMS.UI" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <script language="javascript" type="text/javascript">
        function DeleteConfirm(id, name, state) {
            if (state == "true") {
                alert("对不起角色【" + name + "】为站群角色，不允许删除！");
            }
            else {
                var messages = "删除角色：【" + name + "】\r\n" +
                "此操作将删除角色信息，以及与该角色相关的授权信息。\r\n" +
                "角色一旦被删除，将不能够恢复！您是否确定删除？";
                var ifConfirm = window.confirm(messages);
                if (ifConfirm) {
                    var IDTextBox = document.getElementById("<%=IDTextBox.ClientID %>");
                    IDTextBox.value = id;
                    var NameTextBox = document.getElementById("<%=NameTextBox.ClientID %>");
                    NameTextBox.value = name;
                    var DeleteButton = document.getElementById("<%=DeleteButton.ClientID %>");
                    DeleteButton.click();
                }
            }

        }
    </script>
    <script type="text/javascript" src="<%=AppPath%>/cgi-bin/search.js"></script>
    <h2 class="title">
        <asp:Image ID="RoleImage" runat="server" ImageUrl="~/admin/Images/icons_user.gif" />
        <asp:Label ID="RoleLabel" runat="server" Text="角色管理">
        </asp:Label>
        <span class="summary">
            <asp:Label ID="RoleSummaryLabel" runat="server" Text="">
            </asp:Label>
        </span>
    </h2>
    <WEC:MessagePanel ID="Messages" runat="server">
    </WEC:MessagePanel>
    <div class="toolbar">
        <asp:HyperLink ID="NewRoleHyperLink" NavigateUrl="Permissions/RoleEdit.aspx" runat="server">
            <asp:Image ID="NewRoleImage" runat="server" ImageUrl="~/admin/Images/icon_new.gif"
                Visible="false" />
            新添角色</asp:HyperLink>
        <span></span>
        <asp:HyperLink ID="RefreshHyperLink" NavigateUrl="Roles.aspx" runat="server">
            <asp:Image ID="RefreshImage" runat="server" ImageUrl="~/admin/Images/icon_refresh.gif"
                Visible="false" />
            刷新</asp:HyperLink>
    </div>
    <div style="display: table; width: 100%">
        <ul class="subsubsub">
            <asp:Literal ID="StateLiteral" runat="server"></asp:Literal>
        </ul>
        <!--
        <p class="search-box">
            <label class="hidden" for="user-search-input">
                搜索角色:</label>
            <input type="text" class="search-input" id="KeyWord" name="KeyWord" value="" onkeydown="javascript:KeyPressSearch('Roles.aspx',event);" />
            <input type="button" value="搜索角色" class="button" id="SearchButton" onclick="javascript:doSearch('Roles.aspx');" />
        </p>-->
    </div>
    <div style="min-height: 35px; width: 100%">
        <%--    <asp:Repeater ID="ListRepeater" runat="server" >
    <HeaderTemplate>
    <table class="List">
    <tr><td>名称</td></tr>
    </HeaderTemplate>
    <ItemTemplate>
    <tr>
    <td><a href="Permissions/RoleEdit.aspx?id=<%# Eval("ID") %>"><%# Eval("Name")%></a></td>
    <td></td>
    </tr>
    </ItemTemplate>
    <FooterTemplate></table></FooterTemplate>
    </asp:Repeater>--%>
    <!--
        <asp:GridView ID="DataGridView" runat="server" AutoGenerateColumns="False" Width="100%"
            CellSpacing="0" ShowFooter="True" CssClass="List" GridLines="Horizontal" EnableViewState="false">
            <AlternatingRowStyle CssClass="alter" />
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="Permissions/RoleEdit.aspx?id={0}"
                    DataTextField="Name" DataTextFormatString="{0}" HeaderText="名称">
                    <HeaderStyle Width="200px" />
                </asp:HyperLinkField>
                <asp:BoundField HeaderText="描述" DataField="Description"></asp:BoundField>
                <asp:BoundField HeaderText="类型" DataField="TypeText"></asp:BoundField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <a href="Permissions/RoleEdit.aspx?id=<%# Eval("ID") %>">编辑</a> <a href="Permissions/RoleEdit.aspx?id=<%# Eval("ID") %>&tab=2">
                            所属用户</a> <a href="Permissions/RoleEdit.aspx?id=<%# Eval("ID") %>&tab=3">模块权限</a>
                        <%--                        <a href="Permissions/RoleEdit.aspx?id=<%# Eval("ID") %>&tab=4">
                            功能权限</a>--%>
                        <a href="javascript:DeleteConfirm('<%# Eval("ID") %>','<%# Eval("Name") %>','<%# Eval("IsGroupRole") %>');">
                            删除</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div class="pagination">
        <p>
            <WEC:Pager ID="Pager" PageSize="15" PageIndex="0" runat="server" />
        </p>
    </div>
    <div style="display: none">
        <asp:TextBox ID="IDTextBox" runat="server"></asp:TextBox>
        <asp:TextBox ID="NameTextBox" runat="server"></asp:TextBox>
        <input type="submit" id="DoNothingButton" onclick="return false" />
        <input type="submit" id="DeleteButton" runat="server" onserverclick="DeleteButton_Click" />
    </div>-->
    <script type="text/javascript" src="/scripts/we7/we7.loader.js">
	
		// 定义条件
        var FromSiteID = new we7.BindCondition("FromSiteID", we7.bindVerb.equals, "<%=SiteID%>");
		var CurrentState=null;
         <%if (CurrentState!=OwnerRank.All)
       {%>
         CurrentState= new we7.BindCondition("RoleType", we7.bindVerb.equals, "<%=(int)CurrentState%>");
       <%}%>
		
		// 定义要绑定的资源的目标
		var bindDestination = new we7.BindOption({
			tableName: "Role"
		    , fields: {
                      "ID":{},"Name":{},"Description":{},"Created":{},
                      "RoleType":{
                        value:{"0":"管理员角色","1":"普通用户角色"}
                      }
            }
			, sortField: "Created"
			, sortOrder: "desc"
		});
		bindDestination.conditions.push(FromSiteID);
		bindDestination.conditions.push(CurrentState);
		//绑定过程
		function bindData(){
			var options = {
				caption: "角色列表",
				height: 220,
                autowidth:true,
                rowNum:10,
                editColumnHeader:"管理",
			};
			
			we7("#ModelList").bind(bindDestination, options);
		}
		
		$(document).ready(function () {
			bindData();
		});
    </script>
    <table id="ModelList" style="display:none">
        <tr><td header="名称"><a href="Permissions/RoleEdit.aspx?id=${ID}">${Title}</a></td><td header="描述" editable="text">${Description}</td><td header="类型" editable="select" editkey="RoleType" sortkey="RoleType">{{if RoleType==0}}管理员角色{{else}}普通用户角色{{/if}}</td><td header="操作"><a href="Permissions/RoleEdit.aspx?id=${ID}&tab=2">所属用户</a> <a href="Permissions/RoleEdit.aspx?id=${ID}&tab=3">模块权限</a></td></tr>
    </table>
</asp:Content>
