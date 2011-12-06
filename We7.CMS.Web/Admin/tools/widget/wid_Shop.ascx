<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wid_Shop.ascx.cs" Inherits="We7.CMS.Web.Admin.tools.widget.wid_Shop" %>
<div class="widget movable collapsable removable  closeconfirm" id="widget-shop">
    <div class="widget-header">
        <strong>推荐商铺</strong>
    </div>
    <div class="widget-content">
        <div class="inside">
            <div class="wrap_div"  id="insideShop">
            </div>
           <%-- <asp:Repeater ID="rpStores" runat="server">
               <HeaderTemplate>
                    <div class="wrap_div">
                </HeaderTemplate>
                <ItemTemplate>
                    <div class="sub_div">
                        <dl>
                            <dt><a href='<%# Eval("Url") %>' class="shopLink" target="_blank" title='推荐理由:<%# GetClearHtml(Eval("StoreIntro").ToString()) %>'>
                                <img src='<%#Eval("Face") %>' tile='<%# GetClearHtml(Eval("StoreIntro").ToString()) %>' width="100" height="100" />
                            </a></dt>
                            <dd>
                                <ul class="line20">
                                    <li>
                                        <b>名称:</b> <a href='<%#Eval("Url") %>' class="shopLink" target="_blank" title='店铺名称:<%# Eval("StoreName")%>'>
                                            <%# GetChopString(Eval("StoreName").ToString(), 10, "...")%></a>
                                    </li>
                                    <li>
                                        <b>等级：</b><%# Eval("TechnicalLevel") %>
                                    </li>
                                    <li>
                                        <b>评分：</b><%# GetLevelString(Eval("Level").ToString())%>
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
                $("#insideShop").html("暂时无法连接商城！");
            }
        });

        $("#insideShop").html("<img src='/scripts/we7/css/res/blue-loading.gif' /> 连接商城中...");

        $.getJSON("/admin/theme/classic/main.aspx?action=shop",
             function (json) {
                 var html = '<div class="wrap_div">';
                 for (var i = 0; i < json.length; i++) {
                     html += '<div class="sub_div">';
                     html += '<dl>';
                     html += '<dt>';
                     html += '<a href=' + json[i].Url + ' class="shopLink" target="_blank">';
                     html += '<img src=' + json[i].Face + ' tile=' + json[i].StoreIntro + ' width="100" height="100" /></a>'
                     html += '</dt>';
                     html += '<dd>';
                     html += '<ul class="line20">';
                     html += ' <li>';
                     html += '<b>名称:</b><a href=' + json[i].Url + ' class="shopLink" target="_blank" title="店铺名称:' + json[i].StoreIntro + '">' + json[i].NameHtml + '</a>';
                     html += ' </li>';
                     html += ' </li><b>等级：</b>' + json[i].TechnicalLevel + '</ul>';
                     html += ' </li><b>评分：</b>' + json[i].LevelHtml + '</ul>';
                     html += '</dd>'
                     html += '</dl>';
                     html += '</div>';
                 }
                 html += '</div>';
                 $("#insideShop").html(html);
             });
         });    
</script>
<div style="clear:both;"></div>
