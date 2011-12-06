using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.Model.Core;
using System.Data;
using We7.Framework.Util;

namespace We7.CMS.Web.Admin.ContentModel
{
    public partial class EditField : ContentModelBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitPageSetting();
                BindMapping();
                EditBind();
            }
        }

        /// <summary>
        /// ContentModel Type
        /// </summary>
        public string modelType
        {
            get
            {
                string t = We7Request.GetQueryString("modelType");
                if (t.Length == 0)
                    return ModelType.ARTICLE.ToString();
                else
                    return t;
            }
        }


        private void InitPageSetting()
        {
            if (Action == ActionType.Edit)
            {
                this.NameLabel.Text = "修改字段";
                this.SummaryLabel.Text = "用于修改字段";
            }

            //this.ReturnHyperLink.NavigateUrl = "/admin/ContentModel/field.aspx?modelname=" + ModelName;
        }

        public string FieldName
        {
            get
            {
                return RequestHelper.Get<string>("fieldname");
            }
        }
        //编辑绑定

        private void EditBind()
        {
            if (Action == ActionType.Edit)
            {
                We7DataColumn column = new We7DataColumn();
                column = Model.DataSet.Tables[0].Columns[FieldName];


                this.FieldNameTextBox.Text = column.Name;
                this.FieldNameTextBox.Enabled = false;
                this.FieldDataTypeDropDownList.Enabled = false;
                this.MaxlengthTextBox.Enabled = false;

                if (!string.IsNullOrEmpty(column.Label))
                {
                    this.FieldLabelTextBox.Text = column.Label;
                }
                else
                {
                    this.FieldLabelTextBox.Text = column.Name;
                }

                try
                {
                    this.FieldDataTypeDropDownList.SelectedValue = column.DataType.ToString();
                    if (column.DataType.ToString().ToLower() == "string")
                    {
                        this.div_maxlength.Visible = true;
                        this.MaxlengthTextBox.Text = column.MaxLength.ToString();
                    }
                    else
                    {
                        this.div_maxlength.Visible = false;
                    }
                }
                catch { this.FieldDataTypeDropDownList.SelectedValue = "String";
                if (column.DataType.ToString().ToLower() == "string")
                {
                    this.div_maxlength.Visible = true;
                    this.MaxlengthTextBox.Text = column.MaxLength.ToString();
                }
                else
                {
                    this.div_maxlength.Visible = false;
                }
                }

                string[] mappings = GetMappingFileds(Model.Type);
                if (column.Mapping == "Title")
                {
                    //标题
                    TitleCheckBox.Checked = true;
                }

                for (int i = 0; i < mappings.Length; i++)
                {
                    
                    if (!string.IsNullOrEmpty(column.Mapping) && mappings[i].ToLower() == column.Mapping.ToLower())
                    {
                        SearchFieldCheckBox.Checked = true;
                    }
                }

            }
        }


        private void BindMapping()
        {
            ModelInfo model = ModelHelper.GetModelInfoByName(ModelName);

            string table = string.Empty;

            ModelType type = model.Type;
            if (type == ModelType.ARTICLE)
            {
                table = "Article";
            }
            else if (type == ModelType.ADVICE)
            {
                table = "Advice";
            }
            else if (type == ModelType.ACCOUNT)
            {
                table = "Account";
            }

            else
            {
                return;
            }


        }
        //保存字段
        protected void Save()
        {
            //获取对应的modelinfo
            ModelInfo modelInfo = ModelHelper.GetModelInfoByName(ModelName);

            if (modelInfo == null)
            {
                return;
            }
            We7DataColumn column = new We7DataColumn();
            column.DataType = (TypeCode)Enum.Parse(typeof(TypeCode), FieldDataTypeDropDownList.SelectedValue, true);

            if (column.DataType == TypeCode.String)
            {
                column.MaxLength = int.Parse(this.MaxlengthTextBox.Text.Trim());
            }
            column.Label = FieldLabelTextBox.Text.Trim();
            column.Name = FieldNameTextBox.Text.Trim();

            ParameterDirection direction = ParameterDirection.Input;
            string mapping = string.Empty;
            if (TitleCheckBox.Checked)
            {
                direction = ParameterDirection.Output;
                if (hasTitle(modelInfo))
                {
                    Messages.ShowError("已经拥有标题项!");
                    return;
                }
                mapping = "Title";
            }
            if (!TitleCheckBox.Checked && SearchFieldCheckBox.Checked)
            {
                int count = 0;
                direction = ParameterDirection.Output;
                mapping = GetMapping(modelInfo, out count);

                if (string.IsNullOrEmpty(mapping))
                {
                    Messages.ShowError("已经拥有最大查询项:" + count + "!");
                    return;
                }
            }
            column.Direction = direction;
            //column.Require = true;
            column.Mapping = mapping;
            //TODO::tedyding 是否存在Tables 以及多个表
            if (modelInfo.DataSet.Tables == null)
            {
                We7.Model.Core.We7DataTable table = new We7DataTable();
                modelInfo.DataSet.Tables.Add(table);
            }


            modelInfo.DataSet.Tables[0].Columns.AddOrUpdate(column);

            if (string.IsNullOrEmpty(this.FieldNameTextBox.Text.Trim()))
            {
                Messages.ShowError("字段名称不能为空!");
                return;
            }
            bool success = ModelHelper.SaveModelInfo(modelInfo, ModelName);
            if (success)
            {
                if (Action == ActionType.Add)
                {
                    Messages.ShowMessage("添加成功!");
                }
                else if (Action == ActionType.Edit)
                {
                    Messages.ShowMessage("修改成功!");
                }
            }
            else
            {
                if (Action == ActionType.Add)
                {
                    Messages.ShowError("添加失败!");
                }
                else if (Action == ActionType.Edit)
                {
                    Messages.ShowError("修改失败!");
                }
            }
        }

        //获取查询项
        private We7DataColumnCollection GetConditionControl(ModelInfo modelInfo)
        {
            We7DataColumnCollection collections = new We7DataColumnCollection();

            We7DataColumnCollection cols = modelInfo.DataSet.Tables[0].Columns;

            IList<DefaultModel> defaultModels = ModelHelper.GetDefaultModels();

            DefaultModel defaultModel = null;
            for (int i = 0; i < defaultModels.Count; i++)
            {
                if (defaultModels[i].Name == ConvertModelType(modelInfo.Type))
                {
                    defaultModel = defaultModels[i];
                }
            }
            string[] mappingField = null;
            if (defaultModel != null)
            {
                mappingField = defaultModel.MappingFields.Split(new char[] { '|' });
            }

            bool hasTitle = false;

            if (mappingField != null && mappingField.Length > 0)
            {
                for (int i = 0; i < mappingField.Length; i++)
                {

                    for (int j = 0; j < cols.Count; j++)
                    {
                        if (mappingField[i] == cols[j].Mapping)
                        {
                            collections.Add(cols[j]);
                        }

                        if (!hasTitle && cols[j].Mapping == "Title")
                        {
                            collections.Add(cols[j]);
                            hasTitle = true;
                        }

                    }
                }

            }

            return collections;
        }

        private string[] GetMappingFileds(ModelType type)
        {
            IList<DefaultModel> defaultModels = ModelHelper.GetDefaultModels();

            DefaultModel defaultModel = null;
            for (int i = 0; i < defaultModels.Count; i++)
            {
                if (defaultModels[i].Name == ConvertModelType(type))
                {
                    defaultModel = defaultModels[i];
                }
            }

            string[] mappingField = null;
            if (defaultModel != null)
            {
                mappingField = defaultModel.MappingFields.Split(new char[] { '|' });
            }

            return mappingField;

        }
        //获取查询项
        private string GetMapping(ModelInfo modelInfo, out int count)
        {

            We7DataColumnCollection cols = modelInfo.DataSet.Tables[0].Columns;


            IList<DefaultModel> defaultModels = ModelHelper.GetDefaultModels();

            DefaultModel defaultModel = null;
            for (int i = 0; i < defaultModels.Count; i++)
            {
                if (defaultModels[i].Name == ConvertModelType(modelInfo.Type))
                {
                    defaultModel = defaultModels[i];
                }
            }

            string[] mappingField = null;
            if (defaultModel != null)
            {
                mappingField = defaultModel.MappingFields.Split(new char[] { '|' });
            }

            string mapping = string.Empty;

            if (mappingField != null && mappingField.Length > 0)
            {
                bool bout = false;
                for (int i = 0; i < mappingField.Length; i++)
                {
                    if (bout)
                    {
                        break;
                    }

                    for (int j = 0; j < cols.Count; j++)
                    {
                        if (mappingField[i] == cols[j].Mapping)
                        {
                            break;
                        }
                        if (j == cols.Count - 1 && mappingField[i] != cols[j].Mapping)
                        {
                            mapping = mappingField[i];
                            bout = true;
                        }
                    }
                }

            }
            count = mappingField.Length;
            return mapping;
        }


        //转换默认类型
        private string ConvertModelType(ModelType modelType)
        {
            string str = string.Empty;
            switch (modelType)
            {
                case ModelType.ARTICLE:
                    str = "Template.ArticleModel";
                    break;
                case ModelType.ADVICE:
                    str = "Template.AdviceModel";
                    break;
                case ModelType.ACCOUNT:
                    str = "Template.AccountModel";
                    break;
                default:
                    str = "Template.ArticleModel";
                    break;
            }

            return str;
        }
        //是否包含标题项
        private bool hasTitle(ModelInfo modelInfo)
        {
            bool has = false;
            We7DataColumnCollection cols = modelInfo.DataSet.Tables[0].Columns;

            for (int i = 0; i < cols.Count; i++)
            {
                if (cols[i].Mapping == "Title")
                {
                    has = true;
                    break;
                }
            }

            return has;
        }
        private string ConvertControlType(TypeCode typeCode)
        {
            string controlType = "TextInput";
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    controlType = "Select";
                    break;
                case TypeCode.Byte:
                    break;
                case TypeCode.Char:
                    break;
                case TypeCode.DBNull:
                    break;
                case TypeCode.DateTime:
                    controlType = "DateTime";
                    break;
                case TypeCode.Decimal:
                    controlType = "Number";
                    break;
                case TypeCode.Double:
                    controlType = "Number";
                    break;
                case TypeCode.Empty:
                    break;
                case TypeCode.Int16:
                    controlType = "Number";
                    break;
                case TypeCode.Int32:
                    controlType = "Number";
                    break;
                case TypeCode.Int64:
                    controlType = "Number";
                    break;
                case TypeCode.Object:
                    break;
                case TypeCode.SByte:
                    break;
                case TypeCode.Single:
                    break;
                case TypeCode.String:
                    break;
                case TypeCode.UInt16:
                    break;
                case TypeCode.UInt32:
                    break;
                case TypeCode.UInt64:
                    break;
                default:
                    controlType = "TextInput";
                    break;
            }

            return controlType;
        }
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            Save();
        }

        protected void ChangeType(object sender, EventArgs e)
        {
            if (FieldDataTypeDropDownList.SelectedIndex != 0)
            {
                this.div_maxlength.Visible = false;
            }
            else
            {
                this.div_maxlength.Visible = true;
            }
        }

    }
}
