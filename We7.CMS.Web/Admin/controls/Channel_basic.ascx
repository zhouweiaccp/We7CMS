<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Channel_basic.ascx.cs"
    Inherits="We7.CMS.Web.Admin.controls.Channel_basic" %>
<%@ Register TagPrefix="WEC" Namespace="We7.CMS.Controls" Assembly="We7.CMS.UI" %>

<script language="vbscript" type="text/vbscript">
function vbToAsc(str)
        toAsc = hex(asc(str))
end function
</script>

<script src="<%=AppPath%>/cgi-bin/pinyin.js" type="text/javascript"></script>

<WEC:MessagePanel ID="Messages" runat="server">
</WEC:MessagePanel>
<div id="conbox">
    <dl>
        <dt>»栏目的基本信息<br />
            <img src="/admin/images/bulb.gif" align="absmiddle" alt="" /><label class="block_info">修改栏目属性，可以对栏目的内容及展示效果进行改变；</label>
            <input class="Btn" id="SaveButton" runat="server" type="submit" value="保存文章信息" onserverclick="SaveButton_ServerClick"
                visible="false" />
        </dt>
        <dd>
            <table id="personalForm" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="formTitle">
                        标题
                    </td>
                    <td class="formValue">
                        <asp:TextBox ID="NameTextBox" runat="server" Columns="50" CssClass="txt"></asp:TextBox>
                    </td>
                </tr>
                <tr id="linkSpan" runat="server">
                    <td class="formTitle"  >
                        栏目URL地址
                    </td>
                    <td class="formValue" valign="middle">
                    <span id="channelUrlLabel" runat="server" class="channelurl"></span>
                        <asp:TextBox ID="ChannelNameTextBox" runat="server" Columns="30" MaxLength="54" Width="150px" CssClass="channelurlText"></asp:TextBox>
                        <span rel="xml-hint" title="栏目URL：会根据标题生成首字母拼音缩写，当然，您也可以自己输入。" style="margin-left:10px" ></span>
                    </td>
                </tr>
                 <tr>
                    <td class="formTitle">
                        描述
                    </td>
                    <td class="formValue">
                        <asp:TextBox ID="DescriptionTextBox" runat="server" Columns="40" Rows="5" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
<%--                <tr>
                    <td class="formTitle">
                        目录
                    </td>
                    <td class="formValue">
                        <asp:Literal ID="ChannelFolderLiteral" runat="server"></asp:Literal><asp:Label ID="ChannelNameLabel"
                            Text="" runat="server"></asp:Label>
                    </td>
                </tr>--%>
                <tr>
                    <td class="formTitle">
                        节点类型
                    </td>
                    <td class="formValue">
                        <asp:DropDownList ID="TypeDropDownList" runat="server">
                            <asp:ListItem Value="0" Selected="True">原创内容</asp:ListItem>
                            <asp:ListItem Value="1">专题</asp:ListItem>
                            <asp:ListItem Value="3">跳转</asp:ListItem>
                            <asp:ListItem Value="4">Rss源</asp:ListItem>
                            <asp:ListItem Value="5">空节点</asp:ListItem>
                        </asp:DropDownList>
                        <label id="typeMessage" for=""></label>
                    </td>
                </tr>
                <tr id="modelRow"  style="display: <%=ModelRowDisplay%>" >
                    <td class="formTitle">
                        内容模型
                    </td>
                    <td class="formValue">
                        <asp:DropDownList ID="ContentTypeDropDownList" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr style="display: <%=ReturnUrlRowDisplay%>" id="ReturnUrlRow">
                    <td class="formTitle" id="urlLabel">
                            <%=UrlTitle %>
                    </td>
                    <td class="formValue">
                        <asp:TextBox ID="ReturnUrlTextBox" runat="server" Columns="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="formTitle">
                        状态
                    </td>
                    <td class="formValue">
                        <asp:DropDownList ID="StateDropDownList" runat="server">
                            <asp:ListItem Value="1">可用</asp:ListItem>
                            <asp:ListItem Value="0" Selected="True">不可用</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="formTitle">
                    </td>
                    <td class="formValue">
                        <label>
                            <asp:Label ID="CreatorLabel" runat="server" Text=""></asp:Label>
                            创建于
                            <asp:Label ID="CreatedLabel" runat="server" Text=""></asp:Label>
                        </label>
                        <asp:Label ID="IDLabel" runat="server" Text="" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td style="padding-top: 15px; padding-left: 0px">
                        <input class="Btn" id="SaveButton2" runat="server" type="submit" value="更新栏目信息" onserverclick="SaveButton_ServerClick" />
                    </td>
                </tr>
            </table>
        </dd>
    </dl>
    <div style="display: none">
        <asp:TextBox ID="ParentIDTextBox" runat="server"></asp:TextBox>
        <asp:TextBox ID="ChannelNameHidden" runat="server"></asp:TextBox>
        <asp:TextBox ID="MoveToIDTextBox" runat="server"></asp:TextBox>
        <asp:TextBox ID="TemplateIDTextBox" runat="server"></asp:TextBox>
        <asp:TextBox ID="DetailTemplateIDTextBox" runat="server"></asp:TextBox>
        <asp:TextBox ID="ReferenceIDTextBox" runat="server"></asp:TextBox>
        <asp:TextBox ID="AreaIDTextBox" runat="server"></asp:TextBox>
        <asp:TextBox ID="SouceIDTextBox" runat="server"></asp:TextBox>
        <asp:TextBox ID="AliasWordsTextBox" runat="server"></asp:TextBox>
    </div>
</div>
