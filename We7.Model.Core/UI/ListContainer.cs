using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;

namespace We7.Model.Core.UI
{
    public abstract class ListContainer:ModelContainer
    {
        protected override void InitModelData()
        {
        }
        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="result"></param>
        public abstract void BindData(ListResult result);

        /// <summary>
        /// 取得所选行的主键字段
        /// </summary>
        /// <returns></returns>
        public virtual List<DataKey> GetDataKeys()
        {
            return null;
        }

        protected ColumnInfoCollection Columns
        {
			get { return PanelContext.Panel.ListInfo.Groups[GroupIndex].Columns; }
        }
        //private List<ColumnInfo> sortedFields;
        //protected List<ColumnInfo> SortedFields
        //{
        //    get
        //    {
        //        if (sortedFields ==null)
        //        {
        //            sortedFields = new List<ColumnInfo>(PanelContext.Panel.ListInfo.Columns);
        //            sortedFields.Sort(delegate(ColumnInfo A, ColumnInfo B)
        //            {
        //                return A.Index > B.Index ? 1 : (A.Index < B.Index ? -1 : 0);
        //            });
        //        }
        //        return sortedFields;
        //    }
        //}
    }
}
