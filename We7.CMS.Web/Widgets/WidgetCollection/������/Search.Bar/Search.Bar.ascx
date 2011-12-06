<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.Bar.cs"
 Inherits="We7.CMS.Web.Widgets.Search_Bar" %>
<script type="text/C#" runat="server">
    [ControlDescription("搜索框")]
    string MetaData;

    /// <summary>
    /// 搜索地址
    /// </summary>
    [Parameter(Title="搜索结果地址",Type="String")]
    public string SearchPage = "Search.aspx";
</script>
<div class="<%=CssClass %>">
<div class="web_tool">
                <div class="full_model_search">
                    <label><input value="" class="model_search"></label><label><input type="button" class="model_search_click" value="搜索"></label>
                    <script type="text/javascript">$('.model_search').useKeypressSubmit($('.model_search_click'))</script>
                    <a title="高级搜索" id="model_adv_search" href="javascript:void(0)">高级搜索</a> <a class="model_search_all" title="全文检索" href="#">全文检索</a>
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
                location.href = '<%=SearchPage %>?keywords=' + searchValue;
                return;
            }
            switch (modelId) {
                case 1:
                    location.href = '<%=SearchPage %>?searchtype=1&modelId=' + modelId + '&nodeId=1&fieldOption=title&keywords=' + searchValue;
                    break;
                case 2:
                    location.href = '<%=SearchPage %>?searchType=1&modelId=' + modelId + '&nodeId=2&fieldOption=title&keywords=' + searchValue;
                    break;
                default:
                    location.href = '<%=SearchPage %>?searchtype=1&modelId=' + modelId + '&nodeId=3&fieldOption=title&keywords=' + searchValue;
                    break;
            }
        }
});
</script>

