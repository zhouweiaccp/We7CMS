<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/theme/classic/content.Master"
    CodeBehind="PluginList.aspx.cs" Inherits="We7.CMS.Web.Admin.Modules.Plugin.PluginList" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<%@ Import Namespace="We7.CMS.Common" %>
<asp:Content ID="We7ContentPanel" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <link rel="Stylesheet" href="" id="scrollshow" type="text/css" />
    <link rel="stylesheet" type="text/css" href="<%=ThemePath%>/css/article.css" media="screen" />
    <script src="/Admin/Ajax/Mask.js" type="text/javascript"></script>
    <script src="/Admin/Ajax/Prototype/prototype.js" type="text/javascript"></script>
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
        function submitAction(action) {
            var list = [];
            var param = {};
            param.submitbutton = document.getElementById('<%=refresh.ClientID %>');
            param.action = action;
            param.isPluginList = true;
            param.pltype = '<%=PluginType %>'.toLocaleLowerCase();

            switch (action) {
                case "install":
                    param.message = "安装成功！";
                    param.title = "安装插件:";
                    list = buildParam("DisableSeletedCheckbox");
                    break;
                case "uninstall":
                    param.message = "卸载成功！";
                    param.title = "卸载插件:";
                    list = buildParam("EnableSeletedCheckbox");
                    break;
                case "update":
                    param.message = "更新成功！";
                    param.title = "更新插件:";
                    var urlParam = buildParam("DisableSeletedCheckbox");
                    urlParam = urlParam.length > 0 ? (urlParam + "," + buildParam("EnableSeletedCheckbox")) : buildParam("EnableSeletedCheckbox");
                    list = buildParam("DisableSeletedCheckbox");
                    break;
                case "delete":
                    param.message = "删除成功！";
                    param.title = "删除插件:";
                    list = buildParam("EnableSeletedCheckbox");
                    break;
                case "delSelected":
                    param.message = "删除成功！";
                    param.title = "删除插件:";
                    param.action = "delete";
                    list = buildParam("DisableSeletedCheckbox");
                    break;
            }
            param.plugin = list;
            new MaskWin().showMessageProgressBar(param);
            return false;
        }

        function submitSingleAction(action, type, menu) {
            var param = {};
            param.submitbutton = document.getElementById('<%=refresh.ClientID %>');
            param.action = action;
            param.plugin = type;
            param.pltype = '<%=PluginType %>'.toLocaleLowerCase();
            param.menu = menu;
            switch (action) {
                case "install":
                    param.title = "安装插件:";
                    param.message = "安装成功！";
                    break;
                case "uninstall":
                    param.title = "卸载插件:";
                    param.message = "卸载成功！";
                    break;
                case "updatelocal":
                    param.title = "更新插件:";
                    param.message = "更新成功！";
                    new MaskWin().showFrame("/Admin/Plugin/LocalPluginUpdate.aspx?type=" + type + "&pltype=<%=PluginType %>", "更新插件", { width: 281, height: 55 });
                    return;
                    break;
                case "delete":
                    param.message = "删除成功！";
                    param.title = "删除插件:";
                    break;
                case "remoteinstall":
                    param.title = "安装插件:";
                    param.message = "安装成功！";
                    break;
                case "remoteupdate":
                    param.title = "更新插件:";
                    param.message = "更新成功！";
                    break;
                case "update":
                    param.title = "更新插件:";
                    param.message = "更新成功！";
                    break;
                case "insctr":
                    param.title = "更新控件:";
                    param.message = "更新成功！";
                    break;
            }
            new MaskWin().showMessageProgressBar(param);
            return false;
        }

        function buildParam(elName) {
            var param = [];
            var list = document.getElementsByName(elName);
            for (var i = 0; i < list.length; i++) {
                if (list[i].checked)
                    param.push(list[i].value);
            }
            return param;
        }

        function DoMultiPlugin(action, elName) {
        }
    
    </script>
    <h2 class="title" runat="server" id="TitleH2">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_plugins.gif" />
        <label id="NameLabel">
            <%=Message.PluginLabel %>管理</label>
        <span class="summary">
            <label id="SummaryLabel">
                通过安装<%=Message.PluginLabel %>，可能轻易扩展系统功能</label>
        </span>
        <div class="clear">
        </div>
    </h2>
    <div id="position">
        <a href="PluginList.aspx">
            <%=Message.PluginLabel %></a>&gt;<%=Message.PluginLabel %>管理
    </div>
    <WEC:MessagePanel ID="Messages" runat="server">
    </WEC:MessagePanel>
    <div class="toolbar2">
        <li class="smallButton4"></li>
    </div>
    <br />
    <div id="conbox">
        <dl>
            <dt>»当前激活插件<br>
                <div style="padding: 0px 0 0 12px">
                    <img src="/admin/images/bulb.gif" align="absmiddle" />
                    <label class="block_info">
                        <%=Message.PluginLabel %>扩展、增强 We7 的功能。<%=Message.PluginLabel %>安装后，您可以在这里启用或者停用它。</label>
                </div>
                <dd>
                    <div>
                        <div id="plugin" class="toolbar2">
                            <li class="smallButton4">
                                <asp:LinkButton ID="UnstallLinkButton" runat="server" Text="卸载" OnClientClick="return submitAction('uninstall');" /><a
                                    href="PluginAdd.aspx?pltype=<%=PluginType %>"> 安装</a><asp:LinkButton ID="ctrDel"
                                        runat="server" Text="删除" OnClientClick="return submitAction('delete');" />
                                <asp:LinkButton ID="DownLoadLinkButton" runat="server" Text="下载" OnClick="DownLoadLinkButton_Click" />
                            </li>
                        </div>
                        <br />
                        <div style="min-height: 35px; width: 100%">
                            <asp:GridView ID="EnableGridView" runat="server" AutoGenerateColumns="false" CssClass="List"
                                OnRowDataBound="EnableGridView_RowDataBound" GridLines="Horizontal" RowStyle-VerticalAlign="Middle"
                                OnRowCommand="EnableGridView_RowCommand">
                                <AlternatingRowStyle CssClass="alter" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemStyle Width="20px" />
                                        <HeaderTemplate>
                                            <input type="checkbox" onclick="SelectAll('EnableSeletedCheckbox',this.checked)"
                                                name="EnableSeletedHeaderCheckbox" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <input type="checkbox" name="EnableSeletedCheckbox" value='<%# Eval("Directory") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Name" HeaderText="名称" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="Version" HeaderText="版本" ItemStyle-Width="60px" />
                                    <asp:TemplateField HeaderText="描述">
                                        <ItemStyle Width="500px" />
                                        <ItemTemplate>
                                            <div style="cursor: pointer; width: 100%;" onclick='new MaskWin().showDetails("PluginDetails.aspx?pltype=<%=PluginType %>&key=<%# Eval("Directory") %><%# Convert.ToBoolean(Eval("IsLocal"))?"":"&remote=1" %>","<%# Eval("Name")  %>");return false;'>
                                                <%# Eval("Description") %>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="缩略图" ItemStyle-Width="70px">
                                        <ItemTemplate>
                                            <img style="cursor: pointer;" src='<%# GetPluginImg(Eval("Directory").ToString()) %>'
                                                onclick='new MaskWin().showDetails("PluginDetails.aspx?pltype=<%=PluginType %>&key=<%# Eval("Directory") %><%# Convert.ToBoolean(Eval("IsLocal"))?"":"&remote=1" %>&tab=3","<%# Eval("Name")  %>");return false;'
                                                style="width: 50px; height: 50px;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="操作">
                                        <ItemStyle Width="100px" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" Visible='<%# PluginType==PluginType.RESOURCE %>' ID="lnk"
                                                CommandArgument='<%# Eval("Directory") %>' CommandName="downloadzip">下载</asp:LinkButton>
                                            <asp:Literal ID="MenuManage" runat="server"></asp:Literal>
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
                            <WEC:Pager ID="EnablePager" PageSize="5" runat="server" OnFired="EnablePager_Fired" />
                        </div>
                    </div>
                </dd>
            </dt>
        </dl>
    </div>
    <asp:Panel ID="bottom" runat="server">
        <div id="conbox">
            <dl>
                <dt>»不活动插件
                    <div id="Div1" style="padding: 0 0 0 12px">
                        <img src="/admin/images/bulb.gif" align="absmiddle" />
                        <label class="block_info">
                            如果某个插件和您的系统不兼容导致您的 We7 无法使用，请删除应用程序Plugins 文件夹中的错误插件，We7 会自动停用它。</label>
                    </div>
                    <dd>
                        <div>
                            <div id="plugin" class="toolbar2">
                                <li id="plugin" class="smallButton4">
                                    <asp:LinkButton ID="LinkButton6" runat="server" Text="激活" OnClientClick="return submitAction('install')" /><asp:LinkButton
                                        ID="DeleteSelectedLinkButton" runat="server" Text="删除" OnClientClick="return submitAction('delSelected')" />
                                   </li>
                            </div>
                            <br />
                            <div style="min-height: 35px; width: 100%">
                                <asp:GridView ID="DisableGridView" runat="server" AutoGenerateColumns="false" CssClass="List"
                                    OnRowDataBound="DisableGridView_RowDataBound" GridLines="Horizontal" RowStyle-VerticalAlign="Middle">
                                    <AlternatingRowStyle CssClass="alter" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemStyle Width="20px" />
                                            <HeaderTemplate>
                                                <input type="checkbox" onclick="SelectAll('DisableSeletedCheckbox',this.checked)"
                                                    name="DisableSeletedHeaderCheckbox" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <input type="checkbox" name="DisableSeletedCheckbox" value='<%# Eval("Directory") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Name" HeaderText="名称" ItemStyle-Width="100px" />
                                        <asp:BoundField DataField="Version" HeaderText="版本" ItemStyle-Width="60px" />
                                        <asp:TemplateField HeaderText="描述">
                                            <ItemStyle Width="500px" />
                                            <ItemTemplate>
                                                <div style="cursor: pointer; width: 100%;" onclick='new MaskWin().showDetails("PluginDetails.aspx?key=<%# Eval("Directory") %><%# Convert.ToBoolean(Eval("IsLocal"))?"":"&remote=1" %>&pltype=<%=PluginType %>","<%# Eval("Name")  %>");return false;'>
                                                    <%# Eval("Description") %>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="缩略图" ItemStyle-Width="70px">
                                            <ItemTemplate>
                                                <img style="cursor: pointer;" src='<%# GetPluginImg(Eval("Directory").ToString()) %>'
                                                    onclick='new MaskWin().showDetails("PluginDetails.aspx?key=<%# Eval("Directory") %><%# Convert.ToBoolean(Eval("IsLocal"))?"":"&remote=1" %>&tab=3&pltype=<%=PluginType %>","<%# Eval("Name")  %>");return false;'
                                                    style="width: 50px; height: 50px;" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="操作">
                                            <ItemStyle Width="100px" />
                                            <ItemTemplate>
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
                                <WEC:Pager ID="DisablePager" PageSize="5" runat="server" OnFired="DisablePager_Fired" />
                            </div>
                        </div>
                    </dd>
                </dt>
            </dl>
        </div>
    </asp:Panel>
    <asp:Button ID="refresh" CssClass="refreshbutton" runat="server" OnClick="OnClearMenuClick"
        Style="display: none" />
</asp:Content>
