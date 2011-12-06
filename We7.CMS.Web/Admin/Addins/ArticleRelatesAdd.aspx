<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ArticleRelatesAdd.aspx.cs" Inherits="We7.CMS.Web.Admin.ArticleRelatesAdd" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WEC" %>
    
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加相关文章</title>
    <base target="_self" />
    <link rel="stylesheet" type="text/css" href="../theme/classic/css/main.css" media="screen" />
    <style>
    td{font-size:12px}
    </style>
<script src="<%=AppPath%>/cgi-bin/DialogHelper.js" type="text/javascript"></script>
    <script type="text/javascript">
        function returnUrl(url)
        {
            if(parent.document.getElementById('TargetUrl'))
            {
                 parent.document.getElementById('TargetUrl').value=parent.document.getElementById('ChannelFullUrl').value + url;  
            }
        }
        function addArticles() {
            var button = document.getElementById("<%=SubmitButton.ClientID %>");
            button.click();
            weCloseDialog(null, null);
       }
    </script>

</head>
<body style="margin: 10; padding: 0;background-color: #D4D0C8" class="blank" >
    <form id="form1" runat="server">
        <div>
            <div style="font-size:14px;">文章标签：<asp:Label ID="TagsLabel" runat="server"></asp:Label>
             
            </div>
            <div>
            <asp:TextBox ID="KeyTextBox" runat="server"></asp:TextBox>
            <asp:Button ID="QueryButton" runat="server" Text="按关键字查询" OnClick="QueryButton_Click" />
            <asp:Button ID="FilterButton" runat="server" Text="按相同标签过滤" OnClick="FilterButton_Click" />
           </div>
           <p>
            <asp:Label ID="MessageLabel" runat="server" ForeColor="Firebrick" ></asp:Label><br />
            </p>
            <asp:GridView ID="DetailGridView" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                GridLines="None"  Width="300px" CellPadding="3"
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
        <div class="pagination" runat="server" id="PagerDiv">
            <p>
                <WEC:Pager ID="ArticlePager" PageSize="25" PageIndex="0" runat="server" OnFired="Pager_Fired" />
            </p>
        </div>
        <p>
        <asp:HyperLink ID="QuoteHyperLink" NavigateUrl="javascript:addArticles();" runat="server">
            添加选中文章为相关文章</asp:HyperLink>
        </p>
        <div style="display: none">
        <asp:Button ID="SubmitButton"   runat="server"   Text="Start" OnClick="AddArticlesButton_Click" />
</div>
    </form>
</body>
</html>
