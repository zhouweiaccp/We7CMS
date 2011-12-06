using System;
using System.Collections.Generic;
using System.Text;
using We7.Model.Core.Data;
using We7.Framework.Cache;
using Thinkment.Data;
using System.Data;
using System.Web;
using System.IO;

namespace We7.Model.Core.Command
{
	/// <summary>
	/// 删除单条记录命令
	/// </summary>
	public class DeleteCommand : ICommand
	{
		public object Do(PanelContext data)
		{
			ModelDBHelper subHelper = ModelDBHelper.Create(data.ModelName);
			DataTable db = subHelper.Query(new Criteria(CriteriaType.Equals, "ID", data.DataKey.Value), new List<Order> { new Order("ID") });
			DbProvider.Instance(data.Model.Type).Delete(data);
			foreach (We7Control ctrl in ModelHelper.GetPanelContext(data.ModelName, "edit").Panel.EditInfo.Controls)
			{
				if (!string.IsNullOrEmpty(ctrl.Params["count"]))
				{
					Dictionary<string, object> dic = new Dictionary<string, object>();
					dic.Add(string.Format("{0}_Count", data.Model.Name),
						subHelper.Count(new Criteria(CriteriaType.Equals, ctrl.Name, db.Rows[0][ctrl.Name])));
					ModelDBHelper helper = ModelDBHelper.Create(ctrl.Params["model"]);
					helper.Update(dic, new Criteria(CriteriaType.Equals, ctrl.Params["valuefield"], db.Rows[0][ctrl.Name]));
				}
				string fileurl = HttpContext.Current.Server.MapPath(db.Rows[0][ctrl.Name].ToString());
				if (ctrl.Type == "MultiUploadify" && File.Exists(fileurl))
				{
					File.Delete(fileurl);
					File.Delete(fileurl.Insert(fileurl.LastIndexOf('.'), "_thumb"));
				}
			}
			CacheRecord.Create(data.ModelName).Release();
			return null;
		}
	}
}
