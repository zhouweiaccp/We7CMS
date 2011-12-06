<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdviceHistory.ascx.cs"
    Inherits="We7.CMS.Web.Admin.AdviceHistory" %>
<style>
    .num
    {
        font-size: 42px;
        font-weight: bold;
        font-family: Eras Bold ITC;
        color: #ddd;
    }
    .title p
    {
        margin-top: 5px;
        margin-left: 5px;
    }
    .remarkBody p,.remarkBody his
    {
        border: solid 1px #eeead6;
        background-color: #fffad6;
        width: 80%;
        margin: 10px 10px 10px 15px;
        padding: 10px;
    }
</style>
<table cellpadding="0" cellspacing="0" width="100%" border="0">
    <tr>
        <td class="title" colspan="3">
            <span class="num">
                <%=SN++ %>.</span> <b>提交记录.(<%=Advice.Name %>admin于<%=FormatDate(Advice.Created) %>)</b>
        </td>
    </tr>
</table>
<hr />
<% foreach (We7.CMS.Common.AdviceTransfer tran in Trans)
   { %>
<table cellpadding="0" cellspacing="0" width="100%" border="0">
    <tr>
        <td class="title" colspan="3">
            <span class="num"><%=SN++ %>.</span><b> 转办：<%=GetTypeName(tran.FromTypeID)%>-><%=GetTypeName(tran.ToTypeID) %>.(<%=String.IsNullOrEmpty(Advice.Name)?"匿名":Advice.Name%>于<%=FormatDate(Advice.Created)%>)</b>
        </td>
    </tr>
    <% if (!String.IsNullOrEmpty(tran.Suggest))
       { %>
    <tr>
        <td class="remarkBody" colspan="3">
            <p>
                <%=tran.Suggest%></p>
        </td>
    </tr>
    <%} %>
</table>
<hr />
<%} %>
<% if (Advice.State == 9 || Advice.State == 1)
   {%>
<table cellpadding="0" cellspacing="0" width="100%" border="0">
    <tr>
        <td class="title" colspan="3">
            <span class="num"><%=SN++ %>.</span><b><%=Advice.StateText %>.(<%=Advice.Name%>admin于<%=FormatDate(Advice.Created)%>)
        </td>
    </tr>
</table>
<%} %>
