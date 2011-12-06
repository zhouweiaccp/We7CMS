<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Article_option.ascx.cs"
    Inherits="We7.CMS.Web.Admin.controls.Article_option" %>
<%@ Register TagPrefix="WEC" Namespace="We7.CMS.Controls" Assembly="We7.CMS.UI" %>
<%@ Register Assembly="FCKeditor.net" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>

<script language="JavaScript" type="text/javascript">
    function SaveArticle() {
        var saveBtn = document.getElementById("<%=SaveButton2.ClientID %>");
        if (saveBtn) saveBtn.click();
    }
</script>

<link rel="stylesheet" type="text/css" media="screen" href="<%=ThemePath%>/css/jquery.easywidgets.css" />
<link rel="stylesheet" type="text/css" media="screen" href="<%=ThemePath%>/css/mywidgets.css" />

<script src="<%=ThemePath%>/js/jquery-ui.min.js" type="text/javascript"></script>

<script src="<%=ThemePath%>/js/jquery.easywidgets.js" type="text/javascript"></script>

<script src="<%=ThemePath%>/js/mywidgets.js" type="text/javascript"></script>

<script src="/Admin/Ajax/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

<script src="/Admin/cgi-bin/Article.js" type="text/javascript"></script>

<WEC:MessagePanel ID="Messages" runat="server">
</WEC:MessagePanel>
<div id="conbox">
    <dl>
        <dt>»文章/稿件的基本信息<br>
            <input class="Btn" id="SaveButton" runat="server" visible="false" type="submit" value="保存文章信息"
                onserverclick="SaveButton_ServerClick">
            <dd>
                <div id="channelList" runat="server" class="channelList">
                    <img src="/admin/images/bulb.gif" align="absmiddle" />
                    <asp:DropDownList ID="ChannelDropDownList" runat="server" onchange="contentEdited(true);">
                    </asp:DropDownList>
                    <label class="block_info">
                        &larr; 要发布到哪个栏目？</label>
                </div>
                <div id="ParentArticleDiv" runat="server" class="channelList" visible="false">
                    <p>
                        父级文章：<asp:Label ID="ParentArticleTitle" runat="server"></asp:Label>
                        <input type="hidden" id="ParentArticleID" runat="server" />
                    </p>
                </div>
                <div id="titlediv">
                    <input class="TitleText" id="TitleTextBox" runat="server" size="30" title="文章的主标题，请仔细填写，不可以为空。"
                        onchange="contentEdited(true);">
                </div>
                <div runat="server" id="BodyDiv">
                    <FCKeditorV2:FCKeditor ID="ContentTextBox" EnableSourceXHTML="true" EnableXHTML="true"
                        HtmlEncodeOutput="false" runat="server" Height="300px" Width="545px" BasePath="/admin/fckeditor/">
                    </FCKeditorV2:FCKeditor>
                    <div class="editorFooter inputWidth">
                    </div>
                </div>
                <br />
                <div class="widget collapsable  inputWidth" id="widget-myprocess" style="margin-left: 0px">
                    <div class="widget-header">
                        <strong>高级选项</strong>
                    </div>
                    <div class="widget-content" >
                        <div class="inside">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr id="linkSpan" style="display: none" runat="server">
                                    <td class="formTitle">
                                        源文件URL地址
                                    </td>
                                    <td class="formValue">
                                        <input class="txt" id="ContentUrlTextBox" runat="server" style="width: 450px" onchange="contentEdited(true);">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formTitle">
                                        标题样式
                                    </td>
                                    <td class="formValue" style="vertical-align:bottom;">
                                        <asp:CheckBox ID="chkBold" runat="server" Text="加粗" />&nbsp;&nbsp;<asp:CheckBox ID="chkItalic"
                                            runat="server" Text="斜体" />
                                        &nbsp;
                                        <asp:DropDownList ID="ddlColor" runat="server" Style="height:20px;font-size:12px;padding:0px; margin:0px; text-align: center;vertical-align:middle;color:#666">
                                            <asp:ListItem Text="默认" Value=""></asp:ListItem>
                                            <asp:ListItem Text="红色" Value="#FF0000"></asp:ListItem>
                                            <asp:ListItem Text="橙色" Value="#FF2400"></asp:ListItem>
                                            <asp:ListItem Text="黄色" Value="#FFFF00"></asp:ListItem>
                                            <asp:ListItem Text="绿色" Value="#00FF00"></asp:ListItem>
                                            <asp:ListItem Text="蓝色" Value="#0000FF"></asp:ListItem>
                                            <asp:ListItem Text="靛色" Value="#8A2BE2"></asp:ListItem>
                                            <asp:ListItem Text="紫色" Value="#9932CD"></asp:ListItem>
                                            <asp:ListItem Text="黑色" Value="#000000"></asp:ListItem>
                                            <asp:ListItem Text="白色" Value="#FFFFFF"></asp:ListItem>
                                            <asp:ListItem Text="灰色" Value="#C0C0C0"></asp:ListItem>
                                        </asp:DropDownList>
                                        色
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formTitle">
                                        文章类型
                                    </td>
                                    <td class="formValue">
                                        <%--<asp:DropDownList ID="ActicleTypeDropDownList" runat="server" onchange="contentEdited(true);">--%>

                                        <script>
                                            var linkSpanID=document.getElementById('<%=this.ClientID %>_linkSpan');
                                        </script>

                                        <asp:DropDownList ID="ActicleTypeDropDownList" runat="server" onchange="ActicleTypeDropDownList_onchange(linkSpanID,this);">
                                            <asp:ListItem Value="">请选择文章类型</asp:ListItem>
                                            <asp:ListItem Value="0" Selected="True">原创文章</asp:ListItem>
                                            <asp:ListItem Value="8">引用文章</asp:ListItem>
                                            <asp:ListItem Value="10">共享文章</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formTitle">
                                        副标题
                                    </td>
                                    <td class="formValue">
                                        <input class="txt" id="SubTitleTextBox" runat="server" style="width: 450px" onchange="contentEdited(true);">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formTitle">
                                        摘要
                                        <br />
                                        <br />
                                    </td>
                                    <td class="formValue">
                                        <textarea id="DescriptionTextBox" runat="server" rows="6" cols="50" style="width: 450px"
                                            onchange="contentEdited(true);"></textarea>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        文章关键字
                                    </td>
                                    <td>
                                        <textarea id="KeywordTextBox" runat="server" style="width: 450px; height: 42px;"
                                            name="S1"></textarea>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        文章描述
                                    </td>
                                    <td>
                                        <textarea id="DescriptionKeyTextBox" runat="server" cols="50" style="width: 450px;
                                            height: 43px;" name="S2"></textarea>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formTitle">
                                        作者
                                    </td>
                                    <td class="formValue">
                                        <input runat="server" id="AuthorTextBox" class="txt" style="width: 250px" onchange="contentEdited(true);">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formTitle">
                                        来源
                                    </td>
                                    <td class="formValue">
                                        <input id="SourceTextBox" runat="server" class="txt" style="width: 250px" onchange="contentEdited(true);" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formTitle">
                                        排序
                                    </td>
                                    <td class="formValue">
                                        <input id="IndexTextBox" runat="server" class="txt" style="width: 250px" onchange="contentEdited(true);"
                                            value="999" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formTitle">
                                        修改时间
                                    </td>
                                    <td class="formValue">
                                        <input id="UpdatedTextBox" runat="server" class="txt" style="width: 250px" onchange="contentEdited(true);" onfocus="WdatePicker({isShowClear:false,readOnly:true,dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td class="formTitle">
                                        <asp:Label ID="CreatorLabel" runat="server" Text=""></asp:Label>
                                        创建于
                                        <asp:Label ID="CreatedLabel" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formTitle">
                                        过期时间：
                                    </td>
                                    <td class="formValue">
                                        <input id="txtInvalidDate" name="" class="txt" onfocus="WdatePicker({isShowClear:false,readOnly:true,dateFmt:'yyyy-MM-dd HH:mm:ss'})"
                                            style="width: 250px" runat="server" />&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formTitle">
                                        选项
                                    </td>
                                    <td class="formValue">
                                        <label for="sex_female">
                                            <span style="display: <%=ButtonVisble%>">
                                                <asp:CheckBox ID="IsShowCheckBox" runat="server" />在首页显示</span>
                                            <asp:CheckBox ID="AllowCommentsCheckBox" runat="server" Checked="True" />允许评论</label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="formTitle">
                                        状态
                                    </td>
                                    <td class="formValue">
                                        <asp:DropDownList ID="StateDropDownList" runat="Server" onchange="contentEdited(true);">
                                            <asp:ListItem Text="启用" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="禁用" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="审核中" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="过期" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="已删除" Value="4"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr style="display: <%=ValidateVisble%>">
                                    <td>
                                        验证码：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="ValidateTextBox" runat="server" Columns="30" /><img alt="x" src="/Admin/cgi-bin/controls/CaptchaImage/SmallJpegImage.aspx"
                                            runat="server" id="Img" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td style="padding-top: 15px; padding-left: 0px">
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <input id="SaveButton2" class="Btn" runat="server" type="submit" value="保存文章信息" onserverclick="SaveButton_ServerClick" />
                <br />
    </dl>
    <div style="display: none">
        <input type="button" id="saveAricleButton" onclick="SaveArticle()" />
    </div>
</div>
