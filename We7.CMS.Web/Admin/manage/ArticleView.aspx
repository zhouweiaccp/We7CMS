<%@ Page Language="C#" MasterPageFile="~/admin/theme/classic/Contentnomenu.Master" AutoEventWireup="true" CodeBehind="ArticleView.aspx.cs" Inherits="We7.CMS.Web.Admin.ArticleView" Title="文章预览" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
     <script type="text/javascript" language="javascript">
        function iframeAutoFit()
             {
           try
                {
                  if(window!=parent)
                    {
                    var a = parent.document.getElementsByTagName("IFRAME");
                     for(var i=0; i<a.length; i++) //author:meizz
                       {
                        if(a[i].contentWindow==window)
                           {
                          var h1=0, h2=0;
                          a[i].parentNode.style.height = a[i].offsetHeight +"px";
                          a[i].style.height = "10px";
        if(document.documentElement&&document.documentElement.scrollHeight)
                        {
                           h1=document.documentElement.scrollHeight;
                         }
                         if(document.body) h2=document.body.scrollHeight;
                         var h=Math.max(h1, h2);
                       if(document.all) {h += 4;}
                        if(window.opera) {h += 1;}
                        a[i].style.height = a[i].parentNode.style.height = h+h*0.3 +"px";
                        }
                  }
             }
         }
        catch (ex){}
          }
         if(window.attachEvent)
        {
           window.attachEvent("onload",iframeAutoFit);
        }
        else if(window.addEventListener)
        {
             window.addEventListener('load',   iframeAutoFit,   false);
        }
     </script>
  <h2 class="title" style="text-align:center">
        <asp:Label ID="TitleLabel" runat="server" Text="">
        </asp:Label>
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server" Text="预览">
            </asp:Label>
        </span>
    </h2>
<%--    <div class="toolbar" style="text-align:center">
        <a href="javascript:window.close()">关闭</a>
    </div>--%>
    <hr />
    <div style="padding:30px">
    <asp:Label ID="ContentLabel" runat="server" Text="">
    </asp:Label>
    </div>
    
     <div class="Attachment">
        <h3>附件</h3>
        <%for (int i = 0; i < Attachments.Count; i++)
          { %>
          <p>
        <%=(i+1).ToString() %>. <%=Attachments[i].FileName%>
         （大小：<%=Attachments[i].FileSizeText%>）  
        <a href="<%=Attachments[i].DownloadUrl %>" target="_blank">下载</a> 
        <a href="<%=Attachments[i].FilePath+"/"+Attachments[i].FileName %>" target="_blank">打开</a>
           </p>
        <%}%>
   </div>
        
</asp:Content>
