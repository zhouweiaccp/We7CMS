using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using We7.Model.Core.UI;
using System.IO;
using System.Xml;
using We7.CMS.Common;
using We7.Model.Core;
using We7.Framework.Util;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace We7.Model.UI.Controls.system
{
    public partial class ChildrenTableHorizontal : We7FieldControl
    {
        private ModelInfo modelInfo;
        private PanelContext editorContext, listContext;
        private UIHelper uiHelper;
        private DataTable data;

        /// <summary>
        /// UI处理对象
        /// </summary>
        protected UIHelper UIHelper { get { return uiHelper; } }

        private bool IsEdit
        {
            get
            {
                return Data.Select("ID='"+hfID.Value+"'").Length > 0;
            }
        }

        /// <summary>
        /// 数据
        /// </summary>
        protected DataTable Data
        {
            get
            {
                if (data == null)
                {
                    if (string.IsNullOrEmpty(xmldata.Value))
                    {
                        xmldata.Value = Value as string;
                    }
                    data = ConvertXmlToDataTable(xmldata.Value);
                    data.RowChanged += new DataRowChangeEventHandler(data_RowChanged);
                    data.RowDeleted += new DataRowChangeEventHandler(data_RowChanged);
                }
                return data;
            }
            set
            {
                data = value;

            }
        }

        void data_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            xmldata.Value = ConvertDataTableToXml(data);
        }

        /// <summary>
        /// 编辑面板上下文
        /// </summary>
        public PanelContext EditorContext
        {
            get
            {
                if (editorContext == null)
                {
                    string modeltype = Control.Params["data"];
                    if (string.IsNullOrEmpty(modeltype))
                        throw new Exception("子模型控件模型名称不能空");
                    editorContext = ModelHelper.GetPanelContext(modeltype, "edit");
                }
                return editorContext;
            }
        }

        /// <summary>
        /// 列表面板上下文
        /// </summary>
        public PanelContext ListContext
        {
            get
            {
                if (listContext == null)
                {
                    string modeltype = Control.Params["data"];
                    if (string.IsNullOrEmpty(modeltype))
                        throw new Exception("子模型控件模型名称不能空");
                    listContext = ModelHelper.GetPanelContext(modeltype, "list");
                }
                return listContext;
            }
        }

        public override void InitControl()
        {
            uiHelper = new UIHelper(Page, EditorContext);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitEditor(null);
        }

        protected override void OnPreRender(EventArgs e)
        {
            BindList();
            btnSave.Text = IsEdit ? "修改" : "添加";
            btnToggle.Text = hfPanelVisible.Value == "1" ? "收缩编辑项" : "打开编辑项";
            divCt.Attributes["style"] = hfPanelVisible.Value == "1" ? "display:block" : "display:none";
            base.OnPreRender(e);
        }

        private void InitEditor(DataRow row)
        {
            if (row != null)
            {
                foreach (DataColumn dc in row.Table.Columns)
                {
                    if (EditorContext.Table.Columns.Contains(dc.ColumnName))
                    {
                        EditorContext.Row[dc.ColumnName] = row[dc];
                    }
                }
            }
            rptEditor.DataSource = EditorContext.Panel.EditInfo.Controls;
            rptEditor.DataBind();
        }

        protected void gvList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            hfID.Value = gvList.DataKeys[e.NewEditIndex]["ID"] as string;
            DataRow[] drs=Data.Select("ID='" + hfID.Value + "'");
            if (drs != null && drs.Length > 0)
            {
                InitEditor(drs[0]);
            }
        }

        protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string id = gvList.DataKeys[e.RowIndex]["ID"] as string;
            DataRow[] drs = Data.Select("ID='" +id + "'");
            if (drs != null && drs.Length > 0)
            {
                Data.Rows.Remove(drs[0]);
            }
        }

        protected void rptEditor_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            We7Control ctr = e.Item.DataItem as We7Control;
            if (ctr != null)
            {
                HtmlTableRow row = new HtmlTableRow();
                row.Visible = ctr.Visible;

                HtmlTableCell cell = new HtmlTableCell();
                cell.Attributes["css"] = "cth";
                cell.InnerText = ctr.Label;
                row.Cells.Add(cell);

                cell = new HtmlTableCell();
                FieldControl c = UIHelper.GetControl(ctr);
                cell.Controls.Clear();
                cell.Controls.Add(c);
                cell.Visible = ctr.Visible;
                row.Cells.Add(cell);

                e.Item.Controls.Add(row);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataRow row = null;
            if (!IsEdit)
            {
                row = Data.NewRow();
                Data.Rows.Add(row);
            }
            else
            {
                DataRow[] drs = Data.Select("ID='" + hfID.Value + "'");
                row = drs[0];
            }
            foreach (We7Control ctr in EditorContext.Panel.EditInfo.Controls)
            {
                if (IsEdit && ctr.ID == "ID")
                    continue;
                FieldControl fc = FindControl(ctr.Name);
                if (fc != null)
                {
                    row[ctr.Name] = fc.GetValue();
                }
            }
        }

        protected void btnToggle_Click(object sender, EventArgs e)
        {
            if(hfPanelVisible.Value=="1")
            {
                divCt.Attributes["style"] = "display:none";
                hfPanelVisible.Value = "0";
                
            }
            else
            {
                divCt.Attributes["style"] = "display:block";
                hfPanelVisible.Value="1";
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            hfID.Value = String.Empty;
            InitEditor(null);            
        }

        /// <summary>
        /// 供内容模型程序调用，取得模型值。
        /// </summary>
        /// <returns></returns>
        public override object GetValue()
        {
            return xmldata.Value;
        }

        /// <summary>
        /// 绑定列表
        /// </summary>
        private void BindList()
        {

            //TODO::在这儿用 EditorContext.Panel.EditInfo.Controls;中的值来初始化那些列要显示，以及按什么样的方式进行显示
            //TODO::把整个控件的样式调整一下，让它更美观一些           

            gvList.DataSource = Data;

            gvList.Columns.Clear();            

            gvList.AutoGenerateColumns = false;
            gvList.DataKeyNames = new string[] { "ID" };            

            for (int i = 0; i < EditorContext.Panel.EditInfo.Controls.Count; i++)
            {
                We7Control ctrl =  EditorContext.Panel.EditInfo.Controls[i] as We7Control;
                if (ctrl.Visible)
                {
                    BoundField bf = new BoundField();
                    bf.HeaderText = ctrl.Label;
                    bf.DataField = ctrl.Name;                   
                    gvList.Columns.Add(bf);
                }
            }

            CommandField cfModify = new CommandField();  //绑定命令列   
            cfModify.HeaderText = "操作";
            cfModify.ButtonType = ButtonType.Link;
            cfModify.EditText = "修改";
            cfModify.DeleteText = "删除";
            cfModify.ShowEditButton = true;
            cfModify.ShowDeleteButton = true;

            gvList.Columns.Add(cfModify);

            gvList.DataBind();               
        }

        protected void gvList_DataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex > -1)
            {
                int j=0,width;
                for (int i = 0; i < EditorContext.Panel.EditInfo.Controls.Count; i++)
                {
                    We7Control ctrl = EditorContext.Panel.EditInfo.Controls[i] as We7Control;
                    if (ctrl.Visible)
                    {
                        if (int.TryParse(ctrl.Width, out width))
                            e.Row.Cells[j].Width = width;                        
                        j++;
                    }
                }
            }
        }

        /// <summary>
        /// 查找子控件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected FieldControl FindControl(string name)
        {
            return FindControl(name, rptEditor);
        }

        /// <summary>
        /// 递归查找控件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        protected FieldControl FindControl(string name, Control parent)
        {
            FieldControl ctr=parent as FieldControl;
            if (ctr != null && ctr.ID == name)
                return ctr;

            foreach (Control c in parent.Controls)
            {
                ctr = FindControl(name, c);
                if (ctr != null && ctr.ID == name)
                    break;
            }
            return ctr;
        }

        private string ConvertDataTableToXml(DataTable dt)
        {

            if (dt != null)
            {
                StringBuilder sb=new StringBuilder();
                XmlWriter xmlwriter = XmlWriter.Create(sb);
                dt.WriteXml(xmlwriter);
                return sb.ToString();
            }
            return String.Empty;
        }

        private DataTable ConvertXmlToDataTable(string xml)
        {
            DataTable dt = ModelHelper.CreateDataset(EditorContext.ModelName).Tables[0];
            dt.TableName = EditorContext.ModelName.Contains(".") ? EditorContext.ModelName.Substring(EditorContext.ModelName.IndexOf('.')+1) : EditorContext.ModelName;
            if (!String.IsNullOrEmpty(xml))
            {
                try
                {
                    TextReader reader=new StringReader(xml);
                    dt.ReadXml(reader);
                }
                catch { }
            }
            return dt;
        }

        protected global::System.Web.UI.WebControls.GridView gvList;
        protected global::System.Web.UI.WebControls.Repeater rptEditor;
        protected global::System.Web.UI.WebControls.Repeater rptEditorHeader;        
        protected global::System.Web.UI.WebControls.HiddenField xmldata;
        protected global::System.Web.UI.WebControls.Button btnSave;
        protected global::System.Web.UI.WebControls.Button btnReset;
        protected global::System.Web.UI.WebControls.HiddenField hfIsEdit;
        protected global::System.Web.UI.WebControls.HiddenField hfID;
    }
}