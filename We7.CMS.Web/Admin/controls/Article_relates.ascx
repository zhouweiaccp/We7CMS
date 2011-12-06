<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Article_relates.ascx.cs" Inherits="We7.CMS.Web.Admin.controls.Article_relates" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WEC" %>
<script src="<%=AppPath%>/cgi-bin/DialogHelper.js" type="text/javascript"></script>
  <WEC:MessagePanel id="Messages" runat="server" ></WEC:MessagePanel>
  <DIV id=conbox>             
    <DL>
  <DT>»相关文章<br />
  <img src="/admin/images/bulb.gif" align="absmiddle"/><LABEL 
  class=block_info>相关文章是提供给您手动建立文章相关关系的操作，可以在所有文章列表中通过查询关键字或直接使用标签匹配。</LABEL> 

   <DD>
   <H1>列表</H1> 
        <asp:GridView ID="DataGridView" runat="server" AutoGenerateColumns="False"
            ShowFooter="false" CssClass="InBox">
            <Columns>
                <asp:HyperLinkField DataNavigateUrlFields="LinkUrl" DataNavigateUrlFormatString="{0}"
                    Target="_blank" DataTextField="Title" DataTextFormatString="{0}" HeaderText="标题" />
                <asp:BoundField DataField="FullChannelPath" DataFormatString="{0}" HeaderText="所属栏目" />
                <asp:BoundField DataField="Updated" DataFormatString="{0}" HeaderText="日期" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <a href="javascript:deleteRelatedArticle('<%# DataBinder.Eval(Container.DataItem, "ID")  %>')">
                            删除</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

    <DD>
     <H1>添加</H1> 
       <INPUT class=Btn type=button value=浏览文章…  id="UploadImage" onclick="javascript:openAddWindow();" />

</DD></DL>

</DIV>
<div style="display: none">
    &nbsp;<asp:Button ID="DeleteRelatedArticleButton" runat="server" OnClick="DeleteRelatedArticleButton_Click" />
    <asp:TextBox ID="RelatedArticleIDTextBox" runat="server"></asp:TextBox>
    <asp:Button ID="RefreshButton" runat="server" Text="Refresh" OnClick="RefreshButton_Click" />
</div>
        
<script type="text/javascript">    
  function deleteRelatedArticle(aid)
    {
            if(confirm('确认删除吗？'))
            { 
                document.getElementById("<%=RelatedArticleIDTextBox.ClientID %>").value = aid;
                document.getElementById("<%=DeleteRelatedArticleButton.ClientID %>").click();
            }  
    }  

  function openAddWindow()
  {
     var title="模式窗口";
     var nWidth="650";
     var nHeight="480";
     var strFile="ArticleRelatesAdd.aspx?id=<%=ArticleID %>";
     var ret = window.weShowModelDialog(strFile, onAddCallback);
 }
 function onAddCallback(v, t) {
     var button = document.getElementById("<%=RefreshButton.ClientID %>");
     button.click();
 } 
</script>