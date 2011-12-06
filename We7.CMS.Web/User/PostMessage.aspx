<%@ Page Language="C#" MasterPageFile="~/User/DefaultMaster/content.Master" AutoEventWireup="true"
    CodeBehind="PostMessage.aspx.cs" Inherits="We7.CMS.Web.User.PostMessage" Title="无标题页" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MyHeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
    <form id="<%=ActionID %>" action="/User/Action/AddMessage.ashx?Action=send" method="post"
    target="_self">
    <div class="realRight ml10">
        <div class="mybox">
            <div class="mytit">
                发送短消息</div>
            <div class="con">
                <script type="text/javascript">
                    function submitAction(action) {
                        document.getElementById("<%=ActionID %>").action = "/User/Action/AddMessage.ashx?Action=" + action;
                        document.getElementById("<%=ActionID %>").submit();
                    }
                </script>
                <ul style="<%=SMDisplay %>">
                    <li class="notetitle">
                        <%=Subject%></li>
                    <li class="notetime">发送人:
                        <%=Receivers %>
                        <%=SendTime %></li>
                    <li class="notecontent">
                        <%=Content %></li>
                    <li class="notecontent">
                        <img alt="查看信息" src="style/images/leftdot.gif">
                        <a href="message.aspx?state=inbox">返回列表</a>
                        <%--				<a href="usercppostpm.aspx?action=re&amp;pmid=2370">回复</a>
				<a href="usercppostpm.aspx?action=fw&amp;pmid=2370">转发</a>
				<a href="usercpshowpm.aspx?action=noread&amp;pmid=2370">标记为未读</a>
				<a href="usercpshowpm.aspx?action=delete&amp;pmid=2370">删除</a>--%>
                    </li>
                </ul>
                <dl style="<%=MessageDisplay %>">
                    <dd class="pad" style="color: Red">
                        <%=Get("Message") %>
                    </dd>
                </dl>
                <label class="labelshort" for="user">
                    收件人:</label>
                <input type="text" size="20" value="<%=ReReceiver %>" onblur="this.className='colorblue';"
                    onfocus="this.className='colorfocus';" class="colorblue" name="Receivers"><br />
                <br />
                <label class="labelshort" for="email">
                    标题:
                </label>
                <input type="text" size="40" value="<%=ReSubject %>" onblur="this.className='colorblue';"
                    onfocus="this.className='colorfocus';" class="colorblue" name="Subject"><br />
                <br />
                <label class="labelshort" for="comment">
                    内容:</label><textarea style="width: 80%;" onkeydown="if((event.ctrlKey &amp;&amp; event.keyCode == 13) || (event.altKey &amp;&amp; event.keyCode == 83)) document.getElementById('postpm').submit();"
                        onblur="this.className='colorblue';" onfocus="this.className='colorfocus';" class="colorblue"
                        rows="20" cols="80" name="Content" runat="server"><%=ReContent%></textarea><br>
                <label class="labelshort" for="savetosentbox">
                    &nbsp;</label>
                <input type="checkbox" style="border: 0pt none;" value="1" name="SavetoOutbox">发送的同时保存到发件箱>
                <label class="labelshort" for="savetosentbox">
                    &nbsp;</label>
                <input type="submit" class="sbutton" value="立即发送">
                <input type="submit" class="sbutton" value="存为草稿" onclick="submitAction('saveDraft')">
                [完成后可按Ctrl+Enter提交]
                <input name="_ActionID" type="hidden" value="<%=ActionID %>" />
            </div>
        </div>
    </div>
    </form>
</asp:Content>
