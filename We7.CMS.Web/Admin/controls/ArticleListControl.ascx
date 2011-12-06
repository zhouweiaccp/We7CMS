<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="true" CodeBehind="ArticleListControl.ascx.cs"
    Inherits="We7.CMS.Web.Admin.ArticleListControl" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<style type="text/css">
    .style1
    {
        width: 589px;
    }
    .style2
    {
        width: 3px;
    }
    #tagsContainer
    {
        border: 1px solid #990000;
        padding: 5px 10px;
        width: 105px;
        height: 204px;
        background-color: #ffe;
    }
</style>
<script type="text/javascript" src="<%=AppPath%>/cgi-bin/search.js"></script>
<script type="text/javascript" src="<%=AppPath%>/ajax/jquery/jquery.DMenu.js"></script>
<script type="text/javascript">
    function ProcessRemark(id) 
    {
          var title="模式窗口";
          var nWidth="300";
          var nHeight="200";
          var strFile='<%=IsNormalUser?"../Admin/manage/ScanProcessHistory.aspx?ID=":"../manage/ScanProcessHistory.aspx?ID=" %>'+id;
          //var ret = window.showModalDialog(strFile,window,"dialogWidth:" + nWidth + "px;dialogHeight:" + nHeight + "px;center:yes;status:no;scroll:no;help:no;");
          weShowModelDialog(strFile);
    }   

    function doQuoteArticle() 
    {
        var title="模式窗口";
     var nWidth="650";
     var nHeight="480";
     var strFile="ArticleListSelect.aspx?oid=<%=OwnerID %>";
     var ret = window.showModalDialog(strFile,window,"dialogWidth:" + nWidth + "px;dialogHeight:" + nHeight + "px;center:yes;status:no;scroll:no;help:no;");
    }
    
     function doQuoteTopics() 
    {
        var title="模式窗口";
     var nWidth="650";
     var nHeight="480";
     var strFile="../group/ClientTopicSelect.aspx?oid=<%=OwnerID %>";
     var ret = window.showModalDialog(strFile,window,"dialogWidth:" + nWidth + "px;dialogHeight:" + nHeight + "px;center:yes;status:no;scroll:no;help:no;");
    }
    
     function SelectAll(tempControl) 
    { 
        var theBox=tempControl; 
        xState=theBox.checked;     

        elem=theBox.form.elements; 
        for(i=0;i<elem.length;i++) 
        if(elem[i].type=="checkbox" && elem[i].id!=theBox.id) 
        { 
        if(elem[i].checked!=xState) 
	        elem[i].click(); 
        } 
      }   
      
      function selectChannels(action) {
//      debugger;
          <% if(IsWap){  %>
          var url="../ChannelList.aspx?wap=1";
          <%}else{ %>
          var url="../ChannelList.aspx";
          <%} %>
          if(action && action=="linkto")
          {
               url=url+"?type=link";
               weShowModelDialog(url, onChannelListCallback_tolink);
          }
          else if(action && action=="article") //复制到
          {
               url=url+"?type=article";
               weShowModelDialog(url, onChannelListCallback_copyto);
          }
          else
               weShowModelDialog(url, onChannelListCallback);
      }

      function onChannelListCallback(v, t) {
        if(v) {
            var targetID=document.getElementById("<%=TargetIDTextBox.ClientID %>");
            targetID.value=v;
            var moveButton=document.getElementById("<%=MoveToButton.ClientID %>");
            moveButton.click();
        }
      }
      
      function onChannelListCallback_copyto(v, t) {
        if(v) {
            var targetID=document.getElementById("<%=TargetIDTextBox.ClientID %>");
            targetID.value=v;
            var copyToButton=document.getElementById("<%=CopyToButton.ClientID %>");
            copyToButton.click();
        }
      }
      
       function onChannelListCallback_tolink(v, t) {
        if(v) {
            var targetID=document.getElementById("<%=TargetIDTextBox.ClientID %>");
            targetID.value=v;
            var LinkToButton=document.getElementById("<%=LinkToButton.ClientID %>");
            LinkToButton.click();
        }
      }
      
      function orderByTitle()
      {
            var titleButton=document.getElementById("<%=OrderByTitleButton.ClientID %>");
            titleButton.click();
      }
      
      function orderByDate()
      {
            var dateButton=document.getElementById("<%=OrderByDateButton.ClientID %>");
            dateButton.click();
      }
      
        function doSelectWapChannels() {
        weShowModelDialog("../ChannelList.aspx?wap=1", OnWapChannelListCallback);
        }

        function OnWapChannelListCallback(v, t) {
       if(v) {
            document.getElementById("<%=WapOidTextBox.ClientID %>").value = v;
            document.getElementById("<%=WapToButton.ClientID %>").click();
            }
        }
        
     function deleteArticle()
      {
            var button=document.getElementById("<%=DeleteBtn.ClientID %>");
            button.click();
      }
      
      function deleteOneArticle(id)
      {
            var idHidden=document.getElementById("<%=DeleteIDTextBox.ClientID %>");
            if(idHidden)
            {
                idHidden.value=id;
                deleteArticle();
            }
      }
      
     function startArticle()
      {
            var button=document.getElementById("<%=StartButton.ClientID %>");
            button.click();
      }
      
     function stopArticle()
      {
            var button=document.getElementById("<%=StopButton.ClientID %>");
            button.click();
      }
      
      function submitArticle()
      {
            var button=document.getElementById("<%=SubmitButton.ClientID %>");
            button.click();
      }
      
       function upArticle()
      {
            var button=document.getElementById("<%=UpButton.ClientID %>");
            button.click();
      }

       function downArticle()
      {
            var button=document.getElementById("<%=DownButton.ClientID %>");
            button.click();
      }     
     
      function doShareArticles()
      {
            var button=document.getElementById("<%=ShareButton.ClientID %>");
            button.click();
      }
      
      function doGetShareArticles()
      {
            var button=document.getElementById("<%=GetShareButton.ClientID %>");
            button.click();
      }
      
      function addTag(t)
      {
            var targetID=document.getElementById("<%=TargetIDTextBox.ClientID %>");
            targetID.value=t;
            var button=document.getElementById("<%=AddTagButton.ClientID %>");
            button.click();
      }
      
      function removeTag(tag)
      {
            var targetID=document.getElementById("<%=TargetIDTextBox.ClientID %>");
            targetID.value=tag;
            var button=document.getElementById("<%=RemoveTagButton.ClientID %>");
            button.click();
      }

