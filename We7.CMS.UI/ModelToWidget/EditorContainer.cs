using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using We7.Model.Core;
using System.Data;
using We7.Model.Core.UI;
using System.Web.UI.WebControls;
using We7.Framework.Util;

namespace We7.CMS.WebControls
{
    public class EditorContainer : CommandContainer
    {
        public bool IsEdit
        {
            get
            {
                if (ViewState["$IsEdit"] == null)
                {
                    ViewState["$IsEdit"] = false;
                }
                return (bool)ViewState["$IsEdit"];
            }
            set { ViewState["$IsEdit"] = value; }
        }

        /// <summary>
        /// 是否是浏览控件
        /// </summary>
        public bool IsViewer { get; set; }

        private IOrderedDictionary DataKeyValus
        {
            get
            {
                return ViewState["$DataKeyValus_"] as IOrderedDictionary;
            }
            set
            {
                ViewState["$DataKeyValus_"] = value;
            }
        }

        protected sealed override void LoadContainer()
        {
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(Page.GetType(), "jquery1.3.2"))
            {
                Page.ClientScript.RegisterClientScriptInclude(Page.GetType(), "jquery1.3.2", "/Admin/Ajax/jquery/jquery-1.3.2.min.js");
            }
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(Page.GetType(), "jquery_validate"))
            {
                Page.ClientScript.RegisterClientScriptInclude(Page.GetType(), "jquery_validate", "/Admin/Ajax/jquery/jquery.validate.js");
            }
            if (!Page.ClientScript.IsStartupScriptRegistered(Page.GetType(), "jquery_valform"))
            {
                if (Page != null && Page.Form != null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "jquery_valform", @"$(function(){$('#" + this.Page.Form.ClientID + "').validate();});", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "jquery_valform", @"$(function(){$('form').validate();});", true);
                }
            }
            ModelHelper.UpdateFields(PanelContext, null);
            InitContainer(null);
        }

        public void SetData(DataRow row, IOrderedDictionary datakeys)
        {
            IsEdit = row != null;
            if (Page is ModelHandlerPage)
            {
                ModelHandlerPage mhp = Page as ModelHandlerPage;
                mhp.RecordID = (IsEdit && row.Table.Columns.Contains(We7.Model.Core.UI.Constants.EntityID)) ? (string)row[We7.Model.Core.UI.Constants.EntityID] : Utils.CreateGUID();
            }
            DataKeyValus = datakeys;
            InitContainer(row);
        }

        private void InitContainer(DataRow row)
        {
            UpdateFields(row);
            InitContainer();
        }

        protected virtual void InitContainer() { }

        protected virtual void ChangeState()
        {

        }

        public void ChangeState(bool isEdit)
        {
            IsEdit = isEdit;
            ChangeState();
        }

        protected override void InitModelData()
        {
            PanelContext.Row.Clear();

            foreach (Group group in PanelContext.Panel.EditInfo.Groups)
            {
                foreach (We7Control ctr in group.Controls)
                {
                    FieldControl fc = UIHelper.GetControl(ctr.ID, this) as FieldControl;
                    if (fc != null)
                    {
                        object o = fc.GetValue();
                        if (o is IDictionary<string, object>)
                        {
                            IDictionary<string, object> dic = o as IDictionary<string, object>;
                            foreach (string key in dic.Keys)
                            {
                                PanelContext.Row[key] = dic[key];
                            }
                        }
                        else
                        {
                            PanelContext.Row[ctr.Name] = o;
                        }
                    }
                    else
                    {
                        PanelContext.Row[ctr.Name] = null;
                    }
                }
                ProcessAllDataColumns(PanelContext);
            }

            InitDataKeyValus();
        }

        protected void InitDataKeyValus()
        {
            if (DataKeyValus == null)
            {
                DataKeyValus = new OrderedDictionary();

                foreach (DataField df in PanelContext.Row)
                {
                    AddDataKey(df.Column.Name, df.Value, DataKeyValus);
                }
            }
            PanelContext.DataKey = new DataKey(DataKeyValus);
        }

        protected string GetLabel(We7Control control)
        {
            if (String.IsNullOrEmpty(control.Label) && Info.DataSet.Tables.Count > 0 && Info.DataSet.Tables[0].Columns.Contains(control.Name))
            {
                control.Label = Info.DataSet.Tables[0].Columns[control.Name].Label;
            }
            return control.Label;
        }

        /// <summary>
        /// 用数据行更新模型数据
        /// </summary>
        /// <param name="data">模型数据</param>
        /// <param name="row"></param>
        public void UpdateFields(DataRow row)
        {
            PanelContext.Row.Clear();
            if (row == null)
            {
                foreach (We7DataColumn field in PanelContext.Table.Columns)
                {
                    PanelContext.Row[field] = null;
                }
            }
            else
            {
                foreach (We7DataColumn field in PanelContext.Table.Columns)
                {
                    if (field.Direction != ParameterDirection.ReturnValue)
                        PanelContext.Row[field] = row[field.Name];
                }
            }
        }

        /// <summary>
        /// 处理所有数据列
        /// </summary>
        /// <param name="ctx"></param>
        void ProcessAllDataColumns(PanelContext ctx)
        {
            foreach (We7DataColumn dc in ctx.DataSet.Tables[0].Columns)
            {
                if (!String.IsNullOrEmpty(dc.DefaultValue) &&
                    dc.Direction != ParameterDirection.ReturnValue &&
                    (ctx.Row[dc.Name] == null || String.IsNullOrEmpty(ctx.Row[dc.Name].ToString())))
                {
                    if (String.Compare(dc.Name, "AccountID") == 0)
                        continue;
                    if (IsEdit && String.Compare(dc.Name, "Updated", true) == 0)
                        continue;
                    ctx.Row[dc.Name] = DefaultGenerator.GetDefaultValue(dc.DefaultValue, ctx, dc);
                }
            }
        }

        public event EventHandler OnSetData;

        void AddDataKey(string key, object value, IOrderedDictionary dics)
        {
            foreach (string s in PanelContext.DataKeys)
            {
                if (s == key)
                {
                    dics.Add(key, value);
                }
            }
        }
    }
}
