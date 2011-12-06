using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.WebControls.Core;
using Thinkment.Data;

namespace We7.CMS.UI.Widget
{
    [ControlGroupDescription(Label = "反馈模型部件", Description = "自动生成的反馈模型部件", Icon = "反馈模型部件", DefaultType = "")]
    [ControlDescription(Desc = "反馈模型查询列表部件(自动生成)")]
    public class WidgetAdviceQueryList : AdviceProvider
    {
        /// <summary>
        /// 安全查询
        /// </summary>
        [Parameter(Title = "密码验证", Type = "Boolean", DefaultValue = "", Required = true)]
        public bool SecurityQuery;

        /// <summary>
        /// 
        /// </summary>
        public string ErrorMessage { get; set; }


        protected override Thinkment.Data.Criteria CreateListCriteria()
        {
            Criteria c = new Criteria();
            if (!string.IsNullOrEmpty(Request[QueryKey]))
            {
                c.Add(CriteriaType.Equals, "SN", AdviceTypeID);
            }
            if (!string.IsNullOrEmpty(Request["KeyWord"]))
            {
                c.Add(CriteriaType.Like, "Title", "%" + Request["KeyWord"].Trim() + "%");
            }
            if (string.IsNullOrEmpty(Request[QueryKey]) && string.IsNullOrEmpty(Request["KeyWord"]))
            {
                c.Add(CriteriaType.Equals, "IsShow", -1);
            }
            if (IsShow)
            {
                c.Add(CriteriaType.Equals, "IsShow", 1);
            }
            if (SecurityQuery)
            {
                c.Add(CriteriaType.Equals, "MyQueryPwd", Request["Password"]);
            }
            return c;
        }
    }
}
