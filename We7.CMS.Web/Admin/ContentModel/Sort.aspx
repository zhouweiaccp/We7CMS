<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Sort.aspx.cs" Inherits="We7.CMS.Web.Admin.ContentModel.Sort" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>直接拖动位置排序</title>
	<meta http-equiv="pragma" content="no-cache" />
	<meta http-equiv="Cache-Control" content="no-cache,must-revalidate" />
	<script src="/scripts/jquery/jquery-1.4.2.js" type="text/javascript"></script>
	<script src="/Admin/Ajax/jquery/ui1.8.1/ui/minified/jquery.ui.core.min.js" type="text/javascript"></script>
	<script src="/Admin/Ajax/jquery/ui1.8.1/ui/minified/jquery.ui.widget.min.js" type="text/javascript"></script>
	<script src="/Admin/Ajax/jquery/ui1.8.1/ui/minified/jquery.ui.mouse.min.js" type="text/javascript"></script>
	<script src="/Admin/Ajax/jquery/ui1.8.1/ui/minified/jquery.ui.sortable.min.js" type="text/javascript"></script>
	<style type="text/css">
		#sortarea li
		{
			list-style: none;
			float: left;
			padding :10px;
		}
		#toolbar{padding:20px 0;text-align:center;}
		.input-button
		{
			background-color:#005eac;
			border-color:#b8d4e8 #124680 #124680 #b8d4e8;
			border-style:solid;
			border-width:1px;
			color:#fff;
			cursor:pointer;
			font-size:12px;
			padding:2px 15px;
			text-align:center;
			_padding:3px 10px;
			*behavior:expression(function(ele){ele.style.behavior='none';if(ele.disabled){ele.style.backgroundColor='#D4D0C8';ele.style.border="0"}}(this))
			}

	</style>
</head>
<body>
	<form id="form1" runat="server">
	<div>
		<div id="toolbar">
			<input id="btnSave" class="input-button" style="behavior: none" type="button" value="保存" />
			<%--<input class="input-button" onclick="reserve();" style="behavior: none" type="button" value="反转顺序" />--%>
			<input class="input-button" onclick="window.returnValue='no';window.close();" style="behavior: none"
				type="button" value="取消" />
		</div>
		<div>
			<ul id="sortarea">
				<%foreach (DataRow dr in SortData)
	  { %><li id="<%=dr["ID"].ToString() %>">
		  <img style="cursor:move;max-height:100px;max-width:100px;" title="<%=dr[SortText].ToString() %>" src="<%=SortImgUrl(dr) %>" /></li>
				<%} %>
			</ul>
		</div>
	</div>
	</form>
	<script type="text/javascript">
		$("#sortarea").sortable();
		$("#btnSave").click(function () {
			var ajaxdata = '';
			$("#sortarea li").each(function (i) {
				ajaxdata += '|' + $(this).attr("id") + ':' + i;
			});
			if (ajaxdata != '') ajaxdata = ajaxdata.substr(1, ajaxdata.length - 1);
			$.ajax({
				url: '/Admin/ContentModel/ajax/ContentModel.asmx/ModelSort',
				type: "POST",
				contentType: 'application/json; charset=utf-8',
				dataType: 'json',
				data: '{"model":"<%=ModelName %>","data":"' + ajaxdata + '"}',
				success: function (json) {
					window.returnValue = 'ok';
					window.close();
				}
			});
		});
		function reserve() { 
		
		}		
	</script>
</body>
</html>
