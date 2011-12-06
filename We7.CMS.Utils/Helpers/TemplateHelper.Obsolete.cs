using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Reflection;

using We7;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS.Config;
using We7.CMS.Common;

namespace We7.CMS
{
    /// <summary>
    /// 模板处理类：旧版（2.2以前版）
    /// </summary>
    [Serializable]
    [Helper("We7.TemplateHelper")]
    public partial class TemplateHelper : BaseHelper
    {
        public TemplateHelper()
        {
        }

        [Obsolete]
        public virtual string DataControlPath
        {
            get { return Path.Combine(Root, Constants.ControlBasePath); }
        }
        [Obsolete]
        public virtual DataControl[] GetDataControls(string queryName)
        {
            if (!Directory.Exists(DataControlPath))
            {
                Directory.CreateDirectory(DataControlPath);
            }
            DirectoryInfo di = new DirectoryInfo(DataControlPath);
            FileInfo[] files = di.GetFiles("*" + Constants.ControlFileExtension, SearchOption.TopDirectoryOnly);
            List<DataControl> ds = new List<DataControl>();
            foreach (FileInfo f in files)
            {
                DataControl dc = GetDataControl(f.Name, false);
                if (queryName == null || queryName.Length == 0 || dc.Name.IndexOf(queryName) >= 0)
                {
                    ds.Add(dc); ;
                }
            }
            return ds.ToArray();
        }

        [Obsolete]
        public DataControl GetDataControl(string fileName, bool deep)
        {
            DataControl dc = new DataControl();
            dc.DeepLoad = deep;
            dc.FromFile(DataControlPath, fileName);
            return dc;
        }

        public void DeleteDataControl(string fileName)
        {
            string fn = Path.Combine(DataControlPath, fileName);
            if (File.Exists(fn))
            {
                File.Delete(fn);
            }
            fn = fn + ".xml";
            if (File.Exists(fn))
            {
                File.Delete(fn);
            }
        }

        /// <summary>
        /// 保存模板信息
        /// </summary>
        public void SaveTemplate(Template tp)
        {
            string fn = Path.GetFileName(tp.FileName);
            fn += Constants.TemplateFileExtension;
            string target = Path.Combine(DefaultTemplateGroupPath, fn);
            tp.ToFile(target);
        }

        public TemplateGroup[] GetTemplateGroups(string queryName)
        {
            if (!Directory.Exists(TemplateGroupPath))
            {
                Directory.CreateDirectory(TemplateGroupPath);
            }

            DirectoryInfo di = new DirectoryInfo(TemplateGroupPath);
            FileInfo[] files = di.GetFiles("*" + Constants.TemplateGroupFileExtension, SearchOption.TopDirectoryOnly);
            List<TemplateGroup> ts = new List<TemplateGroup>();
            foreach (FileInfo f in files)
            {
                if (!string.IsNullOrEmpty(Path.GetFileNameWithoutExtension(f.Name)))
                {
                    TemplateGroup g = GetTemplateGroup(f.Name);
                    if (queryName == null || queryName.Length == 0 || g.Name.Contains(queryName))
                    {
                        ts.Add(g); ;
                    }
                }
            }
            return ts.ToArray();
        }

        public TemplateGroup GetTemplateGroupCache(string fileName)
        {
            HttpContext context = HttpContext.Current;
            string key = "CD.TemplateGroup." + fileName;
            if (context.Application[key] == null)
            {
                TemplateGroup g = new TemplateGroup();
                g.FromFile(TemplateGroupPath, fileName);
                context.Application[key] = g;
            }
            return (TemplateGroup)context.Application[key];
        }

