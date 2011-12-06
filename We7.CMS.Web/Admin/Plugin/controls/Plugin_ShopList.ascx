<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Plugin_ShopList.ascx.cs"
    Inherits="We7.CMS.Web.Admin.Plugin.controls.Plugin_ShopList" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<link rel="Stylesheet" href="" id="scrollshow" type="text/css" />

<meta http-equiv="Content-Type" content="html/text; charset=utf-8" />
<WEC:MessagePanel ID="Messages" runat="server">
</WEC:MessagePanel>
<div id="conbox">
    <dl>
        <dt>»<%=GetCaption() %><br>
            <dd>
                <div>
                    <div id="plugin" class="toolbar2">
                    </div>
                    <br />
                    <div style="min-height: 35px; width: 100%">
                        <asp:GridView ID="PluginListGridView" runat="server" AutoGenerateColumns="false"
                            CssClass="List" GridLines="Horizontal" RowStyle-VerticalAlign="Top">
                            <AlternatingRowStyle CssClass="alter" />
                            <Columns>
                                <asp:TemplateField HeaderText="缩略图" ItemStyle-Width="70px" ItemStyle-VerticalAlign="Middle">
                                    <ItemTemplate>
                                        <div style="padding:2px; border:solid 1px #e5e5e5;">
                                        <a href="<%# Eval("PageUrl") %>" target="_blank">
                                            <img src='<%# Eval("Thumbnail") %>' onerror="this.src='/admin/images/article_large.gif';this.onerror=null;" style="width: 100px; height: 100px; cursor: hand" /></a>
                                            </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="名称" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-Width="250px">
                                    <ItemTemplate>
                                        <a href="<%# Eval("PageUrl") %>" target="_blank"><b>
                                            <%# Eval("Name") %></b></a><br />
                                        价格：<span style="color:#D30000; font-weight:600;"><%# ((decimal)Eval("Price")).ToString("C2") %></span>（已售<span style="font-weight: 600;
                                            color: #F4CB00"><%# Eval("Sales") %></span>件)<br />
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
                                        <a target="_blank" href="<%# GetBuyLink(Eval("ID")) %>">购买插件</a>
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
                        >><a href="<%=ShopSite %>" target="_blank">更多插件</a>&nbsp;&nbsp;&nbsp;&nbsp;
                        <%--<WEC:Pager ID="Pager" PageSize="10" runat="server" OnFired="Pager_Fired" />--%>
                    </div>
                </div>
            </dd>
        </dt>
    </dl>
</div>
