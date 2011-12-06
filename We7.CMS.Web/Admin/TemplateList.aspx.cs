using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using We7.CMS.Config;

using Thinkment.Data;
using We7.CMS.Common.Enum;
using We7.CMS.Common;
using We7.Framework.Config;

namespace We7.CMS.Web.Admin
{
    public partial class TemplateList : BasePage
    {
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.None;
            }
        }
        bool IsSubTemplate
        {
            get
            {
                if (Request["list"] != null && Request["list"].ToString().ToLower() == "sub")
                    return true;
                else
                {
                    return false;
                }
            }
        }

        bool IsMasterPage
        {
            get
            {
                if (Request["list"] != null && Request["list"].ToString().ToLower() == "master")
                    return true;
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 信息提示关键字
        /// </summary>
        string InfoKey
        {
            get
            {
                if (IsSubTemplate)
                {
                    return "子模板";
                }
                else if (IsMasterPage)
                    return "母版";
                else
                {
                    return "模板";
                }
            }
        }

        protected override void Initialize()
        {
            string query = null;
            if (SearchTextBox.Text.Length > 0)
                query = SearchTextBox.Text.Trim();
            selectLabel.Text = string.Format("选择一个{0}：", InfoKey);

            Template[] tps = null;

           GeneralConfigInfo si = GeneralConfigs.GetConfig();
            if (si.DefaultTemplateGroupFileName != null && si.DefaultTemplateGroupFileName.Length > 0)
            {
                tps = TemplateHelper.GetTemplates(query,si.DefaultTemplateGroupFileName.Replace(".xml",""));
            }
            else
            {
                tps = TemplateHelper.GetTemplates(query);
            }
            object dat;
            if (Request["list"] == "sub")
            {
                List<Template> ts = new List<Template>();
                foreach (Template tp in tps)
                {
                    if (tp.IsSubTemplate)
                    {
                        ts.Add(tp);
                    }
                }
                dat = ts;
            }
            else
            {
                List<Template> ts = new List<Template>();
                foreach (Template tp in tps)
                {
                    if (!tp.IsSubTemplate)
                    {
                        ts.Add(tp);
                    }
                }
                dat = ts;
            }
            DetailGridView.DataSource = dat;
            DetailGridView.DataBind();
        }

        protected void QueryButton_Click(object sender, EventArgs e)
        {
            Initialize();
        }

        void ShowMessage(string s)
        {
            MessageLabel.Text = s;
        }
    }
}
