<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Search.SearchUI.cs" Inherits="We7.CMS.Web.Widgets.Search_SearchUI" %>
<script type="text/C#" runat="server">
    [Description("搜索文章")]
    [We7.CMS.WebControls.Core.ControlDescription(Name = "SearchArticle控件", Author = "老莫"
        , Created = "2011/03/22", Desc = "高级搜索文章控件", Version = "v1.0")]
    string MetaData;
</script>
<div class="<%= Css %>">
<div class="content">
    <div class="article_search ">
        文章搜索：<label><select size="1" id="nodeId" name="nodeId'">
            <option value="<%= Channel.ID %>">
                <%= Channel.Name%>
            </option>
            <% if (ChannelChildren != null && ChannelChildren.Count > 0)
               { %>
            <% foreach (Channel ch in ChannelChildren)
               { %>
            <option value="<%= ch.ID %>">&nbsp;&nbsp;<%= ch.Name%>
            </option>
            <%} %>
            <% } %>
        </select>
        </label>
        <label>
            <select size="1" id="fieldOption" name="fieldOption">
                <option value="<%= ParamTitle %>">文章标题</option>
                <option value="<%= ParamContent %>">文章内容</option>
                <option value="<%= ParamAuthor %>">文章作者</option>
                <option value="<%= ParamInputer %>">录 入 者</option>
                <option value="<%= ParamTag %>">关键字</option>
            </select></label>
        <label>
            <input name="Keyword" value="" size="30" maxlength="100" onfocus="this.value='';"
                class="inputxt" id="keyword_PowerEasy"></label>
        <label>
            <input type="button" onclick="OnSearchCheckAndSubmit();" name="Button" 
                class="input_button" id="Submit"></label>
        <script type="text/javascript">
            $('#keyword_PowerEasy').useKeypressSubmit($('#Submit'));
        </script>

        <script type="text/javascript" language="javascript">
            function OnSearchCheckAndSubmit() {
                var keyword = document.getElementById("keyword_PowerEasy").value;
                if (keyword == '' || keyword == null) {
                    alert("请填写您想搜索的关键词");
                    return;
                }
                else {
                    var nodeSel = document.getElementById("nodeId");
                    var fieldOptionSel = document.getElementById("fieldOption");
                    var channel = nodeSel.options[nodeSel.options.selectedIndex].value;
                    var fieldOption = fieldOptionSel.options[fieldOptionSel.options.selectedIndex].value;
                    window.location = "<%=PageUrl %>?<%=ParamChannel %>=" + channel + "&" + fieldOption + "=" + escape(keyword);
                }
            }
    </script>

    </div>
</div>
</div>