<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultiCommand.ascx.cs"
    Inherits="We7.Model.UI.Container.we7.MultiCommand" %>
<div class="toolbar2">11
    <li id="RemoveTagLi" runat="server" visible="false" class="smallButton8"></li>
    <li class="smallButton4">
        <asp:LinkButton ID="lnkNewArticle" OnClick="OnButtonSubmit" CausesValidation="false" CommandName="publish" runat="server">新增文章</asp:LinkButton>
    </li>
    <li class="smallButton4">
        <asp:LinkButton ID="lnkPubLish" OnClick="OnButtonSubmit" CommandName="setPublish"
            CommandArgument="multirow" runat="server">发布</asp:LinkButton>
    </li>
    <li class="smallButton4">
        <asp:LinkButton ID="lnkStopPubLish" OnClick="OnButtonSubmit" CommandName="cancelPublish"
            CommandArgument="multirow" runat="server">取消发布</asp:LinkButton>
    </li>
    <li class="smallButton4">
        <asp:LinkButton ID="lnkSubmitAudit" OnClick="OnButtonSubmit" CommandName="submitAudit"
            CommandArgument="multirow" runat="server">提交审核</asp:LinkButton>
    </li>
    <li id="QuoteSpan" runat="server" visible="false" class="smallButton6">
        <asp:HyperLink ID="QuoteHyperLink" NavigateUrl="javascript:doQuoteArticle();" runat="server"
            Visible="false">
            引用站内文章</asp:HyperLink>
    </li>
    <li id="Span2" runat="server" visible="false" class="smallButton6">
        <asp:HyperLink ID="AddTopicHyperLink" NavigateUrl="javascript:doQuoteTopics();" runat="server"
            Visible="false">
          引用共享区文章</asp:HyperLink>
    </li>
    <li runat="server" id="MoveToSpan" class="smallButton4">
        <asp:HyperLink ID="MoveToHyperLink" NavigateUrl="javascript:selectChannels();" runat="server">
           移动到...</asp:HyperLink></li>
    <li class="smallButton4">
        <asp:HyperLink ID="HyperLinkCreateRefer" NavigateUrl="javascript:selectChannels('linkto');"
            runat="server">
           引用到...</asp:HyperLink></li>
    <li class="smallButton4">
        <asp:LinkButton ID="lnkSetTop" OnClick="OnButtonSubmit" CommandName="setTop" CommandArgument="multirow"
            runat="server">置顶</asp:LinkButton>
    </li>
    <li class="smallButton4">
        <asp:LinkButton ID="lnkCancelTop" OnClick="OnButtonSubmit" CommandName="cancelTop"
            CommandArgument="multirow" runat="server">取消置顶</asp:LinkButton>
    </li>
    <li class="smallButton6">
        <div id="button">
            <a href="">添加标签▼</a></div>
    </li>
    <li class="smallButton4">
        <asp:HyperLink ID="ShareHyperLink" NavigateUrl="javascript:doShareArticles();" runat="server">
           共享发布</asp:HyperLink></li>
    <li class="smallButton6">
        <asp:HyperLink ID="GetShareHyperLink" NavigateUrl="javascript:doGetShareArticles();"
            runat="server">
           获取最新共享</asp:HyperLink></li>
    <li class="smallButton4">
        <asp:LinkButton ID="lnkDelSel" OnClick="OnButtonSubmit" CommandName="delSel" CommandArgument="multirow"
            runat="server">删除</asp:LinkButton>
    </li>
    <li class="smallButton4">
        <asp:LinkButton ID="lnkRefresh" OnClick="OnButtonSubmit" CommandName="refresh" CommandArgument="multirow"
            runat="server">刷新</asp:LinkButton>
    </li>
</div>
