<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SimpleList.ascx.cs"
    Inherits="CModel.Container.system.SimpleList" %>
<asp:GridView ID="gvList" runat="server" AutoGenerateColumns="false" OnRowEditing="gvList_RowEditing"
    OnRowDeleting="gvList_RowDeleting" CssClass="table_list" CellPadding="0" Caption="信息管理"
    CellSpacing="0" ShowHeader="true" EmptyDataText="列表数据为空" 
    OnRowDataBound="gvList_RowDataBound" onrowcommand="gvList_RowCommand">
    <Columns>
        <asp:TemplateField>
            <ItemStyle Width="25" HorizontalAlign="Center" />
            <HeaderStyle Width="25" HorizontalAlign="Center" />
            <HeaderTemplate>
                <asp:CheckBox ID="chkID" runat="server" onclick="$('input[type=checkbox]').attr('checked', this.checked)" />
                <%--<input type="checkbox" onclick="$('input[type=checkbox]').attr('checked', this.checked)" />--%>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="chkID" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
