<%@ Control Language="C#" AutoEventWireup="true" Debug="true" CodeFile="FlashShow.Default.cs"
    Inherits="We7.CMS.Web.Widgets.FlashShow_Default" %>
<div id="<%=ClientID %>" class="<%=Css %>"></div>

<script type="text/javascript">
    $.installPlugins(["FlashShow.js"]);
    $("#<%=ClientID %>").FlashShow({
        focus_width:<%=FrameWidth %>,
        focus_height:<%=FrameHeight %>,
        pics:'<%=sbThumb %>',
        links:'<%=sbUrl %>',
        texts:'<%=sbTitle %>'
    });
</script>