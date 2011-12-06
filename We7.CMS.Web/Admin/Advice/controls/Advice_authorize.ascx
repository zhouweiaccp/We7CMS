<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Advice_Authorize.ascx.cs"
    Inherits="We7.CMS.Web.Admin.controls.Advice_Authorize" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WEC" %>
<div>
    <WEC:MessagePanel id="Messages" runat="server">
    </WEC:MessagePanel>
</div>
<div id="conbox">
    <dl>
        <dt>»模型的权限设置<br>
            <img src="/admin/images/bulb.gif" align="absmiddle" /><label class="block_info">设置角色与用户对该模型各环节的处理权限；注意：打勾后需要点击“更新模型权限”才可以更新设置。</label>&nbsp;
            <dd>
                <h1>
                    角色</h1>
                <asp:GridView BorderWidth="0" ID="RolesGridView" runat="server" EnableViewState="true"  CssClass="List" GridLines="Horizontal"
                    AutoGenerateColumns="false" Width="98%" OnRowDataBound="RolesGridView_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="0">
                            <ItemTemplate>
                                <input type="hidden" id="IDHidden" runat="server" value='<%# Eval("ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="角色名称" DataField="Name" />
                        <asp:TemplateField HeaderText="查看">
                            <ItemTemplate>
                                <asp:CheckBox ID="AdviceReadCheckbox" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="管理">
                            <ItemTemplate>
                                <asp:CheckBox ID="AdviceAdminCheckbox" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="受理">
                            <ItemTemplate>
                                <asp:CheckBox ID="AdviceAcceptCheckbox" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="办理">
                            <ItemTemplate>
                                <asp:CheckBox ID="AdviceHandleCheckbox" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="审核">
                            <ItemTemplate>
                                <asp:CheckBox ID="AdviceFirstAuditCheckBox" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="二审">
                            <ItemTemplate>
                                <asp:CheckBox ID="AdviceSecondAuditCheckbox" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="三审">
                            <ItemTemplate>
                                <asp:CheckBox ID="AdviceThirdAuditCheckbox" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>
                    <div class="pagination">
        <p>
            <WEC:Pager ID="AdvicePager" PageSize="10" PageIndex="0" runat="server" OnFired="Pager_Fired" />
        </p>
    </div>
                <br />
                <dd>
                    <h1>
                        用户</h1>
                    <input class="txt" id="userNameInput" name="userNameInput" runat="server">
                    <input class="Btn" id="userAddSubmit" type="button" value="添加" runat="server" onserverclick="userAddSubmit_ServerClick">
                    <p class="Hint">
                        请输入已存在的用户登录名。</p>
                    <asp:GridView ID="UsersGridView" runat="server" EnableViewState="true" AutoGenerateColumns="false"
                        Width="98%" OnRowDataBound="UsersGridView_RowDataBound"  CssClass="List" GridLines="Horizontal">
                        <Columns>
                            <asp:TemplateField HeaderStyle-Width="0">
                                <ItemTemplate>
                                    <input type="hidden" id="IDHidden" runat="server" value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="用户名称" DataField="LoginName" />
                        <asp:TemplateField HeaderText="查看">
                            <ItemTemplate>
                                <asp:CheckBox ID="AdviceReadCheckbox" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="管理">
                            <ItemTemplate>
                                <asp:CheckBox ID="AdviceAdminCheckbox" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="受理">
                            <ItemTemplate>
                                <asp:CheckBox ID="AdviceAcceptCheckbox" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="办理">
                            <ItemTemplate>
                                <asp:CheckBox ID="AdviceHandleCheckbox" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="审核">
                            <ItemTemplate>
                                <asp:CheckBox ID="AdviceFirstAuditCheckBox" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="二审">
                            <ItemTemplate>
                                <asp:CheckBox ID="AdviceSecondAuditCheckbox" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="三审">
                            <ItemTemplate>
                                <asp:CheckBox ID="AdviceThirdAuditCheckbox" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                    <dd>
                        <input class="Btn" id="SaveButton2" runat="server" type="submit" value="更新模型权限" onserverclick="SaveButton_ServerClick">
                    </dd>
    </dl>
</div>
