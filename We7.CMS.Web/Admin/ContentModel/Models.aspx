<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/theme/classic/content.Master"
    AutoEventWireup="true" CodeBehind="Models.aspx.cs" Inherits="We7.CMS.Web.Admin.ContentModel.Model" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <style type="text/css">
        .popu
        {
            list-style-type: none;
            z-index: 500;
            position: absolute;
            width: 100px;
            background-color: White;
            border: solid 1px;
            display: none;
        }
        .popu a:hover
        {
            background-color: #3399FF;
            color: #FFFFFF;
        }
        .popu li
        {
            display: block;
            padding-left: 2px;
            padding-top: 2px;
        }
        a.headlink
        {
            width: 70px;
            text-align: left;
            display: block;
            cursor: pointer;
        }
        a.headlink:hover
        {
            background: none repeat scroll 0 0 #6CC8EF;
            cursor: pointer;
            color: Green;
        }
    </style>
    <script type="text/javascript" src="/scripts/we7/we7.loader.js"> </script>
    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icon_settings.png" />
        <%= ModelTypeName%>管理
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server" Text="除了默认的信息类型外，您可以通过自定义内容模型来扩展其他类型，增加如产品、展会、下载等。">
            </asp:Label>
        </span>
    </h2>
    <WEC:MessagePanel ID="Msg" runat="server" Width="900px">
    </WEC:MessagePanel>
    <div class="toolbar">
        <a href="/admin/ContentModel/editmodel.aspx?action=add&type=<%=modelType %>">新建模型</a>
        <asp:LinkButton ID="bttnReCreateIndex" runat="server" OnClick="bttnReCreateIndex_Click"
            Text="重建索引"></asp:LinkButton>
        <% if (modelType.ToString().ToLower() == "advice")
           {%>
        <a href="/admin/advice/advicetypes.aspx">反馈类型</a>
        <% } %>
    </div>
    <div class="list">
        <table class="List" cellpadding="0" cellspacing="0">
            <thead>
                <tr>
                    <td>
                        序号
                    </td>
                    <td>
                        模型名称
                    </td>
                    <td>
                        模型组
                    </td>
                    <td>
                        模型类型
                    </td>
                    <td>
                        描述
                    </td>
                    <td>
                        状态
                    </td>
                    <td align="center">
                        操作
                    </td>
                </tr>
            </thead>
            <%
                We7.Model.Core.ContentModelCollection collection = GetModelCollection();
                if (collection == null || collection.Count <= 0)
                {%>
            <tr>
                <td colspan="7">
                    暂无数据!
                </td>
            </tr>
            <%}
                else
                {
                    for (int i = 0; i < collection.Count; i++)
                    {
            %>
            <tr>
                <td>
                    <%=i+1%>
                </td>
                <td>
                    <%=collection[i].Label%>
                </td>
                <td>
                    <%=CovertModelGroupName(collection[i].Name)%>
                </td>
                <td>
                    <%=ConvertModelType(collection[i].Type)%>
                </td>
                <td>
                    <%=collection[i].Description%>
                </td>
                <td>
                    <%
                        if (collection[i].State == null || collection[i].State == 0)
                        {%>
                    关闭<%
                        }
                        else
                        {
                    %>开启<%
                        }
                    %>
                </td>
                <td align="left">
                    <a class="headlink">管理</a>
                    <ul class="popu">
                        <li><a href="editmodel.aspx?action=edit&type=<%=modelType %>&modelname=<%=collection[i].Name %>">
                            修改</a> </li>
                        <li><a href="javascript:DeleteContentModel('<%=collection[i].Name %>','<%=collection[i].Label %>')">
                            删除</a> </li>
                        <li><a href="field.aspx?modelType=<%=modelType %>&modelname=<%=collection[i].Name %>">字段管理</a></li>
                        <li><a href="EditLayout.aspx?modelname=<%=collection[i].Name %>" >布局管理</a>
                        </li>
                        <% if (collection[i].Type == We7.Model.Core.ModelType.ARTICLE)
                           {%>
                        <li><a href="javascript:CreateModelTable('<%=collection[i].Name %>')">生成数据表</a>
                        </li>
                        <li><a href="/admin/manage/AddMenu.aspx?modelname=<%=collection[i].Name %>" target="_self">
                            添加到左边菜单</a> </li>
                        <%} %>
                        <!-- < % if (collection[i].Type != We7.Model.Core.ModelType.ACCOUNT)
                           {% >
                        <li><a href="javascript:CreateModelControl('< %=collection[i].Name % >','< %=collection[i].Label % >')">
                            生成部件</a> </li>
                        < %} % >
                        < % if (collection[i].Type == We7.Model.Core.ModelType.ARTICLE)
                           {% >
                        
                        < %} % >
                        <li><a href="AdvanceEdit.aspx?modelname=< %=collection[i].Name % >" target="_self">高级编辑</a>
                        </li>
                        < % if (collection[i].Type == We7.Model.Core.ModelType.ARTICLE)
                           {% >
                        <li><a href="/admin/manage/AddMenu.aspx?modelname=< %=collection[i].Name % >" target="_self">
                            添加到左边菜单</a> </li>
                        < %} % > -->
                    </ul>
                </td>
            </tr>
            <%
                    }
                }
            %>
        </table>
    </div>
    <input type="hidden" id="hdDemoSite" value='<%= We7.Framework.Config.GeneralConfigs.GetConfig().IsDemoSite.ToString().ToLower() %>' />
    <script src="/Admin/ContentModel/js/Common.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //  $("td.edit").click(SingleUpdate);
            $(".headlink").each(function () {
                $(this).hover(function () {
                    var left = $(this).offset().left + 0.5 * $(this).width() - $(this).next("ul").width() * 0.5;
                    $(".popu").hide();
                    $(this).next("ul").show().css("left", left);
                }, function () {

                });

            });
            $(document).click(function () {
                $(".popu").hide();
            });

            $("ul.popu a").click(function () {
                var isDemoSite = document.getElementById("hdDemoSite").value;
                if (isDemoSite === "true") {
                    we7.info("对不起，此演示站点您没有该操作权限！");
                    return false;
                }
                return true;
            });
        });
        function DeleteContentModel(model, label) {
            var rs = confirm("你确定要删除模型:[" + label + "]吗?");
            if (rs) {
                var url = "/Admin/ContentModel/ajax/ContentModel.ashx?action=DeleteModel&model=" + model;
                we7.loading("操作中，请稍后...", { autoHide: false });
                $.ajax({
                    url: url,
                    type: "POST",
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (result) {
                        if (result.success ) {
                            we7.info("删除成功");
                            if (!we7.queryString["reload"]) {
                                if (location.href.indexOf("?") > 0)
                                    location.href = location.href + '&reload=menu';
                                else
                                    location.href = location.href + '?reload=menu';
                            }
                        }
                        else {
                            we7.info("删除失败！错误消息：" + result.msg, { autoHide: true, hideTimeout: 6000 });
                        }
                    }
                });
            }
        }

        function CreateModelTable(model) {
            var url = "/Admin/ContentModel/ajax/ContentModel.asmx/CreateModelTable";
            we7.loading("操作中，请稍后...", { autoHide: false });
            var data = { model: model };
            $.ajax({
                url: url,
                data: jsonToString(data),
                type: "POST",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (json) {
                    var result = stringToJSON(json);
                    if (result.success) {
                        we7.info("生成成功");
                    }
                    else {
                        we7.info("生成失败,错误消息："+result.msg);
                    }
                    //document.location.reload();
                },
                failure: function () {
                    alert("应用程序错误!");
                }
            });
        }

     

    </script>
</asp:Content>
