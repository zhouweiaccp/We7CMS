<!--#### name="下拉菜单" type="system" version="1.0" created="2009/11/30" 
desc="多级下拉式菜单" author="We7 Group" #####-->
<%@ Control Language="C#" AutoEventWireup="true" Inherits="We7.CMS.WebControls.UserMenuProvider" %>

<script runat="server">
    protected override void OnLoad(EventArgs e)
    {
        MaxTreeClass = 2;
        Menus = GetMenuTree();
        IncludeJavaScript("jquery.bgiframe.js", "jquery.dimensions.js",
            "jquery.jdMenu.js", "SlideMenuReady.js");
    }

</script>

<%%>
<div  class="UserMenu_popdown_<%= CssClass %>">
<ul class="jd_menu  jd_menu_slate">
<%for (int i = 0; i < Menus.Count; i++)
  { %>
    <li <% if(Menus[i].Items!=null && Menus[i].Items.Count>0 ) { %> class='haveSubMenu' <%} %>>
    <%if (Menus[i].ID==CurrentMenuID)
      { %>
    <span class="active"><%=Menus[i].Name%></span>
    <%}
      else
      { %>
      <a  href="<%=Menus[i].Href %>"><%=Menus[i].Name%></a>
      <%} if (Menus[i].Items != null && Menus[i].Items.Count > 0)
           {%>
           <ul>
               <%for (int j = 0; j < Menus[i].Items.Count; j++)
               {%>

               <li <% if(Menus[i].Items[j].Items!=null && Menus[i].Items[j].Items.Count>0){ %> class='haveSubMenu' <%} %>>
               <a href="<%=Menus[i].Items[j].Href %>"><%=Menus[i].Items[j].Name%></a></li>
               <%}%>
           </ul>                
         <%  } %>
    </li>
    <%} %>
</ul>
</div>