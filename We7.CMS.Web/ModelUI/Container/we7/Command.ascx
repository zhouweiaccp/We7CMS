<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Command.ascx.cs" Inherits="We7.Model.UI.Container.we7.Command" %>
<script type="text/javascript" src="/Admin/cgi-bin/search.js"></script>
<script type="text/javascript" src="/Admin/ajax/jquery/jquery.DMenu.js"></script>
<script type="text/javascript">

	function selectChannels(action) {
		var url = "../ChannelList.aspx";
		if (action && action == "linkto") {
			url = url + "?type=link";
			weShowModelDialog(url, onChannelListCallback_tolink);
		}
		else
			weShowModelDialog(url, onChannelListCallback);
	}

	function onChannelListCallback(v, t) {
		if (v) {
			document.getElementById("<%=hfMoveTo.ClientID %>").value = v;
			document.getElementById("<%=lnkMoveTo.ClientID %>").click();
		}
	}

	function onChannelListCallback_tolink(v, t) {
		if (v) {
			document.getElementById("<%=hfLinkTo.ClientID %>").value = v;
			document.getElementById("<%=lnkLinkTo.ClientID %>").click();
		}
	}

	$(function () {
		$("#button").DMenu("#tagsContainer");
	});

	function addTag(t) {
		var targetID = document.getElementById("<%=hfTag.ClientID %>");
		targetID.value = t;
		var button = document.getElementById("<%=lnkAddTag.ClientID %>");
		button.click();
	}
	function GoToSortPage() {
		var result = window.showModalDialog('/admin/contentmodel/sort.aspx<%=URLParam %>', '', 'dialogHeight:500px;dialogWidth:700px;status:no');
		if (result == 'ok')
		__doPostBack('<%=lnkRefresh.ClientID.Replace('_','$') %>', '')
		return false;
	}
</script>
<style type="text/css">
	.style1
	{
		width: 589px;
	}
	.style2
	{
		width: 3px;
	}
	#tagsContainer
	{
		border: 1px solid #990000;
		padding: 5px 10px;
		width: 105px;
		height: 204px;
		background-color: #ffe;
	}
</style>
<div id="tagsContainer">
	<asp:Literal runat="server" ID="TagsLiteral"></asp:Literal>
</div>
<div class="toolbar2">
	<li id="RemoveTagLi" runat="server" visible="false" class="smallButton8"></li>
	<li class="smallButton4">
		<asp:HyperLink ID="lnkNewArticle" runat="server">新增</asp:HyperLink>
	</li>
	<li class="smallButton4">
		<asp:LinkButton ID="lnkPubLish" OnClick="lnkPubLish_Click" CommandName="setPublish"
			CommandArgument="multirow" runat="server">发布</asp:LinkButton>
	</li>
	<li class="smallButton4">
		<asp:LinkButton ID="lnkStopPubLish" OnClick="lnkStopPubLish_Click" CommandName="cancelPublish"
			CommandArgument="multirow" runat="server">取消发布</asp:LinkButton>
	</li>
	<li class="smallButton4">
		<asp:LinkButton ID="lnkSubmitAudit" OnClick="lnkSubmitAudit_Click" CommandName="submitAudit"
			CommandArgument="multirow" runat="server">提交审核</asp:LinkButton>
	</li>
	<li runat="server" id="MoveToSpan" class="smallButton4">
		<asp:HyperLink ID="MoveToHyperLink" NavigateUrl="javascript:selectChannels();" runat="server">
           移动到...</asp:HyperLink>
		<asp:LinkButton ID="lnkMoveTo" CommandName="moveTo" CommandArgument="multirow" runat="server"
			Style="display: none" OnClick="lnkMoveTo_Click">移动到辅助按钮</asp:LinkButton>
		<asp:HiddenField ID="hfMoveTo" runat="server" />
	</li>
	<li class="smallButton4" runat="server" id="LinkToSpan">
		<asp:HyperLink ID="HyperLinkCreateRefer" NavigateUrl="javascript:selectChannels('linkto');"
			runat="server">
           发到专题...</asp:HyperLink>
		<asp:LinkButton ID="lnkLinkTo" CommandName="linkTo" CommandArgument="multirow" runat="server"
			Style="display: none" OnClick="lnkLinkTo_Click">发布到专题</asp:LinkButton>
		<asp:HiddenField ID="hfLinkTo" runat="server" />
	</li>
	<li class="smallButton4">
		<asp:LinkButton ID="lnkSetTop" OnClick="lnkSetTop_Click" CommandName="setTop" CommandArgument="multirow"
			runat="server">置顶</asp:LinkButton>
	</li>
	<li class="smallButton4">
		<asp:LinkButton ID="lnkCancelTop" OnClick="lnkCancelTop_Click" CommandName="cancelTop"
			CommandArgument="multirow" runat="server">取消置顶</asp:LinkButton>
	</li>
	<li class="smallButton6">
		<div id="button">
			<a href="">添加标签▼</a></div>
		<asp:LinkButton ID="lnkAddTag" CommandName="addTag" CommandArgument="multirow" runat="server"
			Style="display: none" OnClick="lnkAddTag_Click">添加标签辅助按钮</asp:LinkButton>
		<asp:HiddenField ID="hfTag" runat="server" />
	</li>
	<li class="smallButton4">
		<asp:LinkButton ID="lnkPublishShared" OnClick="lnkPublishShared_Click" CommandName="publishShared"
			CommandArgument="multirow" runat="server">共享发布</asp:LinkButton>
	</li>
	<li class="smallButton6">
		<asp:LinkButton ID="lnkReciveShared" OnClick="lnkReciveShared_Click" CommandName="getShared"
			CommandArgument="multirow" runat="server">获取最新共享</asp:LinkButton></li>
	<li class="smallButton4">
		<asp:LinkButton ID="lnkDelSel" OnClick="OnButtonSubmit" CommandName="delSel" CommandArgument="multirow"
			runat="server">删除</asp:LinkButton>
	</li>
	<li class="smallButton4">
		<asp:LinkButton ID="lnkRefresh" OnClick="OnButtonSubmit" CommandName="refresh" CommandArgument="multirow"
			runat="server">刷新</asp:LinkButton>
	</li>
	<li class="smallButton4">
		<asp:LinkButton ID="lnkSort" OnClientClick="return GoToSortPage();" runat="server">排序</asp:LinkButton>
	</li>
</div>
