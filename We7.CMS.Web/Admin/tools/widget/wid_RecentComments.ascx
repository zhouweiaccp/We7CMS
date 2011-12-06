<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wid_RecentComments.ascx.cs" Inherits="We7.CMS.Web.Admin.tools.widget.wid_RecentComments" %>
   <div class="widget movable collapsable removable  closeconfirm" id="widget-RecentComments">
    <div class="widget-header">
     <strong>最新评论</strong>
    </div>
    <div class="widget-content">
       <div class="inside">
       <asp:GridView ID="DataGridView" runat="server" AutoGenerateColumns="False" >
           <Columns>
              <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="/Addins/CommentDetail.aspx?id={0}" DataTextField="Content"
                                            DataTextFormatString="{0}" HeaderText="评论内容" Target="_blank" />
                     <asp:BoundField DataField="Created" DataFormatString="{0:yyyy-MM-dd}" HeaderText="创建时间" /></Columns>
           </asp:GridView>
       <p class="textright"><a href="/admin/AddIns/Comment.aspx"  >
                查看全部</a></p>
    </div>
   </div>
  </div>