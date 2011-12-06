<!--#### name="普通模糊搜索" type="system" version="1.0" created="2009/11/30" 
desc="普通模糊搜索框：仅包含输入框、搜索按钮" author="We7 Group" #####-->
<%@ Control Language="C#" AutoEventWireup="true" Inherits="We7.CMS.WebControls.BaseWebControl" %>
<script type="text/C#" runat="server">
    public string CssClass;
    public string SubmitTitle;
    public int Index;
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        IncludeJavaScript("");
    }
</script>
<script type="text/javascript">
	$(function()
	{
		$('#KeyWord<%=Index %>').bind('keyup',function(event) {  
          if(event.keyCode==13){  
           window.location="/gsearch.aspx?keyword="+encodeURIComponent(this.value);
           }
       });
       $('#Search<%=Index %>').click(function() {
           window.location="/gsearch.aspx?keyword="+encodeURIComponent($('#KeyWord<%=Index %>').val());
       });
	});
</script>
<ul class="SearchBar_<%= CssClass %>">
<li class="keyword"><input type="text" id="KeyWord<%=Index %>" class="keyword"  onblur="if(this.value==''){this.value=this.defaultValue;}"
onfocus="if(this.value==this.defaultValue){this.value='';}" value="关键词"  /></li>
<li class="search"><input type="button" id="Search<%=Index %>"  class="keyword" value="<%=SubmitTitle %>" /></li>
</ul>

