<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="We7.CMS.Web.Admin.UserList" %>

<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<asp:content id="We7Content" contentplaceholderid="MyContentPlaceHolder" runat="server">
    <div>
        <h2 class="title">
            <asp:Image ID="LogoImage" runat="server" ImageUrl="~/admin/Images/icons_user.gif" />
            <asp:Label ID="NameLabel" runat="server" Text="会员管理">
            </asp:Label>
            <span class="summary">
                <asp:Label ID="SummaryLabel" runat="server" Text="">
                </asp:Label>
            </span>
        </h2>
        <div id="position">
            &nbsp;<asp:Label ID="FullPathLabel" runat="server" Text=""> </asp:Label>
        </div>
        <!--
        <div class="toolbar2">
            <li class="smallButton4">
            <asp:LinkButton id="lnkPass" runat="server">启用</asp:LinkButton>
            </li>
             <li class="smallButton4">
            <asp:LinkButton id="lnkStop" runat="server">禁用</asp:LinkButton>
            </li>
            <li class="smallButton8">
            <a href="javascript:sendmail()" id="sendmails" runat="server">给选中用户发邮件</a>
            </li>
        </div>-->
        <wec:messagepanel id="Messages" runat="server">
    </wec:messagepanel>
    </div>
    <!--
    <div style="display: table; width: 100%">
         <ul class="subsubsub">
            <asp:Literal ID="StateLiteral" runat="server"></asp:Literal>
         </ul>  
         <p class="search-box" >
	        <label class="hidden" for="user-search-input">搜索用户:</label>
	        <input type="text" class="search-input" id="KeyWord" name="KeyWord" value=""    />
	        <input type="button" value="搜索用户" class="button" id="SearchButton"    />
        </p>
    </div>
    <div style="min-height: 35px;">
        <asp:GridView ID="AccountsGridView" runat="server" AutoGenerateColumns="False"
            CssClass="List" GridLines="Horizontal" ShowFooter="True">
            <AlternatingRowStyle CssClass="alter" />
            <Columns>
                <asp:TemplateField HeaderText="名称">
                     <HeaderStyle Width="30px" />
                    <HeaderTemplate>
                        <input type="checkbox" name="chkall" onclick="this.checked?$('input[type=\'checkbox\']').attr('checked','checked'):$('input[type=\'checkbox\']').removeAttr('checked')" />
                    </HeaderTemplate>
                     <ItemTemplate>
                        <input type="checkbox" name="ids" value='<%#Eval("ID") %>' />
                     </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                <asp:TemplateField HeaderText="名称">
                 <HeaderStyle Width="130px" />
                     <ItemTemplate>
                     <a href="/admin/Permissions/AccountEdit.aspx?id=<%#Eval("ID") %>">
                     <%# Eval("LoginName") %>
                     </a>
                    </ItemTemplate>
                </asp:TemplateField>
                
                 <asp:BoundField DataField="Email" HeaderText="Email" ItemStyle-Width="200px" />
                 <asp:TemplateField HeaderText="注册时间">
                     <HeaderStyle Width="100px" />
                    <ItemTemplate >
                    <span style="font-size:12px">
                        <%# Eval("CreatedNoteTime")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
       
                <asp:TemplateField HeaderText="Email验证">
                     <HeaderStyle Width="60px" />
                    <ItemTemplate>
                        <%# ((int)Eval("EmailValidate"))==0?"<font style='color:red'>未验证</font>":"<font style='color:green'>通过</font>" %>
                    </ItemTemplate>
                </asp:TemplateField>
                
            <asp:TemplateField HeaderText="类型">
                     <HeaderStyle  />
                    <ItemTemplate>
                        <%# GetAllState(Eval("ModelState"), Eval("ModelName"), Eval("State"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="状态">
                    <HeaderStyle Width="90px" />
                    <ItemTemplate>
                        <%# ((int)Eval("State"))==0?"<font style='color:red'>禁用</font>":"<font style='color:green'>启用</font>" %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div class="pagination">
        <wec:urlpager id="UPager" runat="server" usespacer="False" usefirstlast="true" pagesize="10"
            firsttext="<< 首页" lasttext="尾页 >>" linkformatactive='<span class=Current>{1}</span>'
            urlformat="UserList.aspx?pg={0}" cssclass="Pager" />
    </div>-->
    <script type="text/javascript">
        $(function () {
            $('#KeyWord').bind('keyup', function (event) {
                if (event.keyCode == 13) {
                    window.location = "UserList.aspx?keyword=" + encodeURIComponent(this.value);
                }
            });
            $('#SearchButton').click(function () {
                window.location = "UserList.aspx?keyword=" + encodeURIComponent($('#KeyWord').val());
            });
            if (QueryString('keyword'))
                $('#KeyWord').val(QueryString('keyword'));
        });
</script>
    <script type="text/javascript" src="/scripts/we7/we7.loader.js"></script>
    <script type="text/javascript">
        we7.load.ready(function(){
        // 定义条件
		var FromSiteID = new we7.BindCondition("FromSiteID", we7.bindVerb.equals, "<%=SiteID%>");
		var userType=new we7.BindCondition("UserType", we7.bindVerb.equals, "<%=(int)OwnerRank.Normal%>"); 
        var CurrentState=null;
        var state={};
        <%switch (CurrentState)
			{
				case QueryType.ALL:
					break;
				case QueryType.WaitExamin:
					%>
                    state.name="ModelState";
                    state.value=0;
                    <% 
					break;
				case QueryType.Passed:
                    %>
                    state.name="ModelState";
                    state.value=1;
                    <%
					break;
				case QueryType.WaitValidate: 
                    %>
                    state.name="EmailValidate";
                    state.value=0;
					<%
					break;
            }%>
		// 定义要绑定的资源的目标
		var bindDestination = new we7.BindOption({
			tableName: "Account"
		    , fields: {"ID":{},"loginname":{},"Email":{},"Created":{},"EmailValidate":{},"UserType":{},"Created":{},
				"State":{
				  value:{"0":"禁用","1":"启用"}
				}
            }
			, sortField: "Created"
			, sortOrder: "desc"
            , rows:30
		});
		bindDestination.conditions.push(FromSiteID);
        bindDestination.conditions.push(userType);
        if(state.name!=undefined){
          CurrentState= new we7.BindCondition(state.name, we7.bindVerb.equals, state.value);
          bindDestination.conditions.push(CurrentState);
        }
		//绑定过程
		function bindData(){
			var options = {
				caption: "会员列表",
				height: 230,
                autowidth:true,
                rowNum:20,
                autoencode:false
			};
			
			we7("#ModelList").bind(bindDestination, options);
		}
		
		$(document).ready(function () {
			
			bindData();
			
		});
        });
		
    </script>
	<br />
    <table id="ModelList" style="display:none">
    <tr><td header="名称"><a href="AccountEdit.aspx?id=${ID}">${LoginName}</a></td><td header="Email">${Email}</td><td header="注册时间">{{html Created.substr(0,10)}}</td><td header="Email验证">{{if EmailValidate==0}}<font style='color:red'>未验证</font>{{else}}<font style='color:green'>通过</font>{{/if}}</td><td header="类型">{{if UserType==0}}管理用户{{else}}普通用户{{/if}}</td><td header="状态" editable="select" editkey="State">{{if State==0}}<font style='color:red'>禁用</font>{{else}}<font style='color:green'>启用</font>{{/if}}</td></tr>
    </table>
</asp:content>
