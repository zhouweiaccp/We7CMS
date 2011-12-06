using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using We7.CMS.Common;
using We7.Framework;
using We7.Framework.Util;
using System.Web;

namespace We7.CMS
{
    /// <summary>
    /// 控件业务类
    /// </summary>
    [Serializable]
    [Helper("We7.DataControlHelper")]
    public class DataControlHelper : BaseHelper
    {
        /// <summary>
        /// DataControlInfo的关键字
        /// </summary>
        private const string DCIKEY = "$DCIKEY";

        #region 这儿是扩展的方法

        public void CreateDataControlIndex()
        {
            string path=Path.Combine(Constants.We7ControlPhysicalPath,"We7DataControlIndex.xml");
            List<DataControlInfo> ctrs = GetControls();
            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", null));
            XmlNode root=doc.CreateElement("Controls");
            doc.AppendChild(root);
            foreach (DataControlInfo dcInfo in ctrs)
            {
                XmlElement ctrEl = doc.CreateElement("control");
                ctrEl.SetAttribute("name", dcInfo.Name);
                ctrEl.SetAttribute("group",dcInfo.Group);
                ctrEl.SetAttribute("path", dcInfo.Path);
                ctrEl.SetAttribute("directory", dcInfo.Directory);
                ctrEl.SetAttribute("desc",dcInfo.Desc);

                foreach (DataControl dc in dcInfo.Controls)
                {
                    XmlElement itemEl = doc.CreateElement("item");
                    itemEl.SetAttribute("control",dc.Control);
                    itemEl.SetAttribute("name", dc.Name);
                    itemEl.SetAttribute("type", dc.Type);
                    itemEl.SetAttribute("version", dc.Version);
                    itemEl.SetAttribute("created", dc.Created.ToString("yyyy-MM-dd"));
                    itemEl.SetAttribute("desc", dc.Description);
                    itemEl.SetAttribute("demoUrl", dc.DemoUrl);
                    itemEl.SetAttribute("fileName", dc.FileName);
                    ctrEl.AppendChild(itemEl);
                }
                root.AppendChild(ctrEl);
            }
            doc.Save(path);
        }

        public List<DataControlInfo> GetDataControlsInfos()
        {
            List<DataControlInfo> result=AppCtx.Cache.RetrieveObject<List<DataControlInfo>>(DCIKEY);
            if (result == null)
            {
                string path = Path.Combine(Constants.We7ControlPhysicalPath, "We7DataControlIndex.xml");
                if (!File.Exists(path))
                {
                    CreateDataControlIndex();
                    if (!File.Exists(path))
                        throw new Exception(path + "文件不存在!");
                }

                result = new List<DataControlInfo>();
                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNodeList ctrs = doc.DocumentElement.SelectNodes("control");
                foreach (XmlElement ctr in ctrs)
                {
                    DataControlInfo dci = new DataControlInfo();
                    dci.Name = ctr.GetAttribute("name");
                    dci.Group = ctr.GetAttribute("group");
                    dci.Path = ctr.GetAttribute("path");
                    dci.Directory = ctr.GetAttribute("directory");
                    dci.Desc = ctr.GetAttribute("desc");
                    dci.Default = ctr.GetAttribute("default");
                    XmlNodeList items = ctr.SelectNodes("item");
                    foreach (XmlElement item in items)
                    {
                        DataControl dc = new DataControl();
                        dc.Control = item.GetAttribute("control");
                        dc.Name = item.GetAttribute("name");
                        dc.Type = item.GetAttribute("type");
                        dc.Version = item.GetAttribute("version");
                        DateTime dt;
                        DateTime.TryParse(item.GetAttribute("created"), out dt);
                        dc.Created = dt;
                        dc.FileName = item.GetAttribute("fileName");
                        dc.DemoUrl = item.GetAttribute("demoUrl");
                        dci.Controls.Add(dc);
                    }
                    if(dci.Controls==null||dci.Controls.Count==0)
                        continue;

                    if (String.IsNullOrEmpty(dci.Default))
                    {
                        dci.DefaultControl = dci.Controls.Find(delegate(DataControl dctr)
                        {
                            return String.Compare(dci.Default, dctr.Control,true)==0;
                        });
                    }

                    if (dci.DefaultControl == null)
                    {
                        dci.DefaultControl = dci.Controls[0];
                    }

                    result.Add(dci);
                }
                AppCtx.Cache.AddObjectWithFileChange(DCIKEY, result, path);
            }
            return result;
        }

