<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SimplePager.ascx.cs"
    Inherits="CModel.Container.system.SimplePager" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<webdiyer:AspNetPager ID="Pager" runat="server" OnPageChanging="Pager_PageChanging"
    Style="color: #077ac7; font-size: 12px;" ShowPageIndexBox="Always" PageIndexBoxType="DropDownList"
    Width="100%" PageIndexBoxStyle="width:19px" FirstPageText="【首页】" LastPageText="【尾页】"
    NextPageText="【后页】" PrevPageText="【前页】" NumericButtonTextFormatString="【{0}】"
    HorizontalAlign="right" ShowCustomInfoSection="Left" TextAfterPageIndexBox="页"
    TextBeforePageIndexBox="转到第" CustomInfoHTML="第 <font color='red'><b>%CurrentPageIndex%</b></font> 页 共 %PageCount% 页 显示 %StartRecordIndex%-%EndRecordIndex% 条">
</webdiyer:AspNetPager>