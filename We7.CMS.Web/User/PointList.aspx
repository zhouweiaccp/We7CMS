<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PointList.aspx.cs" MasterPageFile="~/User/DefaultMaster/content.Master"
    Inherits="We7.CMS.Web.User.PointList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MyHeadPlaceHolder" runat="server">
    <style>
        td
        {
            font-size: 12px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <form runat="server" id="form1">
    <div class="realRight ml10">
        <div class="mybox">
            <div class="mytit">
                积分明细</div>
            <div class="con">
                <h2 class="title">
                    用户
                    <%=ThisAccount.LastName%>
                    积分：<%=ThisAccount.Point%>
                </h2>
                <asp:GridView ID="gvList" runat="server" AllowPaging="true" PageSize="10" AutoGenerateColumns="false"
                    Width="100%" OnPageIndexChanging="gvList_PageIndexChanging" DataKeyNames="ID"
                    OnRowDeleting="gvList_RowDeleting" ShowHeader="true">
                    <HeaderStyle Font-Size="14px" BackColor="#f0f0f0" Height="25px" />
                    <Columns>
                        <asp:BoundField DataField="Created" HeaderText="时间" />
                        <asp:BoundField DataField="ActionText" HeaderText="支出/收入" />
                        <asp:BoundField DataField="Value" HeaderText="数值" />
                        <asp:BoundField DataField="Description" HeaderText="描述" />
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
            </div>
        </div>
    </div>
    </form>
</asp:Content>
