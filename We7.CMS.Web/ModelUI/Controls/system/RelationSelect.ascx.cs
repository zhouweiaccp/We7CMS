using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.Model.Core.UI;
using We7.Framework.Util;
using We7.CMS.Common;
using We7.Model.Core;
using System.Data;
using System.IO;
using We7.Framework.Config;
using We7.Model.Core.Data;
using Thinkment.Data;
using We7.CMS;

namespace We7.Model.UI.Controls.system
{
    public partial class RelationSelect :We7FieldControl
    {
        public override object GetValue()
        {
            string textfield = Control.Params["df"];
            object value=TypeConverter.StrToObjectByTypeCode(ddlEnum.SelectedValue, Column.DataType);
            if (String.IsNullOrEmpty(textfield))
            {
                return value;
            }
            else
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add(Column.Name, value);
                dic.Add(textfield, ddlEnum.SelectedItem.Text);
                return dic;
            }            
        }

        public override void InitControl()
        {
            ddlEnum.PreRender+=new EventHandler(ddlEnum_PreRender);
            string model = Control.Params["model"];
            string valuefield = Control.Params["valuefield"];
            string textfield = Control.Params["textfield"];

            if (GeneralConfigs.GetConfig().EnableSingleTable)
            {
                ModelDBHelper helper=ModelDBHelper.Create(model);
                Criteria c=new Criteria(CriteriaType.Equals,"State",1);
                DataTable dt=helper.Query(c, new List<Order>() { new Order("Created",OrderMode.Desc),new Order("ID",OrderMode.Desc)}, 0, 0);
                ddlEnum.DataSource = dt;
            }
            else
            {
                List<Article> list = ArticleHelper.QueryArticleByModel(model);
                DataSet ds = ModelHelper.CreateDataSet(model);
                foreach (Article a in list)
                {
                    TextReader reader = new StringReader(a.ModelXml);
                    ds.ReadXml(reader);
                }

                ddlEnum.DataSource = ds.Tables[0];               
            }

            ddlEnum.DataValueField = valuefield;
            ddlEnum.DataTextField = textfield;
            ddlEnum.DataBind();

            ddlEnum.Items.Insert(0, new ListItem("请选择", ""));
            ddlEnum.SelectedValue = Value == null ? Control.DefaultValue : Value.ToString();

            if (!String.IsNullOrEmpty(Control.Width))
            {
                ddlEnum.Width = Unit.Parse(Control.Width);
            }
            if (!String.IsNullOrEmpty(Control.Height))
            {
                ddlEnum.Height = Unit.Parse(Control.Height);
            }

            ddlEnum.CssClass = Control.CssClass;
            if (Control.Required && !ddlEnum.CssClass.Contains("required"))
            {
                ddlEnum.CssClass += " required";
            }

			string urlParam = We7Helper.GetParamValueFromUrl(Request.RawUrl, Control.Name);
			if (!string.IsNullOrEmpty(urlParam)) ddlEnum.SelectedValue = urlParam;
        }

        void ddlEnum_PreRender(object sender, EventArgs e)
        {
            var convert = Control.Params["convert"];
            if (!String.IsNullOrEmpty(convert))
            {
                if (convert == "cat")
                {
                    CategoryHelper helper = We7.Framework.HelperFactory.Instance.GetHelper<CategoryHelper>();
                    foreach (ListItem item in ddlEnum.Items)
                    {
                        Category cat = helper.GetCategoryByKeyword(item.Text);
                        if (cat != null)
                        {
                            item.Text = cat.Name;
                        }
                    }
                }
            }
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            InitControl();
            ddlEnum.SelectedValue = newID.Value;
        }
    }
}