        /// <summary>
        /// 这个是新扩展的方法，用于取得控件的文件路径
        /// </summary>
        /// <param name="ctr"></param>
        /// <returns></returns>
        public DataControl GetDCByCtrName(string ctr)
        {
            List<DataControlInfo> list = GetDataControlsInfos();
            foreach(DataControlInfo dci in list)
            {
                foreach(DataControl dc in dci.Controls)
                {
                    if (String.Compare(ctr, dc.Control, true) == 0)
                    {
                        return dc;
                    }
                }
            }
            return null;
        }
       
        /// <summary>
        /// 取得控件目录组
        /// </summary>
        /// <returns></returns>
        public List<DataControlGroup> GetDataControlGroups()
        {
            List<DataControlGroup> goups = AppCtx.Cache.RetrieveObject<List<DataControlGroup>>("$DataControlGroups");
            if (goups == null)
            {
                goups = new List<DataControlGroup>();
                List<DataControlInfo> list  =GetControls();
                foreach (DataControlInfo dci in list)
                {
                    DataControlGroup gp=goups.Find(delegate(DataControlGroup dcg)
                    {
                        return dcg.Name == dci.Group;
                    });
                    if (gp == null)
                    {
                        goups.Add(new DataControlGroup() { Name = dci.Group, Label = dci.Group });
                    }
                }
                if (Directory.Exists(Constants.We7WidgetsPhysicalFolder))
                {
                    string[] files = new string[] { Path.Combine(Constants.We7WidgetsPhysicalFolder, "We7DataControlIndex.xml"), Path.Combine(Constants.We7WidgetsPhysicalFolder, "WidgetsIndex.xml") };
                    AppCtx.Cache.AddObjectWithFileChange("$DataControlGroups", goups, files);
                }
            }
            return goups;
        }

        #endregion

        /// <summary>
        /// 获取控件的参数
        /// </summary>
        /// <param name="ctr">控件名称</param>
        /// <returns></returns>
        public DataControl GetDataControl(string ctr)
        {
            DataControlInfo info = GetDataControlInfo(ctr);
            if (info.Controls != null)
            {
                return info.Controls.Find(delegate(DataControl dc)
                {
                    return dc.Control == ctr;
                });
            }
            return null;
        }

        public DataControl GetDataControlByPath(string fileName)
        {
            DataControlInfo info = GetDataControlInfoByPath(fileName);
            string ctr = GetControlName(fileName);
            if (info.Controls != null)
            {
                return info.Controls.Find(delegate(DataControl dc)
                {
                    return dc.Control == ctr;
                });
            }
            return null;
        }

        /// <summary>
        /// 取得控件的完整信息
        /// </summary>
        /// <param name="ctr">控件名称</param>
        /// <returns></returns>
        public DataControlInfo GetDataControlInfo(string ctr)
        {
            DataControlInfo info = new DataControlInfo();
            info.Load(GetDataControlPath(ctr.Split('.')[0]));
            return info;
        }

        /// <summary>
        /// 取得控件完整信息
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public DataControlInfo GetDataControlInfoByPath(string filePath)
        {
            DataControlInfo info = new DataControlInfo();
            info.Load(GetDataControlPathFromPath(filePath));
            return info;
        }

        /// <summary>
        /// 取得所有控件信息
        /// </summary>
        /// <returns></returns>
        public List<DataControlInfo> GetControls()
        {
            List<DataControlInfo> lstctrs=new List<DataControlInfo>();
            LoadDataControls(lstctrs, Constants.We7ControlPhysicalPath, "系统控件");
            LoadModelsControls(lstctrs);
            LoadPluginContrls(lstctrs);            
            return lstctrs;
        }

        private void LoadModelsControls(List<DataControlInfo> controls)
        {
            DirectoryInfo di = new DirectoryInfo(Constants.We7ModelPhysicalPath);
            if (di.Exists)
            {
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    LoadDataControls(controls,dir.FullName, "模型控件");
                }
            }
        }

