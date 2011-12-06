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

namespace We7.CMS.UI.Widget
{
    [ControlGroupDescription(Label = "内容模型分页列表部件", Description = "内容模型分页列表部件", Icon = "内容模型部件", DefaultType = "WidgetPagedList.Generate")]
    [ControlDescription(Desc = "内容模型分页列表部件(自动生成)")]
    public class WidgetPagedList : BaseWidgetList
    {
        private string cssClass;

        /// <summary>
        /// 自定义Css样式
        /// </summary>
        [Parameter(Title = "自定义Css类名称", Type = "String", DefaultValue = "article_list")]
        public override string CssClass
        {
            get { return String.IsNullOrEmpty(cssClass) ? "article_list" : cssClass; }
            set { cssClass = value; }
        }

        /// <summary>
        /// 分页控件
        /// </summary>
        [Children]
        public ControlPager Pager = new ControlPager();

        protected override void OnInitData()
        {            
            int recordcount;      
            ModelDBHelper dbhelper = ModelDBHelper.Create(ModelName);
            List<Order> os = CreateOrders();
            Criteria c = CreateCriteria();
            Items = dbhelper.QueryPagedList(c, os, Pager.PageIndex, Pager.PageSize, out recordcount).Rows;
            Pager.RecordCount = recordcount;
        }

        protected override Criteria CreateCriteria()
        {
            Criteria c = new Criteria();
            List<Order> os = new List<Order>();
            os.Add(new Order("ID"));

            

            if (QueryByColumn) //按栏目查询
            {
                if (String.IsNullOrEmpty(OwnerID))
                {
                    OwnerID = ChannelHelper.GetChannelIDFromURL();
                }
                if (IncludeChildren) //按子栏目查询
                {
                    Criteria subC = new Criteria();
                    subC.Mode = CriteriaMode.Or;
                    subC.Add(CriteriaType.Equals, "OwnerID", OwnerID);

                    List<Channel> chs = ChannelHelper.GetChannels(OwnerID);
                    if (chs != null && chs.Count > 0)
                    {
                        foreach (Channel ch in chs)
                        {
                            c.AddOr(CriteriaType.Equals, "OwnerID", ch.ID);
                        }
                    }
                }
                else
                {
                    c.Add(CriteriaType.Equals, "OwnerID", OwnerID);
                }
            }

            if (ShowAtHome) //只显示置顶信息
            {
                c.Add(CriteriaType.Equals, "IsShow", 1);
            }

            if (!String.IsNullOrEmpty(Tag)) //按标签查询
            {
                c.Add(CriteriaType.Like, "Tags", "%'" + Tag + "'%");
            }
            c.Add(CriteriaType.Equals, "State", 1); //只显示状态为可用的数据
            ProcessQueryParam(c); //处理附加查询数据

            return c;
        }
    }
}
