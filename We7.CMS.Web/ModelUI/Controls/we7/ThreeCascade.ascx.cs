using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Thinkment.Data;
using System.Data;
using We7.Framework.Util;
using System.Collections;
using System.IO;
using System.Xml;
using We7.Model.UI.Controls.cs;
using We7.Framework;

namespace We7.Model.UI.Controls.we7
{
    public partial class ThreeCascade : We7FieldControl
    {
        string dataSourceType, emptyText, field1, field2, field3;
        string field1TextMapping, field1ValueMapping, field2TextMapping, field2ValueMapping, field3TextMapping, field3ValueMapping, tableName;
        We7.Model.Core.DataField field1DataField, field2DataField, field3DataField;
        /// <summary>
        /// 初始化控件
        /// </summary>
        public override void InitControl()
        {
            InitField();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        void InitField()
        {
            dataSourceType = Control.Params["dataSourceType"];
            emptyText = Control.Params["emptyText"];
            field1 = Control.Params["field1"];
            field2 = Control.Params["field2"];
            field3 = Control.Params["field3"];
            field1DataField = PanelContext.Row.IndexOf(field1);
            field2DataField = PanelContext.Row.IndexOf(field2);
            field3DataField = PanelContext.Row.IndexOf(field3);
           
            if (!string.IsNullOrEmpty(dataSourceType) && dataSourceType == "db")
            {
                InitLable();
                tableName = Control.Params["table"];
                //一级类别对应字段名
                field1TextMapping = Control.Params["field1TextMapping"];
                field1ValueMapping = Control.Params["field1ValueMapping"];
                //二级类别对应字段名
                field2TextMapping = Control.Params["field2TextMapping"];
                field2ValueMapping = Control.Params["field2ValueMapping"];
                //三级类别对应字段名
                field3TextMapping = Control.Params["field3TextMapping"];
                field3ValueMapping = Control.Params["field3ValueMapping"];
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
            //一级类别标签
            string strField1Lable = PanelContext.DataSet.Tables[0].Columns[field1].Label;
            this.Field1Lable.Text = strField1Lable;
            //二级类别标签
            string strField2Lable = PanelContext.DataSet.Tables[0].Columns[field2].Label;
            this.Field2Label.Text = strField2Lable;
            //三级类别标签
            string strField3Lable = PanelContext.DataSet.Tables[0].Columns[field3].Label;
            this.Field3Label.Text = strField3Lable;
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

            Field1DropDownList.Attributes.Add("onchange", "ThreeCascadeField2('" + Field1DropDownList.ClientID + "',this.value,'" + Field2DropDownList.ClientID + "','" + field2TextMapping + "','" + field2ValueMapping + "','" + field1ValueMapping + "','" + tableName + "','" + Field3DropDownList.ClientID + "','" + field3TextMapping + "','" + field3ValueMapping + "','" + emptyText + "','" + dataSourceType + "','" + Control.Params["data"] + "','" + nodesName + "','" + attributesName + "')");
            this.Field2DropDownList.Attributes.Add("onchange", "ThreeCascadeField3('" + Field1DropDownList.ClientID + "',this.value,'" + Field3DropDownList.ClientID + "','" + field3TextMapping + "','" + field3ValueMapping + "','" + field2ValueMapping + "','" + field1ValueMapping + "','" + tableName + "','" + emptyText + "','" + dataSourceType + "','" + Control.Params["data"] + "','" + nodesName + "','" + attributesName + "')");
            Field1DropDownList.Items.Insert(0, new ListItem("请选择", ""));
            Field1DropDownList.SelectedValue = field1DataField.Value == null ? "" : field1DataField.Value.ToString();
            if (!String.IsNullOrEmpty(Field1DropDownList.SelectedValue))
            {
                Field2DropDownList.DataSource = provider.QuerySecondLevel(field1DataField.Value.ToString());
                Field2DropDownList.DataTextField = "key";
                Field2DropDownList.DataValueField = "value";
                Field2DropDownList.DataBind();
                if (!string.IsNullOrEmpty(emptyText))
                {
                    Field2DropDownList.Items.Insert(0, new ListItem("请选择", ""));
                }
                if (field2DataField.Value != null)
                {
                    Field2DropDownList.SelectedValue = field2DataField.Value == null ? "" : field2DataField.Value.ToString();
                }
            }
            if (!String.IsNullOrEmpty(Field2DropDownList.SelectedValue))
            {
                Field3DropDownList.DataSource = provider.QueryThirdLevel(field1DataField.Value.ToString(), field2DataField.Value.ToString());
                Field3DropDownList.DataTextField = "key";
                Field3DropDownList.DataValueField = "value";
                Field3DropDownList.DataBind();
                if (!string.IsNullOrEmpty(emptyText))
                {
                    Field3DropDownList.Items.Insert(0, new ListItem("请选择", ""));
                }
                if (field3DataField.Value != null)
                {
                    Field3DropDownList.SelectedValue = field3DataField.Value == null ? "" : field3DataField.Value.ToString();
                }
            }      
        }



        /// <summary>
        /// 绑定数据(db)
        /// </summary>
        private void BindDbData()
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
            Field1DropDownList.Items.Insert(0, new ListItem("请选择", ""));
            //给控件添加选择项改变事件
            this.Field1DropDownList.Attributes.Add("onchange", "ThreeCascadeField2('" + Field1DropDownList.ClientID + "',this.value,'" + Field2DropDownList.ClientID + "','" + field2TextMapping + "','" + field2ValueMapping + "','" + field1ValueMapping + "','" + tableName + "','" + this.Field3DropDownList.ClientID + "','" + field3TextMapping + "','" + field3ValueMapping + "','" + emptyText + "','" + dataSourceType + "')");
            this.Field2DropDownList.Attributes.Add("onchange", "ThreeCascadeField3('" + Field1DropDownList.ClientID + "',this.value,'" + Field3DropDownList.ClientID + "','" + field3TextMapping + "','" + field3ValueMapping + "','" + field2ValueMapping + "','" + field1ValueMapping + "','" + tableName + "','" + emptyText + "','" + dataSourceType + "')");
            BindDbState();
        }

       
        /// <summary>
        /// 绑定控件状态
        /// </summary>
        private void BindDbState()
        {
            IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
            //保存状态（Field1DropDownList状态）
            Field1DropDownList.SelectedValue = field1DataField.Value == null ? "" : field1DataField.Value.ToString();
            //保存状态（Field2DropDownList状态）
            if (!String.IsNullOrEmpty(Field1DropDownList.SelectedValue))
            {
                //加载Field2DropDownList数据
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
            //保存状态（Field3DropDownList状态）
            if (!String.IsNullOrEmpty(Field2DropDownList.SelectedValue))
            {
                //加载Field2DropDownList数据
                string sqlField3 = @" SELECT DISTINCT [" + field3TextMapping + "],[" + field3ValueMapping + "] FROM [" + tableName + "] WHERE [" + field2ValueMapping + "]='" + field2DataField.Value.ToString() + "'  ";//WHERE [ID]={0}            
                SqlStatement sqlstatement3 = new SqlStatement();
                sqlstatement3.SqlClause = sqlField3;
                db.DbDriver.FormatSQL(sqlstatement3);
                DataTable dtField3 = new DataTable();
                using (IConnection conn = db.CreateConnection())
                {
                    dtField3 = conn.Query(sqlstatement3);
                }
                this.Field3DropDownList.DataSource = dtField3;
                this.Field3DropDownList.DataTextField = field3TextMapping;
                this.Field3DropDownList.DataValueField = field3ValueMapping;
                this.Field3DropDownList.DataBind();
                if (!string.IsNullOrEmpty(emptyText))
                {
                    Field3DropDownList.Items.Insert(0, new ListItem("请选择", ""));
                }
                if (field3DataField.Value != null)
                {
                    Field3DropDownList.SelectedValue = field3DataField.Value == null ? "" : field3DataField.Value.ToString();
                }
            }
        }


        /// <summary>
        /// 返回控件值
        /// </summary>
        /// <returns></returns>
        public override object GetValue()
        {
            string field1 = Control.Params["field1"];
            string field2 = Control.Params["field2"];
            string field3 = Control.Params["field3"];
            TypeCode field1Type = PanelContext.DataSet.Tables[0].Columns[field1].DataType;
            TypeCode field2Type = PanelContext.DataSet.Tables[0].Columns[field2].DataType;
            TypeCode field3Type = PanelContext.DataSet.Tables[0].Columns[field3].DataType;
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
            if (Request.Form[Field3DropDownList.UniqueID] == null)
            {
                dic.Add(field3, "");
            }
            else
            {
                dic.Add(field3, TypeConverter.StrToObjectByTypeCode(We7Helper.FilterHtmlChars(Request.Form[Field3DropDownList.UniqueID]), field3Type));//this.Field2DropDownList.SelectedValue
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
                dbProvider.Field3TextMapping = field3TextMapping;
                dbProvider.Field3ValueMapping = field3ValueMapping;
                dbProvider.TableName = tableName;
                return dbProvider;
            }
        }
    }
    
}