//      function CreateHtmlTemplate()
//      {
//         var button=document.getElementById("<%=CreateHtmlButton.ClientID %>");
//         button.click();
//      }
      
    $(function(){
	    $("#button").DMenu("#tagsContainer");
    });
</script>
<link media="screen" rel="stylesheet" href="<%=AppPath%>/ajax/jquery/colorbox/colorbox.css" />
<script src="<%=AppPath%>/ajax/jquery/colorbox/jquery.colorbox-min.js"></script>
<script type="text/javascript">
	$(document).ready(function () {
		$(".ArticleTree").colorbox({ width: "70%", height: "80%", iframe: true });
	});
</script>
<script type="text/javascript" src="/scripts/we7/we7.loader.js">
	$(document).ready(function(){
		we7('.popup').popup();		
	});
</script>
<div id="position">
    <asp:Literal ID="PagePathLiteral" runat="server" Text=""></asp:Literal>
</div>
<WEC:MessagePanel ID="Messages" runat="server">
</WEC:MessagePanel>
<div class="toolbar2">
    <li id="RemoveTagLi" runat="server" visible="false" class="smallButton8"></li>
    <li class="smallButton4">
        <asp:HyperLink ID="NewHyperLink" runat="server" NavigateUrl="">
            <asp:Literal ID="NewLabel" runat="server" Text="">新增文章</asp:Literal></asp:HyperLink>
    </li>
    <li class="smallButton4">
        <asp:HyperLink ID="StartHyperLink" NavigateUrl="javascript:startArticle();" runat="server">
         发布
        </asp:HyperLink>
    </li>
    <li class="smallButton4">
        <asp:HyperLink ID="StopHyperLink" NavigateUrl="javascript:stopArticle();" runat="server">
           取消发布
        </asp:HyperLink>
    </li>
    <li class="smallButton4">
        <asp:HyperLink ID="AuditToHyperLink" NavigateUrl="javascript:submitArticle();" runat="server">
          提交审核</asp:HyperLink></li>
    <li runat="server" id="MoveToSpan" class="smallButton4">
        <asp:HyperLink ID="MoveToHyperLink" title="将文章从源目录移动到目标目标" CssClass="popup" NavigateUrl="javascript:selectChannels();"
            runat="server">
           移动到...</asp:HyperLink></li>
    <li runat="server" id="CopyToSpan" class="smallButton4">
        <asp:HyperLink ID="CopyToHyperLink" title="将文章从源目录复制到目标目标，两个目录中都将存在同样的文章" CssClass="popup"
            NavigateUrl="javascript:selectChannels('article');" runat="server">
           复制到...</asp:HyperLink></li>
    <li class="smallButton4" runat="server" id="LinkToSpan">
        <asp:HyperLink ID="HyperLinkCreateRefer" NavigateUrl="javascript:selectChannels('linkto');"
            runat="server">
           发到专题...</asp:HyperLink></li>
    <li class="smallButton4">
        <asp:HyperLink ID="UpHyperLink" NavigateUrl="javascript:upArticle();" runat="server">
            置顶</asp:HyperLink></li>
    <li class="smallButton4">
        <asp:HyperLink ID="DownHyperLink" NavigateUrl="javascript:downArticle();" runat="server">
            取消置顶</asp:HyperLink></li>
    <%if (!IsNormalUser)
      { %>
    <li class="smallButton6" runat="server" id="TagSpan">
        <div id="button">
            <a href="">添加标签▼</a></div>
    </li>
    <%} %>
    <%--    <li class="smallButton6">
        <asp:HyperLink ID="WapToHyperLink" NavigateUrl="javascript:doSelectWapChannels();"
            runat="server">
           发布到wap</asp:HyperLink></li>--%>
    <li class="smallButton4" id="ShareSpan">
        <asp:HyperLink ID="ShareHyperLink" NavigateUrl="javascript:doShareArticles();" runat="server">
           共享发布</asp:HyperLink></li>
    <li class="smallButton6">
        <asp:HyperLink ID="GetShareHyperLink" NavigateUrl="javascript:doGetShareArticles();"
            runat="server">
           获取最新共享</asp:HyperLink></li>
    <%--    <li class="smallButton4">
       <asp:LinkButton ID="lnkExprot" runat="server" Text="导出示例数据" OnClick="lnkExprot_Click"></asp:LinkButton>
    </li>--%>
    <li class="smallButton4">
        <asp:HyperLink ID="DeleteHyperLink" NavigateUrl="javascript:deleteArticle();" runat="server">
          删除
        </asp:HyperLink>
    </li>
    <li class="smallButton6">
        <asp:HyperLink ID="ArticleTreeHyperLink" CssClass="ArticleTree" NavigateUrl="ArticleTree.aspx"
            runat="server" Visible="false">
          文章树关系
        </asp:HyperLink>
    </li>
    <li class="smallButton4">
        <asp:HyperLink ID="RefreshHyperLink" NavigateUrl="" runat="server">
            刷新</asp:HyperLink>
    </li>
   <!--<li>
        <asp:HyperLink ID="HyperLink1" NavigateUrl="" runat="server">
            生成静态栏目</asp:HyperLink></li>
   -->
