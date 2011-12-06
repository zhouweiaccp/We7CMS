using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using We7.Model.Core.UI;
using We7.Framework.Util;
using We7.CMS;
using We7.CMS.Common.PF;
using We7.Model.Core.Data;
using Thinkment.Data;
using System.Collections.Generic;
using We7.CMS.Accounts;
using We7.Framework;
using We7.Model.Core;
using We7.CMS.Common;

namespace We7.Model.UI.Controls.system
{
    public partial class ModelToAdviceLable : FieldControl
    {
        protected global::System.Web.UI.WebControls.HiddenField hfValue;

        /// <summary>
        /// 业务助手工厂
        /// </summary>
        protected HelperFactory HelperFactory
        {
            get
            {
                return HelperFactory.Instance;
            }
        }
        /// <summary>
        /// 反馈类别助手
        /// </summary>
        protected AdviceTypeHelper AdviceTypeHelper
        {
            get { return HelperFactory.GetHelper<AdviceTypeHelper>(); }
        }
        /// <summary>
        /// 反馈助手
        /// </summary>
        protected AdviceHelper2 AdviceHelper
        {
            get { return HelperFactory.GetHelper<AdviceHelper2>(); }
        }
        public override void InitControl()
        {
            hfValue.Value = lblValue.Text = (Value ?? "").ToString();
            string result = "";
            if(!string.IsNullOrEmpty(Request["model"]))
            {
                ModelInfo mi = ModelHelper.GetModelInfoByName(Request["model"]);
                if(mi != null && !string.IsNullOrEmpty(mi.RelationModelName))
                {
                    AdviceType adviceType = AdviceTypeHelper.GetAdviceTypeByModelName(mi.RelationModelName);
                    if(adviceType != null && !string.IsNullOrEmpty(adviceType.ID))
                    {
                        string adviceTypeID = adviceType.ID;
                        result = "<a href='" + GetUrl() + "?typeID=" + adviceTypeID + "&RelationID=" + hfValue.Value + "'>查看反馈</a>";
                    }
                }                               
            }
            ltlText.Text = result;
        }

        public override object GetValue()
        {
            if (Column.DataType == TypeCode.String
                || Column.DataType == TypeCode.Char)
            {
                return hfValue.Value;
            }
            else
            {
                return TypeConverter.StrToObjectByTypeCode(We7Helper.FilterHtmlChars(hfValue.Value), Column.DataType);
            }
        }

        string GetUrl()
        {
            string data = Control.Params[We7.Model.Core.UI.Constants.DATA];            
            if (data.Length < 2)
            {
                data = "/admin/Advice/AdviceListEx.aspx";
            }
            return data;
        }
    }
}