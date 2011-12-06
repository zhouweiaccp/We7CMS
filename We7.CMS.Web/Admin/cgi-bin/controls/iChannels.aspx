<%@ Page Language="C#" AutoEventWireup="true" Codebehind="iChannels.aspx.cs" Inherits="We7.CMS.Web.Admin.cgi_bin.controls.iChannels" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
    body,td,th {
	font-size: 12px;}
	
	a:link,a:visited
	{
	    text-decoration:none;
	}
	
</style>

    <script type="text/javascript">
        function freshValues(){
        
            if(parent.document.getElementById('SummaryLabel'))
            {
                 parent.document.getElementById('SummaryLabel').innerHTML='<%=CurrentPath %>';  
             }
            if(parent.document.getElementById('ifArticles'))
            {
                 parent.document.getElementById('ifArticles').src='/admin/cgi-bin/controls/iArticles.aspx?id=<%=ColumnID %>';  
            }
            //引用栏目添加文章
            if(parent.document.getElementById('quoteArticles'))
            {
                 parent.document.getElementById('quoteArticles').src='/admin/cgi-bin/controls/iArticles.aspx?id=<%=ColumnID %>&oid=<%=QuoteOwnerID %>&type=quote';  
            }
             
            if(parent.document.getElementById('TargetUrl'))
            {
                 parent.document.getElementById('TargetUrl').value='<%=CurrentUrl %>';  
            }
            
            if(parent.document.getElementById('ChannelFullUrl'))
            {
                 parent.document.getElementById('ChannelFullUrl').value='<%=CurrentUrl %>';  
            }
            
        }
        
        
        window.onload=freshValues;
    </script>

</head>
<body style="margin: 0; padding: 0">
    <form id="form1" runat="server">
        <div>
            <asp:HyperLink ID="GoParentHyperLink" runat="server">
                <asp:Image ID="GoParentImage" runat="server" ImageUrl="~/admin/Images/icon_parent.gif"
                    ToolTip="返回上一级栏目" />
                ..</asp:HyperLink>
            <asp:GridView ID="DetailGridView" runat="server" CellPadding="4" AutoGenerateColumns="False"
                ShowHeader="False" GridLines="None">
                <Columns>
                    <asp:ImageField DataImageUrlField="ID" DataImageUrlFormatString="~/admin/Images/icon_folder.gif">
                        <ItemStyle Width="16px" />
                    </asp:ImageField>
                    <asp:TemplateField HeaderText="名称">
                        <ItemTemplate>
                            <a href="iChannels.aspx?id=<%# DataBinder.Eval(Container.DataItem, "ID")  %>&oid=<%=QuoteOwnerID %>&type=quote">
                                <%# DataBinder.Eval(Container.DataItem, "Name")  %>
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="创建时间" Visible="False">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Created") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Created", "{0}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
