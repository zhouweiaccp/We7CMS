<%@ Page Title="" Language="C#" MasterPageFile="~/admin/theme/classic/content.Master"
    AutoEventWireup="true" CodeBehind="AdviceTag.aspx.cs" Inherits="We7.CMS.Web.Admin.AdviceTag" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_comment.gif" />
        <asp:Label ID="Label1" runat="server" Text="管理反馈标签">
        </asp:Label>
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server" Text="">
            </asp:Label>
        </span>
    </h2>
    <div class="toolbar">
        <asp:HyperLink ID="RefreshHyperLink" NavigateUrl="/admin/advice/advicetag.aspx" runat="server">
                                刷新</asp:HyperLink><span></span>
        <asp:Label ID="lbTagEdit" runat="server">添加新标签:</asp:Label><asp:TextBox ID="tbNewTag"
            runat="server" onblur="TagExits()"></asp:TextBox><asp:Button ID="btnTagEdit" runat="server"
                Text="添加" OnClick="btnTagEditOnClick" OnClientClick="return AddAdviceTag()" /><label
                    id="lbAdvice"></label>
    </div>
    <table width="100%" height="400" border="0" cellpadding="0" cellspacing="0" class="List">
        <tr>
            <td valign="top">
                <table border="5" width="100%" align="center">
                    <tr style="background-color: #E1E1E1;">
                        <td width="59%" align="center">
                            <b>反馈类别</b>
                        </td>
                        <td width="17%" align="center">
                            <b>修改</b>
                        </td>
                        <td width="24%" align="center">
                            <b>删除</b>
                        </td>
                    </tr>
                    <asp:Repeater ID="rptAdviceTag" runat="server">
                        <ItemTemplate>
                            <tr bgcolor="e8e8e8">
                                <td width="59%" align="left">
                                    <b>
                                        <%# Container.DataItem.ToString() %></b>
                                </td>
                                <td width="17%" align="center">
                                    <b><a href="AdviceTag.aspx?tagName=<%# Container.DataItem.ToString() %>">修改</a></b>
                                </td>
                                <td width="24%" align="center">
                                    <asp:LinkButton ID="lbDel" runat="server" CommandArgument='<%# Container.DataItem.ToString() %>'
                                        CommandName='<%# Container.DataItem.ToString() %>' OnClick="DeleteTag" Text="删除"
                                        OnClientClick="javascript:if(confirm('确认要删除该标签嘛？')){return true;}else{return false;}"></asp:LinkButton>
                                    </b>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <td>
                            <webdiyer:AspNetPager ID="Pager" runat="server" OnPageChanging="Pager_PageChanging"
                                Style="color: #7f6d4d; font-size: 12px;" ShowPageIndexBox="Always" PageIndexBoxType="DropDownList"
                                Width="100%" PageIndexBoxStyle="width:19px" FirstPageText="【首页】" LastPageText="【尾页】"
                                NextPageText="【后页】" PrevPageText="【前页】" ShowCustomInfoSection="Left" TextAfterPageIndexBox="页"
                                TextBeforePageIndexBox="转到第" PageSize="25" CustomInfoHTML="第 <font color='red'><b>%CurrentPageIndex%</b></font> 页 共 %PageCount% 页 显示 %StartRecordIndex%-%EndRecordIndex% 条">
                            </webdiyer:AspNetPager>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <script type="text/javascript">
        window.name = "MyWindow";
        function TagExits() {
            var id = '<%= tbNewTag.ClientID %>';
            var url = "AdviceTag.aspx?haveTag=" + $('#' + id).val();
            $.get(url, function(data) {
                var message = "";
                if (data == "None") {
                    message = "可以使用";
                }
                else {
                    message = "已存在该标签！"
                    $('#' + id).val("");
                }
                $('#lbAdvice').html(message);
            });
        }

        function AddAdviceTag() {
            var id = '<%= tbNewTag.ClientID %>';
            if ($('#' + id).val() == "") {
                alert('标签不能为空！');
                return false;
            }
            return true;
        }
    </script>

</asp:Content>