        /// <summary>
        /// 加载插件控件
        /// </summary>
        /// <param name="controls"></param>
        private void LoadPluginContrls(List<DataControlInfo> controls)
        {
            DirectoryInfo di = new DirectoryInfo(Constants.We7PluginPhysicalPath);
            if (di.Exists)
            {
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    LoadDataControls(controls,Path.Combine(dir.FullName, "Control"), "插件控件");
                }
            }
        }

        /// <summary>
        /// 取得指定目录下的控件
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private void LoadDataControls(List<DataControlInfo> controls,string dir,string group)
        {            
            DirectoryInfo di = new DirectoryInfo(dir);

            if (!di.Exists)
                return;

            DirectoryInfo[] cdi = di.GetDirectories();
            foreach (DirectoryInfo d in cdi)
            {
                try
                {
                    FileInfo[] fs = d.GetFiles(Constants.We7ControlConfigFile);
                    if (fs != null && fs.Length > 0)
                    {
                        DataControlInfo dci = DataControlInfo.LoadInfo(d.FullName);// GetDataControlInfo(d.Name);
                        if (dci != null)
                        {
                            dci.Group = group;
                            if (dci.Controls.Count > 0)
                            {
                                controls.Add(dci);
                            }
                        }
                    }
                    else
                    {
                        LoadDataControls(controls, d.FullName, group);
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// 按类别与信息查询控件数据
        /// </summary>
        /// <param name="query">查询类别</param>
        /// <param name="selectQuery">查询信息</param>
        /// <returns></returns>
        public List<DataControlInfo> GetControls(string query, string selectQuery)
        {
            return GetControls().FindAll(delegate(DataControlInfo info)
            {
                if (selectQuery != null && selectQuery == "按关键字查找")
                {
                    return String.IsNullOrEmpty(query) || info.Desc.ToLower().IndexOf(query) >= 0;
                }
                if (selectQuery == "按文件名查找")
                {
                    return String.IsNullOrEmpty(query) || info.Name.ToLower().IndexOf(query.ToLower()) >= 0 || info.Directory.ToLower().IndexOf(query.ToLower()) >= 0;
                }
                return false;
            });
        }

        /// <summary>
        /// 按Tag来查询信息
        /// </summary>
        /// <param name="queryName">查询名称</param>
        /// <returns></returns>
        public List<DataControlInfo> GetControls(string queryName)
        {
            return GetControls().FindAll(delegate(DataControlInfo info)
            {
                if (queryName != "其他")
                {
                    return info.Tag.IndexOf(queryName) >= 0;
                }
                else
                {
                    string[] sortNames = new string[] { "文章", "栏目", "图片", "列表", "菜单", "广告", "登录", "商铺" };
                    int j = 0;
                    for (int i = 0; i < sortNames.Length; i++)
                    {
                        if (info.Tag.IndexOf(sortNames[i]) < 0)
                        {
                            j++;
                        }
                    }
                    return j == 7;
                }
            });            
        }

        /// <summary>
        /// 取得控件的编码
        /// </summary>
        /// <param name="ctr">控件名称</param>
        /// <returns></returns>
        public string GetControlCode(string ctr)
        {
            string result = String.Empty;
            string file = Path.Combine(GetDataControlPath(ctr.Split('.')[0]),String.Format("Page/{0}.ascx",ctr));
            FileInfo fi = new FileInfo(file);
            if (fi.Exists)
            {
                using (StreamReader sr =OpenFile(fi.FullName))
                {
                    result = sr.ReadToEnd();
                }
            }
            return result;
        }

        /// <summary>
        /// 取得控件的编码
        /// </summary>
        /// <param name="group">模板组</param>
        /// <param name="fileName">控件名称</param>
        /// <returns></returns>
        public string GetControlCode(string group,string fileName)
        {
            string result = String.Empty;
            try
            {
                string ctr = GetControlName(fileName);
                string file = Path.Combine(GetGroupDataControlPath(group, ctr.Split('.')[0]), String.Format("Page/{0}.ascx", ctr));
                FileInfo fi = new FileInfo(file);
                if (fi.Exists)
                {
                    using (StreamReader sr =OpenFile(fi.FullName))
                    {
                        result = sr.ReadToEnd();
                    }
                }
                else
                {
                    string ctrFile = GetDataControlPhysicalPath(fileName);
                    if (File.Exists(ctrFile))
                    {
                        using (StreamReader sr =OpenFile(ctrFile))
                        {
                            result = sr.ReadToEnd();
                        }
                        CopyDir(GetDataControlPathFromPath(fileName) , GetGroupDataControlPath(group, GetControlName(fileName).Split('.')[0]));
                    }
                }
            }
            catch(Exception ex)
            {
            }
            return result;
        }

        /// <summary>
        /// 取得控件名称
        /// </summary>
        /// <param name="filePath">控件文件路径</param>
        /// <returns></returns>
        public string GetControlName(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }


        /// <summary>
        /// 加载Css文件
        /// </summary>
        /// <param name="ctr">控件名称</param>
        /// <param name="style">控件样式</param>
        /// <returns></returns>
        public string LoadCss(string ctr,string style)
        {
            string css = "";
            try
            {
                string path = Path.Combine(GetDataControlPath(ctr.Split('.')[0]), "Style");
                DirectoryInfo di = new DirectoryInfo(path);
                string pattern = String.Format("{0}_{1}.*.css", ctr,style);
                FileInfo[] fs = di.GetFiles(pattern);
                if (fs != null && fs.Length > 0)
                {
                    using (StreamReader reader =OpenFile(fs[0].FullName))
                    {
                        css=reader.ReadToEnd();
                    }
                }
            }
            catch
            {
            }
            return css;
        }

        /// <summary>
        /// 检测Css是否有重名
        /// </summary>
        /// <param name="ctr">控件名称</param>
        /// <param name="name">样式关键字</param>
        /// <returns></returns>
        public bool CheckCssName(string ctr, string name)
        {
            string path = Path.Combine(GetDataControlPath(ctr.Split('.')[0]), "Style");
            DirectoryInfo di = new DirectoryInfo(path);
            string pattern = String.Format("{0}_custom*.*.css", ctr);
            FileInfo[] fs = di.GetFiles(pattern);
            Regex reg = new Regex(String.Format(@"{0}_\w+\.{1}\.css", ctr,name), RegexOptions.Compiled | RegexOptions.IgnoreCase);

            foreach (FileInfo f in fs)
            {
                if (reg.IsMatch(f.Name))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 保存CSS
        /// </summary>
        /// <param name="ctr">控件名称</param>
        /// <param name="name">样式名称</param>
        /// <param name="css">保存的Css</param>
        /// <returns></returns>
        public string SaveCss(string ctr,string name,string css)
        {
            string style = "";
            try
            {
                string path = Path.Combine(GetDataControlPath(ctr.Split('.')[0]), "Style");
                DirectoryInfo di = new DirectoryInfo(path);
                string pattern = String.Format("{0}_custom*.*.css", ctr);
                FileInfo[] fs = di.GetFiles(pattern);

                Regex reg = new Regex(String.Format(@"(?<={0}_custom)\d+(?=\.)", ctr), RegexOptions.Compiled | RegexOptions.IgnoreCase);
                string fileName = String.Format("{0}_custom", ctr);
                style = "custom";

                if (fs != null && fs.Length > 0)
                {
                    Array.Sort(fs, delegate(FileInfo A, FileInfo B)
                    {
                        int result = 0;
                        Match m1 = reg.Match(A.Name);
                        Match m2 = reg.Match(B.Name);
                        if (m1.Success && m2.Success)
                        {
                            int n1 = int.Parse(m1.Value);
                            int n2 = int.Parse(m2.Value);
                            result = n1 > n2 ? -1 : (n1 < n2 ? 1 : 0);
                        }
                        else if (m1.Success)
                        {
                            result = -1;
                        }
                        else if (m2.Success)
                        {
                            result = 1;
                        }
                        return result;
                    });
                    Match m = reg.Match(fs[0].Name);
                    int num = m.Success ? (int.Parse(m.Value) + 1) : 0;
                    fileName += num;
                    style += num;
                }
                string fullname = String.Format("{0}.{1}.css", fileName, name);
                style += "." + name;
                
                string fp = Path.Combine(di.FullName, fullname);
                using (FileStream fstr = File.Open(fp, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter wr = OpenWriteFile(fstr))
                    {
                        wr.Write(css);
                    }
                }
            }
            catch
            {
            }
            return style;
        }

        /// <summary>
        /// Css归库
        /// </summary>
        /// <param name="ctr">控件名称</param>
        /// <param name="cnname">样式中文说明</param>
        /// <param name="key">样式关键字</param>
        /// <param name="cssname">样式名称</param>
        /// <param name="css">归库的Css</param>
        /// <returns></returns>
        public string SaveCss(string ctr, string cnname, string key,string cssname,string css)
        {
            string path = Path.Combine(GetDataControlPath(ctr.Split('.')[0]), "Style");
            DirectoryInfo di = new DirectoryInfo(path);

            string pattern = string.Format("{0}_{1}.*.css",ctr,key);
            if (di.GetFiles(pattern).Length > 0)
                throw new Exception("当前类型已经存在");

            string fullname = String.Format("{0}_{1}.{2}.css",ctr,key, cnname);
            css = css.Replace(String.Format(".{0}_{1}", ctr.Replace(".", "_"), cssname), "{WE:STYLE}");

            string fp = Path.Combine(di.FullName, fullname);
            using (FileStream fstr = File.Open(fp, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter wr =OpenWriteFile(fstr))
                {
                    wr.Write(css);
                }
            }
            return String.Format("{0}.{1}",key,cnname);
        }

        /// <summary>
        /// Css归库
        /// </summary>
        /// <param name="ctr">控件路径</param>
        /// <param name="cnname">样式中文说明</param>
        /// <param name="key">样式关键字</param>
        /// <param name="cssname">样式名称</param>
        /// <param name="css">归库的Css</param>
        /// <returns></returns>
        public string SaveCssByPath(string fileName, string cnname, string key, string cssname, string css)
        {
            string ctr = Path.GetFileNameWithoutExtension(fileName);

            string path = Path.Combine(GetDataControlPathFromPath(fileName), "Style");
            DirectoryInfo di = new DirectoryInfo(path);

            string pattern = string.Format("{0}_{1}.*.css", ctr, key);
            if (di.GetFiles(pattern).Length > 0)
                throw new Exception("当前类型已经存在");

            string fullname = String.Format("{0}_{1}.{2}.css", ctr, key, cnname);
            css = css.Replace(String.Format(".{0}_{1}", ctr.Replace(".", "_"), cssname), "{WE:STYLE}");

            string fp = Path.Combine(di.FullName, fullname);
            using (FileStream fstr = File.Open(fp, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter wr = OpenWriteFile(fstr))
                {
                    wr.Write(css);
                }
            }
            return String.Format("{0}.{1}", key, cnname);
        }

        /// <summary>
        /// 编辑CSS样式
        /// </summary>
        /// <param name="ctr">控件名称</param>
        /// <param name="name">控件中文名</param>
        /// <param name="css">修改后的Css</param>
        /// <returns></returns>
        public void EditCss(string ctr,string name ,string style, string css)
        {
            try
            {
                string path = Path.Combine(GetDataControlPath(ctr.Split('.')[0]), "Style");
                DirectoryInfo di = new DirectoryInfo(path);
                string fn = String.Format("{0}_{1}.{2}.css", ctr,style,name);
                fn = Path.Combine(path, fn);
                using (FileStream f = File.Open(fn,FileMode.Create,FileAccess.Write))
                {
                    using (StreamWriter wr =OpenWriteFile(f))
                    {
                        wr.Write(css);
                    }
                }             
            }
            catch
            {
            }
        }

        /// <summary>
        /// 编辑CSS样式
        /// </summary>
        /// <param name="ctr">编辑控件</param>
        /// <param name="style">控件样式</param>
        /// <param name="css">修改后的Css</param>
        /// <returns></returns>
        public void EditCss(string ctr, string style, string css)
        {
            try
            {
                string path = Path.Combine(GetDataControlPath(ctr.Split('.')[0]), "Style");
                DirectoryInfo di = new DirectoryInfo(path);
                FileInfo[] fs=di.GetFiles(String.Format("{0}_{1}.*.css",ctr,style));
                if (fs != null && fs.Length > 0)
                {
                    using (StreamWriter wr =OpenWriteFile(fs[0].FullName))
                    {
                        wr.Write(css);
                    }
                }                
            }
            catch
            {
            }
        }

        /// <summary>
        /// 编辑CSS样式
        /// </summary>
        /// <param name="ctr">编辑控件</param>
        /// <param name="style">控件样式</param>
        /// <param name="css">修改后的Css</param>
        /// <returns></returns>
        public void EditCssByPath(string fileName, string style, string css)
        {
            try
            {
                string ctr = Path.GetFileNameWithoutExtension(fileName);
                string path = Path.Combine(GetDataControlPathFromPath(fileName), "Style");
                DirectoryInfo di = new DirectoryInfo(path);
                FileInfo[] fs = di.GetFiles(String.Format("{0}_{1}.*.css", ctr, style));
                if (fs != null && fs.Length > 0)
                {
                    using (StreamWriter wr = OpenWriteFile(fs[0].FullName))
                    {
                        wr.Write(css);
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 删除当前样式
        /// </summary>
        /// <param name="ctr">控件名称</param>
        /// <param name="style">删除样式</param>
        public void DeleteCssFile(string ctr, string style)
        {
            try
            {
                string path = Path.Combine(GetDataControlPath(ctr.Split('.')[0]), "Style");
                DirectoryInfo di = new DirectoryInfo(path);
                string pattern = String.Format("{0}_{1}.*.css", ctr, style);
                FileInfo[] fs = di.GetFiles(pattern);


                if (fs != null && fs.Length > 0)
                {
                    fs[0].Delete();
                }
            }
            catch
            {
            }
        }


        /// <summary>
        /// 控件归库
        /// </summary>
        /// <param name="ctr">控件名称</param>
        /// <param name="code">修改后的代码</param>
        /// <returns></returns>
        public string SaveControl(string ctr,string code)
        {
            string ctr0 = ctr.Split('.')[0];
            string ctrname = ctr0 + ".Custom";
            DataControlInfo info=GetDataControlInfo(ctr0);
            int num = -1;
            Regex reg = new Regex(@"(?<=\.custom)\d+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            info.Controls.ForEach(delegate(DataControl dc)
            {
                Match mc=reg.Match(dc.Control);
                if (mc.Success)
                {
                    int n=int.Parse(mc.Value);
                    if (int.Parse(mc.Value) > num)
                        num = n;
                }
            });
            ctrname += ++num;
            string fullname=Path.Combine(GetDataControlPath(ctr0),String.Format("Page/{0}.ascx",ctrname));
            using (FileStream stream = File.Open(fullname, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw =OpenWriteFile(stream))
                {
                    sw.Write(code);
                }
            }
            CopyCssFile(ctr, ctrname);
            return ctrname;
        }

        /// <summary>
        /// 控件归库
        /// </summary>
        /// <param name="fileName">控件名称</param>
        /// <param name="code">修改后的代码</param>
        /// <param name="name">控件中文名</param>
        /// <param name="key">控件关键字</param>
        /// <param name="desc">控件描述</param>
        /// <returns></returns>
        public string SaveControl(string fileName, string code, string name, string key, string desc)
        {
            string ctr = GetControlName(fileName);

            string ctr0 = ctr.Split('.')[0];
            string ctrname = ctr0 + "." + key;
            Regex regex = new Regex("(?<=<!-+#+)[^#].*?(?=#+-+>)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match match=regex.Match(code);
            string header = "";
            if (match.Success)
            {
                header = match.Value;

            }
            else
            {
                code = "<!--### ###-->\r\n" + code;
            }

            //下面是添加名称
            Regex kvRegex = new Regex("(?<=name=['|\"]).*?(?=['|\"])", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match mc = kvRegex.Match(header);
            if (mc.Success)
            {
                header = kvRegex.Replace(header, name);
            }
            else
            {
                header += " name=\"" + name + "\"";
            }

            //下面是添加描述
            kvRegex = new Regex("(?<=desc=['|\"]).*?(?=['|\"])", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
            mc = kvRegex.Match(header);
            if (mc.Success)
            {
                header = kvRegex.Replace(header, desc);
            }
            else
            {
                header += " desc=\"" + desc + "\"";
            }

            //下面是添加关键字
            kvRegex = new Regex("(?<=key=['|\"]).*?(?=['|\"])", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
            mc = kvRegex.Match(header);
            if (mc.Success)
            {
                header = kvRegex.Replace(header, key);
            }
            else
            {
                header += " key=\"" + key + "\"";
            }

            code=regex.Replace(code, " "+header.Trim()+" ");

            string fullname = Path.Combine(GetDataControlPathFromPath(fileName), String.Format("Page/{0}.ascx", ctrname));
            using (FileStream stream = File.Open(fullname, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = OpenWriteFile(stream))
                {
                    sw.Write(code);
                }
            }
            CopyCssFile2(fileName, ctrname);
            return fileName.Replace(GetControlName(fileName),ctrname);
        }

        /// <summary>
        /// 修改控件
        /// </summary>
        /// <param name="ctr">控件名称</param>
        /// <param name="code">修改后的代码</param>
        public void EditControl2(string fileName, string code)
        {
            string fullname = GetDataControlPhysicalPath(fileName); //Path.Combine(GetDataControlPath(ctr.Split('.')[0]), String.Format("Page/{0}.ascx", ctr));
            using (FileStream stream = File.Open(fullname, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw = OpenWriteFile(stream))
                {
                    sw.Write(code);
                }
            }
        }


        /// <summary>
        /// 修改控件
        /// </summary>
        /// <param name="ctr">控件名称</param>
        /// <param name="code">修改后的代码</param>
        public void EditControl(string ctr,string code)
        {
            string fullname = Path.Combine(GetDataControlPath(ctr.Split('.')[0]), String.Format("Page/{0}.ascx", ctr));
            using (FileStream stream = File.Open(fullname, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw =OpenWriteFile(stream))
                {
                    sw.Write(code);
                }
            }
        }

        /// <summary>
        /// 修改控件
        /// </summary>
        /// <param name="group">修改的模板组</param>
        /// <param name="fileName">控件名称</param>
        /// <param name="code">修改后的代码</param>
        public void EditControl(string group,string fileName, string code)
        {
            string ctr = GetControlName(fileName);
            string fullpath = Path.Combine(GetGroupDataControlPath(group, ctr.Split('.')[0]), String.Format("Page/{0}.ascx", ctr));
            using (FileStream stream = File.Open(fullpath, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter sw =OpenWriteFile(stream))
                {
                    sw.Write(code);
                }
            }
        }

        /// <summary>
        /// 删除控件
        /// </summary>
        /// <param name="ctr">控件名称</param>
        public void DelControl(string ctr)
        {
            string ctrPath=Path.Combine(GetDataControlPath(ctr.Split('.')[0]),String.Format("Page/{0}.ascx",ctr));
            if (File.Exists(ctrPath))
                File.Delete(ctrPath);
            DelCssFile(ctr);
        }


        /// <summary>
        /// 复制Css文件
        /// </summary>
        /// <param name="ctr">控件名称</param>
        /// <param name="target">新的文件名</param>
        void CopyCssFile(string ctr, string target)
        {
            string path = Path.Combine(GetDataControlPath(ctr), "Style");
            DirectoryInfo di = new DirectoryInfo(path);
            if (!di.Exists)
                return;
            FileInfo[] fs = di.GetFiles(String.Format("{0}_*.css", ctr));
            foreach (FileInfo f in fs)
            {
                f.CopyTo(Path.Combine(di.FullName, f.Name.Replace(ctr, target)), true);
            }
        }

        /// <summary>
        /// 复制Css文件
        /// </summary>
        /// <param name="ctr">控件名称</param>
        /// <param name="target">新的文件名</param>
        void CopyCssFile2(string fileName, string target)
        {
            string ctr = GetControlName(fileName);
            string path = Path.Combine(GetDataControlPathFromPath(fileName) , "Style");
            DirectoryInfo di = new DirectoryInfo(path);
            if (!di.Exists)
                return;
            FileInfo[] fs=di.GetFiles(String.Format("{0}_*.css",ctr));
            foreach (FileInfo f in fs)
            {
                f.CopyTo(Path.Combine(di.FullName,f.Name.Replace(ctr,target)),true);
            }
        }

        /// <summary>
        /// 删除指定控件的Css文件
        /// </summary>
        /// <param name="ctr">控件名称</param>
        void DelCssFile(string ctr)
        {
            string path = Path.Combine(GetDataControlPath(ctr.Split('.')[0]), "Style");
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] fs = di.GetFiles(String.Format("{0}_*.css",ctr));
            foreach (FileInfo f in fs)
            {
                f.Delete();
            }
        }

        /// <summary>
        /// 取得控件的根路径
        /// </summary>
        /// <param name="ctr">控件名称</param>
        /// <returns>控件根路径</returns>
        public string GetDataControlPath(string ctr)
        {
            return Path.Combine(Constants.We7ControlPhysicalPath,ctr);
        }

        /// <summary>
        /// 从文件路径中取得控件路径
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string GetDataControlPathFromPath(string filePath)
        {
            filePath=HttpContext.Current.Server.MapPath(filePath);
            FileInfo fi = new FileInfo(filePath);
            return fi.Directory.Parent.FullName;
        }

        /// <summary>
        /// 从文件的相对路径取得物理路径
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public string GetDataControlPhysicalPath(string relativePath)
        {
            return HttpContext.Current.Server.MapPath(relativePath);
        }

        /// <summary>
        /// 取得模板组下面的控件路径
        /// </summary>
        /// <param name="group">模板组</param>
        /// <param name="ctr">控件名称</param>
        /// <returns>控件路径</returns>
        public string GetGroupDataControlPath(string group, string ctr)
        {
            return String.Format("{0}/_skins/{1}/{2}/{3}",AppDomain.CurrentDomain.BaseDirectory,group,Constants.We7ControlsBasePath,ctr);
        }

        /// <summary>
        /// 向一个节点中添加所有数据
        /// </summary>
        /// <param name="parentnode">父节点</param>
        /// <param name="children">要添加的子结点</param>
        void AppendNodeList(XmlNode parentnode, XmlNodeList children)
        {
            foreach (XmlNode n in children)
            {
                parentnode.AppendChild(n.Clone());
            }
        }

        /// <summary>
        /// 复制文件夹下的内容
        /// </summary>
        /// <param name="src">源目录</param>
        /// <param name="target">目标目录</param>
        void CopyDir(string src, string target)
        {
            DirectoryInfo dir = new DirectoryInfo(src);
            if (dir.Name.StartsWith(".") || dir.Name.StartsWith("~"))
                return;
            DirectoryInfo dirtarget = new DirectoryInfo(target);
            if (!dirtarget.Exists)
                dirtarget.Create();

            foreach (FileInfo f in dir.GetFiles())
            {
                if (f.Name.StartsWith(".") || f.Name.StartsWith("~"))
                    continue;
                try
                {
                    string targetFile = Path.Combine(target, f.Name);
                    using (StreamReader sr =OpenFile(f.FullName))
                    {
                        string txt = sr.ReadToEnd();
                        using (StreamWriter sw =OpenWriteFile(targetFile))
                        {
                            sw.Write(txt);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            foreach (DirectoryInfo d in dir.GetDirectories())
            {
                CopyDir(d.FullName, Path.Combine(target, d.Name));
            }
        }

        /// <summary>
        /// 通过文件路径取得StreamReader
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        StreamReader OpenFile(string path)
        {
            return new StreamReader(path, Encoding.UTF8);
        }

        /// <summary>
        /// 通过文件路径取得StreamWriter
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        StreamWriter OpenWriteFile(string path)
        {
            return new StreamWriter(path,false,Encoding.UTF8);
        }

        /// <summary>
        /// 通过数据流取得StreamWriter
        /// </summary>
        /// <param name="path">数据流</param>
        /// <returns></returns>
        StreamWriter OpenWriteFile(Stream stream)
        {
            return new StreamWriter(stream, Encoding.UTF8);
        }
    }
}
