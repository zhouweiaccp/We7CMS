<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Condition.ascx.cs" Inherits="We7.Model.UI.Container.we7.Condition" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<div style="display: block; width: 100%">
    <table id="tblQuery" runat="server" cellpadding="0" style="width: auto;" cellspacing="1">
        <tr runat="server" id="trQuery">
            <td>
                <asp:Button ID="bttnQuery" CommandName="query" runat="server" Text="查询" OnClick="bttnQuery_Click"
                    CssClass="button_style" />
            </td>
        </tr>
    </table>
    <ul class="subsubsub">
        <li>
            <asp:LinkButton ID="lnkAll" CommandName="query" OnClick="lnkAll_Click" runat="server"
                CssClass="current">全部<span class="count">(<%=GetCount(ArticleStates.All)%>)</span></asp:LinkButton>|</li><li>
                    <asp:LinkButton ID="lnkPublish" CommandName="query" OnClick="lnkPublish_Click" runat="server">已发布<span class="count">(<%=GetCount(ArticleStates.Started)%>)</span></asp:LinkButton>|</li><li>
                        <asp:LinkButton ID="lnkDraft" CommandName="query" OnClick="lnkDraft_Click" runat="server">草稿<span class="count">(<%=GetCount(ArticleStates.Stopped)%>)</span></asp:LinkButton>|</li><li>
                            <asp:LinkButton ID="lnkAudit" CommandName="query" OnClick="lnkAudit_Click" runat="server">审核中<span class="count">(<%=GetCount(ArticleStates.Checking)%>)</span></asp:LinkButton>|</li><li>
                                <asp:LinkButton ID="lnkOverdue" CommandName="query" OnClick="lnkOverdue_Click" runat="server">过期<span class="count">(<%=GetCount(ArticleStates.Overdued)%>)</span></asp:LinkButton></li>               
        <li><WEC:MessagePanel ID="errMsg" runat="server" /></li>
    </ul>        
    <ul class="subsubsub" style="display: none;">
        <asp:Literal ID="StateLiteral" runat="server"></asp:Literal>
        <asp:Literal ID="IncludeSubLiteral" runat="server"></asp:Literal>
    </ul>
    <p>
    </p>
</div>
