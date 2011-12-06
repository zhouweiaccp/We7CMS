using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using We7.Model.Core.Data;
using We7.Framework.Cache;
using System.Data;
using Thinkment.Data;
using System.IO;
using System.Web;

namespace We7.Model.Core.Command
{
	/// <summary>
	/// 多选删除操作
	/// </summary>
	public class DeleteSelected : ICommand
	{
		/// <summary>
		/// 执行操作
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public object Do(PanelContext data)
		{
			ModelDBHelper subHelper = ModelDBHelper.Create(data.ModelName);
			List<DataKey> dataKeys = data.State as List<DataKey>;
			if (dataKeys.Count == 0) return null;
			DataTable db = subHelper.Query(new Criteria(CriteriaType.Equals, "ID", dataKeys[0].Value), new List<Order> { new Order("ID") });
			ModelDBHelper temhelper = null;
			string columnMapping = string.Empty;
			if (dataKeys != null)
			{
				foreach (We7DataColumn column in data.DataSet.Tables[0].Columns)
				{
					if (column.Name.Contains("_Count"))
					{
						temhelper = ModelDBHelper.Create(data.Model.GroupName + "." + column.Name.Remove(column.Name.IndexOf("_Count")));
						columnMapping = column.Mapping;
					}
				}
				foreach (DataKey key in dataKeys)
				{
					if (temhelper != null)
					{
						DataTable dt = subHelper.Query(new Criteria(CriteriaType.Equals, "ID", key.Value), new List<Order> { new Order("ID") });
						temhelper.Delete(new Criteria(CriteriaType.Equals, columnMapping.Split('|')[0], dt.Rows[0][columnMapping.Split('|')[1]]));
					}
					data.DataKey = key;
					DbProvider.Instance(data.Model.Type).Delete(data);
				}
				data.DataKey = null;
			}
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
					File.Delete(fileurl.Insert(fileurl.LastIndexOf('.'),"_thumb"));
				}
			}
			CacheRecord.Create(data.ModelName).Release();
			return null;
		}
	}
}
