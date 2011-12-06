<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FullTextSearch.Result.cs"
 Inherits="We7.CMS.Web.Widgets.ShopDownload.FullTextSearch_Result" %>

<div class="<%= CssClass %>">
<h1>搜索 <%=Keyword%> 获得约 <%=Pager.RecordCount%> 条结果<% if (Pager.RecordCount > 0)
   { %>
，以下是第 <%=Pager.StartItem + 1%>-<%=Pager.EndItem + 1%> 条<% } %></h1>
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
<%= Pager.PagedHtml%>
</div>
