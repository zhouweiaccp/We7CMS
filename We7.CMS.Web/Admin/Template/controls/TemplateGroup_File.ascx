    <%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TemplateGroup_File.ascx.cs"
    Inherits="We7.CMS.Web.Admin.TemplateGroup_File" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>

<script type="text/javascript" src="/Admin/Ajax/jquery/jquery.dataTables.min.js"></script>

<script type="text/javascript">

    function doDeleteFile(fa) {
        
        if (confirm("您确认要删除模板文件 " + fa + " 吗？")) {
        
            document.getElementById("<%=FileTextBox.ClientID %>").value = fa;
            document.getElementById("<%=DeleteFileButton.ClientID %>").click();
        }
        return false;
    }

    function RefreshRpt() {
        document.getElementById("<%=btnRefresh.ClientID %>").click();
        return false;
    }
</script>

<script type="text/javascript" charset="utf-8">
    $(document).ready(function () {
        var flag = ($.browser.msie && $.browser.version >= 7) || (!$.browser.msie);

        if (flag) {

            $('#templatesTable').dataTable({
                "bPaginate": false,
                "bLengthChange": false,
                "bInfo": false,
                "oLanguage": {
                    "sLengthMenu": "每页显示 _MENU_ 条",
                    "sZeroRecords": "抱歉，没有记录。",
                    "sInfo": "共有 _TOTAL_ 个，本页为第 _START_ 个到第 _END_ 个",
                    "sInfoEmpty": "没有模板。",
                    "sInfoFiltered": "(从 _MAX_ 个模板中过滤)",
                    "sSearch": "快速查询"
                }
            });

        }
        $(".bindAction").colorbox({ width: "80%", height: "80%", iframe: true, onClosed: function () { RefreshRpt(); } });

    });
</script>

<div style="display: table; width: 100%; z-index: 1000">
    <ul class="subsubsub">
        <asp:Literal ID="StateLiteral" runat="server"></asp:Literal>
    </ul>
</div>
<WEC:MessagePanel ID="Messages" runat="server">
</WEC:MessagePanel>
<div style="z-index: 0">
    <asp:Repeater ID="TempldatesRepeater" runat="server">
        <HeaderTemplate>
            <table class="List display" border="0" cellpadding="3" cellspacing="0" id="templatesTable">
                <thead>
                    <tr>
                        <th style="width: 100px">
                            模板名称
                        </th>
                        <th>
                            模板文件
                        </th>
                        <th>
                            模板类型
                        </th>
                        <th style="width: 90px">
                            子模板/母版
                        </th>
                        <th style="width: 150px">
                            默认指定
                        </th>
                        <th style="width: 150px">
                            创建日期
                        </th>
                        <th style="width: 170px">
                            操作
                        </th>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%# GetVisualEditUrl(Eval("IsVisualTemplate"), Eval("FileName").ToString(),Eval("Name")??"".ToString())%>
                </td>
                <td>
                    <%# Eval("FileName", "{0}") %>
                </td>
                <td>
                    <%# (bool)Eval("IsVisualTemplate")?"可视化模板":"普通模板"%>
                </td>
                <td>
                    <%# Eval("IsSubTemplateText", "{0}")%>
                </td>
                <td style="color: Red">
                    <%# Eval("DefaultBindText", "{0}")%>
                </td>
                <td>
                    <%# Eval("Created", "{0}")%>
                </td>
                <td>
                    <%# GetVisualEditUrl(Eval("IsVisualTemplate"), Eval("FileName").ToString(),"编辑")%>
                    | <a href="javascript:void(0)" onclick="javascript:return doDeleteFile('<%#DataBinder.Eval(Container.DataItem, "FileName") %>');">
                        删除 </a>| <a class='bindAction' href="TemplateBindTo.aspx?filename=<%# Eval("FileName", "{0}") %>">
                            默认指定</a>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody> </table>
        </FooterTemplate>
    </asp:Repeater>
</div>
<div style="display: none">
    <asp:TextBox ID="FileTextBox" runat="server"></asp:TextBox>
    <asp:Button ID="DeleteFileButton" runat="server" OnClick="DeleteFileButton_Click" />
     <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" />
</div>
