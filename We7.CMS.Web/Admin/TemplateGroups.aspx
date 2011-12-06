<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TemplateGroups.aspx.cs"
    Inherits="We7.CMS.Web.Admin.TemplateGroups" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:content id="We7Content" contentplaceholderid="MyContentPlaceHolder" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=ThemePath%>/css/article.css" media="screen" />  
    <link media="screen" rel="stylesheet" href="<%=AppPath%>/ajax/jquery/colorbox/colorbox.css" />
<script src="<%=AppPath%>/ajax/jquery/colorbox/jquery.colorbox-min.js"></script>
<script type="text/javascript">

    var UxEvents = new Object();

    $(document).ready(function () {
        $(".editAction").colorbox({ width: "70%", height: "95%", iframe: true, onClosed: function () {

            if (UxEvents && UxEvents.reload) {
                location.reload();
            }
        }
        });
    });
</script>
    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_look.gif" />
        <asp:Label ID="NameLabel" runat="server" Text="管理系统的模板组">
        </asp:Label>
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server" Text="">
            </asp:Label>
        </span>
    </h2>
    <div class="toolbar">
        <li class="smallButton4">
            <a class="editAction" href="Template/TemplateGroupInfo.aspx" >
                                创建模板组</a></li>
        <li class="smallButton4">
            <asp:HyperLink ID="UploadHyperLink" NavigateUrl="~/admin/Plugin/PluginAdd.aspx" runat="server">
                                上传模板组</asp:HyperLink></li>
        <li class="smallButton4">
            <asp:HyperLink ID="RefreshHyperLink" NavigateUrl="TemplateGroups.aspx" runat="server">
                                刷新</asp:HyperLink></li>
    </div>
    <br />
    <br /> 
     <DIV id=conbox>             
      <DL>
        <DT>»当前模板组 
            <div id="fragment-1">
                <h3>
                    <span>
                        <asp:Label ID="UseTemplateGroupsLabel" runat="server" Text=""> </asp:Label>
                    </span>
                </h3>
                <asp:DataList ID="UseTemplateGroupsDataList" Width="15%" BorderWidth="0" CellSpacing="10"
                    CellPadding="2" runat="server" ShowFooter="False" ShowHeader="False" RepeatDirection="Horizontal"
                    ItemStyle-VerticalAlign="Top" RepeatColumns="3">
                    <ItemTemplate>
                        <table width="100">
                            <tr>
                                <td align="Center">
                                    <asp:Literal runat="server" Text='<%# CheckLength(DataBinder.Eval(((DataListItem)Container).DataItem,"Name").ToString(),15) %>'
                                        ID="LiteralName" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="center">
                                    <img style="border-color: Black; border-width: 1px" src='<%# GetImageUrl((string)DataBinder.Eval(Container.DataItem, "FileName")) %>'
                                        border="1" width="200" height="140" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:HyperLink runat="server" NavigateUrl='<%#GetTemplateGroupUrl(DataBinder.Eval(((DataListItem)Container).DataItem, "FileName").ToString(),"编辑")%>'
                                        Text="编辑" ID="HyperLinkEdit" />
                                    <asp:HyperLink runat="server" NavigateUrl='<%#GetTemplateGroupUrl(DataBinder.Eval(((DataListItem)Container).DataItem, "FileName").ToString(),DataBinder.Eval(((DataListItem)Container).DataItem, "Name").ToString(),"删除")%>'
                                        Text="删除" ID="HyperLinkDelete"   />
                                    <asp:HyperLink runat="server" NavigateUrl='<%#GetTemplateGroupUrl(DataBinder.Eval(((DataListItem)Container).DataItem, "FileName").ToString(),"打包")%>'
                                        Text="打包下载" ID="HyperLinkDown" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <ItemStyle VerticalAlign="Top" />
                </asp:DataList>
            </div>
            </DT>
            
   <DT>»可选模板组
            <div id="fragment-2">
                <asp:DataList ID="TemplateGroupsDataList" Width="15%" BorderWidth="0" CellSpacing="20"
                    CellPadding="2" runat="server" ShowFooter="False" ShowHeader="False" RepeatDirection="Horizontal"
                    ItemStyle-VerticalAlign="Top" RepeatColumns="3" >
                    <ItemTemplate>
                        <table width="100">
                            <tr>
                                <td align="Center">
                                    <asp:Literal runat="server" Text='<%# CheckLength(DataBinder.Eval(((DataListItem)Container).DataItem,"Name").ToString(),8) %>'
                                        ID="LiteralName" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" align="center">
                                    <img style="border-color: Black; border-width: 1px" src='<%# GetImageUrl((string)DataBinder.Eval(Container.DataItem, "FileName")) %>'
                                        border="1" width="200" height="140" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:HyperLink runat="server" NavigateUrl='<%#GetTemplateGroupUrl(DataBinder.Eval(((DataListItem)Container).DataItem, "FileName").ToString(),DataBinder.Eval(((DataListItem)Container).DataItem, "Name").ToString(),"应用")%>'
                                        Text="使用此模板" ID="HyperLinkUsed" />
                                    <asp:HyperLink runat="server" NavigateUrl='<%#GetTemplateGroupUrl(DataBinder.Eval(((DataListItem)Container).DataItem, "FileName").ToString(),
                                    DataBinder.Eval(((DataListItem)Container).DataItem, "Name").ToString(),"删除")%>'
                                        Text="删除" ID="HyperLinkDelete" />
                                    <asp:HyperLink runat="server" NavigateUrl='<%#GetTemplateGroupUrl(DataBinder.Eval(((DataListItem)Container).DataItem, "FileName").ToString(),"打包")%>'
                                        Text="打包下载" ID="HyperLink1" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <ItemStyle VerticalAlign="Top" />
                </asp:DataList>
            </div>
    </DT>
    <dt id="dtTemplateGroups" runat="server">»商城模板组
