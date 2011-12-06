<%@ Page Language="C#"  AutoEventWireup="true"  MasterPageFile="~/admin/theme/classic/content.Master"   Codebehind="ArticleProcess.aspx.cs" Inherits="We7.CMS.Web.Admin.ArticleProcessPage" %>
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
            <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_home.gif" />
            <asp:Label ID="NameLabel" runat="server" Text="文章审核">
            </asp:Label>
            <span class="summary">
                <asp:Label ID="SummaryLabel" runat="server" Text="">
                </asp:Label>
            </span>
        </h2>
        <div class="toolbar">
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
                    <asp:HyperLinkField DataNavigateUrlFields="ID" HeaderText="标题" DataNavigateUrlFormatString="/admin/addins/ArticleEdit.aspx?id={0}"
                        DataTextField="Title" DataTextFormatString="{0}" Target="_blank" />
               <%--     <asp:HyperLinkField DataNavigateUrlFields="OwnerID" HeaderText="所属栏目" DataNavigateUrlFormatString="/addins/Articles.aspx?oid={0}"
                        DataTextField="FullChannelPath" DataTextFormatString="{0}" />--%>
           
           <asp:TemplateField HeaderText="来源">
                <ItemTemplate>
                    <%# Eval("Source", "{0}")%>
                </ItemTemplate>
                <ItemStyle Width="100px" />
            </asp:TemplateField>
                     <asp:TemplateField>
                        <HeaderTemplate>
                            所属栏目
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# GetChannelText(Eval("ID").ToString())%>
                        </ItemTemplate>
                          <ItemStyle Width="100px" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            状态
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# GetProcessState(Eval("ID").ToString())%>
                        </ItemTemplate>
                          <ItemStyle Width="100px" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            最后提交时间
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Eval("Updated")%>
                        </ItemTemplate>
                          <ItemStyle Width="150px" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            操作
                        </HeaderTemplate>
                        <ItemTemplate>
                            <a href="ProcessSign.aspx?id=<%# Eval("ID") %>&t=<%# Eval("Title")%>" target="_blank">签署意见</a>
                        </ItemTemplate>
                          <ItemStyle Width="80px" />
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
        </div>
</asp:Content>
