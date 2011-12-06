<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendEmailDetail.aspx.cs"
    Inherits="We7.CMS.Web.Admin.SendEmailDetail" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WEC" %>
<%@ Register Assembly="FCKeditor.net" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<asp:content id="We7Content" contentplaceholderid="MyContentPlaceHolder" runat="server">

<script type="text/javascript">

    </script>
  <h2 class="title">                  
      <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_comment.gif" />
      <asp:Label ID="NameLabel" runat="server" Text="系统未能自动处理邮件详细信息">
      </asp:Label>
      <span class="summary">
          <asp:Label ID="SummaryLabel" runat="server" Text="">
          </asp:Label>
      </span>
  </h2>
      <div id="position">
        <asp:Literal ID="PagePathLiteral" runat="server"> </asp:Literal>
    </div>
    <br />
        <div class="toolbar">
        </div>
        
<wec:messagepanel id="Messages" runat="server">
</wec:messagepanel>

        <br />
        <div>
            <asp:panel id="CtrlContainer" runat="server">
            </asp:panel>
        </div>
        <table>
            <tr>
                <td>
                   <b>收件人：</b><asp:Label id="UserLabel" runat="server" ></asp:Label>
                </td>

            </tr>
            <tr>
                <td>
                   <b>发件人：</b><asp:Label id="FormUserLabel" runat="server" ></asp:Label>
                </td>

            </tr>            
            <tr>
               <td>
                   <b> 主题：</b> <asp:Label id="EmailTitleLabel" runat="server" ></asp:Label>
               </td>

            </tr>
            <tr>
               <td>
                   <b> 发件时间：</b> <asp:Label id="EmailTimeLabel" runat="server" ></asp:Label>
               </td>

            </tr>            
            <tr>
                <td >
                    <b>回复内容：</b><br />
                        <div id="ReplayContent" runat="server" style="margin-left:40px; width:500px">
                        </div>
                </td>
           </tr>
        </table>
      
   <div style="display: none">
       <asp:TextBox ID="TitleTextBox" runat="server"></asp:TextBox>
       <div id="InfoRawManage" runat="server"></div>
   </div>
</asp:content>
