<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Thumbnail.ascx.cs" Inherits="We7.Model.UI.Controls.system.Thumbnail" %>
<div>
    <div style="display: none">
        <asp:TextBox ID="TxtPath" runat="server" Width="200" onblur="this.className='input_blur'"
            onfous="this.className='input_focus'"></asp:TextBox></div>

    <script type="text/javascript" src="/admin/Ajax/Prototype/prototype.js"></script>
     <script type="text/javascript" src="/admin/cgi-bin/DialogHelper.js"></script>

    <script type="text/jscript">

    function ShowImgList()
    {
         var url='/ModelUI/Controls/system/page/ThumbnailServer.ashx';
         var pars='Type=BindImgList&ID=<%=ArticleID %>&h='+Math.random(); 
         var myAjax = new Ajax.Request(url, { method: 'get',parameters: pars, onComplete:showResponseshow,asynchronous:false  } ); 
    }
    function DelFile(str)
    {
         var url='/ModelUI/Controls/system/page/ThumbnailServer.ashx';
         var pars='Type=DelFile&ID=<%=ArticleID %>&fileName='+str+'&h='+Math.random(); 
         var myAjax = new Ajax.Request(url, { method: 'get',parameters: pars, onComplete:showResponseDel,asynchronous:false  } );  
    }
    function showResponseshow(originalRequest)
    {
         $('ImgList').innerHTML=originalRequest.responseText;
    }
    function showResponseDel(originalRequest)
   {
        if(originalRequest.responseText!="")
        {
        ShowImgList(); 
        }
   }
        //弹出更多对话框
   function UploadThumbnail()
   {
           window.showModelessDialog('<%=DialogUrl%>?ID=<%=ArticleID%>&ClientID=<%=Server.UrlEncode(TxtPath.ClientID)%>',window,'scroll:0;status:1;help:1;resizable:1;dialogWidth:600px;dialogHeight:370px');
   }
    </script>
    <input type="button" value="上传" onclick="UploadThumbnail()" style="vertical-align:middle"/>
    <div style="height: 7px">
    </div>
    <div style="width: 650px" id="ImgList">
        <asp:Literal ID="ImagesLiteralList" runat="server"></asp:Literal>
    </div>
</div>
