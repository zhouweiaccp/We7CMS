<%@ Page Language="C#" AutoEventWireup="true"   Codebehind="Articles.aspx.cs" Inherits="We7.CMS.Web.Admin.Articles" MasterPageFile="~/admin/theme/classic/content.Master" %>
<%@ Register src="../controls/ArticleListControl.ascx" tagname="ArticleListControl" tagprefix="uc1" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
<script language="javascript">
var StyleSheetPath="<%=ThemePath%>/";
var ThisSiteName="<%=ThisSiteName %>";
function switchSysBar()
{
    var obj = document.getElementById("switchPoint");
    if (obj.alt == "关闭栏目树")
    {
        obj.alt = "打开栏目树";
        obj.src = "" + StyleSheetPath + "Images/tree_open.gif";
        document.getElementById("frmTitle").style.display="none";
    }
    else
    {
        obj.alt = "关闭栏目树";
        obj.src = "" + StyleSheetPath + "Images/tree_close.gif";
        document.getElementById("frmTitle").style.display="";
    }
}
function closeBar()
{
    var obj = document.getElementById("switchPoint");
    if (obj)
    {
        obj.alt = "打开栏目树";
        obj.src = "" + StyleSheetPath + "Images/tree_open.gif";
        document.getElementById("frmTitle").style.display="none";
     }
}

</script>
  <script type="text/javascript" src="<%=AppPath%>/ajax/ArticleTreeController.js"></script><!-- 初始化 -->
        <h2  class="title">
            <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_article.gif" />
            <asp:Label ID="NameLabel" runat="server" Text="管理栏目文章">
            </asp:Label>
            <span class="summary"><span id="navChannelSpan"></span>
                <asp:Label ID="SummaryLabel" runat="server" Text=" 小贴士：在栏目树上点击右键有惊喜哦~">
                </asp:Label> 
             </span>
              <div id="buttonShow"  style="padding-top:15px" >
                     <asp:HyperLink ID="ListTypeHyperLink" runat="server" ToolTip="按文章列表形式展示"> <img  src="/admin/images/button_list.gif" />
                    </asp:HyperLink>
                    <asp:HyperLink ID="TreeTypeHyperLink"  runat="server" ToolTip="按栏目树形式展示"> <img src="/admin/images/button_tree.gif" />
                    </asp:HyperLink>
            </div>
        </h2>        
               
        <table  style="border: solid 0px #eee;width:100%" cellpadding="0" cellspacing="0"><tr>
        <td style="width:170px; display:<%=ChannelTreeVisble%>" valign="top" id="frmTitle" >
        <div id="tree-div" style="overflow:visible; height:400px;width:170px;border:0px solid #fff; "></div>
        </td>
        <td onclick="switchSysBar()"  valign="top"  style="cursor:hand;width: 12px;" align="left">
                <img id="switchPoint" src="<%=ThemePath%>/images/tree_close.gif" alt="关闭栏目树" style="border: 0px;
                   " />
        </td>
        <td align="left" valign="top" style="height:600px" >
                 <div id="rightTabs" >
                 <iframe  id="ifRightDetail" name="ifRightDetail" frameborder="0"  src="ArticleList.aspx"  width="100%" scrolling="no"  ></iframe>
                 </div>
        </td></tr>
        </table>
        <script language="javascript" type="text/javascript">
        function initIframeSrc()
        {
            var url="ArticleList.aspx?oid="+QueryString('oid')+"&wap="+QueryString('wap')+"&type="+QueryString('type');
             if(document.getElementById("ifRightDetail"))
                    document.getElementById("ifRightDetail").src=url;
        }
        initIframeSrc();
        </script>
</asp:Content>
