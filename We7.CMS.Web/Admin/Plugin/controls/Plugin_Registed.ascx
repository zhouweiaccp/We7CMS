<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Plugin_Registed.ascx.cs"
    Inherits="We7.CMS.Web.Admin.Plugin.controls.Plugin_Registed" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<link rel="Stylesheet" href="" id="scrollshow" type="text/css" />

<script src="/Admin/Ajax/Mask.js" type="text/javascript"></script>

<script src="/Admin/Ajax/Prototype/prototype.js" type="text/javascript"></script>

<meta http-equiv="Content-Type" content="html/text; charset=utf-8" />

<script type="text/javascript" src="/Admin/Ajax/Ext2.0/adapter/ext/ext-base.js"></script>

<script type="text/javascript" src="/Admin/Ajax/Ext2.0/ext-all.js"></script>

<script type="text/javascript" src="/Install/js/Plugin.js"></script>

<script type="text/javascript">
    var mask = new MaskWin();

    function SelectAll(checkboxname, checked) {
        var els = document.getElementsByName(checkboxname);
        if (els && els.length > 0) {
            for (var i = 0; i < els.length; i++) {
                els[i].checked = checked;
            }
        }
    }
    var refreshbutton = '<%=refresh.ClientID %>';
        
    function submitSingleAction(action, type,menu) {
        var param = {};
        param.action = action;
        param.plugin = type;
        param.menu = menu;
        param.submitbutton = document.getElementById('<%=refresh.ClientID %>');
        switch (action) {
            case "remoteinstall":
                param.title = "安装插件";
                param.message = "安装成功！";
                break;
            case "remoteupdate":
                param.message = "更新成功！";
                param.title = "更新插件";
                break;
            case "insctr":
                param.message = "安装成功！";
                param.title = "安装控件";
                break;
        }
        new MaskWin().showMessageProgressBar(param);
        return false;
    }

    function buildParam(elName) {
        var param = "";
        var list = document.getElementsByName(elName);
        for (var i = 0; i < list.length; i++) {
            if (list[i].checked)
                param += list[i].value + ",";
        }
        if (param.length > 0)
            param = param.substr(0, param.length - 1);
        return param;
    }
    
</script>

<div id="conbox" style="width: 100%">
    <dl>
        <dt>»<asp:Literal ID="TitleLiteral" runat="server">注册到本站的插件</asp:Literal><br>
            <dd>
                <div id="plugin" class="toolbar2">
                </div>
                <br />
                <div style="min-height: 35px; width: 100%; min-width:600px">
                    <WEC:MessagePanel ID="Messages" runat="server" Width="100%">
                    </WEC:MessagePanel>
                    <asp:Panel ID="plList" runat="server" Width="100%">
                        <div style="min-height: 35px; width: 100%">
                            <asp:GridView ID="PluginListGridView" runat="server" AutoGenerateColumns="false"
                                CssClass="List" GridLines="Horizontal" RowStyle-VerticalAlign="Top">
                                <AlternatingRowStyle CssClass="alter" />
                                <Columns>
                                    <asp:TemplateField HeaderText="缩略图" ItemStyle-Width="70px" ItemStyle-VerticalAlign="Middle">
                                        <ItemTemplate>
                                            <div style="padding: 2px; border: solid 1px #e5e5e5;">
                                                <a href="<%# Eval("PageUrl") %>" target="_blank">
                                                    <img src='<%# Eval("Thumbnail") %>' onerror="this.src='/admin/images/article_large.gif';this.onerror=null;"
                                                        style="width: 100px; height: 100px; cursor: hand" /></a>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="名称" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center"
                                        ItemStyle-Width="250px">
                                        <ItemTemplate>
                                            <a href="<%# Eval("PageUrl") %>" target="_blank"><b>
                                                <%# Eval("Name") %></b></a><br />
                                            价格：<span style="color: #D30000; font-weight: 600;"><%# ((decimal)Eval("Price")).ToString("C2") %></span>（已售<span
                                                style="font-weight: 600; color: #F4CB00"><%# Eval("Sales") %></span>件)<br />
                                            更新时间：<%# ((DateTime)Eval("Updated")).ToString("yyyy-MM-dd HH:mm")%><br />
                                            版本：<%# Eval("Version") %><br />
                                            评分等级：<%# GetStar(Eval("Level"))%>
                                            <br />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="描述">
                                        <ItemStyle Width="500px" />
                                        <ItemTemplate>
                                            <div style="cursor: pointer; width: 100%;" onclick=''>
                                                <a href="<%# Eval("PageUrl") %>" target="_blank">
                                                    <%# Eval("Description") %></a>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="操作" ItemStyle-VerticalAlign="Middle">
                                        <ItemStyle Width="100px" />
                                        <ItemTemplate>
                                            <%# GetUpdateOrInstallString( Eval("ProductCategory"),Eval("Url")) %>                                           
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="pagination" style="text-align: right">
                            <ul class="subsubsub">
                                <asp:Literal ID="PageLiteral" runat="server"></asp:Literal>
                            </ul>
                            <WEC:URLPager ID="Pager" runat="server" UseSpacer="False" UseFirstLast="true" PageSize="10"
                                FirstText="<< 首页" LastText="尾页 >>" LinkFormatActive='<span class=Current>{1}</span>'
                                CssClass="Pager" />
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="plSN" runat="server" Width="100%">
                        <table>
                            <tr>
                                <td width="50">
                                    会员名：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLoginName" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    密 码：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPassword" TextMode="Password" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="bttnSave" CssClass="Btn" runat="server" Text="保存" OnClick="bttnSave_Click" />
                                    &nbsp;&nbsp; 如果您还没有插件商店帐号,请<a href="<%=RegisteUrl %>">点击注册</a>。
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </dd>
        </dt>
    </dl>
    <asp:Button ID="refresh" runat="server" OnClick="OnClearMenuClick" Style="display: none" />
</div>
