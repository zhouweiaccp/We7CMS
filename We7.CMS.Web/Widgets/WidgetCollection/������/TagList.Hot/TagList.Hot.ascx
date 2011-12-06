<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TagList.Hot.cs" Inherits="We7.CMS.Web.Widgets.TagList_Hot" %>
<script type="text/C#" runat="server">
    [ControlDescription(Desc = "热门标签",Author="ss")]
    string MetaData;
</script>
<div class="<%=Css %>">
<div id="Tag_list">
    <span><a target="_blank" title="更多Tag标签" href="/TagList.aspx">更多&gt;&gt;</a></span>
    <strong>热门关键字：</strong>
    <ul>
        <li><a href="/search.aspx?keywords=<%=HttpUtility.UrlEncode("We7") %>">We7</a></li>
        <li><a href="/search.aspx?keywords=<%=HttpUtility.UrlEncode("网络") %>">网络</a></li>
        <li><a href="/search.aspx?keywords=<%=HttpUtility.UrlEncode("网站") %>">网站</a></li>
        <li><a href="/search.aspx?keywords=<%=HttpUtility.UrlEncode("电子商务") %>">电子商务</a></li>
        <li><a href="/search.aspx?keywords=<%=HttpUtility.UrlEncode("内容管理") %>">内容管理</a></li>
        <li><a href="/search.aspx?keywords=<%=HttpUtility.UrlEncode("站群") %>">站群</a></li>
        <li><a href="/search.aspx?keywords=<%=HttpUtility.UrlEncode("政府") %>">政府</a></li>
        <li><a href="/search.aspx?keywords=<%=HttpUtility.UrlEncode("学校") %>">学校</a></li>
        <li><a href="/search.aspx?keywords=<%=HttpUtility.UrlEncode("建站") %>">建站</a></li>
        <li><a href="/search.aspx?keywords=<%=HttpUtility.UrlEncode("新版") %>">新版</a></li>
    </ul>
</div>
</div>