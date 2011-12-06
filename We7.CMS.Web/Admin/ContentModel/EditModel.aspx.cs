using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.Model.Core;
using ContentModelEntity = We7.Model.Core.ContentModel;
using We7.Framework.Util;

namespace We7.CMS.Web.Admin.ContentModel
{
    /// <summary>
    /// ADD/Edit Model
    /// </summary>
    public partial class EditModel : ContentModelBasePage
    {
        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind();
            }
        }

        /// <summary>
        /// Is Edit
        /// </summary>
        protected bool IsEdit
        {
            get { return Action == ActionType.Edit; }
        }


        /// <summary>
        /// Model Type 
        /// </summary>
        public string MyModelType
        {
            get
            {
                return Request["type"];
            }
        }

        /// <summary>
        /// 模型名称（区别用户和信息）
        /// </summary>
        public string ModelTypeName
        {
            get
            {
                switch (MyModelType.ToUpper())
                {
                    case "ACCOUNT": return "用户模型";
                    case "ARTICLE": return "信息模型";
                    default: return "反馈模型";
                }
            }
        }

        // 初始化绑定
        private void Bind()
        {
            //绑定控件

            BindModelGroup();
            //新建内容模型
            if (Action == ActionType.Add)
            {
                this.NameLabel.Text = "新建" + ModelTypeName;
                this.SummaryLabel.Text = "请您首先填写" + ModelTypeName + "的基本描述信息";

            }
            //修改内容模型
            else if (Action == ActionType.Edit)
            {
                if (!string.IsNullOrEmpty(ModelName))
                {
                    BindControlsValue();
                }
                this.NameLabel.Text = "修改" + ModelTypeName;
                this.SummaryLabel.Text = "修改" + ModelTypeName + "的概要描述信息";
            }
            else
            {
                Response.Redirect("~/admin/error.aspx");
            }
        }

        private void BindModelGroup()
        {

            ModelGroupCollection groupCollection = ModelHelper.GetModelGroups();

            if (groupCollection != null && groupCollection.Count > 0)
            {
                this.GroupDropDownList.DataSource = groupCollection;
                this.GroupDropDownList.DataTextField = "Label";
                this.GroupDropDownList.DataValueField = "Name";
                this.GroupDropDownList.DataBind();
                this.GroupDropDownList.SelectedValue = Request[GroupDropDownList.UniqueID];
            }
        }
        //修改时绑定字段
        private void BindControlsValue()
        {
            ContentModelEntity model = ModelHelper.GetContentModelByName(ModelName);
            if (model != null)
            {
                string[] tempName = model.Name.Split(new char[] { '.' });

                this.GroupDropDownList.SelectedValue = tempName[0].Trim();
                this.ModelNameTextBox.Text = tempName[1];
                this.ModelStateDropDownList.SelectedValue = model.State.ToString();
                this.ModelLabelTextBox.Text = model.Label;
                this.ModelNameTextBox.Enabled = false;
                this.DescriptionTextBox.Text = model.Description;


                //tedyding 2010-10-15
                ModelInfo mi = ModelHelper.GetModelInfoByName(ModelName);
                if (mi != null)
                {
                    this.AuthorityTypeCheckBox.Checked = mi.AuthorityType;
                }
                if (!string.IsNullOrEmpty(mi.Parameters))
                {
                    string[] p = mi.Parameters.Split(':');
                    if (p[0] == "role" && p.Length > 1)
                        this.RoleTextBox.Text = p[1];
                }
                GroupDropDownList.Enabled = false;
            }
        }

        bool CheckInput()
        {
            if (String.IsNullOrEmpty(ModelLabelTextBox.Text.Trim()))
            {
                Messages.ShowError("模型名称不能为空");
                return false;
            }
            if (String.IsNullOrEmpty(ModelNameTextBox.Text.Trim()))
            {
                Messages.ShowError("配置文件名不能为空");
                return false;
            }
            if (!Utils.IsCharAndNumber(ModelNameTextBox.Text.Trim()))
            {
                Messages.ShowError("配置文件名只能为英文字母或字符");
                return false;
            }


            ContentModelCollection cmc = ModelHelper.GetAllContentModel();
            foreach (We7.Model.Core.ContentModel cm in cmc)
            {
                if (!IsEdit)
                {
                    if (String.Compare(cm.Name, String.Format("{0}.{1}", GroupDropDownList.SelectedValue, ModelNameTextBox.Text.Trim()), true) == 0)
                    {
                        Messages.ShowError("当前模型配置文件名已存在，请改更配置文件名称");
                        return false;
                    }

                    if (String.Compare(cm.Label, ModelLabelTextBox.Text.Trim(), true) == 0)
                    {
                        Messages.ShowError("当前模型名称已存在,请更改模型名称");
                        return false;
                    }
                }
                else
                {
                    string defaultModelName = RequestHelper.Get<string>("modelname");
                    ModelInfo modelInfo = ModelHelper.GetModelInfoByName(defaultModelName);

                    if (String.Compare(cm.Label, ModelLabelTextBox.Text.Trim(), true) == 0 &&
                        String.Compare(cm.Label, modelInfo.Label, true) != 0)
                    {
                        Messages.ShowError("当前模型名称已存在,请更改模型名称");
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 获得DefaultModelName
        /// TODO:入口不确定 此方法有待修正
        /// </summary>
        /// <returns></returns>
        private string GetDefaultModelName()
        {
            IList<DefaultModel> list = ModelHelper.GetDefaultModels();
            foreach (DefaultModel item in list)
            {
                if (item.Name.ToLower().Contains(MyModelType.ToLower()))
                    return item.Name;
            }
            return "Template.ArticleModel";
        }

        /// <summary>
        /// Form Submit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (!CheckInput())
                return;
            string defaultModelName = RequestHelper.Get<string>("modelname");
            if (String.IsNullOrEmpty(defaultModelName))
            {
                defaultModelName = GetDefaultModelName();
            }
            //获取默认的模型
            ModelInfo modelInfo = ModelHelper.GetModelInfoByName(defaultModelName);

            ContentModelEntity model = new ContentModelEntity();
            model.Name = string.Format("{0}.{1}", GroupDropDownList.Enabled ? Request[GroupDropDownList.UniqueID] : GroupDropDownList.SelectedValue, ModelNameTextBox.Text.Trim());
            model.State = Convert.ToInt32(ModelStateDropDownList.SelectedValue);
            model.Label = ModelLabelTextBox.Text.Trim();
            model.DefaultContentName = defaultModelName;
            ModelType modelType = ModelType.ARTICLE;
            if (defaultModelName == "Template.AdviceModel" || MyModelType.ToLower() == "advice")
            {
                modelType = ModelType.ADVICE;
            }
            else if (defaultModelName == "Template.AccountModel" || MyModelType.ToLower() == "account")
            {
                modelType = ModelType.ACCOUNT;
                if (!string.IsNullOrEmpty(RoleTextBox.Text))
                    modelInfo.Parameters = "role:" + RoleTextBox.Text; ;
            }

            model.Description = DescriptionTextBox.Text.Trim();
            model.Type = modelType;
            if (string.IsNullOrEmpty(this.ModelNameTextBox.Text.Trim()))
            {
                Messages.ShowError("模型名称不能为空!");
                return;
            }
            bool success = ModelHelper.SaveContentModel(model);
            BindModelGroup();
            if (success)
            {
                modelInfo.Label = model.Label;
                modelInfo.ModelName = model.Name;
                modelInfo.Desc = model.Description;
                modelInfo.DataSet.Tables[0].Name = ModelNameTextBox.Text.Trim();
                modelInfo.Type = modelType;
                modelInfo.AuthorityType = AuthorityTypeCheckBox.Checked;

                if (defaultModelName == "Template.ArticleModel")
                {
                    string tempvalue = modelInfo.Layout.Panels["list"].ListInfo.Groups[0].Columns["Manage"].Params["cmd"].Replace("Template.ArticleModel", model.Name);
                    modelInfo.Layout.Panels["list"].ListInfo.Groups[0].Columns["Manage"].Params["cmd"] = tempvalue;
                }
                ModelHelper.SaveModelInfo(modelInfo, model.Name);
            }

            if (success)
            {
                string tempName = RequestHelper.Get<string>("modelname");
                string msg = ModelTypeName + " {0} 成功！继续 <a href='EditLayout.aspx?modelname={1}' target='_blank'>编辑模型布局</a> ";
                if (Action == ActionType.Add)
                {
                    msg = string.Format(msg, "添加", model.Name);
                }
                else if (Action == ActionType.Edit)
                {
                    msg = string.Format(msg, "修改", tempName);
                }
                Messages.ShowMessage(msg);
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
    }
}
