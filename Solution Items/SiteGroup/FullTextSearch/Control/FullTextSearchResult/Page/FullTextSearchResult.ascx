<!--### name="站群搜索结果列表_仿百度" type="system" version="1.0" created="2009/12/03" 
desc="文章搜索列表样式展示：样式与baidu、google类似" author="We7 Group" ###-->
<%@ Control Language="C#" AutoEventWireup="true" Inherits="We7.Plugin.FullTextSearch.SearchResultProvider" %>

<div class="FullTextSearchResult_<%= CssClass %>">
<h1>搜索 <%=KeyWord%> 获得约 <%=RecordCount%> 条结果，以下是第 <%=StartItem + 1%>-<%=EndItem+1 %> 条</h1>
<%for (int i = 0; i < Articles.Count; i++)
  { %>
    <div class="Contain">
            <div class="title">
				<li><a target="_blank" href="<%=Articles[i]. LinkUrl%>"><%=Articles[i].Title%></a></li>
			</div>
            <div class="summary"><%=Articles[i].Description%></div>
            <div class="myLink"><a target="_blank" href="<%=Articles[i]. LinkUrl%>"><%= Articles[i].LinkUrl%></a></div>
      </div>
<% }%>
</div>
