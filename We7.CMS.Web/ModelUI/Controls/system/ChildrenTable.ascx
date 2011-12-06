<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChildrenTable.ascx.cs"
    Inherits="We7.Model.UI.Controls.system.ChildrenTable" Debug="true" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<div>
  <asp:UpdatePanel runat="server" ID="updatepanel">
        <ContentTemplate>
            <div class="list">
                <asp:GridView ID="gvList" runat="server" CssClass="List"
                     OnRowEditing="gvList_RowEditing" OnRowDeleting="gvList_RowDeleting" DataKeyNames="ID" AutoGenerateColumns="false" OnRowDataBound="gvList_DataBound" >
                    <%--<Columns>
                        <asp:CommandField EditText="编辑" DeleteText="删除" ButtonType="Link" ShowEditButton="true" ShowDeleteButton="true" />
                    </Columns>--%>
                </asp:GridView>
            </div>
            <div>
                <table>
                    <tr>
                        <asp:Repeater ID="rptEditorHeader" runat="server" OnItemDataBound="rptEditorHeader_ItemDataBound">
                        </asp:Repeater>
                        <td align="center">管理</td>
                    </tr>
                    <tr>
                        <asp:Repeater ID="rptEditor" runat="server" OnItemDataBound="rptEditor_ItemDataBound">
                        </asp:Repeater>
                        <td>
                            <asp:Button ID="btnSave" Text="添加" OnClick="btnSave_Click" runat="server" />
                            <asp:Button ID="btnReset" Text="清空" OnClick="btnReset_Click" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="xmldata" runat="server" />
            <asp:HiddenField ID="hfID" runat="server" />
       </ContentTemplate>
    </asp:UpdatePanel>
</div>
