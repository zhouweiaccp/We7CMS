<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FullTextSearch.Bar.cs"
 Inherits="We7.CMS.Web.Widgets.ShopDownload.FullTextSearch_Bar" %>
<script type="text/C#" runat="server">
    [ControlDescription("站群搜索框")]
    string MetaData;
</script>
<script type="text/javascript">
    $(function () {
        $('#KeyWord<%=Index %>').bind('keyup', function (event) {
            if (event.keyCode == 13) {
                window.location = "/gsearch.aspx?keyword=" + encodeURIComponent(this.value);
            }
        });
        $('#Search<%=Index %>').click(function () {
            window.location = "/gsearch.aspx?keyword=" + encodeURIComponent($('#KeyWord<%=Index %>').val());
        });
    });
</script>
<div class="<%= CssClass %>">
    <ul>
        <li class="keyword"><input type="text" id="KeyWord<%=Index %>" class="keyword"  onblur="if(this.value==''){this.value=this.defaultValue;}"
        onfocus="if(this.value==this.defaultValue){this.value='';}" value="关键词"  /></li>
        <li class="search"><input type="button" id="Search<%=Index %>"  class="keyword" value="<%=SubmitTitle %>" /></li>
    </ul>
</div>

