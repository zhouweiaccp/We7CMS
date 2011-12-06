<%@ Control Language="C#" AutoEventWireup="true" Codebehind="Account_Roles.ascx.cs"
    Inherits="We7.CMS.Web.Admin.Permissions.Account_Roles" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WEC" %>
<div>
    <WEC:MessagePanel ID="Messages" runat="server">
    </WEC:MessagePanel>
</div>

<script language="javascript" type="text/javascript">
    function onBodyLoad() {
        var radios = document.getElementsByName("Radios");
       
        if(radios) {
            var ovs =document.getElementById("<%=ValuesTextBox.ClientID %>");
            var list = ovs.value.split(";");
            if(list) {   
                var i, j;
                for(i=0; i<radios.length; i++) {
                    r = radios[i];
                    for(j=0; j<list.length; j++) {
                        o = list[j];
                        if(r.value == o) {
                            r.checked = true;
                        }
                    }
                }
            }
        }
    }

    function onSaveButtonClick() {
    
        var ret = null;
        var radios = document.getElementsByName("Radios");
        if(radios) {
            var i, r;
            for(i=0; i<radios.length; i++) {
                r = radios[i];
                if(r.checked) {
                    if(ret == null) {
                        ret = "";
                    }
                    else {
                        ret = ret + ";";
                    }
                    ret = ret + r.value;
                }
            }
        }
        var vt = document.getElementById("<%=ValuesTextBox.ClientID %>");
        vt.value = ret;
//        var SearchButton=document.getElementById("<%=SaveButton.ClientID %>");
//        SearchButton.click();
    }

</script>

<div id="conbox">
    <dl>
        <dt>»角色设置<br />
            <img src="/admin/images/bulb.gif" align="absmiddle" alt="" />
            <label class="block_info">
                此处进行当前用户的角色设置工作，同一用户可以设置多个角色。</label>
        </dt>
        <dd>
            <asp:GridView ID="personalForm" runat="server" AutoGenerateColumns="False" ShowFooter="True" GridLines="Horizontal"  CssClass="List">
                <Columns>
                    <asp:TemplateField>
                        <ItemStyle Width="10px" />
                        <ItemTemplate>
                            <input type="checkbox" name="Radios" value='<%#DataBinder.Eval(Container.DataItem, "ID") %>'
                                title='<%#DataBinder.Eval(Container.DataItem, "Name") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" DataFormatString="{0}" HeaderText="名称" />
                    <asp:BoundField DataField="Description" DataFormatString="{0}" HeaderText="描述" />
                </Columns>
            </asp:GridView>
            <div class="pagination">
                <p>
                    <WEC:Pager ID="Pager" PageSize="15" PageIndex="0" runat="server" />
                </p>
            </div>
            <div>
                <input class="Btn" type="submit" value="保存当前信息" runat="server" onclick="javascript:onSaveButtonClick();" onserverclick="SaveButton_Click"  />
            </div>
        </dd>
    </dl>
</div>
<div style="display: none">
    <asp:TextBox ID="ValuesTextBox" runat="server"></asp:TextBox>
    <asp:Button ID="SaveButton" runat="server" OnClick="SaveButton_Click" />
</div>
