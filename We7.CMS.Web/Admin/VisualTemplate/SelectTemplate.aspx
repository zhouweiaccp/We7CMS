<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/theme/classic/content.Master"
    AutoEventWireup="true" CodeBehind="SelectTemplate.aspx.cs" Inherits="We7.CMS.Web.Admin.VisualTemplate.SelectTemplate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <script src="/Scripts/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <style type="text/css">
        div.sfTemlpateWrapper
        {
            width: 100%;
            height: auto;
            min-height: 200px;
        }
        div.sfFaqForm
        {
            float: left;
            width: 100%;
            min-height: 200px;
        }
        div.sfFaq
        {
            float: left;
            width: 22%;
            border-left: solid 1px Gray;
            min-height: 400px;
            margin-left: 5px;
            padding-left: 20px;
        }
        ul.sfTemplatesList li
        {
            cursor: pointer;
            display: inline-block;
            float: left;
            height: 170px;
            margin: 0 10px 10px 10;
            padding: 1px;
            vertical-align: top;
            width: 138px;
        }
        div.sfSTWrapper
        {
            padding: 14px 16px 0 17px;
        }
        div.sfSTWrapper em
        {
            cursor: default;
            display: block;
            font-size: 11px;
            line-height: 1.2;
            padding: 2px 0 7px;
            color: #333333;
            font-style: normal;
            text-align: center;
            margin-top: 5px;
        }
        li.SelectTemplate
        {
            background-color: Silver;
            border: solid 1px Silver;
        }
        li.hoverTemplate
        {
            background-color: Silver;
            border: solid 1px Silver;
        }
    </style>
    <center>
        <table cellpadding="0" cellspacing="0" border="0" style="width: 600px; height: 100%;
            margin-top: 80px; border: solid 10px #f0f0f0">
            <tr>
                <td valign="middle">
                    <h2 class="title">
                        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_look.gif" />
                        <asp:Label ID="TitleLabel" runat="server" Text="创建新模板">
                        </asp:Label>
                        <span class="summary">
                            <asp:Label ID="SummaryLabel" runat="server" Text="分三步创建可视化模板">
                            </asp:Label></span>
                        <ul style="font-size: 16px; margin-top: 20px; padding-left: 30px;">
                            ①模板基本信息 <span style="color: Red">②选择模板布局</span> ③开始在线设计模板
                        </ul>
                    </h2>
                    <div class="sfClearfix sfTemlpateWrapper">
                        <div class="sfFaqForm">
                            <div id="templatesWrapper">
                                <h2>
                                </h2>
                                <ul class="sfTemplatesList">
                                    <%
                                        We7.CMS.Module.VisualTemplate.Models.TemplateList templateList = GetTemplateList();
                                        foreach (We7.CMS.Module.VisualTemplate.Models.Template item in templateList.Templates)
                                        {
                                    %>
                                    <li class="sfSimpleTemplate" data="<%=item.FullFilePath %>">
                                        <div class="sfSTWrapper">
                                            <img src="<%=item.FullIconPath %>" alt="<%=item.Name %>" />
                                            <em><span>
                                                <%=item.Name %></span> </em>
                                        </div>
                                    </li>
                                    <%
}
            
                                    %>
                                </ul>
                            </div>
                        </div>
                    </div>
                    <asp:HiddenField ID="layoutName" runat="server" />
                    <div class="toolbar" style="text-align: center">
                        <asp:Button ID="btnSave" runat="server" CssClass="button" Text="确定" OnClick="Save" OnClientClick="CheckLayout()" />
                    </div>
                </td>
            </tr>
        </table>
    </center>
    <script type="text/javascript">
        $(document).ready(function () {
            //选择模板
            $("li.sfSimpleTemplate").click(function () {
                $(".sfTemplatesList .sfSimpleTemplate").removeClass("SelectTemplate");
                $(this).addClass("SelectTemplate");
                $("#<%=layoutName.ClientID%>").attr("value", $(this).attr("data"));
            }).hover(function () {
                $(this).addClass("hoverTemplate");
            },
              function () {
                  $(this).removeClass("hoverTemplate");
              });
          });

          function CheckLayout() {
              var value = $("#<%=layoutName.ClientID %>").val();
              if (value == "") {
                  $("form").submit(function () {
                      return false;
                  });
                  alert("请选择一个模板布局！");
              }
          }
    </script>
</asp:Content>
