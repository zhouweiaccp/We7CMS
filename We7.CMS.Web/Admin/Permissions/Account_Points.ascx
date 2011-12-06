<%@ Control Language="C#" AutoEventWireup="true" Codebehind="Account_Points.ascx.cs"
    Inherits="We7.CMS.Web.Admin.Permissions.Account_Points" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WEC" %>
    <style>
        h2
        {
        	font-size:14px;
           font-weight:normal;
        }
    </style>
<div>
    <WEC:MessagePanel ID="Messages" runat="server">
    </WEC:MessagePanel>
</div>
<div id="conbox">
    <dl>
        <dt>» 用户现有积分：<%=ThisAccount.Point%><br />
            <img src="/admin/images/bulb.gif" align="absmiddle" alt="" />
            <label class="block_info">
               操作： <a href="javascript:weShowModelDialog('PointAdd.aspx?action=0&id=<%=CurrentAccountID %>')"  >奖励积分</a> | <a href="javascript:weShowModelDialog('PointAdd.aspx?action=1&id=<%=CurrentAccountID %>')" >扣除积分</a></label>
        </dt>
        <dd>
   <asp:GridView ID="gvList" runat="server" AllowPaging="true" PageSize="10" AutoGenerateColumns="false"
        Width="100%" OnPageIndexChanging="gvList_PageIndexChanging" DataKeyNames="ID"
        OnRowDeleting="gvList_RowDeleting" ShowHeader="true">
        <HeaderStyle Font-Size="14px" BackColor="#f0f0f0" Height="25px" />
        <Columns>
            <asp:BoundField DataField="Created"  HeaderText="时间" />
            <asp:BoundField DataField="ActionText"  HeaderText="支出/收入" />
            <asp:BoundField DataField="Value"  HeaderText="数值" />
            <asp:BoundField DataField="Description"  HeaderText="描述" />
            <asp:TemplateField HeaderText="操作">
                <ItemStyle Width="100px" HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:LinkButton ID="lbAdd" CommandName="Delete" runat="server">删除</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerTemplate>
            <div style="float: right">
                第<asp:Label ID="lblPageIndex" runat="server" Text='<%# ((GridView)Container.Parent.Parent).PageIndex + 1  %>' />页
                共/<asp:Label ID="lblPageCount" runat="server" Text='<%# ((GridView)Container.Parent.Parent).PageCount  %>' />页
                <asp:LinkButton ID="btnFirst" runat="server" CausesValidation="False" CommandArgument="First"
                    CommandName="Page" Text="首页" />
                <asp:LinkButton ID="btnPrev" runat="server" CausesValidation="False" CommandArgument="Prev"
                    CommandName="Page" Text="上一页" />
                <asp:LinkButton ID="btnNext" runat="server" CausesValidation="False" CommandArgument="Next"
                    CommandName="Page" Text="下一页" />
                <asp:LinkButton ID="btnLast" runat="server" CausesValidation="False" CommandArgument="Last"
                    CommandName="Page" Text="尾页" />
            </div>
        </PagerTemplate>
    </asp:GridView>
     </dd>
    </dl>
</div>