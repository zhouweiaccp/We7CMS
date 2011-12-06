<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/admin/theme/classic/Content.Master"
  EnableViewState="true"  CodeBehind="ArticleList.aspx.cs" Inherits="We7.CMS.Web.Admin.Addins.ArticleList"  %>
<%@ Register Src="../controls/ArticleListControl.ascx" TagName="ArticleListControl"
    TagPrefix="uc1" %>

<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
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
          <h2  class="title" runat="server" id="TitleH2" visible="false" >
            <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_article.gif" />
            <asp:Label ID="NameLabel" runat="server" Text="管理文章">
            </asp:Label>
            <span class="summary">
                <asp:Label ID="SummaryLabel" runat="server" Text=" ">
                </asp:Label>
             </span>
             <div class="clear"></div>
             <div id="buttonShow">
                 <asp:HyperLink ID="ListTypeHyperLink" runat="server" ToolTip="按文章列表形式展示"> <img  src="/admin/images/button_list.gif" />
                </asp:HyperLink>
                <asp:HyperLink ID="TreeTypeHyperLink"  runat="server" ToolTip="按栏目树形式展示"> <img src="/admin/images/button_tree.gif" />
                </asp:HyperLink>
             </div>
             <div id="channelList" runat="server" class="channelSelect"   >
                      <asp:DropDownList ID="ChannelDropDownList"  runat="server"  EnableViewState="false"  onChange="MM_jumpMenu('parent',this,0)"  Visible="false"   >
                    </asp:DropDownList>
              </div>
        </h2>        
        
        <uc1:ArticleListControl ID="ArticleListControl1" runat="server" />
    
</asp:Content>