</div>
<div style="display: table; width: 100%">
    <div class="searchForm" runat="server" id="searchDiv">
        <table class="personalForm">
            <tr>
                <td class="formTitle">
                    标题中包含：
                </td>
                <td class="formValue">
                    <asp:Literal ID="SearchFormLiteral" runat="server"></asp:Literal>
                </td>
            </tr>
        </table>
    </div>
    <ul class="subsubsub">
        <asp:Literal ID="StateLiteral" runat="server"></asp:Literal>
        <asp:Literal ID="IncludeSubLiteral" runat="server"></asp:Literal>
    </ul>
    <p class="search-box" runat="server" id="searchBox">
        <asp:Literal ID="SearchSimpleLiteral" runat="server"></asp:Literal>
    </p>
</div>
<div style="min-height: 35px; width: 100%">
    <asp:GridView ID="DataGridView" runat="server" AutoGenerateColumns="false" CssClass="List"
        GridLines="Horizontal" RowStyle-VerticalAlign="Top">
        <AlternatingRowStyle CssClass="alter" />
        <Columns>
            <asp:TemplateField>
                <HeaderStyle Width="5px" HorizontalAlign="Right" />
                <HeaderTemplate>
                    <asp:CheckBox ID="chkHeader" runat="server" onclick="javascript:SelectAll(this);"
                        AutoPostBack="false" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chkItem" runat="server" />
                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="False"></asp:Label>
                </ItemTemplate>
                <ItemStyle VerticalAlign="Top" Width="5px" HorizontalAlign="Right" />
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:HyperLink ID="OrderByTitleHyperLink" NavigateUrl="javascript:orderByTitle();"
                        runat="server"><span style="text-decoration:underline">标题</span></asp:HyperLink>
                </HeaderTemplate>
                <ItemTemplate>
                    <%# GetUrl(Eval("ID").ToString(),Convert.ToInt32(Eval("State")),Eval("Title").ToString())%>
                    <%# GetIcons(Eval("IsImage").ToString(), Eval("IsLinkArticle").ToString(), Eval("IsShow").ToString())%>
                    <%# GetActionsLink(Eval("ID").ToString(), Convert.ToInt32(Eval("State")), Eval("LinkUrl").ToString())%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="作者">
                <ItemTemplate>
                    <span title="<%# Eval("Author")%>" style="cursor: pointer">
                        <%# We7.Framework.Util.Utils.GetUnicodeSubString(Eval("Author").ToString(), 10,"") %></span>
                </ItemTemplate>
                <ItemStyle Width="60px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="标签">
                <ItemTemplate>
                    <%# Eval("Tags", "{0}")%>
                </ItemTemplate>
                <ItemStyle Width="50px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="所属栏目">
                <ItemTemplate>
                    <a href="<%=IsNormalUser?"":"../addins/" %>ArticleList.aspx?notiframe=1&oid=<%# Eval("OwnerID", "{0}")%>&type=<%# Eval("ContentType", "{0}")%>"
                        title="<%# Eval("FullChannelPath", "{0}")%>">
                        <%# Eval("ChannelName", "{0}")%></a>
                </ItemTemplate>
                <ItemStyle Width="90px" />
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:HyperLink ID="OrderByDateHyperLink" NavigateUrl="javascript:orderByDate();"
                        runat="server"><span style="text-decoration:underline">修改日期</span></asp:HyperLink>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("TimeNote", "{0}") %>' ToolTip='<%# Bind("Updated", "{0}") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="80px" Font-Size="11px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="状态">
                <ItemTemplate>
                    <%# Eval("AuditText", "{0}") %>
                </ItemTemplate>
                <ItemStyle Width="50px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="审核">
                <ItemTemplate>
                    <a href="javascript:ProcessRemark('<%#Eval("ID")%>');" onclick="return <%#GetProcessEable(Eval("ID").ToString())%> ">
                        <%# GetProcessState(Eval("ID").ToString())%></a>
                </ItemTemplate>
                <ItemStyle Width="80px" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</div>
