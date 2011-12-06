<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ThumbnailEx.ascx.cs"
    Inherits="We7.CMS.Web.Admin.ContentModel.Controls.ThumbnailEx" %>
<div id="<%=this.ClientID %>" style="width:600px;">
</div>
<asp:HiddenField ID="hfValue" runat="server" />
<script type="text/javascript">
    $(function() {
        new ImageBuilder({ Src: "<%=Src %>",FrameWidth:<%=FrameWidth %>,FrameHeight:<%=FrameHeight %>, HiddenValue: $("#<%=hfValue.ClientID %>"), Ct: $("#<%=this.ClientID %>"), ArticleID: '<%=ArticleID %>' });
    });
</script>

