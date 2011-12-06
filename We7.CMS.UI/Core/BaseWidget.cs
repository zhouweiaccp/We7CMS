using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using We7.Framework.TemplateEnginer;
using System.Web.UI;
using System.IO;
using We7.CMS.WebControls.Core;
using Thinkment.Data;
using We7.Framework.Algorithm;

namespace We7.CMS.WebControls
{
    public class BaseWidget:FrontUserControl
    {
        /// <summary>
        /// 包装器路径
        /// </summary>
        [Option("KeyValueSelector", "vswrapper")]
        [Required]
        [Weight(3)]
        [Desc("外观", "定义部件最外围样式")]
        public string Wrapper { get; set; }

        /// <summary>
        /// more链接地址
        /// </summary>        
        [Option("String")]
        [Required]
        [Weight(1)]
        [Desc("导航地址", "定义用于包装器的导航地址")]
        public virtual string NavigationUrl { get; set; }

        /// <summary>
        /// 头部标题
        /// </summary>        
        [Option("String")]
        [Required]
        [Weight(2)]
        [Desc("头部标题", "定义用于包装器的头部标题")]
        public virtual string HeadTitle { get; set; }



        /// <summary>
        /// 重写信息重现
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            //RenderArguments args = new RenderArguments(this, UxParams);

            //StringWriter strWriter = new StringWriter();
            //HtmlTextWriter tempWriter = new HtmlTextWriter(strWriter);
            //try
            //{
            //    base.Render(tempWriter);
            //}
            //catch (Exception ex)
            //{
            //    args.Exception = ex;
            //    args.IsError = true;
            //};

            //string content = strWriter.ToString();
            //new RenderChain().DoRender(ref content, args);
            //writer.Write(content);
        }
    }

    public abstract class ListWidget<T> : BaseWidget
        where T : class, new()
    {
        private Pager pager;

        protected virtual List<T> Items
        {
            get
            {
                Criteria c = CreateCriteria();
                if (c == null)
                    throw new ArgumentNullException("查询条件不能为空");

                RecordCount = Assistant.Count<T>(c);
                return Assistant.List<T>(c, CreateOrders(), Pager.StartItem, Pager.PageItemsCount);
            }
        }

        public virtual IPager Pager
        {
            get
            {
                return pager = pager ?? new Pager();
            }
        }

        /// <summary>
        /// 禁止分页
        /// </summary>
        public virtual bool DisablePager
        {
            get { return Pager.Disable; }
            set { Pager.Disable = value; }
        }

        /// <summary>
        /// 记录总条数
        /// </summary>
        public virtual int RecordCount
        {
            get { return Pager.RecordCount; }
            set { Pager.RecordCount = value; }
        }

        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize
        {
            get { return Pager.PageSize; }
            set { Pager.PageSize = value; }
        }

        /// <summary>
        /// 创建查询条件
        /// </summary>
        /// <returns></returns>
        protected abstract Criteria CreateCriteria();

        /// <summary>
        /// 创建排序列表
        /// </summary>
        /// <returns></returns>
        protected abstract Order[] CreateOrders();
    }

    public class DetailWidget<T> : BaseWidget where T : class, new()
    {
        /// <summary>
        /// 当前数据信息
        /// </summary>
        protected virtual T Item
        {
            get
            {
                Criteria c = CreateCriteria();
                if (c == null)
                    throw new ArgumentNullException("查询条件不能为空");

                List<T> list = Assistant.List<T>(c, null, 0, 0);
                return list != null && list.Count > 0 ? list[0] : new T();
            }
        }

        protected virtual Criteria CreateCriteria()
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ID", UrlHelper.GetID());
            return c;
        }
    }

}
