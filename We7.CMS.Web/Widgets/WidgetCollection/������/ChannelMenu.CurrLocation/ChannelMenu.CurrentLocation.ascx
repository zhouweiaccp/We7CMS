<%@ Control Language="C#" AutoEventWireup="true" Debug="true" CodeFile="ChannelMenu.CurrLocation.cs" Inherits="We7.CMS.Web.Widgets.ChannelMenu_CurrLocation" %>
<script type="text/C#" runat="server">
    [ControlDescription(Desc = "当前位置", Author = "系统")]
    [RemoveParameter("Tag")]
    string MetaData;
    protected string GetLocation()
    {
        if (CurrentChannel != null && !string.IsNullOrEmpty(CurrentChannel.FullPath))
        {
            return CurrentChannel.FullPath.Substring(1).Replace("/", "&gt;&gt;");
        }
        return "";    
    }
</script>
<div class="<%=CssClass %>">
<div <%=BackgroundIcon() %> class="sitepath">
    您现在的位置：<em><%=GetLocation()%></em>
</div>
</div>