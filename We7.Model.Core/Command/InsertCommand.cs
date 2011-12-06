using System;
using System.Collections.Generic;
using System.Text;
using We7.Model.Core.Data;
using System.Web;
using System.Diagnostics;
using We7.Framework.Cache;
using Thinkment.Data;

namespace We7.Model.Core.Command
{
	/// <summary>
	/// 添加数据
	/// </summary>
	class InsertCommand : ICommand
	{
		public object Do(PanelContext data)
		{
			DbProvider.Instance(data.Model.Type).Insert(data);
			foreach (We7Control ctrl in data.Panel.EditInfo.Controls)
			{
				if (!string.IsNullOrEmpty(ctrl.Params["count"]))
				{
					Dictionary<string, object> dic = new Dictionary<string, object>();
					dic.Add(string.Format("{0}_Count", data.Model.Name), ModelDBHelper.Create(data.ModelName).Count(new Criteria(CriteriaType.Equals, ctrl.Name, data.Row[ctrl.Name])));
					ModelDBHelper helper = ModelDBHelper.Create(ctrl.Params["model"]);
					helper.Update(dic, new Criteria(CriteriaType.Equals, ctrl.Params["valuefield"], data.Row[ctrl.Name]));
				}
			}

			CacheRecord.Create(data.ModelName).Release();
			return null;
		}
	}
}
