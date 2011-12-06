<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Channel_tag.ascx.cs"
    Inherits="We7.CMS.Web.Admin.controls.Channel_tag" %>
<div class="TagsList">
    <div id="conbox">
        <dl>
            <dt>»栏目标签<br />
                <img src="/admin/images/bulb.gif" align="absmiddle" />
                <label class="block_info">
                    栏目标签用于为栏目分组，和与文章关联。下面的系统标签与常用标签点击即可加入为本篇文章的标签。</label>
                <dd>
                    <ul id="tagList">
                        <asp:Literal ID="TagListLitiral" runat="server"></asp:Literal>
                    </ul>
                </dd>
                <dd style="clear:both">
                    <input class="txt" id="tagNameInput" name="tagNameInput" maxlength="20">
                    <input class="Btn" id="tagAddSubmit" type="button" value="添加" onclick="addTag(tagNameInput.value)"
                        hidefocus>
                    <p class="Hint">
                        标签必须是标准的中文或英文单词，且文字与字母间不允许有其他字符或空格。</p>
                </dd>
                <dd >
                    <h1>
                        系统字典标签</h1>
                    <div>
                        <asp:Literal ID="TagDictionaryLiteral" runat="server"></asp:Literal>
                    </div>
                </dd>
                <dd>
                    <h1>
                        本站常用标签 <span rel="xml-hint" title="常用标签：取出本站使用频率最高的标签"></span>
                    </h1>
                    <div class="usefulTags">
                        <input type="hidden" id="pi" value="2" />
                        <asp:Literal ID="CommonTagsLiteral" runat="server"></asp:Literal>
                        <img src="/admin/images/icon_down.gif" onclick="GetTags()" class="tipit" title="点击获取更多标签"
                            style="cursor: pointer; padding-left: 10px;" />
                    </div>
                </dd>
            </dt>
        </dl>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $(".Del").each(function (i) {
            $(this).bind("click", function (event) {
                removeTag($(this).attr("title"), event);
            });
        });
    });
</script>
<asp:Literal ID="ChannelIDHidden" runat="server"></asp:Literal>
