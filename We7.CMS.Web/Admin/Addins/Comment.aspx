<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Comment.aspx.cs" Inherits="We7.CMS.Web.Admin.Addins.Comment" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:content id="We7Content" contentplaceholderid="MyContentPlaceHolder" runat="server">
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

        function deleteArticle() {
            var button = document.getElementById("<%=DeleteBtn.ClientID %>");
            button.click();
        }

        function startArticle() {
            var button = document.getElementById("<%=StartButton.ClientID %>");
            button.click();
        }

        function stopArticle() {
            var button = document.getElementById("<%=StopButton.ClientID %>");
            button.click();
        }
      
    </script>
    <script type="text/javascript" src="/scripts/we7/we7.loader.js">
	    $(document).ready(function(){
		    we7('.tipit').tip();
	    });
    </script>
     <h2   class="title">                  
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_comment.gif" />
        <asp:Label ID="Label1" runat="server" Text="管理评论">
        </asp:Label>
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server" Text="">
            </asp:Label>
        </span> 
    </h2>
        <div class="toolbar">
            <asp:HyperLink ID="StartHyperLink" NavigateUrl="javascript:startArticle();"
                runat="server">
                启用
            </asp:HyperLink>
            <span>   </span>
            <asp:HyperLink ID="StopHyperLink" NavigateUrl="javascript:stopArticle();"
                runat="server">
                屏蔽
            </asp:HyperLink>
            <span>  </span>
            <asp:HyperLink ID="DeleteHyperLink" NavigateUrl="javascript:deleteArticle();"
                runat="server">
                删除
            </asp:HyperLink>
            <span>   </span>
            <asp:HyperLink ID="RefreshHyperLink" NavigateUrl="/Admin/Addins/Comment.aspx" runat="server">
                刷新</asp:HyperLink>
        </div>
        <asp:Panel ID="MessagePanel" runat="Server">
            <asp:Image ID="WarningImage" runat="server" ImageUrl="/admin/images/icon_warning.gif" visible="false" />
            <asp:Label ID="MessageLabel" runat="server" Text=""></asp:Label>
        </asp:Panel>
        <asp:Panel ID="ListPanel" runat="server">
            <div style="min-height: 300px">
                <asp:GridView ID="DataGridView" runat="server" AutoGenerateColumns="False" ShowFooter="True"  CssClass="List"  GridLines="Horizontal"
                    OnRowDataBound="DataGridView_RowDataBound"   >
                    <Columns>
                        <asp:TemplateField>
                            <HeaderStyle Width="5px" />
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkHeader" runat="server" onclick="javascript:SelectAll(this);"
                                    AutoPostBack="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkItem" runat="server" />
                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="False"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="评论内容">
                            <ItemTemplate>
                                <a href="javascript:void(0);" class="tipit" title='内容：<%#Eval("Content") %><br/><%#Eval("ArticleTitle", "{0}") %><br/>状态：<%#Eval("AuditText", "{0}") %>'>
                                    <%# We7.Framework.Util.Utils.CutString(Eval("Content").ToString(),0,20) %>
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:BoundField DataField="Author" DataFormatString="{0}" HeaderText="作者" />
                        <asp:TemplateField HeaderText="所属">
                            <ItemTemplate>                                                
                               <%# Eval("ArticleTitle", "{0}") %>                                              
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="IP" DataFormatString="{0}" HeaderText="IP" >
                            <ItemStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Created" DataFormatString="{0}" HeaderText="发表日期" >
                            <ItemStyle Width="120px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="状态">
                            <ItemTemplate>
                                <%# Eval("AuditText", "{0}") %>
                            </ItemTemplate>
                                <ItemStyle Width="50px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="pagination">
                <p>
                    <WEC:Pager ID="CommentPager" PageSize="15" PageIndex="0" runat="server" OnFired="Pager_Fired" />
                </p>
            </div>
        </asp:Panel>
    </div>
    <div style="display: none">
        <asp:Button ID="DeleteBtn" runat="server" Text="Delete" OnClientClick="return confirm('您确认要将选中的评论都删除吗？')"
            OnClick="DeleteBtn_Click" />
        <asp:Button ID="StartButton" runat="server" Text="Start" OnClick="StartButton_Click" />
        <asp:Button ID="StopButton" runat="server" Text="Stop" OnClick="StopButton_Click" />
    </div>
</asp:content>
