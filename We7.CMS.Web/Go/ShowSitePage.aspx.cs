using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using We7.CMS.Config;
using We7.Framework.Config;
using We7.Framework.Util;
using HtmlAgilityPack;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Reflection;
using We7.Framework;

namespace We7.CMS.Web
{
    public partial class ShowSitePage : FrontBasePage
    {
        
        /// <summary>
        /// 处理者
        /// </summary>
        protected override string GoHandler { get { return "site"; } }

        /// <summary>
        /// 模板子分类，如登录、注册、搜索等
        /// </summary>
        protected override string ColumnMode
        {
            get
            {
                if (Request["mode"] != null)
                    return Request["mode"].ToString();
                else
                    return "";
            }
        }


        protected override string TemplatePath{get;set;}



        protected override void Initialize()
        {
            base.Initialize();

            //初始化TemplatePath
            string result = IsHtmlTemplate ?
                TemplateHelper.GetHtmlTemplateByHandlers(ColumnMode, "/", null, null)
                :TemplateHelper.GetTemplateByHandlers(ColumnMode, "/", null, null);

            if (!string.IsNullOrEmpty(result))
            {
                if (!result.StartsWith("/"))
                {
                    TemplatePath = "/" + result;
                }
                else
                {
                    TemplatePath = result;
                }
            }

            if (!string.IsNullOrEmpty(TemplatePath))
            {
                if (File.Exists(Context.Server.MapPath(TemplatePath)))
                {
                    if (!TemplatePath.EndsWith(".html"))
                    {
                        //Control ctl = LoadControl(TemplatePath);

                        /*
                         * Add:检查分块部件是否错误
                         * author:丁乐
                         * time:2011/11/22
                         */
                        Control ctl =CheckControlByBuilder();
                        if (ctl != null)
                        {
                            this.Controls.Add(ctl);
                            if (this.Page.Header != null && this.Title != null)
                            {
                                this.Title = GeneralConfigs.GetConfig().DefaultHomePageTitle;
                                //meta标记
                                HtmlGenericControl KeywordsMeta = new HtmlGenericControl("meta");
                                KeywordsMeta.Attributes["name"] = "keywords";
                                string strContent = GeneralConfigs.GetConfig().KeywordPageMeta;
                                KeywordsMeta.Attributes["content"] = strContent;
                                this.Header.Controls.Add(KeywordsMeta);

                                HtmlGenericControl DescriptionMeta = new HtmlGenericControl("meta");
                                DescriptionMeta.Attributes["name"] = "description";
                                string strDescriptionMetaContent = GeneralConfigs.GetConfig().DescriptionPageMeta;
                                DescriptionMeta.Attributes["content"] = strDescriptionMetaContent;
                                this.Header.Controls.Add(DescriptionMeta);
                            }
                            //else
                            //    throw new Exception("模板文件" + TemplatePath + "不符合规格！请保证<head> 标记中有 runat=server 的属性定义 。");
                        }
                        else
                            DisplayError("无法加载模板文件" + TemplatePath + "！请检查模板是否正确。");
                    }
                }
                else
                {
                    //throw new Exception("没有找到模板" + TemplatePath + "！请确认模板路径是否正确。");
                    DisplayError("没有找到模板" + TemplatePath + "！请确认模板路径是否正确。");
                }
            }
            else
            {
                if (ColumnMode == "login")
                    Server.Execute("/user/login.aspx", true);
                else
                    Server.Execute(TemplateGuideUrl, true);
            }

        }

        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!BaseConfigs.ConfigFileExist())
            {
                Response.Write("您的数据库配置文件尚未生成，看起来数据库尚未建立，您需要建立数据库配置文件或生成数据库。现在开始吗？<a href='/install/index.aspx'><u>现在配置数据库</u></a>");
                Response.End();
            }

            AddStatistic(string.Empty, string.Empty);
        }

        #region 静态化添加

        /// <summary>
        /// 重写Render方法
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            #region 注释掉的Md5比较
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter htmlw = new HtmlTextWriter(sw); 
            //base.Render(htmlw);
            //htmlw.Flush();
            //htmlw.Close();
            ////获取到页面内容
            //string pageContent = sw.ToString();
            //Response.Write(pageContent);

            ////MD5比对
            //byte[] byteArry = System.Text.Encoding.Default.GetBytes(pageContent);
            //System.Security.Cryptography.MD5CryptoServiceProvider get_md5 
            //    = new System.Security.Cryptography.MD5CryptoServiceProvider();
            //byte[] hash_byte = get_md5.ComputeHash(byteArry);
            //string result = System.BitConverter.ToString(hash_byte);
            #endregion

            #region 注释
            //if (!string.IsNullOrEmpty(Request["Createhtml"]) && Request["Createhtml"] == "1")
            //{
            //    StringWriter strWriter = new StringWriter();
            //    HtmlTextWriter tempWriter = new HtmlTextWriter(strWriter);
            //    try
            //    {
            //        base.Render(tempWriter);
            //    }
            //    catch (Exception ex)
            //    {
            //        strWriter.Write("");
            //    }
            //    //读取原始模板内容
            //    HtmlDocument doc = new HtmlDocument();
            //    doc.OptionAutoCloseOnEnd = true;
            //    doc.OptionCheckSyntax = true;
            //    doc.OptionOutputOriginalCase = true;
            //    try
            //    {
            //        doc.Load(Server.MapPath(TemplatePath), Encoding.UTF8);
            //    }
            //    catch
            //    {
            //        throw new Exception("格式化HTML错误");
            //    }
            //    string strContent = doc.DocumentNode.InnerText;
            //    //提取控件注册信息
            //    string pat = @"<%@[^>]*>";
            //    Regex reg = new Regex(pat);
            //    MatchCollection m = reg.Matches(strContent);
            //    StringBuilder sb = new StringBuilder();
            //    for (int i = 0; i < m.Count; i++)
            //    {
            //        string temp = m[i].Value;
            //        if (!m[i].ToString().Contains("Src=\"/"))
            //        {
            //            temp = m[i].Value.Replace("Src=\"", "Src=\"" + TemplatePath.Remove(TemplatePath.LastIndexOf("/")) + "/");
            //        }
            //        sb.Append(temp + "\r\n");
            //    }
            //    //给生成后模板添加控件注册信息  
            //    string content = strWriter.ToString();
            //    string pat1 = @"<html[^>]*>";
            //    string RegAndHtml = sb.ToString() + "\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">";
            //    content = Regex.Replace(content, pat1, RegAndHtml, RegexOptions.IgnoreCase);
            //    content = content.Replace("<head>", "<head runat=\"server\">");

            //    writer.Write(content);
            //    string fileName = "";
            //    if (string.IsNullOrEmpty(ColumnMode))
            //    {
            //        fileName = "index.ascx";
            //    }
            //    else
            //    {
            //        fileName = ColumnMode + ".ascx";
            //    }
            //    string resultPath = Server.MapPath(TemplatePath.Remove(TemplatePath.LastIndexOf("/")) + "/HtmlTemplate/" + fileName);
            //    FileHelper.WriteFileEx(resultPath,content, false);
            //}
            //else
            //{

            //}
            #endregion
        }

        #endregion

        

    }
   
}
