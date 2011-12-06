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
using System.Text;
using We7.CMS.Accounts;

namespace We7.Model.UI.Controls.system
{
	public partial class MultiUploadify : We7FieldControl
	{
		public override object GetValue()
		{
			string[] values = pageValues.Value.Split('|');
			List<Dictionary<string, object>> dics = new List<Dictionary<string, object>>();
			for (int i = 0; i < values.Length; i = i + 3)
			{
				Dictionary<string, object> dic = new Dictionary<string, object>();
				string[] keyV = values[i].Split(':');
				dic.Add(keyV[0], HttpUtility.UrlDecode(keyV[1]));
				keyV = values[i + 1].Split(':');
				dic.Add(keyV[0], HttpUtility.UrlDecode(keyV[1]));
				keyV = values[i + 2].Split(':');
				dic.Add(keyV[0], HttpUtility.UrlDecode(keyV[1]));
				dic.Add("ID", We7Helper.CreateNewID());
				dic.Add("AccountID", Security.CurrentAccountID);
				dic.Add("Created", System.DateTime.Now);
				dic.Add("State", 1);
				dic.Add("Index", 999);
				dics.Add(dic);
			}
			return dics;
		}

		public override void InitControl()
		{
			//ddlEnum.PreRender += new EventHandler(ddlEnum_PreRender);
			//string model = Control.Params["model"];
			//string valuefield = Control.Params["valuefield"];
			//string textfield = Control.Params["textfield"];

			//if (GeneralConfigs.GetConfig().EnableSingleTable)
			//{
			//    ModelDBHelper helper = ModelDBHelper.Create(model);
			//    Criteria c = new Criteria(CriteriaType.Equals, "State", 1);
			//    DataTable dt = helper.Query(c, new List<Order>() { new Order("Created", OrderMode.Desc), new Order("ID", OrderMode.Desc) }, 0, 0);
			//    ddlEnum.DataSource = dt;
			//}
			//else
			//{
			//    List<Article> list = ArticleHelper.QueryArticleByModel(model);
			//    DataSet ds = ModelHelper.CreateDataSet(model);
			//    foreach (Article a in list)
			//    {
			//        TextReader reader = new StringReader(a.ModelXml);
			//        ds.ReadXml(reader);
			//    }

			//    ddlEnum.DataSource = ds.Tables[0];
			//}

			//ddlEnum.DataValueField = valuefield;
			//ddlEnum.DataTextField = textfield;
			//ddlEnum.DataBind();

			//ddlEnum.Items.Insert(0, new ListItem("请选择", ""));
			//ddlEnum.SelectedValue = Value == null ? Control.DefaultValue : Value.ToString();

			//if (!String.IsNullOrEmpty(Control.Width))
			//{
			//    ddlEnum.Width = Unit.Parse(Control.Width);
			//}
			//if (!String.IsNullOrEmpty(Control.Height))
			//{
			//    ddlEnum.Height = Unit.Parse(Control.Height);
			//}

			//ddlEnum.CssClass = Control.CssClass;
			//if (Control.Required && !ddlEnum.CssClass.Contains("required"))
			//{
			//    ddlEnum.CssClass += " required";
			//}
		}
	}


}