<%--        <span style="color:Red;">下载完成后，请解压成新的站点</span>--%>
        <div id="fragment-3">            
            <asp:Repeater ID="ShopTemplateGroupsDataList" runat="Server">
                    <HeaderTemplate>
                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <td>
                            <table width="188">         
                                <tr>
                                    <td valign="top" align="center">
                                        <%# Eval("Name")%>
                                    </td>
                                </tr>                   
                                <tr>
                                    <td valign="top" align="center">
                                        <a href='<%# Eval("PageUrl") %>' class="shopLink" title='访问<%#Eval("Name") %>的页面！' target="_blank">
                                            <img src='<%# Eval("Thumbnail") %>' />
                                        </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">                                    
                                        <b>商品价格：</b>￥<%# Eval("Price") %>.00<br />
                                        <b>文件大小：</b><%# GetProductFileSize(Eval("ProductSize").ToString())%><br />
                                        <b>模板详情：</b><a href='<%# Eval("PageUrl") %>' class='shopLink' target='_blank'>商城地址</a>
                                        <%--<%# IsFree(Eval("Price")) 
                                            ? "<b>下载地址：</b><a href='"+Eval("Url")+"' class='shopLink' target='_blank'>打包下载</a>"
                                            : "<b>模板详情：</b><a href='" + Eval("PageUrl") + "' class='shopLink' target='_blank'>商城地址</a>"
                                        %>--%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </ItemTemplate>   
                    <FooterTemplate>
                            </tr> 
                        </table>
                    </FooterTemplate>                 
                </asp:Repeater>
        </div>
    </dt>
    </DL>
    </div>
         <div style="display: none">
         <asp:Button runat="server" ID="deleteGroupButton" 
                 onclick="deleteGroupButton_Click" />
         <asp:Button runat="server" ID="applyGroupButton" onclick="applyGroupButton_Click" />
         <asp:TextBox runat="server" ID="currentGroup" />
        </div>
    <script type="text/javascript">
        function deleteGroup(name, filename) {
            if (confirm("您确认要删除模板组 " + name + " 吗？")) {
                var btn = document.getElementById("<%=deleteGroupButton.ClientID %>");
                document.getElementById("<%=currentGroup.ClientID %>").value = filename;
                if (btn) btn.click();
            }
        }

        function applyGroup(name, filename) {
            if (confirm("您确认要使用模板组 " + name + " 吗？")) {
                var btn = document.getElementById("<%=applyGroupButton.ClientID %>");
                document.getElementById("<%=currentGroup.ClientID %>").value = filename;
                if (btn) btn.click();
            }
        }
    

    </script>
</asp:content>