        public void SaveTemplateGroup(TemplateGroup data)
        {
            if (data.FileName == null)
            {
                DateTime dt = DateTime.Now;
                data.Created = dt;
                data.FileName = String.Format("{0:D2}{1:D2}{2:D2}_{3:D2}{4:D2}{5:D2}.xml",
                    dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
            }
            data.ToFile(TemplateGroupPath, data.FileName);
            ClearTemplateGroupCache(data.FileName);
        }
        /// <summary>
        /// 保存模板组并返回模板组文件名 zn 2007-07-19 
        /// </summary>
        /// <param name="data"></param>
        /// <returns>返回模板组文件名</returns>
        public string SaveTemplateGroupAndPreviewFile(TemplateGroup data)
        {
            if (data.FileName == null)
            {
                DateTime dt = DateTime.Now;
                data.Created = dt;
                data.FileName = String.Format("{0:D2}{1:D2}{2:D2}_{3:D2}{4:D2}{5:D2}.xml",
                    dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
            }
            data.ToFile(TemplateGroupPath, data.FileName);
            ClearTemplateGroupCache(data.FileName);
            return data.FileName;
        }

        public string SaveTemplateGroupAndPreviewFile(TemplateGroup data, string folderName)
        {
            if (data.FileName == null)
            {
                DateTime dt = DateTime.Now;
                data.Created = dt;
                data.FileName = String.Format("{0}.xml", folderName);
            }
            data.ToFile(TemplateGroupPath, data.FileName);
            ClearTemplateGroupCache(data.FileName);
            return data.FileName;
        }

        public TemplateGroup GetTemplateGroup(string fileName)
        {
            TemplateGroup g = new TemplateGroup();
            g.FromFile(TemplateGroupPath, fileName);
            return g;
        }


        public void DeleteTemplateGroup(string fileName)
        {
            string target = Path.Combine(TemplateGroupPath, fileName);
            if (File.Exists(target))
            {
                File.Delete(target);
            }
            //zn 2007-07-19 删除模板组的预览效果图
            string targetImg = Path.Combine(TemplateGroupPath, fileName + ".jpg");
            if (File.Exists(targetImg))
            {
                File.Delete(targetImg);
            }
            string targetFile = Regex.Split(target, ".xm", RegexOptions.IgnoreCase)[0];
            if (Directory.Exists(targetFile))
            {
                DirectoryInfo targetDI = new DirectoryInfo(targetFile);
                We7Helper.DeleteFileTree(targetDI);
            }
        }

        public string GetThisPageTemplate(string ColumnMode, string ColumnID, string SearchWord, string SeSearchWord)
        {
            string TemplatePath = "";
            bool IsDetail = false;
            if (ColumnMode == "detail" || ColumnMode == "productDetail" || ColumnMode == "contentMode" || ColumnMode == "adviceMode")
            {
                IsDetail = true;
            }
            if (!We7Helper.IsEmptyID(ColumnID))
            {
                HelperFactory helperFactory = (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID];
                ChannelHelper channelHelper = helperFactory.GetHelper<ChannelHelper>();
                Channel ch = channelHelper.GetChannel(ColumnID, null);

                SiteSettingHelper cdHelper = helperFactory.GetHelper<SiteSettingHelper>();

                GeneralConfigInfo si = GeneralConfigs.GetConfig();
                if (si.DefaultTemplateGroupFileName != null && si.DefaultTemplateGroupFileName.Length > 0) //判断一下模板文件是否真实存在
                {
                    if (IsDetail && ch != null && ch.DetailTemplate != null && ch.DetailTemplate != "" && GetTemplate(ch.DetailTemplate) != null)//详细模板
                    {
                        TemplatePath = GetTemplatePath(si.DefaultTemplateGroupFileName, ch.DetailTemplate);
                    }
                    else if (ch != null && ch.TemplateName != null && ch.TemplateName != "" && !IsDetail)
                    {
                        TemplatePath = GetTemplatePath(si.DefaultTemplateGroupFileName, ch.TemplateName);
                    }
                }

                if (TemplatePath == "" || TemplatePath == null) //按别名匹配
                {
                    if (ch != null && ch.Alias != null && ch.Alias != "" && !IsDetail)
                    {
                        string tmp = GetDefaultTemplatePath(ch.Alias, IsDetail);
                        if (tmp != null && tmp != "")
                            TemplatePath = tmp;
                    }
                }
                if (TemplatePath == "" || TemplatePath == null) //按标签匹配
                {
                    if (!IsDetail)
                    {
                        List<string> tags = channelHelper.GetTags(ColumnID);
                        if (tags.Count != 0)
                        {
                            string tmp = GetDefaultTemplatePath(tags[0], IsDetail);
                            if (tmp != null && tmp != "")
                                TemplatePath = tmp;
                        }
                    }
                }
            }

            if (TemplatePath == "" || TemplatePath == null) //赋值默认模板
            {
                //if (ColumnMode == "productDetail")
                //{
                //    TemplatePath = GetDefaultTemplatePath("[productcontentpage]");
                //}
                if (SeSearchWord != null)
                {
                    TemplatePath = GetDefaultTemplatePath("[sesearch]");
                }
                else if (SearchWord != null)
                {
                    TemplatePath = GetDefaultTemplatePath("[search]");
                }
                else if (We7Helper.IsEmptyID(ColumnID) || ColumnID == "/")
                {
                    TemplatePath = GetDefaultTemplatePath("[homepage]");
                }
                else if (ColumnMode == "detail")
                {
                    TemplatePath = GetDefaultTemplatePath("[contentpage]", true);
                }
                else if (ColumnMode == "productDetail")
                {
                    TemplatePath = GetDefaultTemplatePath("[productcontentpage]");
                }
                else if (ColumnMode == "contentMode")
                {
                    TemplatePath = GetDefaultTemplatePath("[ContentMode]");
                }
                else if (ColumnMode == "adviceMode")
                {
                    TemplatePath = GetDefaultTemplatePath("[AdviceMode]");
                }
                else
                {
                    TemplatePath = GetDefaultTemplatePath("[channel]");
                }
            }
            return TemplatePath;
        }

        public string GetThisHtmlPageTemplate(string ColumnMode, string ColumnID, string SearchWord, string SeSearchWord)
        {
            string TemplatePath = "";
            bool IsDetail = false;
            if (ColumnMode == "detail" || ColumnMode == "productDetail" || ColumnMode == "contentMode" || ColumnMode == "adviceMode")
            {
                IsDetail = true;
            }
            if (!We7Helper.IsEmptyID(ColumnID))
            {
                HelperFactory helperFactory = (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID];
                ChannelHelper channelHelper = helperFactory.GetHelper<ChannelHelper>();
                Channel ch = channelHelper.GetChannel(ColumnID, null);

                SiteSettingHelper cdHelper = helperFactory.GetHelper<SiteSettingHelper>();

                GeneralConfigInfo si = GeneralConfigs.GetConfig();
                if (si.DefaultTemplateGroupFileName != null && si.DefaultTemplateGroupFileName.Length > 0) //判断一下模板文件是否真实存在
                {
                    if (IsDetail && ch != null && ch.DetailTemplate != null && ch.DetailTemplate != "" && GetTemplate(ch.DetailTemplate) != null)//详细模板
                    {
                        TemplatePath = GetTemplatePath(si.DefaultTemplateGroupFileName, ch.DetailTemplate);
                    }
                    else if (ch != null && ch.TemplateName != null && ch.TemplateName != "" && !IsDetail)
                    {
                        TemplatePath = GetTemplatePath(si.DefaultTemplateGroupFileName, ch.TemplateName);
                    }
                }

                if (TemplatePath == "" || TemplatePath == null) //按别名匹配
                {
                    if (ch != null && ch.Alias != null && ch.Alias != "" && !IsDetail)
                    {
                        string tmp = GetDefaultTemplatePath(ch.Alias, IsDetail);
                        if (tmp != null && tmp != "")
                            TemplatePath = tmp;
                    }
                }
                if (TemplatePath == "" || TemplatePath == null) //按标签匹配
                {
                    if (!IsDetail)
                    {
                        
                        List<string> tags = channelHelper.GetTags(ColumnID);
                        if (tags.Count != 0)
                        {
                            string tmp = GetDefaultTemplatePath(tags[0], IsDetail);
                            if (tmp != null && tmp != "")
                                TemplatePath = tmp;
                        }
                    }
                }
            }

            if (TemplatePath == "" || TemplatePath == null) //赋值默认模板
            {
                //if (ColumnMode == "productDetail")
                //{
                //    TemplatePath = GetDefaultTemplatePath("[productcontentpage]");
                //}
                if (SeSearchWord != null)
                {
                    TemplatePath = GetDefaultTemplatePath("[sesearch]");
                }
                else if (SearchWord != null)
                {
                    TemplatePath = GetDefaultTemplatePath("[search]");
                }
                else if (We7Helper.IsEmptyID(ColumnID) || ColumnID == "/")
                {
                    TemplatePath = GetDefaultTemplatePath("[homepage]");
                }
                else if (ColumnMode == "detail")
                {
                    TemplatePath = GetDefaultTemplatePath("[contentpage]", true);
                }
                else if (ColumnMode == "productDetail")
                {
                    TemplatePath = GetDefaultTemplatePath("[productcontentpage]");
                }
                else if (ColumnMode == "contentMode")
                {
                    TemplatePath = GetDefaultTemplatePath("[ContentMode]");
                }
                else if (ColumnMode == "adviceMode")
                {
                    TemplatePath = GetDefaultTemplatePath("[AdviceMode]");
                }
                else
                {
                    TemplatePath = GetDefaultTemplatePath("[channel]");
                }
            }
            return TemplatePath;
        }


        public string GetTemplatePath(string fileName)
        {
            return String.Format("/{0}/{1}", Constants.TemplateGroupBasePath.Replace("\\", "/"), fileName);
        }

        string GetTemplatePath(string groupFileName, string templateFileName)
        {
            if (EnableSiteSkins)
                return String.Format("/{0}/{1}/{2}", Constants.SiteSkinsBasePath, Path.GetFileNameWithoutExtension(groupFileName), templateFileName);
            else
                return String.Format("/{0}/{1}", Constants.TemplateUrlPath, templateFileName);
        }

        public string GetDefaultTemplatePath(string templateAlias)
        {
            return GetDefaultTemplatePath(templateAlias, false);
        }

        public string GetDefaultTemplatePath(string templateAlias, bool isDetail)
        {
            HelperFactory helperFactory = (HelperFactory)HttpContext.Current.Application[HelperFactory.ApplicationID];
            SiteSettingHelper cdHelper = helperFactory.GetHelper<SiteSettingHelper>();

            GeneralConfigInfo si = GeneralConfigs.GetConfig();
            if (si.DefaultTemplateGroupFileName != null && si.DefaultTemplateGroupFileName.Length > 0)
            {
                TemplateGroup tg = GetTemplateGroupCache(si.DefaultTemplateGroupFileName);
                string DefaultTemplatePath = "";

                foreach (TemplateGroup.Item it in tg.Items)
                {
                    if (it.Alias == templateAlias && it.IsDetailTemplate == isDetail)
                    {
                        DefaultTemplatePath = it.Template;
                        break;
                    }
                }

                if (DefaultTemplatePath != "")
                {
                    if (EnableSiteSkins)
                        DefaultTemplatePath = String.Format("{0}/{1}", Path.GetFileNameWithoutExtension(si.DefaultTemplateGroupFileName), DefaultTemplatePath);
                    return GetTemplatePath(DefaultTemplatePath);
                }
                else
                    return "";
            }
            else
            {

                return "";
            }
        }

        /// <summary>
        /// 清除application变量值，使其可以重新取得最新值
        /// </summary>
        /// <param name="fileName"></param>
        void ClearTemplateGroupCache(string fileName)
        {
            HttpContext context = HttpContext.Current;
            string key = "CD.TemplateGroup." + fileName;
            context.Application.Remove(key);
        }

        #region 数据控件分类整理
        /// <summary>
        /// 数据控件按关键字等来查询
        /// </summary>
        /// <param name="queryName"></param>
        /// <param name="selectQuery"></param>
        /// <returns></returns>
        public virtual DataControl[] GetDataControls(string queryName, string selectQuery)
        {
            if (!Directory.Exists(DataControlPath))
            {
                Directory.CreateDirectory(DataControlPath);
            }
            DirectoryInfo di = new DirectoryInfo(DataControlPath);
            FileInfo[] files = di.GetFiles("*" + Constants.ControlFileExtension, SearchOption.TopDirectoryOnly);
            List<DataControl> ds = new List<DataControl>();
            foreach (FileInfo f in files)
            {
                DataControl dc = GetDataControl(f.Name, false);
                if (selectQuery != null && selectQuery == "按关键字查找")
                {
                    if (queryName == null || queryName.Length == 0 || dc.Description.ToLower().IndexOf(queryName.ToLower()) >= 0)
                    {
                        ds.Add(dc);
                    }

                }
                if (selectQuery != null && selectQuery == "按文件名查找")
                {
                    if (queryName == null || queryName.Length == 0 || dc.Name.ToLower().IndexOf(queryName.ToLower()) >= 0)
                    {
                        ds.Add(dc);
                    }
                }
            }
            return ds.ToArray();

        }
        /// <summary>
        /// 数据控件按类来分(通过标签)
        /// </summary>
        /// <param name="queryName"></param>
        /// <returns></returns>
        public DataControl[] SortDataControls(string queryName)
        {
            if (!Directory.Exists(DataControlPath))
            {
                Directory.CreateDirectory(DataControlPath);
            }
            DirectoryInfo di = new DirectoryInfo(DataControlPath);
            FileInfo[] files = di.GetFiles("*" + Constants.ControlFileExtension, SearchOption.TopDirectoryOnly);
            List<DataControl> ds = new List<DataControl>();
            foreach (FileInfo f in files)
            {
                DataControl dc = GetDataControl(f.Name, false);
                if (queryName != "其他")
                {
                    if (dc.Description.IndexOf(queryName) >= 0)
                    {
                        ds.Add(dc);
                    }
                }
                else
                {
                    string[] sortNames = new string[] { "文章", "栏目", "图片", "列表", "菜单", "广告", "登录", "商铺" };
                    int j = 0;
                    for (int i = 0; i < sortNames.Length; i++)
                    {
                        if (dc.Tag.IndexOf(sortNames[i]) < 0)
                        {
                            j++;
                        }
                    }
                    if (j == 7)
                    {
                        ds.Add(dc);
                    }
                }
            }
            return ds.ToArray();
        }
        #endregion
    }
}
