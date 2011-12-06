<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Departments.aspx.cs"  Inherits="We7.CMS.Web.Admin.Departments" MasterPageFile="~/admin/theme/classic/content.Master" ValidateRequest="false" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>

<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">

    <script language="javascript" type="text/javascript">
    function DeleteConfirm(id,name) {
		var msg = "删除部门：【"+name+"】\r\n"+
                "删除部门操作将彻底删除该部门以及其所属的子部门、用户等信息。\r\n" +
                "部门一旦被删除，将不能够恢复！您是否确定删除？";

		var IDTextBox=document.getElementById("<%=IDTextBox.ClientID %>"),
			NameTextBox=document.getElementById("<%=NameTextBox.ClientID %>"),
			DeleteButton=document.getElementById("<%=DeleteDepartmentButton.ClientID %>");
			IDTextBox.value=id;
			NameTextBox.value = name;

			function doDelete() {
				DeleteButton.click(); 
			}

			if (window.we7 && we7.confirm) {
				we7.confirm('<div style="text-align:left">' + msg.replace("\r\n", "<br />") + '</div>', "删除", { autoTip: false }).ok(doDelete);
			} else if(window.confim(msg)){
				doDelete();
			}
    }
    </script>
    <div>
    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_user.gif"/>
        <asp:Label ID="NameLabel" runat="server" Text="部门管理">
        </asp:Label>
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server" Text=""></asp:Label>
        </span>
    </h2>
   <div id="position">
       &nbsp;<asp:Label ID="FullPathLabel" runat="server" Text=""> </asp:Label>
   </div>
   
    <div class="toolbar">
        <!--<asp:HyperLink ID="GoParentHyperLink" runat="server">
            上一级</asp:HyperLink><span></span>
        <asp:HyperLink ID="EditHyperLink" runat="server">
            编辑</asp:HyperLink>-->
            <span>  </span>
        <asp:HyperLink ID="NewDepartmentHyperLink" runat="server">
            新建部门</asp:HyperLink>
            <span>  </span>
        <asp:HyperLink ID="NewUserHyperLink" runat="server">
            新建用户</asp:HyperLink>
            <span>  </span>
        <asp:HyperLink ID="RefreshHyperLink" runat="server">
            刷新</asp:HyperLink>
    </div>
	<div style="display:none" id=""><WEC:MessagePanel ID="Messages" runat="server"></WEC:MessagePanel></div>
    </div>
    <!--
    <div style="display:table;width:100%">
         <ul class="subsubsub">
            <asp:Literal ID="StateLiteral" runat="server"></asp:Literal>
         </ul>  
         <p class="search-box" >
	        <label class="hidden" for="user-search-input">搜索部门:</label>
	        <input type="text" class="search-input" id="KeyWord" name="KeyWord" value=""    />
	        <input type="button" value="搜索部门" class="button" id="SearchButton"    />
        </p>
    </div>
    <div style="min-height: 35px;">
        <asp:GridView ID="DepartmentsGridView" runat="server" AutoGenerateColumns="False"  CssClass="List"  GridLines="Horizontal"  ShowFooter="True" >
        <AlternatingRowStyle CssClass="alter" />
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                <asp:BoundField DataField="Mode" HeaderText="模式" Visible="False" />
                <asp:ImageField DataImageUrlField="Mode" DataImageUrlFormatString="/admin/images/icon_{0}.gif">
                    <HeaderStyle Width="20px" />
                </asp:ImageField>
                <asp:HyperLinkField DataNavigateUrlFields="Url" DataNavigateUrlFormatString="{0}"
                    DataTextField="Text" DataTextFormatString="{0}" HeaderText="名称">
                    <HeaderStyle Width="200px" />
                </asp:HyperLinkField>
                <asp:BoundField HeaderText="描述" DataField="Summary"></asp:BoundField>
                <asp:BoundField HeaderText="类型" DataField="State"></asp:BoundField>
                <asp:BoundField HeaderText="注册日期" DataField="RegisterDate"></asp:BoundField>
                <asp:TemplateField>
                    <ItemTemplate>
                      <%# Eval("ManageLinks")%>   <a href="<%# Eval("EditUrl") %>">编辑</a>
                        <a href="<%# Eval("DeleteUrl") %>">删除</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div class="pagination" style="display:none">
			 <WEC:URLPager ID="UPager" runat="server" UseSpacer="False" UseFirstLast="true"  PageSize="10"  FirstText="<< 首页" LastText="尾页 >>"
			LinkFormatActive='<span class=Current>{1}</span>' UrlFormat="Departments.aspx?pg={0}"
			CssClass="Pager" />
    </div>-->
    <div style="display:none">
        <input type="submit"  id="DoNothingButton" onclick="return false"  />
        <asp:TextBox ID="IDTextBox" runat="server"></asp:TextBox>
        <asp:TextBox ID="NameTextBox" runat="server"></asp:TextBox>
        <asp:Button ID="DeleteDepartmentButton" runat="server" OnClick="DeleteDepartmentButton_Click" />
        <asp:Button ID="DeleteAccountButton" runat="server" OnClick="DeleteAccountButton_Click" />
    </div>


    <script type="text/javascript" src="/scripts/we7/we7.loader.js"></script>
	<script>
		we7.load.ready(function () {
			function bindData() {
				var bindDestination = new we7.BindOption({
					tableName: "Department"
					, fields: { "ID": {}, "Name": {}, "Description": {}, "Created": {} }
					, sortField: "Created"
					, sortOrder: "desc"
				});

				var options = {
					caption: "部门列表",
					height: 240,
					autowidth: true,
					rowNum: 30,
					deletableRow: true,
					onBeforeDelete: function (e, id, row) {
						e.preventDefault();
						DeleteConfirm(row.ID, row.Title, "Department");
						return false;
					}
				};
				we7("#ModelList").bind(bindDestination, options);
			}

			$(document).ready(function () {
				var msg = $("#message_panel").find("div.MessagePanel td:last").html();

				if ($.trim(msg)) {
					we7.info(msg);
				}

				bindData();
			});
		});
	</script>
    <table id="ModelList" style="display:none">
		 <tr><td header="名称"><img src="/admin/images/icon_Department.gif" style="border-width:0px;"><a href="DepartmentDetail.aspx?id=${ID}">${Title}</a></td><td header="描述" editable="text">${Description}</td><td header="添加日期" editable="date" editkey="Created">{{html Created.substr(0,10)}}</td></tr>
    </table>
   <script type="text/javascript">
	$(function(){
		$('#KeyWord').bind('keyup',function(event) {  
          if(event.keyCode==13){  
           window.location="Departments.aspx?keyword="+encodeURIComponent(this.value);
          }
       });
       $('#SearchButton').click(function() {
           window.location="Departments.aspx?keyword="+encodeURIComponent($('#KeyWord').val());
       });
       if(QueryString('keyword'))
            $('#KeyWord').val(QueryString('keyword'));
	});
</script>
</asp:Content>
