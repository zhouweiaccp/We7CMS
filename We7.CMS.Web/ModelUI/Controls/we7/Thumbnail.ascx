<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Thumbnail.ascx.cs" Inherits="We7.CMS.Web.Admin.ContentModel.Controls.Thumbnail" %>
<div>
    <asp:TextBox ID="txtPath" runat="server" Width="200" onblur="this.className='input_blur'"
        onfous="this.className='input_focus'"></asp:TextBox>
    <input type="button" value="上传图片" onclick="<%=ClientID %>()" class="button_style" />
   <%-- <input type="button" value="选择..." onclick="alert('添加图片')" class="button_style" />
    <input type="button" value="裁剪图片" onclick="alert('添加图片')" class="button_style" />--%>
    <script type="text/javascript">
        function <%=ClientID %>()
        {
            var isMSIE= (navigator.appName == "Microsoft Internet Explorer"); 
            if(isMSIE)
            {
                window.showModelessDialog('/ModelUI/Controls/Page/ImageUpload.aspx?aid=<%=ArticleID %>&v='+$("#<%=txtPath.ClientID %>").val(),$("#<%=txtPath.ClientID %>")[0],'scroll:0;status:0;help:0;resizable:0;dialogWidth:500px;dialogHeight:450px');
            }
            else
            {
                alert('请用IE进行数据添加');
            }
        }
    </script>

</div>
