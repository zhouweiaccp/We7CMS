<%@ Control Language="C#" AutoEventWireup="true" Inherits="We7.CMS.Web.Widgets.CommentsList_Common"
	CodeFile="CommentsList.Common.cs" %>
<% Response.Cookies["AreYouHuman"].Value = We7.CMS.Web.Admin.CaptchaImage.GenerateRandomCode(); %>
<script runat="server" type="text/C#">
	[ControlDescription(Desc = "评论添加列表一体", Author = "西部动力")]
	string MetaData;
</script>
<script type="text/javascript">
    function getCommentInfo(type) {
        var articleID = $("#hidArticleID").val();
        var pageSize = $("#hidPageSize").val();
        var pageindex = $("#hidPageIndex").val();
        var pageCount=<%=PageCount %>;
        var recordCount=<%=RecordCount %>;
        var currPageIndex = pageindex;
                
        //type为true表示+
        if (type) {
            pageindex = pageindex<pageCount ? (Number(pageindex) + Number(1)) :pageCount;
        } else {
            pageindex = pageindex>1 ? (Number(pageindex) - Number(1)) : 1;
        }
        if(currPageIndex!=pageindex){            
            $("#PageIndex").html(pageindex);
            $("#hidPageIndex").val(pageindex);
            $.post("/admin/Ajax/Comment/ListCommentsAction.ashx", { "ArticleID": articleID, "PageSize": pageSize, "PageIndex": pageindex }, function (data, states) {
                if (states == "success") {
                    var json = eval('(' + data + ')');
                    var value;
                    for (var i = 0; i < json.length; i++) {
                        if (value != null && value != undefined) {
                            value = value + "<div id=\"commentinfo\"><div style=\"padding: 10px 5px;\"> <div style=\"float: left;\">" + json[i].Author + "</div><div style=\"float: right\">" + json[i].Updated + "&nbsp;&nbsp;&nbsp;                </div></div> <div style=\"height: 40px; width: 100%; border-top: solid 1px #ececec; border-bottom: solid 1px #ececec; padding: 5px; text-align: left;\">" + json[i].Content + "</div></div>"
                        }
                        else {
                            value = "<div id=\"commentinfo\"><div style=\"padding: 10px 5px;\"> <div style=\"float: left;\">" + json[i].Author + "</div><div style=\"float: right\">" + json[i].Updated + "&nbsp;&nbsp;&nbsp;                </div></div> <div style=\"height: 40px; width: 100%; border-top: solid 1px #ececec; border-bottom: solid 1px #ececec; padding: 5px; text-align: left;\">" + json[i].Content + "</div></div>"
                        }
                    }
                    document.getElementById("commentinfo").innerHTML = value;  //输出评论内容 
                }
            });
        }
    }

