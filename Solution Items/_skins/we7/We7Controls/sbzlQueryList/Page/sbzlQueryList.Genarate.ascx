<!--### name="上报资料反馈列表(自动布局)" type="system" version="1.0" created="2011/7/5 17:52:20" 
desc="" author="We7 Group" ###-->
<%@ Control Language="C#" AutoEventWireup="true" Inherits="We7.CMS.WebControls.AdviceQueryProviderEx" %>
<% using (Html.BeginFrom())
   { %>
<table>
    <tr>
        <td>
            受理编号：
        </td>
        <td>
            <input type="text" name="SN" value="<%=Html.Request<string>("SN") %>" />
        </td>
        <% if(SecurityQuery) {%>
        <td>
            密码：
        </td>
        <td>
            <input type="text" name="Password"  value="<%=Html.Request<string>("Password") %>" />
        </td>
        <%} else {%>
			<td>
            关键字：
        </td>
        <td>
            <input type="text" name="KeyWord"  value="<%=Html.Request<string>("KeyWord") %>" />
        </td>
        <%}%>
        <td>
            <input type="submit" value="查询" /><%=ErrorMessage %>
        </td>
    </tr>
</table>
<%} %>
<table>
    <tr>
        <th>
            受理编号
        </th>
        <th>
            标题
        </th>
        <th>
            办理人
        </th>
        <th>
            受理时间
        </th>
        <th>
            状态
        </th>
        		   <th>
				ID
		   </th>
	    		   <th>
				是否前台显示
		   </th>
	    		   <th>
				标题
		   </th>
	    		   <th>
				姓名
		   </th>
	    		   <th>
				乳品名称
		   </th>
	    		   <th>
				乳品 指标
		   </th>
	        </tr>
    <% foreach (AdviceInfo Item in Items)
       { %>
    <tr>
        <td>
            <%=Item.SN %>
        </td>
        <td>
        <%if (Item.Public==1){ %>
			<a href="<%=GetUrl(Item.ID) %>">
        <%} else {%>
        <a href="<%=GetUrl(Item.ID) %>" onclick="return checkadvice(this,'<%=Item.ID%>')">
        <%}%>
         <%=Item.Title %></a>
        </td>
        <td>
            <%=Item.Name %>
        </td>
        <td>
            <%=Item.Created.ToString("yyyy-MM-dd") %>
        </td>
        <td>
            <%=Item.StateText %>
        </td>
        		   <td>
				<%=Item["ID"] %>
		   </td>
	    		   <td>
				<%=Item["IsShow"] %>
		   </td>
	    		   <td>
				<%=Item["Title"] %>
		   </td>
	    		   <td>
				<%=Item["Name"] %>
		   </td>
	    		   <td>
				<%=Item["rpmc"] %>
		   </td>
	    		   <td>
				<%=Item["rpzb"] %>
		   </td>
	        </tr>
    <%} %>
</table>
<%--系统提供的方法
string ToStr(object fieldValue)
string ToStr(object fieldValue, int maxlength)
string ToStr(object fieldValue, int maxlength, string tail)
string ToDateStr(object fieldValue, string fmt)
string ToDateStr(object fieldValue)
int? ToInt(object fieldValue)
string GetUrl(object id)
--%>
