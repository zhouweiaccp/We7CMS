<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wid_Product.ascx.cs"
    Inherits="We7.CMS.Web.Admin.tools.widget.wid_Product" %>
<div class="widget movable collapsable removable  closeconfirm" id="widget-product">
    <div class="widget-header">
        <strong>推荐商品</strong>
    </div>
    <div class="widget-content">
        <div class="inside">
            <div class="wrap_div" id="insideProduct">
            </div>
          <%--  <asp:Repeater ID="rpProducts" runat="server">
                <HeaderTemplate>
                    <div class="wrap_div">
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="sub_div">
                        <dl>
                            <dt><a href='<%# Eval("PageUrl") %>' class="shopLink" target="_blank">
                                <img src='<%#Eval("Thumbnail") %>' tile='<%#Eval("Name") %>' width="100" height="100" />
                            </a></dt>
                            <dd>
                                <ul class="line20">
                                    <li><b>名称:</b> <a href='<%#Eval("PageUrl") %>' class="shopLink" target="_blank">
                                        <%# GetChopString(Eval("Name").ToString(), 8, "...")%></a> </li>
                                    <li>
                                        <b>价格：</b><%# Eval("Price") %>
                                    </li>
                                    <li>
                                        <b>销量：</b><%# Eval("Sales") %>
                                    </li>
                                    <li>
                                        <b>人气：</b><%# Eval("Point") %>
                                    </li>
                                    <li><b>评分：</b>
                                        <%# GetLevelString(Eval("Level").ToString())%>
                                    </li>
                                </ul>
                            </dd>
                        </dl>
                    </div>
                </ItemTemplate>
                <FooterTemplate>
                    </div>
                </FooterTemplate>
            </asp:Repeater>--%>

        </div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $.ajaxSetup({
            error: function (x, e) {
                $("#insideProduct").html("暂时无法连接商城！");
            }
        });

        $("#insideProduct").html("<img src='/scripts/we7/css/res/blue-loading.gif' /> 连接商城中...");

        $.getJSON("/admin/theme/classic/main.aspx?action=product",
             function (json) {
                 var html = '<div class="wrap_div">';
                 for (var i = 0; i < json.length; i++) {
                     html += '<div class="sub_div">';
                     html += '<dl>';
                     html += '<dt>';
                     html += '<a href=' + json[i].PageUrl + ' class="shopLink" target="_blank">';
                     html += '<img src=' + json[i].Thumbnail + ' tile=' + json[i].Name + ' width="100" height="100" /></a>'
                     html += '</dt>';
                     html += '<dd>';
                     html += '<ul class="line20">';
                     html += ' <li>';
                     html += '<b>名称:</b><a href=' + json[i].PageUrl + ' class="shopLink" target="_blank">' + json[i].NameHtml + '</a>';
                     html += ' </li>';
                     html += ' <li><b>价格：</b>' + json[i].Price + '</li>';
                     html += ' <li>';
                     html += ' </li><b>销量：</b>' + json[i].Sales + '<li>';
                     html += ' </li><b>人气：</b>' + json[i].Point + '</ul>';
                     html += ' </li><b>评分：</b>' + json[i].LevelHtml + '</ul>';
                     html += '</dd>'
                     html += '</dl>';
                     html += '</div>';
                 }
                 html += '</div>';
                 $("#insideProduct").html(html);
             });
    });
</script>
<div style="clear: both;">
</div>
