<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdviceReplyEmailControl.ascx.cs"
    Inherits="We7.CMS.Web.Admin.controls.AdviceReplyEmailControl" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>

<input type="hidden" id="hdDemoSite"
             value='<%= We7.Framework.Config.GeneralConfigs.GetConfig().IsDemoSite.ToString().ToLower() %>' />
<script type="text/javascript">
    function SelectAll(tempControl) {
        var theBox = tempControl;
        xState = theBox.checked;

        elem = theBox.form.elements;
        for (i = 0; i < elem.length; i++)
            if (elem[i].type == "checkbox" && elem[i].id != theBox.id) {
            if (elem[i].checked != xState)
                elem[i].click();
        }
    }
    function DeleteClick(title) {
        var isDemoSite = document.getElementById("hdDemoSite").value;
        if (isDemoSite === "true") {
            alert("对不起，此演示站点您没有该操作权限！");
            return;
        }

        if (confirm('您确认要删除此邮件吗？此操作不可恢复，请确认！')) {
            var deleteBtn = document.getElementById("<%=DeleteBtn.ClientID %>");
            var titleLabel = document.getElementById("<%=xmlTitleText.ClientID %>");
            titleLabel.value = title;
            deleteBtn.click();
        }
    }

    function DeleteBtnClick() {
        var isDemoSite = document.getElementById("hdDemoSite").value;
        if (isDemoSite === "true") {
            alert("对不起，此演示站点您没有该操作权限！");
            return;
        }

        if (confirm('您确认要把选中邮件删除吗？此操作不可恢复，请确认！')) {
            var deleteBtn = document.getElementById("<%=DeleteBtn.ClientID %>");
            var titleLabel = document.getElementById("<%=xmlTitleText.ClientID %>");
            titleLabel.value = "";
            deleteBtn.click();
        }
    }

    function UntreadClick() {
        var isDemoSite = document.getElementById("hdDemoSite").value;
        if (isDemoSite === "true") {
            alert("对不起，此演示站点您没有该操作权限！");
            return;
        }

        var untread = document.getElementById("<%=UntreadBtn.ClientID %>");
        untread.click();
    }
    function ReceiveClick() {
        var isDemoSite = document.getElementById("hdDemoSite").value;
        if (isDemoSite === "true") {
            alert("对不起，此演示站点您没有该操作权限！");
            return;
        }

        if (confirm('您确认在接收存储邮件后并删除服务器上的邮件吗？此操作不可恢复，请确认！')) {
            var deleteBtn = document.getElementById("<%=DeleteEmailTextBox.ClientID %>");
            deleteBtn.value = "1";
        }
        var receiveBtn = document.getElementById("<%=ReceiveBtn.ClientID %>");
        receiveBtn.click();
    }
</script>

<WEC:MessagePanel ID="Messages" runat="server">
</WEC:MessagePanel>
<div id="conbox">
    <dl>
        <dt>»邮件回复反馈信息<br />
            <img src="/admin/images/bulb.gif" align="absmiddle" />
            <label class="block_info">
                此处对回复反馈信息不准确的邮件进行处理！</label>
        </dt>
        <dd style="width: 650px">
            <div class="toolbar">
                <li>
                    <asp:HyperLink ID="DeleteHyperLink" NavigateUrl="javascript:DeleteBtnClick();" runat="server">
                    删除
                    </asp:HyperLink>
                </li>
                <li class="smallButton8">
                    <asp:HyperLink ID="ReceiveHyperLink" NavigateUrl="javascript:ReceiveClick();" runat="server">
            接收反馈回复邮件
                    </asp:HyperLink>
                </li>
                <span></span>
                <%--       <asp:HyperLink ID="UntreadHyperLink" NavigateUrl="javascript:UntreadClick();" runat="server">
            取消流转
                </asp:HyperLink>
                <span></span>--%>
            </div>
            <asp:GridView ID="DataGridView" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                CssClass="List" GridLines="Horizontal">
                <Columns>
                    <asp:TemplateField>
                        <HeaderStyle Width="5px" />
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkHeader" runat="server" onclick="javascript:SelectAll(this);"
                                AutoPostBack="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkItem" runat="server" />
                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("MailFile") %>' Visible="False"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:HyperLinkField DataNavigateUrlFields="MailFile" HeaderText="回复主题" DataNavigateUrlFormatString="/admin/Advice/ErrorEmailDetail.aspx?fileName={0}"
                        DataTextField="Title" DataTextFormatString="{0}" />
                    <asp:TemplateField>
                        <HeaderTemplate>
                            回复人
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# GetAdviceReplyUser(Eval("UserEmail").ToString())%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            回复时间
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Eval("CreateDate")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            删除回复邮件
                        </HeaderTemplate>
                        <ItemTemplate>
                            <a href="javascript:DeleteClick('<%#Eval("MailFile")%>');">删除</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <div class="pagination">
                <p>
                    <WEC:Pager ID="Pager" PageSize="20" PageIndex="0" runat="server" OnFired="Pager_Fired" />
                </p>
            </div>
            <div style="display: none">
                <asp:Button ID="DeleteBtn" runat="server" Text="Delete" 
                    OnClick="DeleteBtn_Click" />
                <asp:Button ID="UntreadBtn" runat="server" Text="Untread" OnClientClick="return confirm('您确认要将选中的文章都取消流转吗？')"
                    OnClick="UntreadBtn_Click" />
                <asp:Button ID="ReceiveBtn" runat="server" Text="Receive" OnClick="ReceiveBtn_Click" />
                <asp:TextBox ID="xmlTitleText" runat="server"></asp:TextBox>
                <asp:TextBox ID="DeleteEmailTextBox" runat="server"></asp:TextBox>
            </div>
        </dd>
    </dl>
</div>
