<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SimpleCommand.ascx.cs"
    Inherits="We7.Model.UI.Container.system.SimpleCommand" %>
<div class="button_box">
    <span style="width: 60px"><a href="###" onclick="javascript:$('input[type=checkbox]').attr('checked', true)">
        全选</a>/<a href="###" onclick="javascript:$('input[type=checkbox]').attr('checked', false)">取消</a></span>
    <asp:Button CommandArgument="multirow" CommandName="delSel" Text="删除所选" ID="bttnDelSel"
        runat="server" CssClass="button_style" OnClick="OnButtonSubmit" />
    <asp:Button CommandArgument="multirow" CommandName="setPublish" Text="　发布　" ID="bttnExamine"
        runat="server" CssClass="button_style" OnClick="OnButtonSubmit" />
    <asp:Button CommandArgument="multirow" CommandName="cancelPublish" Text="　停用　" ID="bttnShare"
        runat="server" CssClass="button_style" OnClick="OnButtonSubmit" />
    <asp:Button CommandArgument="multirow" CommandName="setTop" Text="　置顶　" ID="bttnSetTop"
        runat="server" CssClass="button_style" OnClick="OnButtonSubmit" />
    <asp:Button CommandArgument="multirow" CommandName="publishShared" Text="共享发布" ID="bttnPublishShared"
        runat="server" CssClass="button_style" OnClick="OnButtonSubmit" />
    <asp:Button CommandArgument="multirow" CommandName="getShared" Text="获取共享" ID="bttnGetShared"
        runat="server" CssClass="button_style" OnClick="OnButtonSubmit" />
</div>
