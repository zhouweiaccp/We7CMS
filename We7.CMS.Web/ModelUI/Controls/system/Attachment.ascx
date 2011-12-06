<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Attachment.ascx.cs"
    Inherits="CModel.Controls.system.Attachment" %>

<script language="javascript">
     function check_upwttach()
   {
     var strFileName=document.getElementById("<%=AttachUpload.ClientID%>").value;
     if (strFileName=="")
     {
      alert("请选择要上传的文件");
      return false;
      }else
      {
      return true;
      }
    }
</script>
<div>
    <asp:FileUpload ID="AttachUpload" runat="server" /><asp:Button ID="AttachUploadButton"
        runat="server" Text="上传" OnClick="AttachUploadButton_Click" OnClientClick=" return check_upwttach()"
        CausesValidation="false" />
</div>
<div style="margin-top: 0px">
    <table style="width: 22%">
        <asp:Repeater ID="ReptImgList" runat="server" OnItemCommand="BtnDel_Click">
            <ItemTemplate>
                <tr>
                    <td align="right">
                        <%#Container.DataItem%>
                    </td>
                    <td align="left">
                        <asp:ImageButton ID="BtnDel" runat="server" CommandName="<%#Container.DataItem%>"
                            ImageUrl="~/modelui/skin/images/close.gif" ToolTip="删除" align="left" CausesValidation="false" />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    <div style="display: none">
        <asp:TextBox ID="TxtPath" runat="server" Width="200" onblur="this.className='input_blur'"
            onfous="this.className='input_focus'"></asp:TextBox></div>
</div>
