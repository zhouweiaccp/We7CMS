<%@ Page Language="C#" CodeBehind="Channels.aspx.cs" MasterPageFile="~/admin/theme/classic/Content.Master"
    Inherits="We7.CMS.Web.Admin.Channels" %>

<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <script language="javascript">
        var StyleSheetPath = "<%=ThemePath%>/";
        var ThisSiteName = "<%=ThisSiteName %>";
        function switchSysBar() {
            var obj = document.getElementById("switchPoint");
            if (obj.alt == "关闭栏目树") {
                obj.alt = "打开栏目树";
                obj.src = "" + StyleSheetPath + "Images/tree_open.gif";
                document.getElementById("frmTitle").style.display = "none";
            }
            else {
                obj.alt = "关闭栏目树";
                obj.src = "" + StyleSheetPath + "Images/tree_close.gif";
                document.getElementById("frmTitle").style.display = "";
            }
        }

        function freshNodeTree(newNodeID, newNodeText) {
            var butt = document.getElementById("freshNodeTreeButton");
            var newText = document.getElementById("newNodeText");
            newText.value = newNodeText;
            document.getElementById("newNodeID").value = newNodeID;
            butt.click();
        }

        function freshNodeText(newTitle) {
            var butt = document.getElementById("freshNodeTextButton");
            var newText = document.getElementById("newNodeText");
            newText.value = newTitle;
            butt.click();
        }
    </script>
    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_img.gif" />
        <asp:Label ID="NameLabel" runat="server" Text="栏目结构管理">
        </asp:Label>
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server" Text="小帖士：您可以拖动栏目改变排序与结构，在栏目树上点击右键开始奇妙旅程:)">
            </asp:Label>
        </span>
    </h2>
    <%--<div class="toolbar">
            <asp:HyperLink ID="RefreshHyperLink" NavigateUrl="~/channels.aspx" runat="server">
                <asp:Image ID="RefreshImage" runat="server" ImageUrl="~/admin/Images/icon_refresh.gif" />
                刷新</asp:HyperLink>
              <span style="font-size:12px">小贴士：单击栏目名称弹出菜单可以开始操作，可以用鼠标拖拽来改变栏目的顺序及位置。</span>
         </div>--%>
    <div>
        <script type="text/javascript" src="<%=AppPath%>/ajax/Controller.js"></script>
        <!-- 初始化 -->
        <table style="border: solid 0px #eee; width: 100%">
            <tr>
                <td style="width: 170px" valign="top" id="frmTitle">
                    <div id="tree-div" style="overflow: visible; height: 400px; width: 170px; border: 0px solid #fff;">
                    </div>
                </td>
                <td onclick="switchSysBar()" valign="top" style="cursor: hand; width: 12px">
                    <img id="switchPoint" src="<%=ThemePath%>/images/tree_close.gif" alt="关闭栏目树" style="border: 0px;
                        cursor: hand;" />
                </td>
                <td align="left" valign="top" style="height: 600px">
                    <div id="rightTabs">
                        <iframe id="ifRightDetail" name="ifRightDetail" src="Help/WelcomeToChannel.aspx"
                            frameborder="0" width="100%" scrolling="no" height="300px"></iframe>
                    </div>
                </td>
            </tr>
        </table>
        <div style="display: none">
            <input type="button" id="freshNodeTreeButton" value="freshNodeTreeButton" />
            <input type="button" id="freshNodeTextButton" value="freshNodeTextButton" />
            <input type="hidden" id="newNodeText" value="" />
            <input type="hidden" id="newNodeID" value="" />
            <input type="hidden" id="hdDemoSite"
             value='<%= We7.Framework.Config.GeneralConfigs.GetConfig().IsDemoSite.ToString().ToLower() %>' />
        </div>
    </div>
    <script language="javascript">
        function initIframeSrc() {
            if (QueryString('id') && QueryString('id') != "") {
                var url = "ChannelEdit.aspx?id=" + QueryString('id');
                if (document.getElementById("ifRightDetail"))
                    document.getElementById("ifRightDetail").src = url;
            }
        }
        initIframeSrc();
    </script>
</asp:Content>
