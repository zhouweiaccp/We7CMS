<%@ Page Language="C#"  AutoEventWireup="true"  MasterPageFile="~/admin/theme/classic/content.Master"   Codebehind="AdviceProcess.aspx.cs" Inherits="We7.CMS.Web.Admin.AdviceProcess" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WEC" %>
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
    </script>

        <h2  class="title">
            <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_comment.gif" />
            <asp:Label ID="NameLabel" runat="server" Text="反馈审核">
            </asp:Label>
            <span class="summary">
                <asp:Label ID="SummaryLabel" runat="server" Text="">
                </asp:Label>
            </span>
        </h2>
        <div class="toolbar">
<%--            <asp:HyperLink ID="SubmitHyperLink" NavigateUrl="javascript:SubmitClick();"
                runat="server">审核通过
            </asp:HyperLink>
            <span> </span>
            <asp:HyperLink ID="UntreadHyperLink" NavigateUrl="javascript:UntreadClick();"
                runat="server">退回
            </asp:HyperLink>--%>
            <span> </span>
            <asp:HyperLink ID="RefreshHyperLink" NavigateUrl="ArticleProcess.aspx" runat="server">
                刷新</asp:HyperLink>
        </div>
        <WEC:MessagePanel id="Messages" runat="server" ></WEC:MessagePanel>
        <div>
            <asp:GridView ID="DataGridView" runat="server" AutoGenerateColumns="False" ShowFooter="True"  CssClass="List" GridLines="Horizontal" >
            <AlternatingRowStyle CssClass="alter" />
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
                    <asp:HyperLinkField DataNavigateUrlFields="ID" HeaderText="标题" DataNavigateUrlFormatString="/admin/Advice/AdviceDetail.aspx?adviceID={0}"
                        DataTextField="Title" DataTextFormatString="{0}"/>
                       <asp:TemplateField>
                        <HeaderTemplate>
                            所属模型
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# GetAdviceText(Eval("ID").ToString())%> 
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            状态
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
                            操作
                        </HeaderTemplate>
                        <ItemTemplate>
                            <a href="javascript:weShowModelDialog('/admin/manage/ProcessSign.aspx?adviceID=<%# Eval("ID") %>&t=<%# Eval("Title")%>');">签署意见</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <div class="pagination">
            <p>
                <WEC:Pager ID="Pager" PageSize="20" PageIndex="0" runat="server" OnFired="Pager_Fired" />
            </p>
        </div>
        <div style="display: none">
<%--            <asp:Button ID="SubmitBtn" runat="server" Text="Submit" OnClick="SubmitBtn_Click" />
            <asp:Button ID="UntreadBtn" runat="server" Text="Untread" OnClientClick="return confirm('您确认要将选中的文章都退回吗？')"
                OnClick="UntreadBtn_Click" />--%>
        </div>
</asp:Content>
