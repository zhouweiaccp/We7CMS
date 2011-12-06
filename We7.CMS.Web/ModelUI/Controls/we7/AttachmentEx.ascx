<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AttachmentEx.ascx.cs" Inherits="We7.Model.UI.Controls.we7.AttachmentEx" %>
<div id="<%=this.ClientID %>" style="width:600px;">
<asp:Label ID="bttnUploader" Text="上传附件" runat="server" onmouseover="this.style.background='#e5e5e5'" onmouseout="this.style.background='#f0f0f0'" style=" margin:5px 0px; width:100px; text-align:center; display:block;padding:3px 10px; border:solid 1px #e0e0e0; background:#f0f0f0; cursor:pointer; color:#000;"  />
</div>
<asp:HiddenField ID="hfValue" runat="server" />
<script type="text/javascript">
    $(function() {
        new AttachmentBuilder({TriggerElement:$('#<%=bttnUploader.ClientID %>'),Src: "<%=Src %>",FrameWidth:<%=FrameWidth %>,FrameHeight:<%=FrameHeight %>, HiddenValue: $("#<%=hfValue.ClientID %>"), Ct: $("#<%=this.ClientID %>"), ArticleID: '<%=ArticleID %>' });
    });
</script>
