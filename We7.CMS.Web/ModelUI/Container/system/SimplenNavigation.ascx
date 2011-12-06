<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SimplenNavigation.ascx.cs"
    Inherits="We7.Model.UI.Container.system.SimplenNavigation" %>
<table cellpadding="0" cellspacing="1" class="table_form">
    <caption>
        <%=PanelContext.Model.Desc %>
        栏目管理</caption>
    <tr>
        <td>
            <asp:LinkButton ID="lnkPublish" CausesValidation="false" CommandName="publish" runat="server">发布信息</asp:LinkButton>
            |
            <asp:LinkButton ID="lnkList" CausesValidation="false" CommandName="manage" ForeColor="Red"
                runat="server">管理</asp:LinkButton>
        </td>
    </tr>
</table>
