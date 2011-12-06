<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultiUploadify.ascx.cs"
	Inherits="We7.Model.UI.Controls.system.MultiUploadify" %>
<link href="/ModelUI/Controls/system/page/uploadify/uploadify.css" rel="stylesheet"
	type="text/css" />
<script type="text/javascript" src="/ModelUI/Controls/system/page/uploadify/swfobject.js"></script>
<script src="/ModelUI/Controls/system/page/uploadify/jquery.uploadify.v2.1.0.min.js"
	type="text/javascript"></script>
<script type="text/javascript">
	$(document).ready(function () {
		$("#uploadify").uploadify({
			'uploader': '/ModelUI/Controls/system/page/uploadify/uploadify.swf',
			'script': '/ModelUI/Controls/system/page/uploadify/UpDownloadHandler.ashx?GetFunction=UploadFile',
			'cancelImg': '/ModelUI/Controls/system/page/uploadify/cancel.png',
			'folder': 'uploads',
			'queueID': 'fileQueue',
			'buttonText': '浏览本地文件',
			'onSelect': function () { $("#actionOK").show(); $('#actionCacel').show(); },
			'sizeLimit': '5242880', //5M 
			'auto': false,
			'multi': true,
			'onComplete': function (event, response, status, d) {
				var $photo = $("#example .photo:eq(0)").clone();
				$photo.find(".thumb img:eq(0)").attr("src", d.substring(0, d.lastIndexOf('.')) + '_thumb' + d.substring(d.lastIndexOf('.'), d.length));
				$("#multiEdit").append($photo);
			},
			'onAllComplete': function () { $('#actionCacel').hide(); setTimeout(function () { $("#multiEdit").show(); }, 500); GetValues(); },
			'onError': function (a, b, c, d) {
				if (d.status == 404)
					alert('Could not find upload script.');
				else if (d.type === "HTTP")
					alert('error ' + d.type + ": " + d.status);
				else if (d.type === "File Size")
					alert(c.name + ' ' + d.type + ' Limit: ' + Math.round(d.sizeLimit / 1024) + 'KB');
				else
					alert('error ' + d.type + ": " + d.info);
			}
		});
	});  

</script>
<input type="file" name="uploadify" id="uploadify" /><div style="float: right; width: 100px;
	margin-right: 20px; direction: rtl;">
	<a id="actionOK" style="display: none;" href="javascript:$('#uploadify').uploadifyUpload();$('#actionOK').hide();void(0);">
		上传</a> <a id="actionCacel" style="display: none;" href="javascript:$('#uploadify').uploadifyClearQueue();$('#actionCacel').hide();$('#actionOK').hide();void(0);">
			取消</a></div>
<div id="fileQueue">
</div>
<div id="example" style="display: none">
	<div class="photo">
		<div class="thumb">
			<img src="" alt="" />
		</div>
		<div class="editform">
			<table>
				<tr>
					<td>
						<%=PanelContext.DataSet.Tables[0].Columns[ Control.Params["col1"]].Label %>
					</td>
					<td>
						<input onblur="GetValues();" maxlength="<%=PanelContext.DataSet.Tables[0].Columns[ Control.Params["col1"]].MaxLength %>" />
					</td>
				</tr>
				<tr>
					<td>
						<%=PanelContext.DataSet.Tables[0].Columns[ Control.Params["col2"]].Label %>
					</td>
					<td>
						<input onblur="GetValues();" maxlength="<%=PanelContext.DataSet.Tables[0].Columns[ Control.Params["col2"]].MaxLength %>" />
					</td>
				</tr>
			</table>
		</div>
	</div>
</div>
<div id="multiEdit" style="display: none;">
	<input id="pageValues" class="pageValues" type="hidden" runat="server" />
</div>
<script type="text/javascript">
	function GetValues() {
		var list = '';
		$.each($("#multiEdit .photo"), function (i) {
			list += '<%=Control.Name %>:' + escape($(this).find(".thumb img").attr("src").replace("_thumb.", ".")) + '|<%=Control.Params["col1"] %>:' +
							escape($(this).find(".editform tr:eq(0) input:first").attr("value")) + '|<%=Control.Params["col2"] %>:' +
							 escape($(this).find(".editform tr:eq(1) input:first").attr("value"));
			if (i + 1 < $("#multiEdit .photo").length) list += '|';
		});
		$(".pageValues").val(list);
	}
</script>
