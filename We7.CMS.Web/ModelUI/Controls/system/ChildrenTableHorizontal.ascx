<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChildrenTableHorizontal.ascx.cs"
    Inherits="We7.Model.UI.Controls.system.ChildrenTableHorizontal" Debug="true" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<div>
    <asp:UpdatePanel runat="server" ID="updatepanel">
        <ContentTemplate>
            <div class="list">
                <asp:GridView ID="gvList" runat="server" CssClass="List" OnRowEditing="gvList_RowEditing"
                    OnRowDeleting="gvList_RowDeleting" DataKeyNames="ID" AutoGenerateColumns="false"
                    OnRowDataBound="gvList_DataBound">
                </asp:GridView>
            </div>
            <div>
                <asp:Button ID="btnToggle" OnClick="btnToggle_Click" runat="server" Text="展开编辑项" /></div>
                <div id="divCt" runat="server" style="display:none">
                    <table>
                        <asp:Repeater ID="rptEditor" runat="server" OnItemDataBound="rptEditor_ItemDataBound">
                        </asp:Repeater>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="btnSave" Text="添加" OnClick="btnSave_Click" runat="server" />
                                <asp:Button ID="btnReset" Text="清空" OnClick="btnReset_Click" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
            <asp:HiddenField ID="xmldata" runat="server" />
            <asp:HiddenField ID="hfID" runat="server" />
            <asp:HiddenField ID="hfPanelVisible" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
