<%@ Page Language="C#" MasterPageFile="~/admin/theme/classic/content.Master" AutoEventWireup="true" CodeBehind="ContentTypeList.aspx.cs" Inherits="We7.CMS.Web.Admin.manage.ContentTypeList"   %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WEC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
<script type="text/javascript">
function doDelete(url)
{
    var ret=confirm("内容模型有可能正在使用，删除后可能引起系统的错误，你确认删除吗？");
    if(ret) window.location=url;
}
</script>
    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icon_settings.png" />
        <asp:Label ID="NameLabel" runat="server" Text="内容模型管理">
        </asp:Label>
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server" Text="除了默认的文章类型外，您可以通过自定义内容模型来扩展其他类型，增加如产品、展会、下载等。" >
            </asp:Label>
        </span>
    </h2>
   <div class="toolbar">
        <asp:HyperLink ID="NewHyperLink" NavigateUrl="ContentTypeDetail.aspx" runat="server">
            新建模型</asp:HyperLink>
        <span> </span>
        <asp:HyperLink ID="RefreshHyperLink" NavigateUrl="ContentTypeList.aspx" runat="server">
            刷新</asp:HyperLink>
    </div>
       <WEC:MessagePanel runat="Server" ID="Messages">
    </WEC:MessagePanel>
    <div>
        <asp:GridView ID="FileGridView" runat="server" AutoGenerateColumns="False" CssClass="List"  GridLines="Horizontal"
            ShowFooter="True">
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="Name" DataNavigateUrlFormatString="ContentTypeDetail.aspx?file={0}"
                    DataTextField="Name" DataTextFormatString="{0}" HeaderText="名称" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <a href="javascript:doDelete('ContentTypeList.aspx?del=<%# Eval("Name") %>')" >删除</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
