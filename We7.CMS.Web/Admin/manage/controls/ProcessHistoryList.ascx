<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProcessHistoryList.ascx.cs"
    Inherits="We7.CMS.Web.Admin.manage.controls.ProcessHistoryList" %>
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
    .remarkBody p
    {
        border: solid 1px #eeead6;
        background-color: #fffad6;
        width: 80%;
        margin: 10px 10px 30px 15px;
        padding: 10px;
    }
</style>
<asp:DataList ID="ViewDataList" runat="server" RepeatDirection="Vertical">
    <ItemTemplate>
        <table cellpadding="0" cellspacing="0" width="100%" border="0">
            <tr>
                <td class="title" colspan="3">
                    <span class="num">
                        <%#Eval("ItemNum")%>.</span><b>
                            <%#Eval("ProcessingText")%><%#Eval("ProcessDirectionText")%><%#Eval("ProcessText")%></b> 
                </td>
            </tr>
            <tr>
                <td class="title" colspan="3">
                    <p>
                        <%#Eval("ApproveTitle")%></p>
                </td>
            </tr>
            <tr>
                <td class="remarkBody" colspan="3">
                    <p>
                        <%#Eval("Remark")%></p>
                </td>
            </tr>
            <tr align="right">
                <td class="title">
                </td>
                <td class="title" colspan="2">
                    <p>
                        <b><u>&nbsp;&nbsp;&nbsp;<%#Eval("ApproveName")%>&nbsp;&nbsp;&nbsp;</u></b>签字&nbsp;&nbsp;&nbsp;<b><u>&nbsp;&nbsp;&nbsp;<%# DataBinder.Eval(Container.DataItem,"UpdateDate","{0:yyyy-MM-dd}") %>&nbsp;&nbsp;&nbsp;</u></b>日期&nbsp;&nbsp;&nbsp;
                    </p>
                </td>
            </tr>
        </table>
        <hr />
    </ItemTemplate>
</asp:DataList>