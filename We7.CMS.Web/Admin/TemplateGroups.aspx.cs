using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using We7.CMS.Config;
using System.Xml;
using We7.CMS.Common;
using We7.Framework.Config;
using We7.CMS.ShopService;

namespace We7.CMS.Web.Admin
{
    public partial class TemplateGroups : BasePage
    {
        private List<ProductInfo> _Products;
        public List<ProductInfo> Products
        {
            get
            {
                if (_Products == null)
                {
                    _Products = base.GetFreeTemplates(5);
                }
                return _Products;
            }
        }

        protected override void Initialize()
        {
            string msg = @"您尚未指定当前模板组，您可以：<br>" +
                "（1）在下面可选模板组中，选择一个，点击”应用“；<br>" +
                "（2）创建一个新的模板组（上面工具条中点击”创建模板组“）;<br>" +
                "（3）上传一个模板组包（工具条中点击”上传模板“）。";

            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            if (si.DefaultTemplateGroupFileName != "")
            {
                List<TemplateGroup> tg = new List<TemplateGroup>();
                try
                {
                    tg.Add(TemplateHelper.GetTemplateGroup(si.DefaultTemplateGroupFileName));
                    UseTemplateGroupsDataList.DataSource = tg;
                    UseTemplateGroupsDataList.DataBind();
                }
                catch
                {
                    UseTemplateGroupsLabel.Text = msg;
                }
            }
            else
            {
                UseTemplateGroupsLabel.Text = msg;
            }
            TemplateGroupsDataList.DataSource = TemplateHelper.GetTemplateGroups(null);
            TemplateGroupsDataList.DataBind();

            //获取商城的免费模板
            GetShopFreeTemplateGroup();
        }

        /// <summary>
        /// 获取商城的免费模板
        /// </summary>
        protected void GetShopFreeTemplateGroup()
        {
            if (!base.IsShopServicesCanWork())
            {
                dtTemplateGroups.Visible = false;
                return;
            }
            ShopTemplateGroupsDataList.DataSource = Products;
            ShopTemplateGroupsDataList.DataBind();
        }

        /// <summary>
        /// 得到模板组对应的效果图 2007-07-19 zn
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        protected string GetImageUrl(string FileName)
        {
            string PreviewFileName = FileName + ".jpg";
            string path = "/" + Path.Combine(Constants.TemplateGroupBasePath, PreviewFileName);
            if (!File.Exists(Server.MapPath(path)))
                path = "images/template_default.jpg";
            string phyPath = path.Replace("\\", "/");

            return phyPath;
        }

        public string GetTemplateGroupUrl(string templateGroupName, string str)
        {
            return GetTemplateGroupUrl(templateGroupName, "", str);
        }

        /// <summary>
        /// 配置按钮地址 2007-07-19 zn
        /// </summary>
        /// <param name="TemplateGroupName"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public string GetTemplateGroupUrl(string templateGroupName, string groupName, string action)
        {
            switch (action)
            {
                case "编辑":
                    string url = "";
                    string name = templateGroupName;
                    if (!name.ToLower().EndsWith(".xml"))
                    {
                        name = name + ".xml";
                    }
                    string file = Path.Combine(TemplateHelper.TemplateGroupPath, name);
                    XmlDocument doc = new XmlDocument();
                    doc.Load(file);
                    XmlNode node = doc.SelectSingleNode("TempateGroup");
                    XmlAttribute attr = node.Attributes["ver"];
                    url = String.Format("/admin/TemplateGroupDetail.aspx?file={0}", templateGroupName);
                    if (attr != null)
                    {
                        string version = attr.Value;
                        if (!String.IsNullOrEmpty(version))
                        {
                            if (version.StartsWith("V")) version = version.Remove(0, 1);
                            if (String.CompareOrdinal(version, "2.1") >= 0)
                            {
                                url = String.Format("/admin/Template/TemplateGroupEdit.aspx?file={0}", templateGroupName);
                            }
                        }
                    }
                    return url;
                case "删除":
                    return string.Format("javascript:deleteGroup('{0}','{1}') ", groupName, templateGroupName);
                case "应用":
                    return string.Format("javascript:applyGroup('{0}','{1}') ", groupName, templateGroupName);
                case "打包":
                    return String.Format("TemplateGroupDonwload.aspx?file={0}", templateGroupName);
                default:
                    return "";
            }
        }

        void ApplyDefaultTemplate(string fileName)
        {
            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            si.DefaultTemplateGroupFileName = fileName;
            GeneralConfigs.SaveConfig(si);
        }

        /// <summary>
        /// 截取长度 2007-07-19 zn
        /// </summary>
        /// <param name="content"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        protected string CheckLength(string content, int len)
        {
            if (content.Length > len)
            {
                return string.Format("{0}...", content.Substring(0, len));
            }
            else
            {
                return content;
            }
        }

        protected void deleteGroupButton_Click(object sender, EventArgs e)
        {
            if (DemoSiteMessage) return;//是否是演示站点
            TemplateHelper.DeleteTemplateGroup(currentGroup.Text);
            GeneralConfigInfo config = GeneralConfigs.GetConfig();
            if (config.DefaultTemplateGroupFileName.ToLower() == currentGroup.Text.ToLower())
            {
                config.DefaultTemplateGroupFileName = "";
                GeneralConfigs.SaveConfig(config);
                GeneralConfigs.ResetConfig();
            }
            Response.Redirect(Request.RawUrl);
        }

        protected void applyGroupButton_Click(object sender, EventArgs e)
        {
            if (DemoSiteMessage) return;//是否是演示站点
            ApplyDefaultTemplate(currentGroup.Text);
            Response.Redirect(Request.RawUrl);
        }
    }
}
