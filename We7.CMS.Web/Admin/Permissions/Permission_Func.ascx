<%@ Control Language="C#" AutoEventWireup="true" Codebehind="Permission_Func.ascx.cs"
    Inherits="We7.CMS.Web.Admin.Permission_Func" %>
<%@ Register TagPrefix="WEC" Namespace="We7.CMS.Controls" Assembly="We7.CMS.UI" %>
<div>
    <WEC:MessagePanel ID="Messages" runat="server">
    </WEC:MessagePanel>
</div>
<%--<script type="text/javascript" src="<%=AppPath%>/ajax/jquery/jquery.js"></script>
--%>	
    <script type="text/javascript" src="<%=AppPath%>/ajax/jquery/jquery.tree.js"></script>
	<script type="text/javascript" src="<%=AppPath%>/ajax/jquery/plugins/jquery.tree.checkbox.js"></script>
	<script type="text/javascript">
	    $(function () { 
		    $("#menuTree").tree({
			    ui : {
				    theme_name : "checkbox"
			    },
			    plugins : { 
				    checkbox : { }
			    }
		    });
	    });
	      //取得选中的菜单id 
      function getMenuIds(){
       //取得所有选中的节点，返回节点对象的集合
       var menu = jQuery.tree.plugins.checkbox.get_checked($.tree.reference($("#menuTree")));
       //得到节点的id，拼接成字符串
       var ids="";
       for(i=0;i<menu.size();i++){
        ids += menu[i].id+";";
        }
	    return(ids);
      }

    function onSaveClick() {
        var AddsTextBox;
        var SaveButton;
        AddsTextBox= document.getElementById("<%=AddsTextBox.ClientID %>");
        SaveButton= document.getElementById("<%=SaveButton.ClientID %>");
        AddsTextBox.value = getMenuIds();
        SaveButton.click();
    }
    
</script>

<div id="conbox">
    <dl>
        <dt><span id="tipTitle" runat="server">»功能权限</span><br />
            <img src="/admin/images/bulb.gif" align="absmiddle" alt="" />
            <label id="tipValue" class="block_info" runat="server">
                此处对本角色进行功能权限设置工作！</label>
        </dt>
        <dd>
            <div>
                <asp:CheckBox ID="ApplyPermissionToSubChannelsCheckBox" runat="server" Text="应用该权限到子栏目"
                    Visible="False" />
            </div>
            <div id="menuTree">
    		        <ul>
			        <li id="root" class="open "><a href="#"  class="open "><ins>&nbsp;</ins>全部</a>
                    <%=MenuTreeHtml %>
                    </li>
                    </ul>
            </div>
            <br />
            <div style="margin-left:200px; margin-top:30px">
            <a href="javascript:onSaveClick();" class="button">选好了，保存一下</a>
            </div>
        </dd>
    </dl>
</div>
<div style="display: none">
    <asp:TextBox ID="AddsTextBox" runat="server"></asp:TextBox>
    <asp:Button ID="SaveButton" runat="server" OnClick="SaveButton_Click" />
</div>
