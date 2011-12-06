<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="We7.CMS.Web.Plugin.FullTextSearch_UI_index" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WEC" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="/admin/Images/icons_img.gif" />
        <asp:Label ID="NameLabel" runat="server" Text="全文检索设置">
        </asp:Label>
        <span class="summary"><span id="navChannelSpan"></span>
            <asp:Label ID="SummaryLabel" runat="server" Text="设置本站全文检索相关参数">
            </asp:Label>
        </span>
    </h2>
    <WEC:MessagePanel ID="Messages" runat="server">
    </WEC:MessagePanel>
    
     <table style="border: solid 0px #fff;">
        <tr >
            <td align="right">
                站群搜索引擎服务器访问接口：
            </td>
            <td>
                <asp:TextBox ID="SEUrlTextBox" runat="server" Columns="60"></asp:TextBox><input type="button" value="默认值" onclick="initDefaultValue()" />
            </td>
        </tr>
      <%--   <tr >
            <td align="right">
            <label for="InitIndexDB">
                同时初始化索引数据库：</label>
            </td>
            <td>
                <asp:CheckBox id="InitIndexDB" name="InitIndexDB" runat="server" ></asp:CheckBox>
            </td>
        </tr>--%>
    </table>
    <p>
    <asp:Button ID="SaveButton" runat="server" Text="保存" OnClick="SaveButton_Click"  CssClass="button" Width="74px" />
    <asp:Button ID="InitDBButton" runat="server" Text="对已有数据全部重建索引" OnClick="InitButton_Click"  CssClass="button" Width="174px" />
    </p>
    <script type="text/javascript">
    function initDefaultValue()
    {
        var url=document.getElementById("<%=SEUrlTextBox.ClientID %>");
        if(url)
        {
            url.value="tcp://localhost:11001";
        }
    }
    </script>
</asp:Content>
