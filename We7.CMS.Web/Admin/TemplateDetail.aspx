<%@ Page Language="C#" AutoEventWireup="true" Codebehind="TemplateDetail.aspx.cs"
    Inherits="We7.CMS.Web.Admin.TemplateDetail" %>
<%@ Register TagPrefix="WEC" Namespace="We7.CMS.Controls" Assembly="We7.CMS.UI" %>
<asp:Content ID="We7Content" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <script type="text/javascript"> 
function onSelectAliasHyperLinkClick() 
{
    var title="模式窗口";
     var nWidth="650";
     var nHeight="480";
     var strFile="TagsList.aspx";
     var ret = window.showModalDialog(strFile,window,"dialogWidth:" + nWidth + "px;dialogHeight:" + nHeight + "px;center:yes;status:no;scroll:auto;help:no;");
     if (ret != null) {
             var arry = new Array();
             arry = ret.split(",");//一般返回一个字符串，用斗号分割
             document.all["<%= AliasWordsTextBox.ClientID%>"].value = arry[0];
            //document.all["AliasLabel"].innerText = arry[1];
             document.all["<%= AliasTextBox.ClientID%>"].innerText = arry[0]; 
      } 
}

function onCancelAliasHyperLinkClick() 
{
    document.all["<%= AliasWordsTextBox.ClientID%>"].value = "";
    document.all["<%= AliasTextBox.ClientID%>"].innerText = "";
} 

function onSaveButtonClick() {
    document.getElementById("<%=SaveButton.ClientID %>").click();
}
    </script>

                        <h2 class="title">
                            <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_look.gif" />
                            <asp:Label ID="TitleLabel" runat="server" Text="新模板">
                            </asp:Label>
                            <span class="summary">
                                <asp:Label ID="SummaryLabel" runat="server" Text="创建一个新模板">
                                </asp:Label>
                            </span>
                        </h2>
                        <div class="toolbar">
                            <asp:HyperLink ID="SaveHyperLink" NavigateUrl="javascript:onSaveButtonClick();"   runat="server">
                                保存</asp:HyperLink>
                            <asp:HyperLink ID="ComposeHyperLink" NavigateUrl="Compose.aspx" runat="server" Target="_blank">
                                模板编辑</asp:HyperLink>
                            <asp:HyperLink ID="ReturnHyperLink" NavigateUrl="Templates.aspx" runat="server">
                            返回</asp:HyperLink>
                            <br />
                        </div>
                        <div>
                            <table>
                                <tr>
                                    <th style="width: 20%">
                                        项目</th>
                                    <th>
                                        值</th>
                                </tr>
                                <tr>
                                    <td>
                                        名称：</td>
                                    <td>
                                        <asp:TextBox ID="NameTextBox" runat="server" Columns="50"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>
                                        描述：</td>
                                    <td>
                                        <asp:TextBox ID="DescriptionTextBox" runat="server" Columns="40" Rows="5" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 22px">
                                        文件名：</td>
                                    <td style="height: 22px">
                                        <asp:TextBox ID="FileNameTextBox" runat="server" Columns="50"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        类型：</td>
                                    <td>
                                        <asp:DropDownList ID="TypeList" runat="server" OnSelectedIndexChanged="TypeList_SelectedIndexChanged"
                                            AutoPostBack="true">
                                            <asp:ListItem Value="False">普通模板</asp:ListItem>
                                            <asp:ListItem Value="True">子模版</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        创建时间：</td>
                                    <td>
                                        <asp:Label ID="CreatedLabel" runat="server" BorderStyle="None"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="CodeDropDownList" runat="server" Visible="False">
                                            <asp:ListItem Selected="True" Value="False">HTML可视化编辑器</asp:ListItem>
                                            <asp:ListItem Value="True">纯代码编辑器</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <asp:Panel ID="AliasPanel" runat="server">
                                    <tr>
                                        <td>
                                            别名：
                                        </td>
                                        <td>
                                            <asp:TextBox ID="AliasTextBox" runat="server" Columns="20" ReadOnly="True"></asp:TextBox>
                                            <span></span>
                                            <asp:HyperLink ID="CancelAliasHyperLink" NavigateUrl="javascript:onCancelAliasHyperLinkClick()"
                                                runat="server">
                                                <asp:Image ID="CancelAliasImage" runat="server" ImageUrl="~/admin/Images/icon_cancel.gif" />
                                                取消</asp:HyperLink>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="TemplatesTagDropDownList" runat="server" AutoPostBack="true"
                                                OnSelectedIndexChanged="TemplatesTagDropDownList_SelectedIndexChanged">
                                                <asp:ListItem Value="">--选择系统别名--</asp:ListItem>
                                                <asp:ListItem Value="[homepage]">首页模板</asp:ListItem>
                                                <asp:ListItem Value="[channel]">栏目页模板</asp:ListItem>
                                                <asp:ListItem Value="[contentpage]">详细信息页模板</asp:ListItem>
                                                <asp:ListItem Value="[search]">搜索结果页模板</asp:ListItem>
                                                <asp:ListItem Value="[login]">登录页模板</asp:ListItem>
                                                <asp:ListItem Value="[error]">错误报告页模板</asp:ListItem>
                                                <asp:ListItem Value="[forgetPassword]">忘记密码页模板</asp:ListItem>
                                                <asp:ListItem Value="[productcontentpage]">产品详细页模板</asp:ListItem>
                                                <asp:ListItem Value="[ContentMode]">内容模型详细页模板</asp:ListItem>
                                                      <asp:ListItem Value="[AdviceMode]">反馈模型详细页模板</asp:ListItem>
                                                 <asp:ListItem Value="[sesearch]">站群搜索结果页模板</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:HyperLink ID="SelectAliasHyperLink" NavigateUrl="javascript:onSelectAliasHyperLinkClick();"
                                                runat="server">
                                                <asp:Image ID="SelectAliasImage" runat="server" ImageUrl="~/admin/Images/icon_jtem.gif" />
                                                从别名字典选择
                                            </asp:HyperLink>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            <span class="note">系统别名是系统的页面分类方式。</span>
                                        </td>
                                    </tr>
                                     <tr>
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                    <span class="note">别名字典是站点自带的模板分类方式,可从字典中选择别名与栏目别名相关联</span>
                                                </td>
                                            </tr> 
                                    <tr>
                                        <td>
                                            是否详细模板：
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="IsDetailTemplateDropDownList" runat="server" OnSelectedIndexChanged="IsDetailTemplateDropDownList_SelectedIndexChanged">
                                                <asp:ListItem Value="False">否</asp:ListItem>
                                                <asp:ListItem Value="True">是</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                     <tr>
                                                <td>
                                                    &nbsp;</td>
                                                <td>
                                                    <span class="note">详细模板是指呈现页面详细信息的页面 </span>
                                                </td>
                                            </tr> 
                                </asp:Panel>
                            </table>
                        </div>
                       <WEC:MessagePanel ID="Messages" runat="server" Visible="false">
                        </WEC:MessagePanel>
                        <div style="display: none">
                            <asp:Button ID="SaveButton" runat="server" Text="Save" OnClick="SaveButton_Click" />
                            <asp:TextBox ID="OldFileTextBox" runat="server"></asp:TextBox>
                            <asp:TextBox ID="AliasWordsTextBox" runat="server"></asp:TextBox>
                            <asp:TextBox ID="DeleteItemAliasTextBox" runat="server" />
                            <asp:TextBox ID="DeleteItemIsDetailTextBox" runat="server" />
                             <asp:TextBox ID="FileDetailTextBox" runat="server" />
                        </div>
</asp:Content>
