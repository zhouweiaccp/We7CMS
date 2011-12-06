<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/theme/classic/content.Master"
    CodeBehind="CatTypeMgr.aspx.cs" Inherits="We7.CMS.Web.Admin.CatTypeMgr" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <base target="_self" />
    <script type="text/javascript">

    	function checkAll(checked) {
    		var list = document.getElementsByName("ids");
    		for (var i = 0; i < list.length; i++) {
    			list[i].checked = checked;
    		}
    	}       

    </script>
    <script src="<%=AppPath%>/cgi-bin/DialogHelper.js" type="text/javascript"></script>
    <script type="text/javascript" src="/scripts/we7/we7.loader.js">
	    $(document).ready(function(){
		    we7('span[rel=xml-hint]').help();
	    });
    </script>
    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icon_settings.png" />
        <asp:Label ID="NameLabel" runat="server" Text="自定义类别">
        </asp:Label>
        <span rel="xml-hint" title="类别：一种层级的数据结构，与内容模型结合使用，能为您的应用提供更多的功能。" style="margin-left:10px" ></span>
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server" Text="系统允许自定义多种类别，以实现多类别的管理。">
            </asp:Label>            
        </span>
    </h2>
    <div class="toolbar">
        <li>
            <asp:HyperLink ID="lnkAdd" runat="server" Text="新建" NavigateUrl="CatTypeAdd.aspx"></asp:HyperLink>
        </li>        
        <li>
            <asp:LinkButton ID="lnkDel" OnClientClick="return confirm('确认删除？');" runat="server"
                Text="删除"></asp:LinkButton>
        </li>
    </div>
    <div style="display: table; width: 100%">
        <p class="search-box">
        </p>
    </div>
    <WEC:MessagePanel runat="Server" ID="Messages">
    </WEC:MessagePanel>
    <div>
        <asp:GridView ID="DataGridView" runat="server" AutoGenerateColumns="False" Width="100%"
            ShowFooter="True" CssClass="List" GridLines="Horizontal">
            <Columns>
                <asp:TemplateField ItemStyle-Width="10px">
                    <HeaderTemplate>
                        <input type="checkbox" onclick="checkAll(this.checked)" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <input type="checkbox" name="ids" value="<%# Eval("ID") %>" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Name" HeaderText="类别名称" />
                <asp:BoundField DataField="Keyword" HeaderText="标识符" />
                <asp:TemplateField HeaderText="编辑">
                    <ItemTemplate>
                        <a href="CatTypeAdd.aspx?id=<%# Eval("ID") %>">编辑</a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="管理">
                    <ItemTemplate>
                        <a href="CategoryList.aspx?typeId=<%# Eval("ID") %>">数据项管理</a>
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
</asp:Content>
