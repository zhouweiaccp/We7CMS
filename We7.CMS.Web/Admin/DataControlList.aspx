<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataControlList.aspx.cs"
    Inherits="We7.CMS.Web.Admin.DataControlList" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WEC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>选择一个数据控件</title>
    <base target="_self" />
    <link rel="stylesheet" type="text/css" href="theme/classic/css/main.css" media="screen" />
	<meta content="noindex, nofollow" name="robots" />
	<script src="/fckeditor/editor/dialog/common/fck_dialog_common.js" type="text/javascript"></script>
	<script type="text/javascript">
var dialog	= window.parent ;
var oEditor = dialog.InnerDialogLoaded() ;

// Gets the document DOM
var oDOM = oEditor.FCK.EditorDocument ;
var oActiveEl = dialog.Selection.GetSelectedElement() ;

	</script>
	
    <script src="<%=AppPath%>/cgi-bin/DialogHelper.js" type="text/javascript"></script>
     <script src="<%=AppPath%>/cgi-bin/search.js" type="text/javascript"></script>

    <style type="text/css">
        #site-mode
        {
            position: absolute;
            right: 0px;
            top: 0;
            width: 70px;
            height: 31px;
        }

    </style>

    <script type="text/javascript">

        function onSelectThis(v)
        {
             var tempFile=QueryStringFromUrl("file",top.location.search);
             var url= "DataControlBuilder.aspx?isFirst=1&file=" +  v+"&template=" + tempFile;
             window.location=url;
        }

        function onDocumentLoad() {
//           dialog.SetOkButton( true ) ;
        }

        function onModeTypeClick(stylelist) {
            if (stylelist == "list") {
                ListControl.style.display = "";
                ImageControl.style.display = "none";
            }
            if (stylelist == "icon") {
                ListControl.style.display = "none";
                ImageControl.style.display = "";
            }
        }
    </script>

