<%@ Page Language="C#" AutoEventWireup="true" EnableViewState="true" EnableEventValidation="false"
    CodeBehind="ArticleEdit.aspx.cs" Inherits="We7.CMS.Web.Admin.Addins.ArticleEdit"
    ValidateRequest="false" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:content id="We7Content" contentplaceholderid="MyContentPlaceHolder" runat="server">

    <link rel="stylesheet" type="text/css" href="<%=ThemePath%>/css/article.css" media="screen" />  
     <SCRIPT src="<%=AppPath%>/cgi-bin/article.js" type=text/javascript></SCRIPT>
     <SCRIPT src="<%=AppPath%>/cgi-bin/cookie.js" type=text/javascript></SCRIPT>
     <SCRIPT src="<%=AppPath%>/cgi-bin/tags.js" type=text/javascript></SCRIPT> 
     <script type="text/javascript" src="/scripts/we7/we7.loader.js">
      we7("span[rel=xml-hint]").help();
      </script>
     <script type="text/javascript" language="javascript">
         function CreateHtmlTemplate() {
             $.ajax({
                 async: false,
                 type: 'POST',
                 url: "/Plugins/StaticizeHtml/UI/StaticizePageList.aspx",
                 data: {
                     "url": "<%=CurrentChannelUrl %>",
                     "action": "CreateHtmlByUrl"
                 },
                 error: function (XMLHttpRequest, textStatus, errorThrown) {
                     alert('请检查，您是否没有安装生成静态插件？');
                 },
                 success: function (r) {
                     alert('生成成功！');
                 }
             });
         }
         function iframeAutoFit() {
             try {
                 if (window != parent) {
                     var a = parent.document.getElementsByTagName("IFRAME");
                     for (var i = 0; i < a.length; i++) //author:meizz
                     {
                         if (a[i].contentWindow == window) {
                             var h1 = 0, h2 = 0;
                             a[i].parentNode.style.height = a[i].offsetHeight + "px";
                             a[i].style.height = "10px";
                             if (document.documentElement && document.documentElement.scrollHeight) {
                                 h1 = document.documentElement.scrollHeight;
                             }
                             if (document.body) h2 = document.body.scrollHeight;
                             var h = Math.max(h1, h2);
                             if (document.all) { h += 4; }
                             if (window.opera) { h += 1; }
                             a[i].style.height = a[i].parentNode.style.height = h + h * 0.3 + "px";
                         }
                     }

                     if (parent.closeBar) parent.closeBar();
                 }
             }
             catch (ex) { }
         }
         if (window.attachEvent) {
             window.attachEvent("onload", iframeAutoFit);
         }
         else if (window.addEventListener) {
             window.addEventListener('load', iframeAutoFit, false);
         }

     </script>
           <h2  class="title" >
            <asp:Image ID="LogoImage" runat="server" ImageUrl="../Images/icons_article.gif" />
            <asp:Label ID="NameLabel" runat="server" Text="编辑文章">
            </asp:Label>
            <asp:HyperLink ID="hpCreateHtml" NavigateUrl="javascript:CreateHtmlTemplate();" runat="server">
            生成静态栏目页</asp:HyperLink>
            <span class="summary"><span id="navChannelSpan"></span>
                <asp:Label ID="SummaryLabel" runat="server" Text=" ">
                </asp:Label>
             </span>
        </h2>        
        
            <div id="position" >
            <asp:Literal ID="PagePathLiteral" runat="server"></asp:Literal>
            </div>
            <asp:Literal ID="MessageLiteral" runat="server"></asp:Literal>
            <div id="mycontent">
                <DIV class="Tab menuTab">
                <UL class=Tabs>
                  <asp:Label runat="server" ID="MenuTabLabel"></asp:Label>
                  </UL>
                </DIV>
                  <DIV class=clear></DIV>
                <DIV id=rightWrapper>
                <DIV id=container>
                    <asp:PlaceHolder runat="server" ID="ContentHolder" ></asp:PlaceHolder>
                </div>                   
             </div>
            </div>
</asp:content>
