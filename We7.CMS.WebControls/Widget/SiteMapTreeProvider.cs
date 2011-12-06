using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;
using System.Web.UI.WebControls;
using System.Data;

namespace We7.CMS.WebControls
{
    public class SiteMapTreeProvider : BaseWebControl
    {

        #region 属性面板参数

        /// <summary>
        /// 获取当前栏目对象
        /// </summary>
        public Channel CurrentChannel
        {
            get
            {
                string id = ChannelHelper.GetChannelIDFromURL();
                Channel ch = ChannelHelper.GetChannel(id, null);
                return ch; 
            }
        }

        List<Channel> lsChannels = new List<Channel>();
        public void InitChannels(Channel currentChannel)
        {
            if (currentChannel != null)
            {
                lsChannels.Add(currentChannel);
                Channel tempChanel = ChannelHelper.GetChannel(currentChannel.ID, null);
                InitChannels(tempChanel);
            }
        }


        public string CssClass { get; set; }
        #endregion
        protected global::System.Web.UI.WebControls.TreeView TreeViewSiteMap;

        protected void GetAllChildChannel(string parentID, string type)
        {
            List<Channel> childChannels = ChannelHelper.GetChannels(parentID);
            if (childChannels != null)
            {
                for (int j = 0; j < childChannels.Count; j++)
                {
                    if (type == "One")
                    {
                        Response.Write(String.Format("<a href=\"{0}\">{1}</a>", childChannels[j].FullUrl, childChannels[j].Name));
                    }
                    else
                    {
                        Response.Write(String.Format("<li><a href=\"{0}\">{1}</a></li>", childChannels[j].FullUrl, childChannels[j].Name));
                    }
                    GetAllChildChannel(childChannels[j].ID, type);
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            BindTree();
        }

        /// <summary>
        /// 绑定树
        /// </summary>
        private void BindTree()
        {
            TreeViewSiteMap.Nodes.Clear();
            System.Collections.Generic.List<Channel> channels = ChannelHelper.GetChannels(We7.We7Helper.EmptyGUID, false);
            for (int i = 0; i < channels.Count; i++)
            {
                Channel ch = channels[i];
                TreeNode rootNodeTemp = new TreeNode();
                //初始化一级节点
                InitNode(ch, rootNodeTemp);
                //初始化一级节点下所有节点
                InitTreeView(rootNodeTemp, ch.ID);
                this.TreeViewSiteMap.Nodes.Add(rootNodeTemp);
            }
        }

        /// <summary>
        /// 初始化节点方法
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="node"></param>
        private void InitNode(Channel ch,TreeNode node)
        {
            node.Text = ch.Name;            
            node.Value = ch.ID;
            node.ImageUrl = "~/Admin/images/treeNode.jpg";            
            node.NavigateUrl = ch.FullUrl;
            //node.Target = "_blank";
            node.Expanded = false;
            if (ch != null &&  CurrentChannel != null && ch.ID == CurrentChannel.ID)
            {
                ExpandedParent(node);
                node.Checked = true;
            }            
        }

        private void ExpandedParent(TreeNode node)
        {           
            node.Expanded = true;
            //node.ImageUrl = "~/Admin/images/treeNodeExpand.jpg";           
            TreeNode parrentNode = node.Parent;
            if(parrentNode != null)
            {
                ExpandedParent(parrentNode);
            }
            
        }

        public void InitTreeView(TreeNode pNode, string parentID)
        {
            TreeNode node;
            List<Channel> childChannels = ChannelHelper.GetChannels(parentID);

            if (childChannels != null)
            {
                for (int j = 0; j < childChannels.Count; j++)
                {
                    node = new TreeNode();    
                    //添加节点
                    pNode.ChildNodes.Add(node);
                    //初始化节点
                    InitNode(childChannels[j], node);
                    //递归调用
                    InitTreeView(node, childChannels[j].ID);
                }
                    
            }
        }

        protected void TreeViewSiteMap_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
            TreeNode node = (TreeNode)sender;
            if ((bool)node.Expanded)
            {
                node.ImageUrl = "~/Admin/images/treeNode.jpg";
            }
            else
            {
                node.ImageUrl = "~/Admin/images/treeNodeExpand.jpg";
            }
            TreeNode parentNode = node.Parent;
            if(parentNode != null)
            {
                foreach(TreeNode tempNode in parentNode.ChildNodes)
                {
                    if (tempNode == node)
                    {                       
                    }
                    else
                    {
                        tempNode.Expanded = false;
                    }
                }
            }
        }

    }
}
