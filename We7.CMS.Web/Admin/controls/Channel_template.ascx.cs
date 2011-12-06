using System;
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
using System.IO;

using We7.CMS;
using We7.CMS.Controls;
using We7.CMS.Config;
using We7.CMS.Common;
using We7.Framework;
using We7.Framework.Config;

namespace We7.CMS.Web.Admin.controls
{
    public partial class Channel_template : BaseUserControl
    {
        static string TextCannotModifyColumn = "不能修改栏目";

        string ParentID
        {
            get
            {
                string pid = Request["pid"];
                if (pid == null || pid.Length == 0)
                {
                    if (ParentIDTextBox.Text.Length == 0)
                    {
                        pid = We7Helper.EmptyGUID;
                    }
                    else
                        pid = ParentIDTextBox.Text;
                }
                return pid;
            }
            set
            {
                ParentIDTextBox.Text = value;
            }
        }

        public string ChannelID
        {
            get { return Request["id"]; }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                InitControls();
                if (We7Helper.IsEmptyID(ChannelID))
                {
                    throw new CDException(TextCannotModifyColumn);
                }
                LoadFromMap();
            }
        }

        protected void SaveButton_ServerClick(object sender, EventArgs e)
        {
            if (DemoSiteMessage)
            {
                return;
            }

            SaveToMapFile();
            SaveInformationToDB();
            LoadFromMap();
            InitControls();
        }

        void InitControls()
        {
            string mapString = @"    栏目主页：<a href='{0}?template={8}{1}' target='_blank'>{1}</a>
    <br />  
<span style='display:none' >
     栏目列表页：<a href='{5}?template={8}{2}' target='_blank'>{2}</a>
    <br />  </span>
     栏目详细页：<a href='{6}?template={8}{3}' target='_blank'>{3}</a>
    <br />  
     栏目搜索页：<a href='{7}?template={8}{4}' target='_blank'>{4}</a>
    <br />  ";
            Channel ch = ChannelHelper.GetChannel(ChannelID, null);
            if (ch != null)
            {
                string url1 = TemplateMap.GetTemplateFromMap("", ch.FullUrl);
                string url2 = TemplateMap.GetTemplateFromMap("list", ch.FullUrl);
                string url3 = TemplateMap.GetTemplateFromMap("detail", ch.FullUrl);
                string url4 = TemplateMap.GetTemplateFromMap("search", ch.FullUrl);

                string[] fields = new string[] { "ID", "SN", "Updated", "ChannelFullUrl", "State" };
                List<Article> al = ArticleHelper.GetArticlesByUrl(ch.FullUrl.Replace('*', '%'), 0, 1, fields, true);
                string url6 = "";
                if (al != null && al.Count > 0)
                    url6 = ch.FullUrl + al[0].FullUrl;
                else
                    url6 = "#";

                GeneralConfigInfo gi = GeneralConfigs.GetConfig();
                string skinPath = gi.DefaultTemplateGroupFileName;
                if (!string.IsNullOrEmpty(skinPath))
                {
                    skinPath = skinPath.Remove(skinPath.ToLower().IndexOf(".xml"));
                    skinPath = string.Format("/{0}/{1}/", gi.SiteSkinsBasePath, skinPath);
                    mapString = string.Format(mapString, ch.FullUrl, url1, url2, url3, url4, ch.ListUrl, url6, ch.SearchUrl, skinPath);
                    MapListLiteral.Text = mapString;
                }
            }
        }

        #region 保存信息

        void SaveInformationToDB()
        {
            Channel ch = ChannelHelper.GetChannel(ChannelID, null);
            if (ch != null)
            {
                ch.TemplateName = TemplateIDTextBox.Text;
                ch.DetailTemplate = DetailTemplateIDTextBox.Text;

                ChannelHelper.UpdateChannel(ch, new string[] { "TemplateName", "DetailTemplate" });
                //ApplyTemplateToSubChannels();
                Messages.ShowMessage("栏目信息已经成功更新。");

                //记录日志
                string content = string.Format("修改了栏目“{0}”的信息", ch.Name );
                AddLog("编辑栏目", content);
            }
        }

        void SaveToMapFile()
        {
            ChannelTemplateGroup cg = new ChannelTemplateGroup();
            cg.IndexTemplate = TemplateIDTextBox.Text;
            cg.DetailTemplate = DetailTemplateIDTextBox.Text;
            cg.ListTemplate = ListTemplateIDTextBox.Text;
            cg.SearchTemplate = SearchTemplateIDTextBox.Text;
            cg.IndexInherit = indexCheckbox.Checked;
            cg.ListInherit = listCheckbox.Checked;
            cg.DetailInherit = detailCheckbox.Checked;
            cg.SearchInherit = searchCheckbox.Checked;

            string tmpfolder = CDHelper.Config.DefaultTemplateGroupFileName;
            tmpfolder = tmpfolder.Remove(tmpfolder.IndexOf("."));
            tmpfolder = Path.Combine(HttpContext.Current.Server.MapPath("~/" + CDHelper.Config.SiteSkinsBasePath), tmpfolder + ".map");
            Channel ch = ChannelHelper.GetChannel(ChannelID, null);
            TemplateMap.SaveToTemplateMapFile(cg, ch.FullUrl, tmpfolder);
            TemplateMap.ResetInstance();
        }
        #endregion

        void LoadFromMap()
        {
            string tmpfolder = CDHelper.Config.DefaultTemplateGroupFileName;
            tmpfolder = tmpfolder.Remove(tmpfolder.IndexOf("."));
            tmpfolder = Path.Combine(HttpContext.Current.Server.MapPath("~/" + CDHelper.Config.SiteSkinsBasePath), tmpfolder + ".map");
            if (File.Exists(tmpfolder))
            {
                TemplateMap tm = new TemplateMap(tmpfolder);
                Channel ch = ChannelHelper.GetChannel(ChannelID, null);
                ChannelTemplateGroup cg = tm.GetChannelTemplate(ch.FullUrl);
                if (cg != null)
                {
                    TemplateIDTextBox.Text = cg.IndexTemplate;
                    DetailTemplateIDTextBox.Text = cg.DetailTemplate;
                    ListTemplateIDTextBox.Text = cg.ListTemplate;
                    SearchTemplateIDTextBox.Text = cg.SearchTemplate;
                    indexCheckbox.Checked = cg.IndexInherit;
                    listCheckbox.Checked = cg.ListInherit;
                    detailCheckbox.Checked = cg.DetailInherit;
                    searchCheckbox.Checked = cg.SearchInherit;
                }
            }
            else
                ShowInfomation();
        }


        void ShowInfomation()
        {
            Channel ch = ChannelHelper.GetChannel(ChannelID, null);
            ParentID = ch.ParentID;

            TemplateIDTextBox.Text = ch.TemplateName;
            DetailTemplateIDTextBox.Text = ch.DetailTemplate;
        }

        protected void CreateMapLink_Click(object sender, EventArgs e)
        {
            string file = CDHelper.Config.DefaultTemplateGroupFileName;
            if (file != null && file != "" && File.Exists(Path.Combine(TemplateHelper.TemplateGroupPath, file)))
            {
                SkinInfo data = TemplateHelper.GetSkinInfo(file);
                TemplateHelper.CreateMapFileFromSkinInfo(data);
                Messages.ShowMessage("模版地图已成功生成！");
                InitControls();
            }
        }
    }
}