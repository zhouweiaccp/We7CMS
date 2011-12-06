<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Article_file.ascx.cs" Inherits="We7.CMS.Web.Admin.controls.Article_file" %>
 <%@ Register TagPrefix="WEC" Namespace="We7.CMS.Controls" Assembly="We7.CMS.UI" %>
  <WEC:MessagePanel id="Messages" runat="server" ></WEC:MessagePanel>
  <DIV id=conbox>             
    <DL>
  <DT>»文章附件<br />
  <img src="/admin/images/bulb.gif" align="absmiddle"/> <LABEL 
  class=block_info>文章附件主要可在列表或文章内容显示里呈现，用于附件及资源下载。</LABEL> 
    <DD >   

    <asp:GridView ID="DataGridView" CssClass="InBox" runat="server" AutoGenerateColumns="False"
        ShowFooter="false"  GridLines="Horizontal" >
        <Columns>
            <asp:HyperLinkField DataNavigateUrlFields="FileName,FilePath" DataNavigateUrlFormatString="{1}/{0}"
                Target="_blank" DataTextField="FileName" DataTextFormatString="{0}" HeaderText="附件名称" />
            <asp:BoundField DataField="FileType" DataFormatString="{0}" HeaderText="附件类型" />
            <asp:BoundField DataField="UploadDate" DataFormatString="{0}" HeaderText="上传日期" />
            <asp:TemplateField>
                <ItemTemplate>
                    <a href="javascript:deleteAttachment('<%# DataBinder.Eval(Container.DataItem, "ID")  %>')">
                        删除</a>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>  &nbsp;<dd>
      <P></P>
      <br />
      <DT>»单文件上传<br />
  <DL>
     <DD>
        <asp:FileUpload ID="AttachmentFileUpload" runat="server" />
    <DD class=submit>
        <INPUT class=Btn type=submit  value=上传附件  id="AttachmentUpload" runat="server" onserverclick="AttachmentUpload_ServerClick"  > 
    </DD></DL>
        </dd>
</DIV>

<div style="display: none">
        <asp:Button ID="DeleteAttachmentButton" runat="server" OnClick="DeleteAttachmentButton_Click" />
        <asp:TextBox ID="AttachmentIDTextBox" runat="server"></asp:TextBox>
</div>

<script type="text/javascript">    
function deleteAttachment(aid)
{
    if(confirm('确认删除吗？'))
    { 
        document.getElementById("<%=AttachmentIDTextBox.ClientID %>").value = aid;
        document.getElementById("<%=DeleteAttachmentButton.ClientID %>").click();
    }  
}  
</script>