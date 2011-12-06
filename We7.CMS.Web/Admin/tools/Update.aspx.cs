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
using We7.Framework;
using We7.CMS.Common;
using System.Collections.Generic;
using Thinkment.Data;

namespace We7.CMS.Web.Admin.tools
{
    public partial class Update : System.Web.UI.Page, IDataAccessPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            UpdateArticle();//将文章模型添加为"System.Article"
        }


        /// <summary>
        /// 更新文章
        /// </summary>
        protected void UpdateArticle()
        {
            List<Article> list=HelperFactory.Instance.Assistant.List<Article>(null, null);
            if (list != null)
            {
                foreach (Article a in list)
                {
                    if (String.IsNullOrEmpty(a.ModelName))
                    {
                        //try
                        //{
                            a.ModelName = Constants.ArticleModelName;
                            HelperFactory.Instance.Assistant.Update(a);
                        //}
                        //catch (Exception ex)
                        //{

                        //}
                    }
                }
            }
        }
    }
}
