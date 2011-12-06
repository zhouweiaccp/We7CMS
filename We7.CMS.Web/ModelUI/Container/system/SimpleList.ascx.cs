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
using We7.Model.Core;
using System.Collections.Generic;

namespace CModel.Container.system
{
    public partial class SimpleList:ListContainer
    {
        /// <summary>
        /// 当前主键字段
        /// </summary>
        private DataKey dataKey;

        public override void BindData(ListResult result)
        {
            if (!IsPostBack)
            {
                foreach (ColumnInfo field in Columns)
                {
                    if (!field.Visible)
                        continue;

                    ModelControlField lc=ModelHelper.GetDataControl(field.Type);
                    if (lc != null)
                    {
                        lc.Column= field;
                        gvList.Columns.Add(lc);
                    }                    
                }

                gvList.DataKeyNames = PanelContext.DataKeyString.Split(',');
            }
   
                gvList.DataSource = result.DataTable;
                gvList.DataBind();
            
        }

        /// <summary>
        /// 取得所选中行的主键值
        /// </summary>
        /// <returns></returns>
        public override List<DataKey> GetDataKeys()
        {
            List<DataKey> dataKeys = new List<DataKey>();
            foreach (GridViewRow row in gvList.Rows)
            {
                CheckBox c = row.Cells[0].FindControl("chkID") as CheckBox;
                if (c != null && c.Checked)
                {
                    dataKeys.Add(gvList.DataKeys[row.RowIndex]);
                }
            }
            return dataKeys;
        }

        protected override void InitModelData()
        {
            PanelContext.DataKey = dataKey;
        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.className='mouseover'";
                e.Row.Attributes["onmouseout"] = "this.className=''";
            }
        }

        protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            dataKey = gvList.DataKeys[e.RowIndex];
            OnCommandSubmit("delete", null);
        }

        protected void gvList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dataKey = gvList.DataKeys[e.NewEditIndex];
            OnCommandSubmit("get", null);
        }

        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ////TODO::这句会损失一点效率,但是影响不大.
            List<string> list = new List<string>(new string[] { "DELETE", "EDIT", "SELECT", "CANCEL" });
            if (!list.Contains(e.CommandName.ToUpper()))
            {
                dataKey = gvList.DataKeys[Convert.ToInt32(e.CommandArgument)];
                OnCommandSubmit(e.CommandName, e.CommandArgument);
            }
        }
    }
}