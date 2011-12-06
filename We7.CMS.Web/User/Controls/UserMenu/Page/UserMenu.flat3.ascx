<!--#### name="普通菜单(两级显示)" type="system" version="1.0" created="2009/11/30" 
desc="普通横菜单或竖菜单(两级显示)" author="We7 Group" #####-->
<%@ Control Language="C#" AutoEventWireup="true" Inherits="We7.CMS.WebControls.UserMenuProvider" %>
<%
    string[] list = new string[] { "{00000000-0001-0012-0012-000000000000}", "{00000000-0001-0012-0013-000000000000}", "{00000000-0001-0012-0011-000000000000}", "{68497f63-1ac3-4baf-84bf-a3eb3d1e67db}" };
%>
<div class="UserMenu_flat_<%= CssClass %>">
    <%if (Level > 1)
      { %>
    <h5>
        <%=ActiveMenu.Title%></h5>
    <%} %>
    <ul>
        <% if (Menus != null)
           { %>
        <% if (Menus.Count > 0)
           {
               for (int i = 0; i < Menus.Count; i++)
               {
                   if (ExcludeID(list, Menus[i].ID))
                       continue;
                        %>
                    <li>
                        <%if (Menus[i].ID == ActiveMenuID) //菜单被选中的样式
                          { %>
                        
                           <a href="<% =Menus[i].Href %>" title="<%=Menus[i].Title %>">
                            <b><%=Menus[i].Title%></b></a>
                        <%}
                          else    //菜单未被选中的样式
                          { %>
                        <a href="<% =Menus[i].Href %>" title="<%=Menus[i].Title %>">
                            <b><%=Menus[i].Title%></b></a>
                        <%}%>
                        <ul style="margin-left:15px;">
                            <%
                              List<We7.CMS.Common.MenuItem> tempList = GetSubMenuList(Menus, Menus[i].ID);
                              if(tempList != null && tempList.Count > 0)
                              {
                                  for (int j = 0; j < tempList.Count;j++ )
                                  {
                                      if (ExcludeID(list, tempList[j].ID))
                                          continue;   
                                      %>
                                        <li>
                                        <%if (tempList[j].ID == CurrentSubMenuID) //菜单被选中的样式
                                          { %>
                                          <a href="<% =tempList[j].Href %>" title="<%=tempList[j].Title %>" class="Classon"><%=tempList[j].Title%></a>
                                        <%}
                                          else    //菜单未被选中的样式
                                          { %>
                                        <a href="<% =tempList[j].Href %>" title="<%=tempList[j].Title %>">
                                            <%=tempList[j].Title%></a>
                                        <%}%>
                                        </li>
                                  <%}
                              }
                            %>
                        </ul>
                    </li>
                <%}
           }
           }%>
    </ul>
</div>
