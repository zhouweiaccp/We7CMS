<%@ Control Language="C#" AutoEventWireup="true" Codebehind="Role_Accounts.ascx.cs"
    Inherits="We7.CMS.Web.Admin.Role_Accounts" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WED" %>
<div>
    <WED:MessagePanel ID="Messages" runat="server">
    </WED:MessagePanel>
</div>
    <script type="text/javascript" src="<%=AppPath%>/cgi-bin/search.js"></script>
<script language="javascript" type="text/javascript">
    function DeleteConfirm(id,name)
    {
        var messages="用户：【"+name+"】退出当前角色\r\n"+
            "此操作将使此用户退出当前角色，您是否确定？";
        var ifConfirm= window.confirm(messages);
        if(ifConfirm)
        {
            var IDTextBox=document.getElementById("<%=IDTextBox.ClientID %>");
            IDTextBox.value=id;
            var NameTextBox=document.getElementById("<%=NameTextBox.ClientID %>");
            NameTextBox.value=name;
            var DeleteButton=document.getElementById("<%=DeleteButton.ClientID %>");
            DeleteButton.click(); 
        }
    }
</script>

<div id="conbox">
    <dl>
        <dt>»角色所属用户<br />
            <img src="/admin/images/bulb.gif" align="absmiddle" alt="" />
            <label class="block_info">
                此处对本角色下所拥有的用户可进行查看与删除工作，如果想关联用户请在用户编辑处进行！</label>
        </dt>
          <div style="display:table;width:100%">  
         <p class="search-box" >
	        <label class="hidden" for="user-search-input">搜索用户:</label>
	        <input type="text" class="search-input" id="KeyWord" name="KeyWord" value=""  onkeydown="javascript:KeyPressSearch('/admin/Permissions/RoleEdit.aspx?id=<%=RoleID %>&tab=2',event);"  />
	        <input type="button" value="搜索用户" class="button" id="SearchButton"  onclick="javascript:doSearch('/admin/Permissions/RoleEdit.aspx?id=<%=RoleID %>&tab=2');"  />
        </p>
    </div>
        <dd>
            <asp:GridView ID="personalForm" runat="server" AutoGenerateColumns="False" ShowFooter="True"  GridLines="Horizontal"  CssClass="List" >
                <Columns>
                    <asp:ImageField DataImageUrlField="ID" DataImageUrlFormatString="~/admin/Images/icon_folder.gif">
                        <HeaderStyle Width="25px" />
                    </asp:ImageField>
                    <asp:BoundField HeaderText="用户" DataField="AccountName"></asp:BoundField>
                    <asp:BoundField HeaderText="所属部门" DataField="DepartmentName"></asp:BoundField>
                    <%--<asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="RoleAccountDelete.aspx?id={0}"
                        DataTextField="ID" DataTextFormatString="删除" />--%>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <a href="javascript:DeleteConfirm('<%# Eval("ID") %>','<%# Eval("AccountName") %>');">
                                退出当前角色 </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </dd>
    </dl>
</div>
        <div class="pagination">
			 <WED:URLPager ID="ArticleUPager" runat="server" UseSpacer="False" UseFirstLast="true"  PageSize="15"  FirstText="<< 首页" LastText="尾页 >>"
			LinkFormatActive='<span class=Current>{1}</span>' CssClass="Pager" />
    </div>
<div style="display: none">
    <asp:TextBox ID="IDTextBox" runat="server"></asp:TextBox>
    <asp:TextBox ID="NameTextBox" runat="server"></asp:TextBox>
    <asp:Button ID="DeleteButton" runat="server" OnClick="DeleteButton_Click" />
</div>
