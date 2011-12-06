<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Config_SiteSetting.ascx.cs"
    Inherits="We7.Plugin.SiteGroupPlugin.InfoSharing.Config_SiteSetting" %>
<%@ Register TagPrefix="WEC" Namespace="We7.CMS.Controls" Assembly="We7.CMS.UI" %>
<div>
    <WEC:MessagePanel ID="Messages" runat="server">
    </WEC:MessagePanel>
</div>

<script type="text/javascript" language="javascript">
    function onSaveSiteClick() {
       // debugger;
        var SiteReceiveAddsTextBox = document.getElementById("<%=SiteReceiveAddsTextBox.ClientID %>");
        var SiteReceiveDelsTextBox = document.getElementById("<%=SiteReceiveDelsTextBox.ClientID %>");
        var SiteSharingAddsTextBox = document.getElementById("<%=SiteSharingAddsTextBox.ClientID %>");
        var SiteSharingDelsTextBox = document.getElementById("<%=SiteSharingDelsTextBox.ClientID %>");

        var dels = null;
        var adds = null;

        var cs = document.getElementsByName("siteSharing");
        var us = document.getElementsByName("sharingUrl");

        if (cs) {
            if (cs.length) {
                for (var i = 0; i < cs.length; i++) {
                    if (cs[i].checked) {
                        adds = addStr(adds, cs[i].value, cs[i].title, us[i].value);
                    }
                    else {
                        dels = addStr(dels, cs[i].value, cs[i].title, us[i].value);
                    }
                }
            }
            else {
                if (cs.checked) {
                    adds = addStr(adds, cs.value, cs.title, us.value);
                }
                else {
                    dels = addStr(dels, cs.value, cs.title, us.value);
                }
            }
        }

        if (adds) { SiteSharingAddsTextBox.value = adds; } else { SiteSharingAddsTextBox.value = ""; }
        if (dels) { SiteSharingDelsTextBox.value = dels; } else { SiteSharingDelsTextBox.value = ""; }

        dels = null;
        adds = null;
        cs = null;
        cs = document.getElementsByName("siteReceive");
        us = null;
        us = document.getElementsByName("receiveUrl");

        if (cs) {
            if (cs.length) {
                for (var i = 0; i < cs.length; i++) {
                    if (cs[i].checked) {
                        adds = addStr(adds, cs[i].value, cs[i].title, us[i].value);
                    }
                    else {
                        dels = addStr(dels, cs[i].value, cs[i].title, us[i].value);
                    }
                }
            }
            else {
                if (cs.checked) {
                    adds = addStr(adds, cs.value, cs.title, us.value);
                }
                else {
                    dels = addStr(dels, cs.value, cs.title, us.value);
                }
            }
        }

        if (adds) { SiteReceiveAddsTextBox.value = adds; } else { SiteReceiveAddsTextBox.value = ""; }
        if (dels) { SiteReceiveDelsTextBox.value = dels; } else { SiteReceiveDelsTextBox.value = ""; }
    }

    function onDocumentLoad() {
        var SiteReceiveAddsTextBox = document.getElementById("<%=SiteReceiveAddsTextBox.ClientID %>");
        var SiteReceiveDelsTextBox = document.getElementById("<%=SiteReceiveDelsTextBox.ClientID %>");
        var SiteSharingAddsTextBox = document.getElementById("<%=SiteSharingAddsTextBox.ClientID %>");
        var SiteSharingDelsTextBox = document.getElementById("<%=SiteSharingDelsTextBox.ClientID %>");

        var cs = document.getElementsByName("siteSharing");
        if (cs) {
            var adds = SiteSharingAddsTextBox.value.split(";");
            if (cs.length) {
                for (var i = 0; i < cs.length; i++) {
                    cs[i].checked = isInArray(adds, cs[i].value);
                }
            }
            else {
                cs.checked = isInArray(adds, cs.value);
            }
        }

        cs = null;
        cs = document.getElementsByName("siteReceive");
        if (cs) {
            var adds = SiteReceiveAddsTextBox.value.split(";");
            if (cs.length) {
                for (var i = 0; i < cs.length; i++) {
                    cs[i].checked = isInArray(adds, cs[i].value);
                }
            }
            else {
                cs.checked = isInArray(adds, cs.value);
            }
        }
    }

    function isInArray(ar, s) {
        if (ar.length) {
            for (var i = 0; i < ar.length; i++) {
                if (ar[i].indexOf(s) >= 0) {
                    return true;
                }
            }
            return false;
        }
        else {
            return ar.indexOf(s) >= 0;
        }
    }

    function addStr(s, v, w, r) {
        if (s) {
            return s + ";" + v + "," + w + "," + r;
        }
        else {
            return v + "," + w + "," + r;
        }
    }
</script>

<div id="conbox">
    <dl>
        <dt><span id="tipTitle" runat="server">»数据共享通道目标站点设置</span><br />
            <img src="/admin/images/bulb.gif" align="absmiddle" alt="" />
            <label class="block_info">
                您想将信息共享给谁，请选择站点并保存：
            </label>
        </dt>
        <dd>
            <table class="personalForm" cellpadding="0" cellspacing="0">
                <tr style="display:none">
                    <td class="formTitle">
                        共享生效方式：
                    </td>
                    <td class="formValue">
                        <asp:CheckBox ID="ValidateStyle" Text="共享站点验证" runat="server" TextAlign="Left" />
                    </td>
                </tr>
                <tr>
                    <td class="formTitle">
                       <img src="/admin/images/shareOut.png" />
                    </td>
                    <td class="formValue">
                        <label class="block_info">
                            您将共享信息给哪些站点？
                        </label>
                        <br />
                        <asp:DataList ID="SiteListSharing" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                            RepeatColumns="2">
                            <ItemTemplate>
                                <input type="checkbox" name="siteSharing" title="<%# Eval("Name") %>" value="<%# Eval("ID") %>" />
                                <input value="<%# Eval("Url") %>/"
                                    type="text" style="display: none" name="sharingUrl" />
                                <label>
                                    <%# Eval("Name") %>
                                </label>
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
                <tr>
                    <td class="formTitle">
                        <img src="/admin/images/shareIn.png" />
                    </td>
                    <td class="formValue">
                        <label class="block_info">
                            谁会发给您信息？ （以下站点主动建立了发送到您站点的共享通道）
                        </label>
                        <br />
                        <asp:DataList ID="SiteListReceive" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                            RepeatColumns="3">
                            <ItemTemplate>
                                <input type="checkbox" name="siteReceive" title="<%# Eval("FromSiteName") %>" value="<%# Eval("FromSiteID") %>" disabled="disabled" /><label><%# Eval("FromSiteName") %></label>
                                <input value="<%# Eval("FromSiteID") %>/" type="text" style="display: none"
                                    name="receiveUrl" />
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
            </table>
            <div>
                <br />
                <input class="Btn" type="submit" runat="server" value="保存站点关联配置" onclick="javascript:onSaveSiteClick();"
                    onserverclick="SaveSiteButton_Click" />
            </div>
            <div style="display: none">
                <asp:TextBox ID="SiteReceiveAddsTextBox" runat="server" EnableViewState="false"></asp:TextBox>
                <asp:TextBox ID="SiteReceiveDelsTextBox" runat="server" EnableViewState="false"></asp:TextBox>
                <asp:TextBox ID="SiteSharingAddsTextBox" runat="server" EnableViewState="false"></asp:TextBox>
                <asp:TextBox ID="SiteSharingDelsTextBox" runat="server" EnableViewState="false"></asp:TextBox>
            </div>
        </dd>
    </dl>
</div>
