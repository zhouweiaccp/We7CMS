<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Panel_List.ascx.cs"
    Inherits="We7.CMS.Web.Admin.ContentModel.Controls.Admin_List" %>
<div id="stage" class="afi">
    <div id="side">
        <ul id="column">
            <li><a href="#closs" id="addColumn">列</a> </li>
            <li><a href="#props" id="fieldProperties">属性</a></li>
            <li><a href="#pageProperties" id="pageProperty">页面属性</a></li>
        </ul>
        <div id="closs">
            <ul style="text-align: center">
                <li><a href="javascript:void(0)" id="ddt" title="点击即可自动增加"><b>右边查询框增加一项</b></a></li>
                <div id="sdiv">
                    <ul>
                    </ul>
                </div>
                <li><a href="javascript:void(0)" id="btnaddcls" title="点击即可自动增加"><b>右边列表增加一项</b></a></li>
                <!--   <li><a href="javascript:;" id="btnaddclss" title="一键生成列表">一键添加</a></li>-->
                <li>
                    <div id="columContent" style="display: none;">
                        <ul>
                        </ul>
                    </div>
                </li>
            </ul>
        </div>
        <div id="props">
            <div id="pros">
            </div>
        </div>
        <div id="pageProperties">
            <div id="pProperties">
                <table>
                    <tr>
                        <td>
                            列表模式：
                        </td>
                        <td>
                            <select id="ListMode">
                                <option value="0">传统列表</option>
                                <option value="1">矩阵行列</option>
                            </select>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            启用：
                        </td>
                        <td>
                            <select id="IsEnable">
                                <option value="false">否</option>
                                <option value="true">是</option>
                            </select>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="clear">
        </div>
        <div id="drop">
            不想要的→&nbsp;<br />
            拖进来吧↓&nbsp;<br />
            <img id="dropBag" src="images/del.png" alt="可以把右边的列或查询项拖到这里删除" style="text-align: left" />
        </div>
    </div>
    <div id="main">
        <div class="info">
            <h2>
                <asp:Literal runat="server" ID="FormTitleLiteral"></asp:Literal></h2>
            <div >
                <table cellspacing="0">
                    <tbody>
                        <tr>
                            <td style="width: 180px">
                                <table id="copyControls" runat="server" visible="false" style="width: 180px">
                                    <tr>
                                        <td style="width: 12px">
                                            <input type="checkbox" checked="checked" id="copyToUser" />
                                        </td>
                                        <td>
                                            <label for="copyToUser">
                                                同步更新会员中心列表</label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td align="left" style="width: 100px">
                                <a class="button" id="btnSave" href="javascript:void(0);">保存</a>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div>
                <asp:Literal runat="server" ID="FormDesciptionLiteral"></asp:Literal></div>
        </div>
        <div class="searchbar" id="queryFields">
        </div>
        <div class="clear">
        </div>
        <div class="columnbar">
            <ul id="columns">
            </ul>
        </div>
        <div class="pagerbar">
            <div class="Pager">
                翻页→ 共 7 页 · 第 1 页 · <span class="Current">1</span><a href="javascript:void(0)">2</a>
                <a href="javascript:void(0)">3</a><a href="javascript:void(0)">4</a> <a href="javascript:void(0)">
                    5</a><a href="javascript:void(0)">6</a><a href="javascript:void(0)">尾页 &gt;&gt;</a>
            </div>
        </div>
    </div>
    <div class="clear">
    </div>
</div>
<script src="/Admin/ContentModel/js/ListPanel.js" type="text/javascript"></script>
<script type="text/javascript" src="/Admin/Ajax/jquery/jquery.dimensions.js"></script>
<script language="javascript" type="text/javascript">
    var name = "#drop";
    var menuYloc = null;
    $(document).ready(function () {
        menuYloc = parseInt($(name).css("top")) || 0;
        $(window).scroll(function () {
            var height = $(name).height(), dHeight = $(document.body).height(), wHeight = $(window).height();
            var maxHeight = Math.max(dHeight, wHeight);
            var destHeight = menuYloc + $(document).scrollTop();
            var offset = (destHeight + height > maxHeight ? Math.max(maxHeight - height, 0) : destHeight);
            $(name).animate({ top: offset }, { duration: 500, queue: false });
        });
    });

</script>
