<%@ Page Language="C#" AutoEventWireup="true" Codebehind="iArticles.aspx.cs" Inherits="We7.CMS.Web.Admin.iArticles" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
    body,td,th {
	font-size: 12px;}
	
</style>

    <script type="text/javascript">
        function returnUrl(url)
        {
            if(parent.document.getElementById('TargetUrl'))
            {
                 parent.document.getElementById('TargetUrl').value=parent.document.getElementById('ChannelFullUrl').value + url;  
            }
        }
        function addArticles()
        {
           document.form1.AddArticlesButton.click(); 
        } 
    </script>

</head>
<body style="margin: 0; padding: 0">
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="DetailGridView" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                GridLines="None" OnRowCreated="DetailGridView_RowCreated" Width="300px" CellPadding="3"
                OnRowDataBound="DetailGridView_RowDataBound">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkItem" runat="server" />
                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="False"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:ImageField DataImageUrlField="ID" DataImageUrlFormatString="~/admin/Images/logo_topic.gif">
                        <ItemStyle Width="16px" />
                    </asp:ImageField>
                    <asp:BoundField DataField="Title" DataFormatString="{0}" HeaderText="标题" />
                    <asp:TemplateField HeaderText="创建时间" Visible="False">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Created") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Created", "{0}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="SelectLabel" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="UrlLabel" runat="server" Text='<%# Bind("FullUrl") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div style="display: none">
            <asp:Button ID="AddArticlesButton" runat="server" Text="添加文章" OnClick="AddArticlesButton_Click" />
        </div>
    </form>
</body>
</html>