<div class="pagination">
    <ul class="subsubsub">
        <asp:Literal ID="PageLiteral" runat="server"></asp:Literal>
    </ul>
    <WEC:URLPager ID="ArticleUPager" runat="server" UseSpacer="False" UseFirstLast="true"
        PageSize="15" FirstText="<< 首页" LastText="尾页 >>" LinkFormatActive='<span class=Current>{1}</span>'
        CssClass="Pager" />
</div>
<asp:Label ID="ChannelTypeLabel" runat="server" Text="" Visible="false">
</asp:Label>
<div id="tagsContainer">
    <asp:Literal runat="server" ID="TagsLiteral"></asp:Literal>
</div>
<div style="display: none">
    <input type="submit" id="DoNothingButton" onclick="return false" />
    <asp:Button ID="DeleteBtn" runat="server" Text="Delete" OnClientClick="return confirm('您确认要将选中的文章都删除吗？')"
        OnClick="DeleteBtn_Click" />
    <asp:Button ID="StartButton" runat="server" Text="Start" OnClick="StartButton_Click" />
    <asp:Button ID="StopButton" runat="server" Text="Stop" OnClick="StopButton_Click" />
    <asp:Button ID="MoveToButton" runat="server" Text="Move" OnClick="MoveToButton_Click" />
    <asp:Button ID="CopyToButton" runat="server" Text="Copy" OnClick="CopyToButton_Click" />
    <asp:Button ID="LinkToButton" runat="server" Text="Link" OnClick="LinkToButton_Click" />
    <asp:Button ID="UpButton" runat="server" Text="Up" OnClick="UpButton_Click" />
    <asp:Button ID="DownButton" runat="server" Text="Down" OnClick="DownButton_Click" />
    <asp:Button ID="WapToButton" runat="server" Text="WapTo" OnClick="WapToButton_Click" />
    <asp:Button ID="OrderByTitleButton" runat="server" Text="OrderByTitle" OnClick="OrderByTitleButton_Click" />
    <asp:Button ID="OrderByDateButton" runat="server" Text="OrderByDate" OnClick="OrderByDateButton_Click" />
    <asp:TextBox ID="TargetIDTextBox" runat="server"></asp:TextBox>
    <asp:TextBox ID="DeleteIDTextBox" runat="server"></asp:TextBox>
    <asp:TextBox ID="WapOidTextBox" runat="server"></asp:TextBox>
    <asp:Button ID="SubmitButton" runat="server" Text="Start" OnClick="SubmitButton_Click" />
    <asp:Button ID="ShareButton" runat="server" Text="Share" OnClick="ShareButton_Click" />
    <asp:Button ID="GetShareButton" runat="server" Text="GetShare" OnClick="GetShareButton_Click" />
    <asp:Button ID="AddTagButton" runat="server" Text="AddTag" OnClick="AddTagButton_Click" />
    <asp:Button ID="RemoveTagButton" runat="server" Text="RemoveTag" OnClick="RemoveTagButton_Click" />
    <!--<asp:Button ID="CreateHtmlButton" runat="server" Text="RemoveTag"  />-->    
</div>
