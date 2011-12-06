<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Article_wap.ascx.cs" Inherits="We7.CMS.Web.Admin.controls.Article_wap" %>
 <%@ Register TagPrefix="WEC" Namespace="We7.CMS.Controls" Assembly="We7.CMS.UI" %>

  <WEC:MessagePanel id="Messages" runat="server" ></WEC:MessagePanel>
  <DIV id=conbox>             
    <DL>
  <DT>»文章直接发布到WAP手机站点<br />
  <img src="/admin/images/bulb.gif" align="absmiddle"/> <LABEL 
  class=block_info>文章发布到wap站点，HTML格式的内容将会被转换，文章中的图片不能显示，wap文章只能使用缩略图作为插图。</LABEL> 
    <DD >                           
           <asp:HyperLink ID="EditWapHyperLink" runat="server"><font color="#aa0000">同步wap文章已建立，点击进入编辑</font></asp:HyperLink>
  
    <P></P>
    <DD class=submit>
        
        <INPUT class=Btn type=button value=同步发布到wap站点  id="PublishToWap" runat="server" onclick="javascript:doSelectChannels()" > 
    </DD></DL>
</DIV>
<div style="display: none">
    <asp:TextBox ID="WapOidTextBox" runat="server"></asp:TextBox>
    <asp:Button ID="SaveWapButton" runat="server" Text="保存Wap"  OnClick="SaveWapButton_Click"  />
</div>

   <script language="javascript" type="text/javascript">
    function doSelectChannels() {
    showDialog("/ChannelList.aspx?wap=1", OnChannelListCallback);
    }

    function OnChannelListCallback(v, t) {
   if(v) {
        document.getElementById("<%=WapOidTextBox.ClientID %>").value = v;
        document.getElementById("<%=SaveWapButton.ClientID %>").click();
        }
    }
    </script>