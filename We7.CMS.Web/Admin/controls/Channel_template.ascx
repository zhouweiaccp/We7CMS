<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Channel_template.ascx.cs" Inherits="We7.CMS.Web.Admin.controls.Channel_template" %>
  <%@ Register TagPrefix="WEC" Namespace="We7.CMS.Controls" Assembly="We7.CMS.UI" %>
<script language="javascript">
//模板:======================================================
function onSelectTemplateClick(txtTemplate) {
    var title="模式窗口";
     var nWidth="650";
     var nHeight="480";
     var strFile="TemplateList.aspx";
     var ret = window.showModalDialog(strFile,window,"dialogWidth:" + nWidth + "px;dialogHeight:" + nHeight + "px;center:yes;status:no;scroll:auto;help:no;");
      if(ret!=null) {
          var arry = new Array();
          arry = ret.split(";"); //一般返回一个字符串，用斗号分割           
          var textBox=document.getElementById(txtTemplate);
          textBox.value=arry[0];
          freshData();
      } 
}
function freshData()
{
    document.getElementById("<%= TemplateIDTextBox.ClientID%>").value =  document.getElementById("indexTemplateText").value;
    document.getElementById("<%= DetailTemplateIDTextBox.ClientID%>").value =  document.getElementById("detailTemplateText").value;
    document.getElementById("<%= ListTemplateIDTextBox.ClientID%>").value =  document.getElementById("listTemplateText").value;
    document.getElementById("<%= SearchTemplateIDTextBox.ClientID%>").value =  document.getElementById("searchTemplateText").value;
    return true;
}

function loadData()
{
    document.getElementById("indexTemplateText").value=document.getElementById("<%= TemplateIDTextBox.ClientID%>").value ;
    document.getElementById("detailTemplateText").value=document.getElementById("<%= DetailTemplateIDTextBox.ClientID%>").value   ;
    document.getElementById("listTemplateText").value=document.getElementById("<%= ListTemplateIDTextBox.ClientID%>").value   ;
    document.getElementById("searchTemplateText").value=document.getElementById("<%= SearchTemplateIDTextBox.ClientID%>").value   ;
}

</script>
<SCRIPT src="<%=AppPath%>/cgi-bin/pinyin.js" type="text/javascript"></SCRIPT> 

  <WEC:MessagePanel id="Messages" runat="server" ></WEC:MessagePanel>
  <DIV id=conbox>             
    <DL>
  <DT>»栏目的模板设置<BR>
  <img src="/admin/images/bulb.gif" align="absmiddle"/><LABEL 
  class=block_info>修改栏目具体绑定模板，不设置的话通过模板索引图自动匹配模板组的默认模板；</LABEL> 
  <DD>
    <H1>栏目模板自动匹配如下：</H1>
    <div style="font-size:14px;font-weight:lighter;line-height:150%;color:#888">
        <asp:Literal runat="server" ID="MapListLiteral"></asp:Literal>
    </div>
    <br />
    如果上述匹配不够准确的话，请重新生成模板索引地图再试试。<br />
    <asp:LinkButton runat="server" ID="CreateMapLink" 
                onclick="CreateMapLink_Click">重新生成模板索引地图</asp:LinkButton>
</DD>
<DD>
  <H1>本栏目特殊指定模板如下：</H1>
    <div>
    <table cellpadding="0" cellspacing="0" style="font-size:12px;width:500px;">
    <tr>
    <td  style="width:80px">
    栏目主页  ：</td><td style="width:80px"><input type="text" size="30" id="indexTemplateText" /> </td><td style="width:12px">
    <input type="button" value="..." onclick="onSelectTemplateClick('indexTemplateText')" /></td><td style="width:8px"><input type="checkbox" value="" runat="server" id="indexCheckbox" /></td><td>子栏目继承
    </td></tr>
    <tr  style="display:none">
    <td>
     栏目列表页：</td><td><input type="text" size="30"  id="listTemplateText" /> </td><td>
     <input type="button" value="..." onclick="onSelectTemplateClick('listTemplateText')" /></td><td><input type="checkbox" value=""  runat="server" id="listCheckbox" /></td><td>子栏目继承
    </td></tr> 
    <tr>
    <td>
     栏目详细页：</td><td><input type="text"  size="30"  id="detailTemplateText" /></td><td>
     <input type="button" value="..." onclick="onSelectTemplateClick('detailTemplateText')" /> </td><td><input type="checkbox" value=""  runat="server" id="detailCheckbox" /></td><td>子栏目继承
    </td></tr>
    <tr>  
    <td>
     栏目搜索页：</td><td><input type="text"  size="30"  id="searchTemplateText" /></td><td>
      <input type="button" value="..." onclick="onSelectTemplateClick('searchTemplateText')" /></td><td><input type="checkbox" value=""  runat="server" id="searchCheckbox" /></td><td>子栏目继承
    </td></tr>      
     </table>
</div>
</DD>
  </DL>
  <DL>
    <DD>
  <INPUT class=Btn ID="SaveButton2" runat="server"  type=submit value=修改栏目模板 onclick="return freshData()" onserverclick="SaveButton_ServerClick" > 
  </DD>
  </DL>
  <div style="display: none">
            <asp:TextBox ID="ParentIDTextBox" runat="server"></asp:TextBox>
            <asp:TextBox ID="ChannelNameHidden" runat="server"></asp:TextBox>
            <asp:TextBox ID="TemplateIDTextBox" runat="server"></asp:TextBox>
            <asp:TextBox ID="DetailTemplateIDTextBox" runat="server"></asp:TextBox>
               <asp:TextBox ID="ListTemplateIDTextBox" runat="server"></asp:TextBox>
            <asp:TextBox ID="SearchTemplateIDTextBox" runat="server"></asp:TextBox>
    </div> 
</DIV>
<script type="text/javascript">
loadData();
</script>
