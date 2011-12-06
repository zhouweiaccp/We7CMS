<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ListPanel.ascx.cs" Inherits="We7.Model.UI.Panel.system.ListPanel" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<%@ Register Src="../../Container/system/SimpleCondition.ascx" TagName="SimpleCondition"
	TagPrefix="uc2" %>
<%@ Register Src="../../Container/system/SimpleList.ascx" TagName="SimpleList" TagPrefix="uc3" %>
<%@ Register Src="../../Container/system/SimplePager.ascx" TagName="SimplePager"
	TagPrefix="uc4" %>
<%@ Register Src="../../Container/system/SimpleCommand.ascx" TagName="SimpleCommand"
	TagPrefix="uc6" %>
<style>
	.modeStyle
	{
		float: right;
		margin-right: 20px;
		margin-top:-40px;
	}
	.modeStyle a
	{
		padding: 10px;
		text-decoration: none;
	}
	.modeStyle a.cur
	{
		font-weight: bolder;
	}
	.btnBack
	{
	}
</style>
<asp:Panel ID="plList" runat="server">
	<asp:PlaceHolder ID="phNavigation" runat="server"></asp:PlaceHolder>
	<WEC:MessagePanel ID="msg" runat="server">
	</WEC:MessagePanel>
	<asp:PlaceHolder ID="phMode" runat="server"></asp:PlaceHolder>
	<asp:PlaceHolder ID="phCommand" runat="server"></asp:PlaceHolder>
	<asp:PlaceHolder ID="phCondition" runat="server"></asp:PlaceHolder>
	<asp:PlaceHolder ID="phList" runat="server"></asp:PlaceHolder>
	<asp:PlaceHolder ID="phPager" runat="server"></asp:PlaceHolder>
	<script type="text/javascript">		if ($(".modeStyle a[class!='btnBack']").length <= 1) $(".modeStyle a[class!='btnBack']").hide();</script>
</asp:Panel>
