<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdviceSendEmailControl.ascx.cs"
    Inherits="We7.CMS.Web.Admin.controls.AdviceSendEmailControl" %>
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
    function DeleteClick(fileTitle, title) {
        var isDemoSite = document.getElementById("hdDemoSite").value;
        if (isDemoSite === "true") {
            alert("对不起，此演示站点您没有该操作权限！");
            return;
        }
        var deleteBtn = document.getElementById("<%=DeleteBtn.ClientID %>");

        var mailFileText = document.getElementById("<%=MailFileText.ClientID %>");
        mailFileText.value = fileTitle;
        var titleLabel = document.getElementById("<%=TitleText.ClientID %>");
        titleLabel.value = title;
        deleteBtn.click();
    }

    function DeleteBtnClick() {
        var isDemoSite = document.getElementById("hdDemoSite").value;
        if (isDemoSite === "true") {
            alert("对不起，此演示站点您没有该操作权限！");
            return;
        }
        if (confirm('您确认要把选中邮件删除吗？此操作不可恢复，请确认！')) {
            var deleteBtn = document.getElementById("<%=DeleteBtn.ClientID %>");
            var mailFileText = document.getElementById("<%=MailFileText.ClientID %>");
            mailFileText.value = "";
            deleteBtn.click();
        }
    }

</script>

<WEC:MessagePanel ID="Messages" runat="server">
</WEC:MessagePanel>
<div id="conbox">
    <dl>
        <dt>»反馈信息发送邮件<br />
            <img src="/admin/images/bulb.gif" align="absmiddle" />
            <label class="block_info">
                此处对反馈信息发送不准确的邮件进行处理！</label>
        </dt>
        <dd style="width: 650px">
            <div class="toolbar">
                <li>
                    <asp:HyperLink ID="DeleteHyperLink" NavigateUrl="javascript:DeleteBtnClick();" runat="server">
                    删除
                    </asp:HyperLink>
                </li>
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
                    <asp:HyperLinkField DataNavigateUrlFields="MailFile" HeaderText="邮件主题" DataNavigateUrlFormatString="/admin/Advice/SendEmailDetail.aspx?fileName={0}"
                        DataTextField="Title" DataTextFormatString="{0}" />
                    <asp:TemplateField>
                        <HeaderTemplate>
                            收件人
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# GetAdviceReplyUser(Eval("UserEmail").ToString())%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            发件人
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# GetAdviceReplyUser(Eval("FormEmail").ToString())%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            发件时间
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Eval("CreateDate")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            删除邮件
                        </HeaderTemplate>
                        <ItemTemplate>
                            <a href="javascript:DeleteClick('<%#Eval("MailFile")%>','<%#Eval("Title")%>');">删除</a>
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
                <asp:Button ID="DeleteBtn" runat="server" Text="Delete" OnClick="DeleteBtn_Click" />
                <asp:TextBox ID="MailFileText" runat="server"></asp:TextBox>
                <asp:TextBox ID="TitleText" runat="server"></asp:TextBox>
                <asp:TextBox ID="DeleteEmailTextBox" runat="server"></asp:TextBox>
            </div>
        </dd>
    </dl>
</div>
