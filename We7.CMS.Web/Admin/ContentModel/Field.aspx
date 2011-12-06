<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/theme/classic/content.Master"
    AutoEventWireup="true" CodeBehind="Field.aspx.cs" Inherits="We7.CMS.Web.Admin.ContentModel.Field" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <script type="text/javascript">
        function preview() {
            window.open('/user/ModelHandler.aspx?model=<%=ModelName %>');
        }
    </script>
    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icon_settings.png" />
        <asp:Label ID="NameLabel" runat="server" Text="模型字段管理">
        </asp:Label>
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server" Text="可以自定义字段">
            </asp:Label>
        </span>
    </h2>
    <div class="toolbar">
        <a href="/admin/ContentModel/EditField.aspx?modelType=<%=modelType %>&action=add&modelname=<%=ModelName %>">新建字段</a>
        <span></span><a href="/admin/ContentModel/models.aspx?modelType=<%=modelType %>">返回模型列表</a> 
    </div>
    <%
        We7.Model.Core.We7DataTableCollection tables = GetDataTabels();
        if (tables != null && tables.Count > 0)
        {
            for (int i = 0; i < tables.Count; i++)
            {%>
    <h1>
        <%=modelInfo.Label%></h1>
    <table class="List" cellpadding="0" cellspacing="0">
        <% %>
        <thead>
            <tr>
                <td>
                    名称
                </td>
                <td>
                    数据类型
                </td>
                <td>
                    标题字段
                </td>
                <td>
                    字段长度
                </td>
                <td>
                    操作
                </td>
            </tr>
        </thead>
        <%
for (int j = 0; j < tables[i].Columns.Count; j++)
                {%>
        <tr>
            <%if (!string.IsNullOrEmpty(tables[i].Columns[j].Label))
              { %>
            <td>
                <%=tables[i].Columns[j].Label%>
            </td>
            <%}
              else
              { %>
            <td>
                <%=tables[i].Columns[j].Name%>
            </td>
            <%} %>
            <td>
                <%=ConvertDatTypeToString(tables[i].Columns[j].DataType)%>
            </td>
            <td>
                <%=(tables[i].Columns[j].Mapping == "Title" ? "是" : "-") %>
            </td>
            <td>
                <%=tables[i].Columns[j].MaxLength%>
            </td>
            <td>
                <a href="editfield.aspx?modelType=<%=modelType %>&action=edit&modelname=<%=ModelName %>&fieldname=<%=tables[i].Columns[j].Name %>">
                    编辑</a> <a href="javascript:DeleteFiled('<%=ModelName%>','<%=tables[i].Columns[j].Name%>','<%=tables[i].Columns[j].Label%>')">
                        删除</a>
            </td>
        </tr>
        <%
              } %>
    </table>
    <% }
       }
    %>
    <script src="/Admin/ContentModel/js/Common.js" type="text/javascript"></script>
    <script type="text/javascript">
        function DeleteFiled(model, field, label) {
            var rs = confirm("你确定要删除字段:[" + label + "]吗?\n警告:删除字段会删除其相关联控件,请注意!");

            if (rs) {
                var url = "/Admin/ContentModel/ajax/ContentModel.asmx/DeleteFiled";
                var data = { model: model, field: field };
                $.ajax({
                    url: url,
                    data: jsonToString(data),
                    type: "POST",
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (json) {

                        var result = stringToJSON(json);

                        if (result.success) {
                            alert("删除成功！");
                            document.location.reload();
                        }
                        else {
                            alert("删除失败！");
                        }
                    }
                });
            }
        }
    </script>
</asp:Content>
