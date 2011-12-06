using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using We7.Model.Core.UI;
using Thinkment.Data;
using System.Data;
using We7.Framework.Util;
using We7.Model.UI.Controls.cs;
using We7.Framework;

namespace We7.Model.UI.Controls.we7
{
    public partial class DoubleCascade : We7FieldControl
    {
        string dataSourceType,emptyText, field1, field2, field1TextMapping, field1ValueMapping, field2TextMapping, field2ValueMapping, tableName;
        We7.Model.Core.DataField field1DataField, field2DataField;
        /// <summary>
        /// 初始化控件
        /// </summary>
        public override void InitControl()
        {            
            InitField();
        }
        #region 方法

        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitField()
        {      
            dataSourceType = Control.Params["dataSourceType"];   
            emptyText = Control.Params["emptyText"];
            field1 = Control.Params["field1"];
            field2 = Control.Params["field2"];
            field1DataField = PanelContext.Row.IndexOf(field1);
            field2DataField = PanelContext.Row.IndexOf(field2);            
            InitLable();
            if (!string.IsNullOrEmpty(dataSourceType) && dataSourceType == "db")
            {
                tableName = Control.Params["table"];                
                //父类别对应字段名
                field1TextMapping = Control.Params["field1TextMapping"];
                field1ValueMapping = Control.Params["field1ValueMapping"];
                //子类别对应字段名
                field2TextMapping = Control.Params["field2TextMapping"];
                field2ValueMapping = Control.Params["field2ValueMapping"];
                //db类型绑定数据               
                BindDbData();
            }
            else
            {
                //xml类型绑定数据
                BindXmlData();
            }

        }

        /// <summary>
        /// 初始化标签
        /// </summary>
        void InitLable()
        {
            if (Control.Params.Contains("field1Label"))
            {
                Field1Lable.Text = Control.Params["field1Label"];
            }
            else
            {
                //父类别名称
                string strField1Lable = PanelContext.DataSet.Tables[0].Columns[field1].Label;
                this.Field1Lable.Text = strField1Lable;
            }

            if (Control.Params.Contains("field2Label")!= null)
            {
                Field2Label.Text = Control.Params["field2Label"];
            }
            else
            {
                //子类别名称
                string strField2Lable = PanelContext.DataSet.Tables[0].Columns[field2].Label;
                this.Field2Label.Text = strField2Lable;
            }
        }

        /// <summary>
        /// 绑定数据(xml)
        /// </summary>
        void BindXmlData()
        {
            ICascadeDataProvider provider = GetProvider();

            Field1DropDownList.DataSource = provider.QuryFirstLevel("");
            Field1DropDownList.DataTextField = "key";
            Field1DropDownList.DataValueField = "value";
            Field1DropDownList.DataBind();
            string nodesName = Control.Params["NodesName"];
            string attributesName = Control.Params["AttributesName"];

            Field1DropDownList.Attributes.Add("onchange", "DoubleCascadeField2(this.value,'" + Field2DropDownList.ClientID + "','" + field2TextMapping + "','" + field2ValueMapping + "','" + field1ValueMapping + "','" + Control.Params["table"] + "','" + emptyText + "','" + dataSourceType + "','" + Control.Params["data"] + "','" + nodesName + "','" + attributesName + "')");
            if (!string.IsNullOrEmpty(emptyText))
            {
                Field1DropDownList.Items.Insert(0, new ListItem("请选择", ""));
            }
            Field1DropDownList.SelectedValue = field1DataField.Value == null ? "" : field1DataField.Value.ToString();
            if (!String.IsNullOrEmpty(Field1DropDownList.SelectedValue))
            {
                Field2DropDownList.DataSource = provider.QuerySecondLevel(Field1DropDownList.SelectedValue);
                Field2DropDownList.DataTextField = "key";
                Field2DropDownList.DataValueField = "value";
                Field2DropDownList.DataBind();
            }
            else
            {
                Field2DropDownList.Items.Clear();
            }
            if (!string.IsNullOrEmpty(emptyText))
            {
                Field2DropDownList.Items.Insert(0, new ListItem("请选择", ""));
            }
            if (field2DataField.Value != null && !string.IsNullOrEmpty(field2DataField.Value.ToString()))
            {
                Field2DropDownList.SelectedValue = field2DataField.Value == null ? "" : field2DataField.Value.ToString();
            } 
        }

        #endregion

