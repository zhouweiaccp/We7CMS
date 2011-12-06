<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wid_myprocess.ascx.cs" Inherits="We7.CMS.Web.Admin.tools.widget.wid_myprocess" %>
   <div class="widget movable collapsable removable  closeconfirm" id="widget-myprocess">
    <div class="widget-header">
     <strong>待审核稿件</strong>
    </div>
    <div class="widget-content">
       <div class="inside">
   <asp:GridView ID="DataGridView" runat="server" AutoGenerateColumns="False" >
           <Columns>
   <asp:HyperLinkField DataNavigateUrlFields="ID" HeaderText="标题" DataNavigateUrlFormatString="/admin/manage/ArticleView.aspx?id={0}"
                        DataTextField="Title" DataTextFormatString="{0}" Target="_blank" />
                      <asp:TemplateField>
                        <HeaderTemplate>
                            操作
                        </HeaderTemplate>
                        <ItemTemplate>
                            <a href="/admin/manage/ProcessSign.aspx?id=<%# Eval("ID") %>&t=<%# Eval("Title")%>" target="_blank">签署意见</a>
                        </ItemTemplate>
                    </asp:TemplateField></Columns>
           </asp:GridView>
       <p class="textright"><a href="/admin/manage/ArticleProcess.aspx"  >
                查看全部</a></p>
       
    </div>
   </div>
  </div>