</script>
<div class="<%=CssClass %>">
	<div>
		<form id="<%=ActionID %>" action="/admin/Ajax/Comment/ListCommentsAction.ashx" method="post"
		target="_self" style="width: 100%;">
		<div id="commentinfo">
			<% foreach (We7.CMS.Common.Comments c in Items)
	  { %>
			<div style="padding: 10px 5px; height: 15px;">
				<div style="float: left;color:#FF7700;">
					<%=c.Author %>
				</div>
				<div style="float: right">
					<%=Convert.ToDateTime(c.Updated).ToString("yyyy-MM-dd HH:mm") %>
					&nbsp;&nbsp;&nbsp;
				</div>
			</div>
			<div style="height: 40px; width: 100%; border-top: solid 1px #ececec; border-bottom: solid 1px #ececec;
				padding: 5px; text-align: left;">
				<%=c.Content %>
			</div>
			<%} %>
		</div>
		<div>
			<input type="hidden" name="PageIndex" value="<%=PageIndex %>" id="hidPageIndex" />
			<input type="hidden" name="ArticleIDByRedirect" value="<%=ArticleIDByRedirect %>" />
			<input type="hidden" name="ArticleID" value="<%=ArticleID %>" id="hidArticleID" />
			<input name="_ActionID" type="hidden" value="<%=ActionID %>" id="hid_ActionID" />
			<input name="PageSize" type="hidden" value="<%=PageSize %>" id="hidPageSize" />
			<script type="text/javascript">
                var pageCount=<%=PageCount %>
                function PrePage()
                {
                    var form=document.getElementById('<%=ActionID %>');
                    var pageindex=parseInt(form.PageIndex.value);
                    if(pageindex>1)
                        form.PageIndex.value=pageindex-1;
                    form.PageSize.value=<%=PageSize %>;
                    document.getElementById('<%=ActionID %>').submit();
                    return false;
                }
                
                function NextPage()
                {
                    var form=document.getElementById('<%=ActionID %>');
                    var pageindex=parseInt(form.PageIndex.value);
                    if(pageindex<pageCount)
                        form.PageIndex.value=pageindex+1;
                    form.PageSize.value=<%=PageSize %>;
                    document.getElementById('<%=ActionID %>').submit();
                    return false;
                }
              
			</script>
			<table style="width: 100%">
				<tr>
					<td align="left" id="commentInfocount">
						总共有<%=RecordCount %>条记录,当前页<span id="PageIndex"><%= PageIndex %></span>/<%=PageCount%>页&nbsp;&nbsp;&nbsp;
					</td>
					<td style="text-align: right">
						<a id="clickeup" onclick="getCommentInfo(false)" href="javascript:void(0)">上一页</a>&nbsp;&nbsp;<a
							onclick="getCommentInfo(true)" id="clickedown" href="javascript:void(0)">下一页</a>&nbsp;&nbsp;&nbsp;
					</td>
				</tr>
			</table>
		</div>
		</form>
	</div>
	<div style="border: solid 1px #CDCDCD">
		<form id="<%=CreateActionID() %>" action="/admin/Ajax/Comment/AddCommentAction.ashx"
		method="post" target="_self">
		<script type="text/javascript">
			function submitAction(isLogin) {
				if ($("#Author").val() == '') $("#Author").val("无名网友");
				document.getElementById("<%=ActionID %>").action = "/admin/Ajax/Comment/AddCommentAction.ashx?" + (isLogin ? "IsLogin=true" : "");
				document.getElementById("<%=ActionID %>").submit();
			}

			function setAuthor(isAnony) {
				document.getElementById("<%=ActionID %>").Author.value = isAnony ? "匿名用户" : "";
			}
			var showForm = '<%=ShowForm %>';
			if (showForm.toLowerCase() == 'true') $("#<%=ActionID%>").parent().show();
			else $("#<%=ActionID%>").parent().hide();
		</script>
		<dl style="border-bottom: 1px solid #CDCDCD; background: #EFEFEF; margin: 0 0 7px 0;">
			<dd style="padding: 5px;">
				发表评论：
			</dd>
		</dl>
		<dl style="<%=MessageDisplay %>">
			<dd class="pad" style="color: Red">
				<%=Get("Message") %>
			</dd>
		</dl>
		<dl style="display: <%= LoginName %>">
			<dd class="pad">
				<input id="Author" type="text" name="Author" value="<%=Get("Author") %>" />
				<input name="IsAnony" type="checkbox" onclick="setAuthor(this.checked)" value="true"
					<%=Get("IsAnony")!=null&&(bool)Get("IsAnony")?"checked":"" %> />匿名
			</dd>
		</dl>
		<dl>
			<dd class="pad">
				<textarea name="Content" cols="40" rows="7"><%=Get("Content") %></textarea>
			</dd>
		</dl>
		<dl style="display: <%=SelectValidate %>">
			<dd style="padding: 7px 0 0 4px">
				验证码：
			</dd>
			<dd>
				<input name="ValidateCode" type="text" style="width: 70px;" />
			</dd>
			<dd>
				<img alt="" src="/Admin/cgi-bin/controls/CaptchaImage/SmallJpegImage.aspx" style="width: 100px;
					height: 22px;" />
			</dd>
		</dl>
		<dl style="margin-bottom: 7px;">
			<dd class="pad">
				<button type="button" onclick="submitAction(false)" class="button_style">
					提交</button>
			</dd>
		</dl>
		<input name="ArticleID" type="hidden" value="<%=ArticleID %>" />
		<input name="ChannelName" type="hidden" value="<%=ChannelName %>" />
		<input name="ArticleIDByRedirect" type="hidden" value="<%=ArticleIDByRedirect %>" />
		<input name="CurrentAccount" type="hidden" value="<%=AccountID %>" />
		<input name="IsSignin" type="hidden" value="<%=IsSignin %>" />
		<input name="Title" type="hidden" value="<%=Title %>" />
		<input name="ISValidate" type="hidden" value="<%=ISValidate %>" />
		<input name="_ActionID" type="hidden" value="<%=ActionID %>" />
		</form>
		<div style="clear: left;">
		</div>
	</div>
</div>
