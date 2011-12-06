using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.WebControls;
using System.Data;
using We7.CMS.WebControls.Core;
using We7.Model.Core.Data;
using Thinkment.Data;
using We7.CMS.Common;
using System.Web;
using We7.Model.Core;
using We7.Framework.Util;
using System.Collections;
using System.IO;

namespace We7.CMS.UI.Widget
{

    [ControlGroupDescription(Label = "内容模型详细信息部件", Description = "自动生成的内容模型部件", Icon = "内容模型部件", DefaultType = "WidgetDetail.Generate")]
    [ControlDescription(Desc = "内容模型详细信息部件(自动生成)")]
    public class WidgetDetail : ThinkmentDataControl
    {
        private string cssClass;
        private DataRow previousItem;
        private DataRow nextItem;

        /// <summary>
        /// 模型名称
        /// </summary>
        public virtual string ModelName { get; set; }

        [Parameter(Title = "自定义Css类名称", Type = "String", DefaultValue = "article")]
        public virtual string CssClass
        {
            get { return String.IsNullOrEmpty(cssClass) ? "article" : cssClass; }
            set { cssClass = value; }
        }

        /// <summary>
        /// 上边距10像素
        /// </summary>
        [Parameter(Title = "上边距10像素", Type = "Boolean", DefaultValue = "1")]
        public bool MarginTop10 { get; set; }

        /// <summary>
        /// 下边距10像素
        /// </summary>
        [Parameter(Title = "左边距10像素", Type = "Boolean", DefaultValue = "1")]
        public bool MarginLeft10 { get; set; }

        /// <summary>
        /// 附加的Css样式
        /// </summary>
        protected virtual string MarginCss
        {
            get { return (MarginTop10 ? " mtop10" : "") + (MarginLeft10 ? " mleft10" : ""); }
        }

        /// <summary>
        /// 文章ID
        /// </summary>
        public string ItemID
        {
            get
            {
                return HelperFactory.GetHelper<ArticleHelper>().GetArticleIDFromURL();
            }
        }

        /// <summary>
        /// 当前的记录数据
        /// </summary>
        public DataRow Item { get; set; }

        protected override void OnInitData()
        {
            ModelDBHelper helper = ModelDBHelper.Create(ModelName);
            Criteria c = CreateEntryCriteria();
            DataTable dt = helper.Query(CreateEntryCriteria(), CreateOrders(), 0, 0);
            Item = dt.Rows.Count > 0 ? dt.Rows[0] : dt.NewRow();

            //更新点击次数
            if (dt.Columns.Contains("Clicks"))
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                int clicks = Item["Clicks"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(Item["Clicks"]);
                clicks++;
                dic.Add("Clicks", clicks);
                helper.Update(dic, c);
            }
        }

        protected void UpdateModel()
        {
        }

        protected virtual Criteria CreateEntryCriteria()
        {
            Criteria c = new Criteria(CriteriaType.None);
            c.Add(CriteriaType.Equals, "ID", ItemID);
            return c;
        }

        protected virtual List<Order> CreateOrders()
        {
            List<Order> orders = new List<Order>();
            orders.Add(new Order("ID", OrderMode.Desc));
            return orders;
        }

        protected DataRow PreviousItem
        {
            get
            {
                if (previousItem == null && Item != null)
                {
                    Criteria c = new Criteria();
                    c.Add(CriteriaType.NotEquals, "ID", Item["ID"]);
                    c.Add(CriteriaType.LessThan, "Updated", Item["Updated"]);
                    ModelDBHelper helper = ModelDBHelper.Create(ModelName);
                    if (helper.Count(c) > 0)
                    {
                        DataTable dt = helper.Query(c, new List<Order>() { new Order("Updated", OrderMode.Desc), new Order("ID") }, 0, 1);
                        previousItem = dt.Rows.Count > 0 ? dt.Rows[0] : null;
                    }
                }
                return previousItem;
            }
        }

        protected DataRow NextItem
        {
            get
            {
                if (nextItem == null)
                {
                    Criteria c = new Criteria();
                    c.Add(CriteriaType.NotEquals, "ID", Item["ID"]);
                    c.Add(CriteriaType.MoreThan, "Updated", Item["Updated"]);
                    ModelDBHelper helper = ModelDBHelper.Create(ModelName);
                    if (helper.Count(c) > 0)
                    {
                        DataTable dt = helper.Query(c, new List<Order>() { new Order("Updated", OrderMode.Asc), new Order("ID") }, 0, 1);
                        nextItem = dt.Rows.Count > 0 ? dt.Rows[0] : null;
                    }
                }
                return nextItem;
            }
        }

        protected string GetUrl(object o)
        {
            string oid = HelperFactory.GetHelper<ChannelHelper>().GetChannelIDFromURL();
            return UrlHelper.GetUrl(oid, o as string);
        }

        /// <summary>
        /// 获取附件列表
        /// </summary>
        protected Dictionary<string, string> AttachmentsUrls
        {
            get
            {
                Dictionary<string, string> result = new Dictionary<string, string>();
                Article a = new Article() { ID = ItemID };
                string DialogPath = a.AttachmentUrlPath + "/Attachment";
                ArrayList list = new ArrayList();

                DirectoryInfo di = new DirectoryInfo(HttpContext.Current.Server.MapPath(DialogPath));
                try
                {
                    foreach (FileInfo f in di.GetFiles())
                    {
                        string pathrelatively = DialogPath + "/" + f.Name;
                        result.Add(pathrelatively, f.Name);
                    }
                }
                catch
                {
                    //对应目录下没有文件
                }

                return result;
            }
        }
    }
}
