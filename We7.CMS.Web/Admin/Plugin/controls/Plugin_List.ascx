<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Plugin_List.ascx.cs"
    Inherits="We7.CMS.Web.Admin.Plugin.controls.Plugin_List" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls" TagPrefix="WEC" %>
<link rel="Stylesheet" href="" id="scrollshow" type="text/css" />

<script src="/Admin/Ajax/Mask.js" type="text/javascript"></script>

<script src="/Admin/Ajax/Prototype/prototype.js" type="text/javascript"></script>

<meta http-equiv="Content-Type" content="html/text; charset=utf-8" />

<script type="text/javascript" src="/Admin/Ajax/Ext2.0/adapter/ext/ext-base.js"></script>

<script type="text/javascript" src="/Admin/Ajax/Ext2.0/ext-all.js"></script>

<script type="text/javascript" src="/Install/js/Plugin.js"></script>

<script type="text/javascript">
        var mask=new MaskWin();
    
        function SelectAll(checkboxname,checked) {
            var els = document.getElementsByName(checkboxname);
            if (els && els.length > 0) {
                for (var i = 0; i < els.length; i++) {
                    els[i].checked = checked;
                }
            }
        }
        
        function submitSingleAction(action,type)
        {
            var param={};
            param.action=action;
            param.plugin=type;
            param.pltype="<%=PluginType %>".toLowerCase();
           
            switch(action)
            {
                case "remoteinstall":
                    param.title="安装插件";
                    param.message="安装成功！";
                    break;
                case "remoteupdate":
                    param.message="更新成功！";
                    param.title="更新插件";
                    break;
                case "insctr":
                    param.message="安装成功！";
                    param.title="安装控件";
                    break;
            }
            new MaskWin().showMessageProgressBar(param);
            return false;
        }
        
        function buildParam(elName)
        {
            var param="";
            var list=document.getElementsByName(elName);            
            for(var i=0;i<list.length;i++)
            {
                if(list[i].checked)
                    param+=list[i].value+",";
            }
            if(param.length>0)
                param=param.substr(0,param.length-1);
            return param;
        }
    
</script>

<WEC:MessagePanel ID="Messages" runat="server">
</WEC:MessagePanel>
<div id="conbox">
    <dl>
        <dt>»<asp:Literal ID="TitleLiteral" runat="server"></asp:Literal><br>
            <dd>
                <div>
                    <div id="plugin" class="toolbar2">
                    </div>
                    <br />
                    <div style="min-height: 35px; width: 100%">
                        <asp:GridView ID="PluginListGridView" runat="server" AutoGenerateColumns="false"
                            CssClass="List" GridLines="Horizontal" RowStyle-VerticalAlign="Top">
                            <AlternatingRowStyle CssClass="alter" />
                            <Columns>
                                <asp:BoundField DataField="Name" HeaderText="名称" ItemStyle-Width="120px" />
                                <asp:BoundField DataField="Version" HeaderText="版本" ItemStyle-Width="50px" />
                                <asp:BoundField DataField="Author" HeaderText="作者" ItemStyle-Width="60px" />
                                <asp:TemplateField HeaderText="描述">
                                    <ItemStyle Width="500px" />
                                    <ItemTemplate>
                                        <div style="cursor: pointer; width: 100%;" onclick='new MaskWin().showDetails("PluginDetails.aspx?key=<%# Eval("Directory") %>&remote=1&pltype=<%=PluginType %>","<%# Eval("Name") %>");return false;'>
                                            <%# Eval("Description") %>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="缩略图" ItemStyle-Width="70px">
                                    <ItemTemplate>
                                        <div style="padding: 2px; border: solid 1px #e5e5e5;">
                                            <img style="cursor: hand;" src='<%=RemoteUrl %><%# Eval("Directory","/PluginCollage/Temp/{0}/logo.gif").ToString()+Eval("Thumbnail") %>'
                                                onclick='new MaskWin().showDetails("PluginDetails.aspx?key=<%# Eval("Directory") %>&remote=1&tab=3&pltype=<%=PluginType %>","<%# Eval("Name") %>");return false;'
                                                style="width: 100px; height: 100px;" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="操作">
                                    <ItemStyle Width="100px" />
                                    <ItemTemplate>
                                        <a href="#" onclick='return submitSingleAction("<%# GetActionText(Eval("Directory"))%>",<%# Eval("Directory","\"{0}\"") %>)'>
                                            <%# GetProcessText(Eval("Directory")) %></a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="pagination" style="text-align: right">
                        <ul class="subsubsub">
                            <asp:Literal ID="PageLiteral" runat="server"></asp:Literal>
                        </ul>
                        <WEC:Pager ID="Pager" PageSize="10" runat="server" OnFired="Pager_Fired" />
                    </div>
                </div>
            </dd>
        </dt>
    </dl>
</div>
