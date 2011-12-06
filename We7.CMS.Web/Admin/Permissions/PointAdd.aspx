<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PointAdd.aspx.cs"  
    Inherits="We7.CMS.Web.User.PointAdd" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WEC" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <style>
        th, td
        {
            font-size: 12px;
            font-family: 宋体 Arial;
        }
        th
        {
            width: 100px;
            background: #f0f0f0;
            text-align: right;
        }
    </style>
   
  <h2   class="title">
    用户 <%=ThisAccount.LastName%> 积分：<%=ThisAccount.Point%>  
</h2>
<div>
    <WEC:MessagePanel ID="Messages" runat="server">
    </WEC:MessagePanel>
</div>
 <div id="mycontent">
    <table style="border-collapse: collapse" border="1" cellpadding="2">
        <tr>
            <th>
                <%=ActionText%>分数：
            </th>
            <td>
                <asp:TextBox ID="PointValueTextBox" runat="server" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>
                备注：
            </th>
            <td>
                <asp:TextBox TextMode="MultiLine" ID="DescTextBox" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th>
            </th>
            <td>
                <asp:Button runat="server" ID="bttnSave" Text="执行" OnClick="bttnSave_Click" />
                <asp:Button runat="server" ID="bttnReset" Text="返回" OnClick="bttnReset_Click" />
                &nbsp;&nbsp;<asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
  </div>
</asp:Content>
