<%@ Page Language="C#"  AutoEventWireup="true" CodeBehind="ArticleTree.aspx.cs" Inherits="We7.CMS.Web.Admin.Addins.ArticleTree" Title="无标题页" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MyContentPlaceHolder" runat="server">
 <script type="text/javascript" src="/Scripts/jquery/jquery.jstree.js"></script>
 <style>
 html {
background-color:#fff!important;
}
 </style>
          <h2  class="title" >
            <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_article.gif" />
            <asp:Label ID="NameLabel" runat="server" Text="文章树管理">
            </asp:Label>
            <span class="summary">
                <asp:Label ID="SummaryLabel" runat="server" Text="直接拖动文章节点修改文章父子关系与排序 ">
                </asp:Label>
             </span>
             <div class="clear"></div>
        </h2> 
        <script type="text/javascript">
$(function () {
	$("#myTree").jstree({ 
		"json_data" : { 
				"ajax" : {
					"url" : "ajax/GetNodeAction.ashx?ChannelID=<%=OwnerID %>&ArticleID=<%=ArticleID %>",
					"data" : function (n) { 
						return { 
							"operation" : "get_children", 
							"id" : n.attr ? n.attr("id").replace("node_","") : "" 
						}; 
					}
				}
		},
		"plugins" : [ "themes", "json_data","ui", "crrm", "dnd" ],
		"themes" : {
			"theme" : "classic",
			"dots" : true,
			"icons" : true
		}
	    })
		.bind("move_node.jstree", function (e, data) {
			data.rslt.o.each(function (i) {
				$.ajax({
					async : false,
					type: 'POST',
					dataType: "json",
					url: "ajax/EditArticleNode.ashx",
					data : { 
						"operation" : "move_node", 
						"id" : $(this).attr("id").replace("node_",""), 
						"ref" : data.rslt.np.attr("id").replace("node_",""), 
						"position" : data.rslt.cp + i,
						"title" : data.rslt.name,
						"copy" : data.rslt.cy ? 1 : 0
					},
					success : function (r) {
						if(!r.status) {
							$.jstree.rollback(data.rlbk);
						}
						else {
							$(data.rslt.oc).attr("id", r.id);
							if(data.rslt.cy && $(data.rslt.oc).children("UL").length) {
								data.inst.refresh(data.inst._get_parent(data.rslt.oc));
							}
						}
					}
				});
			});
	});
});
</script>

<div id="myTree"  style="height:auto; padding:15px 0; margin-left:50px;"  ></div>

</asp:Content>
