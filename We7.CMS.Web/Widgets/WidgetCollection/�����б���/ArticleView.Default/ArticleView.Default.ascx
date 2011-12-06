<%@ Control Language="C#" AutoEventWireup="true" Debug="true" CodeFile="ArticleView.Default.cs"
    Inherits="We7.CMS.Web.Widgets.ArticleView_Default" %>
<div class="<%=Css %>">
    <div class="article mtop10">
        <div <%=SetBoxBorderColor() %> class="article_content">
            <h1>
                <span><font style="font-weight: normal; font-style: normal;">
                    <%=ThisArticle.Title%></font></span>
            </h1>
            <div class="article_info">
                作者：<%=ThisArticle.Author %>
                来源：<%=ThisArticle.Source %>
                发布时间：
                <%=ToDateStr(ThisArticle.Updated, DateFormat)%>
                点击数：
                <%=ThisArticle.Clicks %>
            </div>
            <!--网站内容开始-->
            <div id="fontzoom" class="article_content_list">
                <div id="articleContnet">
                    <%=ThisArticle.Content %>
                </div>
                <br>
                <div class="page_css">
                    <span class="pagecss" id="pe100_page_contentpage"></span>
                </div>
                <div class="clear">
                </div>
            </div>
            <%if (IsShowAtta)
              { %>
            <div class="articleAttachments">
                <h4>
                    附件</h4>
                <ul>
                    <%
                        foreach (Attachment att in Attachments)
                        { %>
                    <li><a href="<%=att.DownloadUrl %>">
                        <%=att.FileName%></a></li>
                    <%} %>
                </ul>
            </div>
            <%} %>
            <!--网站内容结束-->
            <div class="artilcle_tool">
                【字体：<a class="top_UserLogin" href="javascript:fontZoomA();">小</a> <a class="top_UserLogin"
                    href="javascript:fontZoomB();">大</a>】【<a href="javascript:window.external.AddFavorite(document.location.href,document.title)">收藏</a>】
                【<a href="javascript:window.print();">打印</a>】【<a href="javascript:window.close()">关闭</a>】<span
                    id="content_AdminEdit"></span> <span id="content_signin"></span><span id="content_SigninAjaxStatus"
                        style="display: none;"></span>
            </div>
            <!--上一篇-->
            <div class="article_page">
                <ul>
                    <li><span>上一篇：</span>
                        <%
                            if (PreviousArticle != null)
                            {
                        %>
                        <a target="_self" href="<%=PreviousArticle.Url %>">
                            <%=ToStr(PreviousArticle.Title, TitleLength)%></a>
                        <%}
                            else
                            { %>
                        <span>没有了！</span>
                        <%}%>
                    </li>
                    <li class="next"><span>下一篇：</span>
                        <%
                            if (NextArticle != null)
                            {
                        %>
                        <a target="_self" href="<%=NextArticle.Url %>">
                            <%=ToStr(NextArticle.Title, TitleLength)%></a>
                        <%}
                            else
                            { %>
                        <span>没有了！</span>
                        <%}%>
                    </li>
                </ul>
            </div>
            <!--下一篇-->
        </div>
        <div class="article_about">
            <h3>
                相关文章</h3>
            <ul>
                <%
                    if (RelevantArticles != null && RelevantArticles.Count > 0)
                    {
                        foreach (Article article in RelevantArticles)
                        {                    
                %>
                <li><a target="_self" href="<%=article.Url %>">
                    <%=ToStr(article.Title, TitleLength)%></a>
                    <%=ToDateStr(article.Updated,DateFormat) %></li>
                <%}
                    }
                    else
                    { 
                %>
                <font style="color: red;">没有相关内容</font>
                <%}
                %>
            </ul>
        </div>
    </div>
</div>
<script type="text/javascript" language="javascript">

    // $('#articleContnet img').imageResize();


    //双击鼠标滚动屏幕的代码
    var currentpos, timer;
    function initialize() {
        timer = setInterval("scrollwindow ()", 30);
    }
    function sc() {
        clearInterval(timer);
    }
    function scrollwindow() {
        currentpos = document.body.scrollTop;
        window.scroll(0, ++currentpos);
        if (currentpos != document.body.scrollTop)
            sc();
    }
    document.onmousedown = sc
    document.ondblclick = initialize

    //更改字体大小
    var status0 = '';
    var curfontsize = 10;
    var curlineheight = 18;
    function fontZoomA() {
        if (curfontsize > 8) {
            document.getElementById('fontzoom').style.fontSize = (--curfontsize) + 'pt';
            document.getElementById('fontzoom').style.lineHeight = (--curlineheight) + 'pt';
        }
    }
    function fontZoomB() {
        if (curfontsize < 64) {
            document.getElementById('fontzoom').style.fontSize = (++curfontsize) + 'pt';
            document.getElementById('fontzoom').style.lineHeight = (++curlineheight) + 'pt';
        }
    }

    //设置附件显示
    if($(".ArticleView_Default .articleAttachments ul li").length==0)
        $(".ArticleView_Default .articleAttachments").hide();
</script>
