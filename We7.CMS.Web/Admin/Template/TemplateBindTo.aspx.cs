using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using We7.CMS.Common;
using System.Collections.Generic;
using We7.CMS.Common.Enum;
using We7.Model.Core;
using We7.Framework.Config;

namespace We7.CMS.Web.Admin
{
    public partial class TemplateBindTo : BasePage
    {
        protected override MasterPageMode MasterPageIs
        {
            get
            {
                return MasterPageMode.NoMenu;
            }
        }

        string FileName
        {
            get { return Request["filename"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Template t = TemplateHelper.GetTemplate(FileName);
            if (t != null)
                NameLabel.Text = "模板<em>" + t.Name + "（" + t.FileName + "）</em>默认指定为：";
            else
                NameLabel.Text = "模板<em>" + FileName + "</em>默认指定为：";

            bindListLitiral.Text = TemplateHelper.LoadTemplateBinds(GeneralConfigs.GetConfig().DefaultTemplateGroupFileName, FileName);
            TemplateTypeLiteral.Text = LoadTemplateDictionary();
        }


        string LoadTemplateDictionary()
        {
            string itemString = "<a href=\"javascript:addBind('{0}','{1}','{3}','{4}','{5}')\" title=\"指定为 {4} ？ \"  >{2}</a> ";
            string itemTitle = "<H3>{0}</H3>";
            StringBuilder sb = new StringBuilder();

            List<TemplateBindConfig> list = TemplateHelper.GetTemplateBindConfigList();
            string title = "";
            List<TemplateBindConfig> modelList = new List<TemplateBindConfig>();
            foreach (TemplateBindConfig tb in list)
            {
                if (tb.Handler == "model")
                    modelList.Add(tb);
                else
                {
                    if (title != tb.HandlerName)
                    {
                        sb.AppendLine(string.Format(itemTitle, tb.HandlerName));
                        title = tb.HandlerName;
                    }
                    sb.AppendLine(string.Format(itemString, tb.Handler, tb.Mode, tb.ModeText,"",tb.Description,FileName));
                }
            }

            ContentModelCollection cmc= ModelHelper.GetContentModel(ModelType.ARTICLE);
            foreach (We7.Model.Core.ContentModel cm in cmc)
            {
                sb.AppendLine(string.Format(itemTitle, cm.Label + "模型"));
                foreach (TemplateBindConfig tb in modelList)
                {
                    sb.AppendLine(string.Format(itemString, tb.Handler, tb.Mode, tb.ModeText, cm.Name, cm.Label + tb.Description, FileName));
                }
            }

            sb.AppendLine("<br>");
            return sb.ToString();
        }
    }
}
