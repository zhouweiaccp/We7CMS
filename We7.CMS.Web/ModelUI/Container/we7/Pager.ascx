<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Pager.ascx.cs" Inherits="We7.Model.UI.Container.we7.We7Pager" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<div class="pagination">
    <webdiyer:AspNetPager ID="Pager" AlwaysShow="false" runat="server" OnPageChanging="Pager_PageChanging"
        Style="color: #077ac7; font-size: 12px;" PageIndexBoxType="TextBox"
        Width="100%" PageIndexBoxStyle="width:19px" FirstPageText="首页" LastPageText="尾页"
        NextPageText="后页" PrevPageText="前页" NumericButtonTextFormatString="{0}"
        HorizontalAlign="right" CustomInfoStyle="text-align:left;" ShowCustomInfoSection="Left" TextAfterPageIndexBox="页"
        TextBeforePageIndexBox="转到第" CustomInfoHTML="第 <font color='red'><b>%CurrentPageIndex%</b></font> 页 共 %PageCount% 页 显示 %StartRecordIndex%-%EndRecordIndex% 条">
    </webdiyer:AspNetPager>
</div>
