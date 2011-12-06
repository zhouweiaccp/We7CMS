<%@ Page Language="C#" MasterPageFile="~/admin/theme/classic/content.Master" AutoEventWireup="true"
    CodeBehind="AdviceTracking.aspx.cs" Inherits="We7.CMS.Web.Admin.AdviceTracking"
    Title="Untitled Page" %>

<%@ Register TagPrefix="WEC" Namespace="We7.CMS.Controls" Assembly="We7.CMS.UI" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <script type="text/javascript" language="javascript" src="/Scripts/we7/we7.loader.js">
    we7("#<%=StartTimeText.ClientID %>").pickDate();
    we7("#<%=EndTimeText.ClientID %>").pickDate();
    </script>

    <script type="text/javascript">
        function deleteCompanyInfo() {
            if (confirm('您确认要将此信息删除吗？此操作不可恢复，请确认！')) {
                var DeleteBtn = document.getElementById("<%=DeleteBtn.ClientID %>");
                DeleteBtn.click();
            }
        }
    </script>

    <h2 class="title">
        <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/logo_admin.gif" />
        <asp:Label ID="NameLabel" runat="server" Text="系统操作日志"></asp:Label>
        <span class="summary">
            <asp:Label ID="SummaryLabel" runat="server" Text="">系统操作日志的详细信息，在此可进行删除、查询。
            </asp:Label>
        </span>
    </h2>
    <div class="toolbar">
        <asp:HyperLink ID="DeleteHyperLink" NavigateUrl="javascript:deleteCompanyInfo();"
            runat="server">
            <asp:Image ID="DeleteImage" runat="server" ImageUrl="~/admin/Images/icon_refresh.gif" />
            删除</asp:HyperLink>
    </div>
    <div>
        操作类型:
        <asp:TextBox ID="TypeTextBox" runat="server"></asp:TextBox>
        操作人:
        <asp:TextBox ID="ProcessTextBox" runat="server"></asp:TextBox>
        操作时间：从<input id="StartTimeText" value="请点击选择日期" runat="server" type="text" 
            readonly="readOnly" style="width: 170px" />
        到<input id="EndTimeText" runat="server" type="text" value="请点击选择日期"
            readonly="readOnly" style="width: 170px" />
            <asp:Button ID="SeleteButton" runat="server" Text="查询" OnClick="SeleteButton_Click" />
    </div>
    <WEC:MessagePanel runat="Server" ID="Messages">
    </WEC:MessagePanel>
    <table id="personalForm" cellpadding="0" cellspacing="0">
        <asp:GridView ID="DataGridView" runat="server" AutoGenerateColumns="False" ShowFooter="True"
            CssClass="List">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkHeader" runat="server" OnClick="javascript:SelectAll(this);"
                            AutoPostBack="false" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkItem" runat="server" />
                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("AccountID") %>' Visible="False"></asp:Label>
                    </ItemTemplate>
                    <ControlStyle Width="30px" />
                    <ItemStyle Width="30px" />
                </asp:TemplateField>
                <asp:HyperLinkField DataNavigateUrlFields="ID" HeaderText="操作描述" DataNavigateUrlFormatString=""
                    DataTextField="ProcessDirection" DataTextFormatString="{0}" Target="_blank" />
                <asp:BoundField HeaderText="操作时间" DataField="UpdateDate"></asp:BoundField>
                <asp:TemplateField HeaderText="操作人">
                    <ItemTemplate>
                        <%# GetProcessNameByAccountID(Eval("ProcessAccountID").ToString())%>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作类型">
                    <ItemTemplate>
                        <%# GetTypeByID(Eval("ID").ToString())%>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作IP">
                    <ItemTemplate>
                       <%# GetUserIPByAccountID(Eval("AccountID").ToString())%>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </table>
    <div class="pagination">
        <p>
            <WEC:Pager ID="CompanyPager" PageSize="15" PageIndex="0" runat="server" OnFired="Pager_Fired" />
        </p>
    </div>
    <div style="display: none">
      <asp:Button ID="DeleteBtn" runat="server" Text="Delete" OnClick="DeleteBtn_Click" />
<%--          <asp:Button ID="StopToCommendBtn" runat="server" Text="OutPut" OnClick="StopToCommendBtn_Click" />
--%>        <asp:TextBox ID="ItemTextBox" runat="server"></asp:TextBox>
    </div>
</asp:Content>
