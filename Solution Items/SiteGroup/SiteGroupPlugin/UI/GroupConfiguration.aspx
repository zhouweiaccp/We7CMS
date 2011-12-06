<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GroupConfiguration.aspx.cs"
    Inherits="We7.Plugin.SiteGroupPlugin.GroupConfiguration" %>
<%@ Register TagPrefix="WEC" Namespace="We7.CMS.Controls" Assembly="We7.CMS.UI" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=AppPath%>/ajax/jquery/css/ui.tabs.css" media="all" />
    <script src="<%=AppPath%>/ajax/jquery/jquery-1.2.3.pack.js" type="text/javascript"></script>
    <script src="<%=AppPath%>/ajax/jquery/ui.tabs.pack.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function() {
            $('#container-1 > ul').tabs();
        });
    </script>
      <h2 class="title">
            <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_img.gif" />
            <asp:Label ID="NameLabel" runat="server" Text="站群参数设置">
            </asp:Label>
            <span class="summary">
                <asp:Label ID="SummaryLabel" runat="server" Text="站群服务器的远程访问接口参数配置。">
                </asp:Label>
            </span>
        </h2>
        
        <WEC:MessagePanel ID="Messages" runat="server">
        </WEC:MessagePanel>
        <div id="container-1">
            <ul>
                <li><a href="#fragment-1"><span>站群服务器</span></a></li>
             </ul>
            <div id="fragment-1">
                  <table style="border: solid 0px #fff;">
                        <tr>
                        <td align="right">
                        站群功能是否启用
                        </td>
                        <td>
                             <asp:CheckBox ID="SiteGroupEnabledCheckBox" runat="server" />
                        </td>
                    </tr>

                    <tr>
                        <td align="right">
                            本站站点ID：
                        </td>
                        <td>
                            <asp:TextBox ID="SiteIDTextBox" runat="server" Columns="60"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            站群服务器访问接口：
                        </td>
                        <td>
                            <asp:TextBox ID="WDUrlTextBox" runat="server" Columns="60"></asp:TextBox>
                        </td>
                    </tr>
                      <tr>
                        <td align="right">
                            站群控制台Web地址：
                        </td>
                        <td>
                            <asp:TextBox ID="WDWebUrlTextBox" runat="server" Columns="60"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            共享圈服务器访问接口：
                        </td>
                        <td>
                            <asp:TextBox ID="IDUrlTextBox" runat="server" Columns="60"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            集中身份认证服务器访问接口：
                        </td>
                        <td>
                            <asp:TextBox ID="PassportServiceUrlTextBox" runat="server" Columns="60"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <div style="width: 500px">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                        <input  class="button" id="SaveButton" runat="server" type="submit" value="保存当前信息" onserverclick="SaveButton_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            </div>

            <div style="display: none">
            </div>
</asp:Content>

