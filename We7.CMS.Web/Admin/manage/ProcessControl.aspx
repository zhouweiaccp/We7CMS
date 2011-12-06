<%@ Page Language="C#"  AutoEventWireup="true" MasterPageFile="~/admin/theme/classic/content.Master"
    Codebehind="ProcessControl.aspx.cs" Inherits="We7.CMS.Web.Admin.ProcessControl" %>
   <%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
 
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <script type="text/javascript">
 function SelectAll(tempControl) 
    { 
        var theBox=tempControl; 
        xState=theBox.checked;     

        elem=theBox.form.elements; 
        for(i=0;i<elem.length;i++) 
        if(elem[i].type=="checkbox" && elem[i].id!=theBox.id) 
        { 
        if(elem[i].checked!=xState) 
	        elem[i].click(); 
        } 
    }
      function DeleteClick()
   {
        var deleteBtn=document.getElementById("<%=DeleteBtn.ClientID %>");
        deleteBtn.click();
   }
     function UntreadClick()
   { 
        var untread=document.getElementById("<%=UntreadBtn.ClientID %>");
        untread.click();
   }
    </script>

        <h2 class="title">
            <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_tools.gif" />
            <asp:Label ID="NameLabel" runat="server" Text="文章审核监控">
            </asp:Label>
            <span class="summary">
                <asp:Label ID="SummaryLabel" runat="server" Text="">
                </asp:Label>
            </span>
        </h2>
        <div class="toolbar">
            <asp:HyperLink ID="DeleteHyperLink" NavigateUrl="javascript:DeleteClick();" runat="server">
            删除文章
            </asp:HyperLink>
            <span>  </span>
            <asp:HyperLink ID="UntreadHyperLink" NavigateUrl="javascript:UntreadClick();" runat="server">
            取消流转
            </asp:HyperLink>
            <span>  </span>
            <asp:HyperLink ID="RefreshHyperLink" NavigateUrl="ProcessControl.aspx" runat="server">
                刷新</asp:HyperLink>
        </div>
            <asp:GridView ID="DataGridView" runat="server" AutoGenerateColumns="False" ShowFooter="True"  CssClass="List" GridLines="Horizontal">
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
                    <asp:HyperLinkField DataNavigateUrlFields="ID" HeaderText="标题" DataNavigateUrlFormatString="ArticleView.aspx?id={0}"
                        DataTextField="Title" DataTextFormatString="{0}" Target="_blank" />
                    <%--         <asp:HyperLinkField DataNavigateUrlFields="OwnerID" HeaderText="所属栏目" DataNavigateUrlFormatString="/addins/Articles.aspx?oid={0}"
                                        DataTextField="FullChannelPath" DataTextFormatString="{0}"  />--%>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            所属栏目
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# GetChannelText(Eval("ID").ToString())%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            审批总环节
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# GetTotalAudit(Eval("OwnerID").ToString())%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            当前审批状态
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# GetProcessState(Eval("ID").ToString())%>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField>
                        <HeaderTemplate>
                            最后提交时间
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Eval("Updated")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            浏览
                        </HeaderTemplate>
                        <ItemTemplate>
                            <a href="javascript:weShowModelDialog('ScanProcessHistory.aspx?id=<%# Eval("ID") %>');">查看历史</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <WEC:MessagePanel ID="Messages" runat="server">
        </WEC:MessagePanel>
        <br />
        <div class="pagination">
            <p>
                <WEC:Pager ID="Pager" PageSize="20" PageIndex="0" runat="server" OnFired="Pager_Fired" />
            </p>
        </div>
        <div style="display: none">
            <asp:Button ID="DeleteBtn" runat="server" Text="Delete" OnClientClick="return confirm('您确认要将选中的文章都删除吗？')"
                OnClick="DeleteBtn_Click" />
            <asp:Button ID="UntreadBtn" runat="server" Text="Untread" OnClientClick="return confirm('您确认要将选中的文章都取消流转吗？')"
                OnClick="UntreadBtn_Click" />
        </div>
    </div>
</asp:Content>