</head>
<body onload="onDocumentLoad()"  >
    <form id="mainForm" runat="server">
        <div>
            <span>快速搜索：</span>
            <asp:DropDownList ID="FieldDropDownList" runat="server">
                <asp:ListItem Value="Name">按关键字查找</asp:ListItem>
                <asp:ListItem Value="FileName">按文件名查找</asp:ListItem>
            </asp:DropDownList>
            <asp:TextBox ID="SearchTextBox" runat="server" Columns="20" MaxLength="64"></asp:TextBox>
            <asp:HyperLink ID="SearchHyperLink" runat="server" NavigateUrl="javascript:document.mainForm.QueryButton.click();">
                <asp:Image ID="SearchImage1" runat="server" ImageUrl="~/admin/Images/icon_search.gif" />
                执行</asp:HyperLink>
        </div>
        <br />
        <div id="sortHyperLink">
            <span>按类型分类：</span>
            <asp:HyperLink ID="ArticleHyperLink" runat="server" NavigateUrl="javascript:document.mainForm.ArticleButton.click();"
                CssClass="hypelink">文章相关控件</asp:HyperLink>
            <asp:HyperLink ID="ChannelHyperLink" runat="server" NavigateUrl="javascript:document.mainForm.ChannelButton.click();"
                CssClass="hypelink">栏目相关控件</asp:HyperLink>
            <asp:HyperLink ID="ImgHyperLink" runat="server" NavigateUrl="javascript:document.mainForm.ImgButton.click();"
                CssClass="hypelink">图片相关控件</asp:HyperLink>
            <asp:HyperLink ID="ListLink" runat="server" NavigateUrl="javascript:document.mainForm.ListButton.click();"
                CssClass="hypelink">列表相关控件</asp:HyperLink>
            <asp:HyperLink ID="MenuLink" runat="server" NavigateUrl="javascript:document.mainForm.MenuButton.click();"
                CssClass="hypelink">菜单相关控件</asp:HyperLink>
            <asp:HyperLink ID="AdHyperLink" runat="server" NavigateUrl="javascript:document.mainForm.AdButton.click();"
                CssClass="hypelink">广告相关控件</asp:HyperLink>
            <asp:HyperLink ID="LoginHyperLink" runat="server" NavigateUrl="javascript:document.mainForm.LoginButton.click();"
                CssClass="hypelink">登陆相关控件</asp:HyperLink>
            <asp:HyperLink ID="StoreHyperLink" runat="server" NavigateUrl="javascript:document.mainForm.StoreButton.click();"
                CssClass="hypelink">商铺控件</asp:HyperLink>
            <asp:HyperLink ID="OtherHyperLink" runat="server" NavigateUrl="javascript:document.mainForm.OtherButton.click();"
                CssClass="hypelink">其他控件</asp:HyperLink>
        </div>
        <br />
        <div id="messageLayer">
            <asp:Image ID="MessageImage" runat="server" ImageUrl="~/admin/Images/icon_info.gif" />
            <asp:Label ID="MessageLabel" runat="server" Text="">
            </asp:Label>
        </div>
        <br />
 
       <asp:DataList ID="ModeDataList" runat="server" AutoGenerateColumns="False" ShowFooter="True">
                <ItemTemplate>
                    <div style="width: 100%;padding:10px;">
                        <div style="float: left; ">
                          <a href="javascript:onSelectThis('<%#DataBinder.Eval(Container.DataItem, "FileName") %>');"> <img alt="<%#DataBinder.Eval(Container.DataItem, "Description") %>" id="ModeImage" src="/cgi-bin/templates/controls/CDControlImage/<%#Eval("DemoUrl")%>"
                                title="点击选择控件 <%#DataBinder.Eval(Container.DataItem, "Description") %>"  /></a>
                        </div>
                        <div style="margin-left:210px">
                            <div >
                            <h2>
                                 <a href="javascript:onSelectThis('<%#DataBinder.Eval(Container.DataItem, "FileName") %>');"> 
                                 <%#Eval("Description")%></a>
                                   </h2>                            
                            </div>
                            <div>
                               文件：<%#Eval("FileName")%><br />
                                作者：<%#Eval("Author")%><br />
                                创建时间：<%#Eval("Created")%>
                            </div>
                              <div style="font-weight:lighter">
                              描述：
                                <%#Eval("Remark")%>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:DataList>

        <div id="ListControl" style="display: none;" runat="server">
                        <asp:GridView ID="DetailGridView" runat="server" AutoGenerateColumns="False" ShowFooter="True" Visible="false">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle Width="10px" />
                                    <ItemTemplate>
                                        <input type="radio" name="Radios" value="<%#DataBinder.Eval(Container.DataItem, "FileName") %>"
                                            title="<%#DataBinder.Eval(Container.DataItem, "Name") %>" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Description" HeaderText="描述" />
                                <asp:BoundField DataField="FileName" HeaderText="文件" />
                                <asp:BoundField DataField="Author" HeaderText="作者" />
                                <asp:BoundField DataField="Created" HeaderText="创建时间" />
                            </Columns>
                        </asp:GridView>
   </div>
        <br />
        <div style="display: none">
            <asp:Button ID="QueryButton" runat="server" Text="Query" OnClick="QueryButton_Click" />
            <asp:Button ID="ArticleButton" runat="server" Text="Sort" OnClick="ArticleButton_Click" />
            <asp:Button ID="ChannelButton" runat="server" Text="Sort" OnClick="ChannelButton_Click" />
            <asp:Button ID="ListButton" runat="server" Text="Sort" OnClick="ListButton_Click" />
            <asp:Button ID="MenuButton" runat="server" Text="Sort" OnClick="MenuButton_Click" />
            <asp:Button ID="ImgButton" runat="server" Text="Sort" OnClick="ImgButton_Click" />
            <asp:Button ID="AdButton" runat="server" Text="Sort" OnClick="AdButton_Click" />
            <asp:Button ID="LoginButton" runat="server" Text="Sort" OnClick="LoginButton_Click" />
            <asp:Button ID="StoreButton" runat="server" Text="Sort" OnClick="StoreButton_Click" />
            <asp:Button ID="OtherButton" runat="server" Text="Sort" OnClick="OtherButton_Click" />
            <asp:TextBox ID="IDTextBox" runat="server" Text=""></asp:TextBox>
        </div>
    </form>
</body>
</html>
