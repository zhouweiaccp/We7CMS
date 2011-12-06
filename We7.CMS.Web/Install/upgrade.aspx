<%@ Page Language="c#" AutoEventWireup="false" Inherits="We7.CMS.Install.upgrade" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<%=header%>
<body class="pubbox_login">
    <script language="javascript" type="text/javascript" src="/scripts/jquery/jquery-1.4.2.min.js"></script>
    <script language="javascript" type="text/javascript" src="/scripts/we7/we7.loader.js">
        we7("span[rel=xml-hint]").help();
        we7('.tipit').tip();
    </script>
    <script type="text/javascript">
        function ShowMoreFiles(name, obj) {
            var trlist = $(name);
            var isShow = $(obj).attr("show");
            $.each(trlist, function (i) {
                if (isShow == "t")
                    $(this).attr("style", "");
                else
                    $(this).css("display", "none");
            });
            $(obj).attr("show", isShow == "t" ? "f" : "t");
        }

        function CancelBackUp(obj) {
            if (!obj.attr("checked")) {
                if (!confirm('亲,我们强烈建议您升级前进行备份，谨防网站不能正常运行，您确定要取消备份吗？')) {
                    obj.attr("checked", true);
                }
            }
        }

        $(document).ready(function () {
            $("#<%=BackUpCheckBox.ClientID %>").live("change", function () {
                CancelBackUp($(this));

            });
            UpdateFiles();

            $("#ClearOldCheckBox").live("change", function () {
                if ($("#ClearOldCheckBox").attr("checked")) {
                    $("#divFiles").show("slow");
                }
                else {
                    $("#divFiles").hide("slow");
                }
                $("input[name='delFiles']").each(function (i) {
                    $(this).attr("checked", $("#ClearOldCheckBox").attr("checked"));
                });
            });


        });
        function UpdateFiles() {
            var files = $("#files").val();
            if (files != "") {
                files = eval("(" + files + ")");
                if (files.Exist) {
                    var str = "<table cellSpacing='0' cellPadding='0' width='90%' border='0' bgcolor='#f8f8f8' style='font-size:12px;'>   "
                    var count = 5;
                    $.each(files.Files, function (i) {
                        var t = files.Files[i];

                        if (i == count) {
                            str += "<tr ><td width='20px'></td><td > <a href='javascript:void(0)' onclick=\"ShowMoreFiles('.moreDelFiles',this)\" show='t'>显示全部</a></td></tr>";
                        }
                        str += "<tr " + (i >= count ? "style='display:none;' class='moreDelFiles'" : "") + "><td width='20px'><input type='checkbox' name='delFiles' value='" + t.Path + "' checked='checked' /></td><td>" + (t.Type == "File" ? "<img src='images/file.gif' />" : "<img src='images/folder.gif' />") + "</td><td>" + t.Path + "</td></tr>";

                    });
                    str += "</table>";

                    $("#divFiles").append(str);
                }
            }
            else {
                $("#clearDiv").hide();
            }
        }
    </script>
    <form id="Form1" method="post" runat="server">
    <input type="hidden" id="files" />
    <div>
        <table width="700" border="0" align="center" cellpadding="0" cellspacing="12" bgcolor="#999"
            class="login">
            <tr>
                <td bgcolor="#ffffff">
                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td colspan="2">
                                <table width="100%" border="0" cellspacing="0" cellpadding="8">
                                    <tr>
                                        <td align="left">
                                            <h1>
                                                升级<%=ProductBrand%>站点引擎</h1>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-left: 80px; padding-right: 20px;">
                                <label id="ShowErrorLabel" runat="server" style="display: none; color: #FF3300; font-size: x-large;
                                    font-weight: bold; width: 80%; height: 120px;">
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" width="180">
                                <%=logo%>
                            </td>
                            <td valign="top" width="520">
                                <asp:Panel runat="server" ID="NewversionPanel" Visible="false" Font-Bold="true">
                                    <h2>
                                        <asp:Label runat="server" ID="NewVersionLabel"></asp:Label></h2>
                                    <p style="margin-bottom: 10px">
                                        <asp:Button ID="DownloadInstallButton" runat="server" Text="自动升级" Enabled="true"
                                            class="bprimarypub80"></asp:Button>
                                        <asp:HyperLink ID="DownloadLocalHyperLink" runat="server" Text="下载升级包到本地" Enabled="true"
                                            class="bprimarypub80"></asp:HyperLink>
                                    </p>
                                    <hr style="height: 1px; text-align: left" />
                                </asp:Panel>
                                <div>
                                    请先在本地文件夹下选择 .ZIP更新包，或.DLL文件，.XML数据库结构文件，然后分两步点击按钮进行更新：（1）上传；（2）更新<br />
                                    <asp:FileUpload runat="server" ID="UpdateFileUpload" CssClass="Btn" />
                                    <p>
                                        <br />
                                        （1）
                                        <asp:Button ID="UploadButton" runat="server" Text="上传更新包" Enabled="true" CssClass="Btn"
                                            OnClientClick="we7.loading('操作中，请稍候');return true;"></asp:Button>
                                    </p>
                                    <p>
                                        <asp:Label runat="server" ID="UploadSummary"></asp:Label>
                                    </p>
                                    <p>
                                        <asp:Literal runat="server" ID="UnZipLiteral"></asp:Literal>
                                    </p>
                                    <asp:Panel runat="server" ID="BackUpPanel" Visible="false">
                                        <br />
                                        <asp:CheckBox runat="server" ID="BackUpCheckBox" Text="升级前备份" Checked="true" /><span
                                            style="color: Gray">（为什么要备份？<span rel="xml-hint" title="如果您已经对We7Cms进行了更改，请升级前进行备份，谨防数据丢失，不能正常运行!<br/>备份文件将以*.zip的格式存储在：根目录/_backup/update/"></span>）
                                        </span>
                                        <br />
                                        <div id="clearDiv">
                                        <asp:CheckBox runat="server" ID="ClearOldCheckBox" Text="删除冗余文件" Checked="true" />
                                        <span style="color: Gray">（什么是冗余文件？<span rel="xml-hint" title="冗余文件是指：某些文件在新版中已被删除、改名、合并等，在以后的版本中不再使用的文件!<br/>如果您对这些文件进行了更改，请取消选择，手动进行合并!"></span>）
                                        </span>
                                        <div id="divFiles">
                                        </div>
                                        </div>
                                    </asp:Panel>
                                    <br />
                                    <p>
                                        （2）
                                        <asp:Button ID="CopyFilesButton" runat="server" Text="开始更新" Enabled="true" class="Btn"
                                            OnClientClick="if(confirm('该操作需要对文件及文件夹有写入的权限，请操作前确认是否有此权限？')){we7.loading('操作进行中，耗费时间较长，请耐心等候!',{autoHide:false});return true}else{return false};"></asp:Button>
                                    </p>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <div style="margin: 10px 20px 30px 10px; text-align: right">
                                
                                    <input type="button" onclick="javascript:window.location.href='upgrade-db.aspx?from=upgrade.aspx';"  value="下一步" class="bprimarypub80" title="下一步：重建数据库" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <%=footer%>
    </form>
</body>
</html>
