<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wid_myarticle.ascx.cs" Inherits="We7.CMS.Web.Admin.tools.widget.wid_myarticle" %>
   <div class="widget movable collapsable removable  closeconfirm" id="widget-myarticle">
    <div class="widget-header">
     <strong>草稿箱</strong>
    </div>
    <div class="widget-content">
       <div class="inside">      
           <asp:GridView ID="DataGridView" runat="server" AutoGenerateColumns="False" >
           <Columns>
            <asp:TemplateField>
                    <HeaderTemplate>
                    <span style="text-decoration:underline">标题</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# GetUrl(Eval("ID").ToString())%>'
                            Target="_parent" Text='<%# Eval("Title", "{0}") %>'></asp:HyperLink>
                    </ItemTemplate>
                    </asp:TemplateField>
                     <asp:BoundField DataField="Created" DataFormatString="{0:yyyy-MM-dd}" HeaderText="创建时间" /></Columns>
           </asp:GridView>
       <p class="textright"><a href="/admin/AddIns/ArticlesMy.aspx"  >
                查看全部</a></p>
    </div>
   </div>
  </div>