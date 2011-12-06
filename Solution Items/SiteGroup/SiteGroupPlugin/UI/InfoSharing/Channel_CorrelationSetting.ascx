<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Channel_CorrelationSetting.ascx.cs"
    Inherits="We7.Plugin.SiteGroupPlugin.InfoSharing.Channel_CorrelationSetting" %>
<%@ Register Assembly="We7.CMS.UI" Namespace="We7.CMS.Controls"
    TagPrefix="WEC" %>
    <%@ Register Assembly="We7.Plugin.DataSharing" Namespace="We7.CMS.Controls"
    TagPrefix="WEC2" %>
<div>
    <WEC:MessagePanel ID="Messages" runat="server">
    </WEC:MessagePanel>
</div>

<script type="text/javascript" language="javascript">
    function delSelected() {
        var ChannelSelected = document.getElementById("<%=ChannelSelected.ClientID %>");
        var lstindex = ChannelSelected.selectedIndex;
        if (lstindex >= 0) {
            var v = ChannelSelected.options[lstindex].value + ";";
            ChannelSelected.options[lstindex].parentNode.removeChild(ChannelSelected.options[lstindex]);
        }
    }

    function addChannelSelected() {
        var ChannelSelected = document.getElementById("<%=ChannelSelected.ClientID %>");
        var SiteDropDownList = document.getElementById("<%=SiteDropDownList.ClientID %>");
        var siteIndex = SiteDropDownList.selectedIndex;
        var selectValue = SiteDropDownList.options[siteIndex].value + "→";
        selectValue = selectValue + document.getElementById("ChannelSelectSelFormValue").value;
        var selectText = SiteDropDownList.options[siteIndex].text + "→";
        var length = ChannelSelected.options.length;

//        if (length > 11) {
//            alert("暂最多只能选择12个栏目！");
//            return;
//        }

        var src = ChannelSelected.options;
        for (var i = 0; i < src.length; i++) {
            if (src[i].value == selectValue) {
                alert("您已经选择过此栏目！");
                return;
            }
        }

        for (var i = 0; i < ChannelSelectformObjs.length; i++) {
            if (ChannelSelectformObjs[i].selectedIndex != -1) {
                var index = ChannelSelectformObjs[i].selectedIndex;
                var value = ChannelSelectformObjs[i].options[index].text;
                if (i == 0) {
                    selectText = selectText + value;
                } else {
                    selectText = selectText + "/" + value;
                }
            }
        }

        var tmpOption = new Option(selectText, selectValue);
        src.add(tmpOption);
    }

    function SaveData() {
        var ChannelSelected = document.getElementById("<%=ChannelSelected.ClientID %>");
        var tmp = ChannelSelected.options;
        var strValue = "";
        var strText = "";
        for (var i = 0; i < tmp.length; i++) {
            if (i == 0) {
                strValue = tmp[i].value;
                strText = tmp[i].text;
            }
            else {
                strValue = strValue + "||" + tmp[i].value;
                strText = strText + "||" + tmp[i].text;
            }
        }

        //如果没有任何栏目关联则不执行保存
        if (strValue == "") {
            //return false;
        }

        var ListValue = document.getElementById("<%=ListValue.ClientID %>");
        var ListText = document.getElementById("<%=ListText.ClientID %>");
        ListValue.value = strValue;
        ListText.value = strText;

        return true;
    }
</script>

<div id="conbox">
    <dl>
        <dt><span id="tipTitle" runat="server">»栏目共享通道设置</span><br />
            <img src="/admin/images/bulb.gif" align="absmiddle" alt="" />
            <label class="block_info">
                设置源站点栏目与目标站点栏目的映射关系，从而建立栏目共享直通通道。
            </label>
        </dt>
        <dd>
            <table class="personalForm" cellpadding="0" cellspacing="0">
                <tr runat="server" id="visibleTR" visible="false">
                    <td class="formTitle">
                        选择同步方式：
                    </td>
                    <td class="formValue">
                        <asp:CheckBox ID="IfAutoSharingCHK" Text="是否自动同步信息" AutoPostBack="false" runat="server" />
                    </td>
                </tr>
                <tr style="display:none;">
                    <td class="formTitle">
                        自动匹配用户：
                    </td>
                    <td class="formValue">
                        <asp:CheckBox ID="IfAutoUseringCHK" Text="是否自动匹配关联用户" AutoPostBack="false" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="formTitle">
                        选择目标站点：
                    </td>
                    <td class="formValue">
                        <asp:DropDownList ID="SiteDropDownList" runat="server" Style="width: 180px" OnSelectedIndexChanged="SiteDropDownList_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="formTitle">
                        目标站点下栏目：
                    </td>
                    <td class="formValue">
                        <label class="block_info">
                            请选择想要指定的栏目，点击【添加】，允许建立多个通道。
                        </label>
                        <br />
                        <WEC2:ChannelCascade ID="ChannelSelect" runat="server" />
                        <input type="button" id="SelectChannel" name="SelectChannel" value="↓添加" onclick="addChannelSelected();" />
                    </td>
                </tr>
                <tr>
                    <td class="formTitle">
                    <br />
                        共享通道列表：
                    </td>
                    <td class="formValue">
                        <label class="block_info">
                            每一条为一个独立的共享通道，【删除】可取消。
                        </label>
                        <input type="button" id="DeleteIndustry" name="DeleteIndustry" value="×删除" onclick="delSelected()" />
                        <br />
                        <asp:ListBox ID="ChannelSelected" runat="server" Width="320px" Rows="12"></asp:ListBox>
                    </td>
                </tr>
            </table>
            <div>
                <br />
                <input class="Btn" type="submit" value="保存共享关联" onserverclick="SaveSiteButton_Click"
                    runat="server" id="SaveSiteButton" onclick="return SaveData();" />
            </div>
            <div style="display: none">
                <asp:TextBox runat="server" ID="ListValue" EnableViewState="false"></asp:TextBox>
                <asp:TextBox runat="server" ID="ListText" EnableViewState="false"></asp:TextBox>
            </div>
        </dd>
    </dl>
</div>
