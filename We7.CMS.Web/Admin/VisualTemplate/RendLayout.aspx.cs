using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using HtmlAgilityPack;
using VisualDesign.Module;
using We7.Framework.Util;

namespace We7.CMS.Web.Admin.VisualTemplate
{
    /// <summary>
    /// 可视化设计布局AJAX操作
    /// </summary>
    public partial class RendLayout : System.Web.UI.Page
    {
        #region Property

        /// <summary>
        /// 布局ID
        /// </summary>
        protected string LayoutId
        {
            get;
            set;
        }
        /// <summary>
        /// 布局文件虚拟路径
        /// </summary>
        protected string LayoutVirtualPath
        {
            get;
            set;
        }

        /// <summary>
        /// 模板组名称
        /// </summary>
        protected string TemplateGroup
        {
            get;
            set;
        }
        protected string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// 兄弟节点ID
        /// </summary>
        protected string NextId
        {
            get;
            set;
        }
        /// <summary>
        /// 父节点ID
        /// </summary>
        protected string Target
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// 页面加载
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            //获取参数
            LayoutId = RequestHelper.Get<string>("id");
            LayoutVirtualPath = RequestHelper.Get<string>("path");
            Target = RequestHelper.Get<string>("target");
            NextId = RequestHelper.Get<string>("nextid");
            TemplateGroup = RequestHelper.Get<string>("folder");
            FileName = RequestHelper.Get<string>("file");

            //添加布局控件到页面
            AddLayout(LayoutVirtualPath, LayoutId);
            //添加布局控件到文件
            AddNode();

        }

        #region Private Method

        /// <summary>
        /// 添加布局DOM到文件
        /// </summary>
        protected void AddNode()
        {
            VisualDesignFile vdFile = new VisualDesignFile(TemplateGroup, FileName);

            string layoutHtml=GetLayoutHtml();
            var newNode = HtmlNode.CreateNode(layoutHtml);
            vdFile.Insert(Target, newNode, NextId, null);
            vdFile.SaveDraft();
        }
        /// <summary>
        /// 获取添加ID后的布局文件HTML
        /// </summary>
        /// <returns></returns>
        private string GetLayoutHtml()
        {
            //load 文件
            HtmlDocument doc = new HtmlDocument();
            //设置相关参数
            doc.OptionAutoCloseOnEnd = true;
            doc.OptionCheckSyntax = true;
            doc.OptionOutputOriginalCase = true;

            doc.Load(HttpContext.Current.Server.MapPath(LayoutVirtualPath));

            //遍历所有节点,给相应的Node赋ID
            IEnumerable<HtmlNode> nodes = doc.DocumentNode.DescendantNodes();

            int i = 1;
            foreach (HtmlNode node in nodes)
            {
                if (node.NodeType == HtmlNodeType.Element)
                {
                    //Holder
                    if (string.Compare(node.Name, "we7design:we7layout", true) == 0)
                    {
                        node.SetAttributeValue("id", LayoutId);
                    }
                    if (string.Compare(node.Name, "we7design:We7LayoutColumn", true) == 0)
                    {
                        node.SetAttributeValue("id", string.Format("{0}_cloumn{1}", LayoutId, i.ToString()));
                        i++;
                    }
                }
            }

            return doc.DocumentNode.InnerHtml;
        }
        /// <summary>
        /// 添加布局控件
        /// </summary>
        /// <param name="virtualPath">虚拟路径</param>
        /// <param name="id">ID</param>
        private void AddLayout(string virtualPath,string id)
        {
            UserControl layoutControl = (UserControl)this.LoadControl(virtualPath);
            layoutControl.ID = id;

            int i = 1;
            foreach (Control c in layoutControl.Controls)
            {
                if (string.Compare(c.GetType().Name, "We7Layout", true)==0)
                {
                    c.ID = id;

                    foreach (Control col in c.Controls)
                    {
                        if (string.Compare(col.GetType().Name, "We7LayoutColumn", true) == 0)
                        {
                            col.ID = id + "_cloumn" + i.ToString();
                            i++;
                        }
                    }

                    break;
                }
                
            }

            this.Controls.Add(layoutControl);
        }

        #endregion
    }
}
