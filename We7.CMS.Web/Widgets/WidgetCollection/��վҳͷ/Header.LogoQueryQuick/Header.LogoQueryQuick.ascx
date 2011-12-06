<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header.LogoQueryQuick.cs"
 Inherits="We7.CMS.Web.Widgets.Header_LogoQueryQuick" %>
<script type="text/C#" runat="server">
    [ControlDescription("带Logo带搜索带快捷导航(头)")]
    string MetaData;

    /// <summary>
    /// 搜索地址
    /// </summary>
    [Parameter(Title="搜索导航地址",Type="String")]
    public string SearchPage = "SearchBar";
</script>
<div class="<%=Css %>">
<div class="top">
    <div id="logo">
        <a href="/">
            <img alt="<%=SiteName %>" src="<%=LogoPath %>" >
        </a>
    </div>
    <div class="web_tool">
        <div class="web_tip">
            <a class="reg" id="loginAnchor" href="javascript:void(0)" <%=BackgroundIcon(1) %>>会员登录</a>
            <a class="rss" target="_blank" <%=BackgroundIcon(2) %>  href="/go/rss.aspx?ChannelUrl=<%= ChannelUrl %>">RSS</a>
            <a class="wap" href="javascript:void(0)" <%=BackgroundIcon(3) %>>WAP</a>
            <a class="home" href="javascript:void(0)" <%=BackgroundIcon(4) %>>设为首页</a> 
            <a class="fav" href="javascript:void(0)" <%=BackgroundIcon(5) %>>加入收藏</a></div>
        <div class="full_model_search">
            <label><input name="keywords" value="" class="model_search"></label>
            <label><input type="button" class="model_search_click" value="搜索"></label>
            <a title="高级搜索" id="model_adv_search" href="javascript:void(0)">高级搜索</a> 
            <a class="model_search_all" title="全文检索" href="javascript:void(0)">全文检索</a>
        </div>
        <div class="model_project">
            <a class="cur" id="change_search_model" href="javascript:void(0)">全部</a>
        </div>
        <div class="model_list">
            <ul>
                <li><a href="javascript:void(0)">文章</a></li>
                <li><a href="javascript:void(0)">图片</a></li>
                <li><a href="javascript:void(0)">软件</a></li>
                <li><a href="javascript:void(0)">全部</a></li>
            </ul>
        </div>
    </div>
</div>
</div>
<script type="text/javascript">
$(function () {
        //$('a[class="fav"]').addFavorite();
        //$('a[class="home"]').setHomePage();

        $('#change_search_model').click(function () {
            if($('.model_list').css('display') != 'block')
            {
              $('.model_list').slideDown(500);
              try { clearTimeout(timer); } catch (e) { }
            }else{
              $('.model_list').mouseleave();
            };
        })

        $('.model_list').mouseleave(function () {
            timer = setTimeout(function () {
                $('.model_list').slideUp(100);
            }, 500);
        })

        $('.model_list a').click(function () {
            $('#change_search_model').text($(this).text())
            $('.model_list').slideUp(100);
            try { clearTimeout(timer); } catch (e) { }
        })

        $('.model_search_click').click(function () {
            var model = checkSearchModel();
            doModelSearch(model[0],model[1]);
        })

        $('#model_adv_search').click(function () {
            var model = checkSearchModel();
            doModelSearch(model[0],2);
        })
        
        var checkSearchModel = function(){
        var model = $('#change_search_model').text();
            switch (model) {
                case '文章':
                    return[1, 1];
                case '图片':
                    return[2, 1];
                case '软件':
                    return[3, 1];
                default:
                    return[0, 2];
            }
        }

        var doModelSearch = function (modelId, searchType) {
            var searchValue = $('.model_search').val();
            if (searchType != 2 && (searchValue == null || searchValue == '')) {
                alert('请输入要搜索的关键词');
                $('.model_search').focus();
                return;
            }
            if(searchType == 2 && modelId == 0){
                location.href = 'search.aspx?keywords=' + escape(searchValue);
                return;
            }
            switch (modelId) {
                case 1:
                    location.href = '<%=SearchPage %>?searchtype=1&modelId=' + modelId + '&nodeId=1&fieldOption=title&Keyword=' + searchValue;
                    break;
                case 2:
                    location.href = '<%=SearchPage %>?searchType=1&modelId=' + modelId + '&nodeId=2&fieldOption=title&Keyword=' + searchValue;
                    break;
                default:
                    location.href = '<%=SearchPage %>?searchtype=1&modelId=' + modelId + '&nodeId=3&fieldOption=title&Keyword=' + searchValue;
                    break;
            }
        }
});
</script>