        #region db绑定
        /// <summary>
        /// 绑定控件数据(db)
        /// </summary>
        void BindDbData()
        {
            string sqlField1 = @" SELECT DISTINCT [" + field1TextMapping + "],[" + field1ValueMapping + "] FROM [" + tableName + "] ";
            IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
            SqlStatement sqlstatement = new SqlStatement();
            sqlstatement.SqlClause = sqlField1;
            db.DbDriver.FormatSQL(sqlstatement);
            DataTable dt = new DataTable();
            using (IConnection conn = db.CreateConnection())
            {
                dt = conn.Query(sqlstatement);
            }
            this.Field1DropDownList.DataSource = dt;
            this.Field1DropDownList.DataTextField = field1TextMapping;
            this.Field1DropDownList.DataValueField = field1ValueMapping;
            this.Field1DropDownList.DataBind();
            if (!string.IsNullOrEmpty(emptyText))
            {
                Field1DropDownList.Items.Insert(0, new ListItem("请选择", ""));
            }
            //给控件添加选择项改变事件            
            this.Field1DropDownList.Attributes.Add("onchange", "DoubleCascadeField2(this.value,'" + Field2DropDownList.ClientID + "','" + field2TextMapping + "','" + field2ValueMapping + "','" + field1ValueMapping + "','" + tableName + "','" + emptyText + "','" + dataSourceType + "')");
            BindDbState();
        }

        /// <summary>
        /// 绑定控件状态(db)
        /// </summary>
        void BindDbState()
        {
            //绑定状态（Field1DropDownList状态）
            Field1DropDownList.SelectedValue = field1DataField.Value == null ? "" : field1DataField.Value.ToString();
            //绑定状态（Field2DropDownList状态）
            if (!String.IsNullOrEmpty(Field1DropDownList.SelectedValue))
            {
                //加载Field2DropDownList数据
                IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
                string sqlField2 = @" SELECT DISTINCT [" + field2TextMapping + "],[" + field2ValueMapping + "] FROM [" + tableName + "] WHERE [" + field1ValueMapping + "]='" + field1DataField.Value.ToString() + "'  ";//WHERE [ID]={0}            
                SqlStatement sqlstatement2 = new SqlStatement();
                sqlstatement2.SqlClause = sqlField2;
                db.DbDriver.FormatSQL(sqlstatement2);
                DataTable dtField2 = new DataTable();
                using (IConnection conn = db.CreateConnection())
                {
                    dtField2 = conn.Query(sqlstatement2);
                }
                this.Field2DropDownList.DataSource = dtField2;
                this.Field2DropDownList.DataTextField = field2TextMapping;
                this.Field2DropDownList.DataValueField = field2ValueMapping;
                this.Field2DropDownList.DataBind();
                if (!string.IsNullOrEmpty(emptyText))
                {
                    Field2DropDownList.Items.Insert(0, new ListItem("请选择", ""));
                }
                if (field2DataField.Value != null)
                {
                    Field2DropDownList.SelectedValue = field2DataField.Value == null ? "" : field2DataField.Value.ToString();
                }
            }
        }
        #endregion

        /// <summary>
        /// 返回控件值
        /// </summary>
        /// <returns></returns>
        public override object GetValue()
        {
            string field1 = Control.Params["field1"];
            string field2 = Control.Params["field2"];
            TypeCode field1Type = PanelContext.DataSet.Tables[0].Columns[field1].DataType;
            TypeCode field2Type = PanelContext.DataSet.Tables[0].Columns[field2].DataType;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add(field1, TypeConverter.StrToObjectByTypeCode(We7Helper.FilterHtmlChars(Request.Form[Field1DropDownList.UniqueID]), field1Type));
            if (Request.Form[Field2DropDownList.UniqueID] == null)
            {
                dic.Add(field2, "");
            }
            else
            {
                dic.Add(field2, TypeConverter.StrToObjectByTypeCode(We7Helper.FilterHtmlChars(Request.Form[Field2DropDownList.UniqueID]), field2Type));//this.Field2DropDownList.SelectedValue
            }
            return dic;
        }

        /// <summary>
        /// 取得查询实体
        /// </summary>
        /// <returns></returns>
        ICascadeDataProvider GetProvider()
        {
            if (dataSourceType == "xml" || String.IsNullOrEmpty(dataSourceType))
            {
                return new XmlCascadeDataProvider(Control.Params);
            }
            else
            {
                DbCascadeDataProvider dbProvider = new DbCascadeDataProvider();
                dbProvider.Field1TextMapping = field1TextMapping;
                dbProvider.Field1ValueMapping = field1ValueMapping;
                dbProvider.Field2TextMapping = field2TextMapping;
                dbProvider.Field2ValueMapping = field2ValueMapping;
                dbProvider.TableName = tableName;
                return dbProvider;
            }
        }
    }
}