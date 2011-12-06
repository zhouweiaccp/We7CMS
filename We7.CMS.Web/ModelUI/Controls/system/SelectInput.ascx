<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectInput.ascx.cs"
    Inherits="We7.Model.UI.Controls.system.SelectInput" %>

<script language='javascript'>
       //给上一标签赋值
      function add_select(str,id)
       {
              get_previoussibling(id).value=str; 
       }
       //取当前标签上一标签
      function get_previoussibling(n)   
       {   
         var x=n.previousSibling;   
         while (x.nodeType!=1)   
            {   
           x=x.previousSibling;   
            }   
          return x;   
       } 
       //弹出更多对话框
        function morselect<%=TxtInput.ClientID%>()
        {
           window.showModelessDialog("<%=DialogPath%>?MatchupNode=<%=Server.UrlEncode(MatchupNode)%>&ClientID=<%=Server.UrlEncode(TxtInput.ClientID)%>&SelectValue="+escape(document.getElementById("<%=TxtInput.ClientID%>").value)+"",window,"scroll:0;status:1;help:1;resizable:1;dialogWidth:500px;dialogHeight:300px");
        }
</script>

<div>
    <asp:TextBox runat="server" ID="TxtInput" onblur="this.className='input_blur'" onfous="this.className='input_focus'" style="vertical-align:middle"></asp:TextBox>
    <asp:DropDownList ID="DrPListSelect" runat="server" onchange="add_select(this.value,this)" style="vertical-align:middle">
    </asp:DropDownList >
    <a href="javascript: morselect<%=TxtInput.ClientID%>()">更多>></a>
</div>
