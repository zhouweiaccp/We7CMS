using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace We7.CMS.WebControls
{
    public class TreeMenuProvider : BaseWebControl
    {
        #region 属性面板参数
        public string CssClass { get; set; }
        #endregion
        //protected global::System.Web.UI.WebControls.TreeView TreeViewSiteMap;



        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitJS();         
        }
        void InitJS()
        {
            List<string> paths = new List<string>();
            paths.Add("/admin/ajax/Ext2.0/adapter/ext/ext-base.js");
            paths.Add("/admin/ajax/Ext2.0/ext-all.js");
            //paths.Add("/admin/ajax/ChannelTree.js");
            We7.Framework.Util.JavaScriptManager.Include(paths.ToArray());
            //ClientScriptManager cs = this.Page.ClientScript;
            //string url = "/admin/ajax/Ext2.0/adapter/ext/ext-base.js";
            //if (!cs.IsClientScriptIncludeRegistered("ext-base_js"))
            //    cs.RegisterClientScriptInclude(this.GetType(), "ext-base_js", url);
            //url = "/admin/ajax/Ext2.0/ext-all.js";
            //if (!cs.IsClientScriptIncludeRegistered("ext-all_js"))
            //    cs.RegisterClientScriptInclude(this.GetType(), "ext-all_js", url);
            ////url = "/Ajax/Ext2.0/SessionProvider.js";
            ////if (!cs.IsClientScriptIncludeRegistered("SessionProvider_js"))
            ////    cs.RegisterClientScriptInclude(this.GetType(), "SessionProvider_js", url);
            //url = "/admin/ajax/ChannelTree.js";
            //if (!cs.IsClientScriptIncludeRegistered("ChannelTree_js"))
            //    cs.RegisterClientScriptInclude(this.GetType(), "ChannelTree_js", url);
            HtmlGenericControl cssLink = new HtmlGenericControl("link");
            cssLink.Attributes["rel"] = "stylesheet";
            cssLink.Attributes["type"] = "text/css";
            cssLink.Attributes["href"] = "/admin/ajax/Ext2.0/resources/css/ext-all.css";            
            this.Page.Header.Controls.Add(cssLink);
        }


        //#region 绑定TreeView控件
        ///// <summary>
        ///// 绑定树
        ///// </summary>
        //private void BindTree()
        //{
        //    System.Collections.Generic.List<Channel> channels = ChannelHelper.GetChannels(We7.We7Helper.EmptyGUID, false);
        //    for (int i = 0; i < channels.Count; i++)
        //    {
        //        Channel ch = channels[i];
        //        TreeNode rootNodeTemp = new TreeNode();
        //        //初始化一级节点
        //        InitNode(ch, rootNodeTemp);
        //        //初始化一级节点下所有节点
        //        InitTreeView(rootNodeTemp, ch.ID);
        //        this.TreeViewSiteMap.Nodes.Add(rootNodeTemp);
        //    }
        //}

        ///// <summary>
        ///// 初始化节点方法
        ///// </summary>
        ///// <param name="ch"></param>
        ///// <param name="node"></param>
        //private void InitNode(Channel ch,TreeNode node)
        //{            
        //    node.Text = ch.Name;
        //    node.Value = ch.ID;
        //    node.ImageUrl = "~/Admin/images/treeNode.jpg";            
        //    node.NavigateUrl = ch.FullUrl;
        //    node.Target = "_blank";
        //    node.Expanded = true;
        //}

        ///// <summary>
        ///// 初始化树
        ///// </summary>
        ///// <param name="pNode"></param>
        ///// <param name="parentID"></param>
        //public void InitTreeView(TreeNode pNode, string parentID)
        //{
        //    TreeNode node;
        //    List<Channel> childChannels = ChannelHelper.GetChannels(parentID);
        //    if (childChannels != null)
        //    {
        //        for (int j = 0; j < childChannels.Count; j++)
        //        {
        //            node = new TreeNode();
        //            //初始化节点
        //            InitNode(childChannels[j], node);
        //            //添加节点
        //            pNode.ChildNodes.Add(node);
        //            //递归调用
        //            InitTreeView(node, childChannels[j].ID);
        //        }
                    
        //    }
        //}
        //#endregion

    }
}
