using System;
using System.Collections.Generic;
using System.Text;
using NVelocity.Context;
using NVelocity.App;
using Commons.Collections;
using System.Web;
using NVelocity.Runtime;
using NVelocity;
using System.IO;
using We7.Framework.Util;

namespace We7.Framework.TemplateEnginer
{
    /// <summary>
    ///  NVelocity帮助类
    /// </summary>
    public class NVelocityHelper
    {
        private VelocityEngine velocity = null;
        private IContext context = null;

        /// <summary>
        /// 模板文件夹路径
        /// </summary>
        /// <param name="templatDir"></param>
        //public NVelocityHelper(string templatDir)
        //{
        //    Init(templatDir);
        //}

        //public NVelocityHelper() {
        //    TemplateFolder = Utils.GetMapPath();  //HttpContext.Current.Server.MapPath("~/Models/GeneratorTemplate");
        //    Init(TemplateFolder);
        // }
        public NVelocityHelper(string templateFolder)
        {
            if (String.IsNullOrEmpty(templateFolder) ||
                String.IsNullOrEmpty(templateFolder.Trim()) ||
                !Directory.Exists(templateFolder))
            {
                throw new Exception("模板目录不存在");
            }

            TemplateFolder = templateFolder;
            Init(templateFolder);
        }


        /// <summary>
        /// 初始化NVelocity
        /// </summary>
        /// <param name="templatDir">模板文件夹路径</param>
        public void Init(string templatDir)
        {
            //创建VelocityEngine实例对象
            velocity = new VelocityEngine();
            //设置模板文件夹

            TemplateFolder = templatDir;

            //使用设置初始化VelocityEngine
            ExtendedProperties props = new ExtendedProperties();
            props.AddProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, TemplateFolder);
            props.AddProperty(RuntimeConstants.INPUT_ENCODING, "utf-8");
            props.AddProperty(RuntimeConstants.OUTPUT_ENCODING, "utf-8");
            velocity.Init(props);

            //为模板变量赋值
            context = new VelocityContext();
        }



        /// <summary>
        /// 模板文件夹路径
        /// </summary>
        public string TemplateFolder { get; set; }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="key">标识符</param>
        /// <param name="value">值</param>
        public void Put(string key, object value)
        {
            if (context == null)
                context = new VelocityContext();
            context.Put(key, value);
        }


        /// <summary>
        /// 保存生成的文件
        /// </summary>
        /// <param name="inputFileName">模板名称</param>
        /// <param name="outPutFilePath">生成文件完全限定名(包括路径)</param>
        public void Save(string inputFileName, string outPutFilePath)
        {
            Template template = velocity.GetTemplate(inputFileName);
            using (StreamWriter sw = new StreamWriter(outPutFilePath, false, Encoding.UTF8))
            {
                template.Merge(context, sw);
                sw.Flush();
                sw.Close();
            }
        }

        public string Save(string inputFileName)
        {
            Template template = velocity.GetTemplate(inputFileName);
            var writer = new StringWriter();
            template.Merge(context, writer);

            writer.Flush();
            string outTxt = writer.ToString();

            writer.Close();

            return outTxt;
        }

        /// <summary>
        /// 缓存模板对象
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static  Template GetTemplate(string folder, string fileName)
        {
            string cacheKey = String.Format("NVelocityHelper_{0}_{1}", folder, fileName);
            Template template = AppCtx.Cache.RetrieveObject<Template>(cacheKey);
            if (template == null)
            {
                VelocityEngine velocity = new VelocityEngine();
                ExtendedProperties props = new ExtendedProperties();
                props.AddProperty(RuntimeConstants.RESOURCE_LOADER, "file");
                props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, folder);
                props.AddProperty(RuntimeConstants.INPUT_ENCODING, "utf-8");
                props.AddProperty(RuntimeConstants.OUTPUT_ENCODING, "utf-8");
                velocity.Init(props);
                template = velocity.GetTemplate(fileName);

                if (template == null)
                    throw new NullReferenceException("模板目录不存在。");

                AppCtx.Cache.AddObjectWithFileChange(cacheKey, template, folder.TrimEnd('/').TrimStart('\\') + '\\' + fileName.TrimStart('/').TrimStart('\\'));
            }
            return template;
        }

        /// <summary>
        /// 得到模板字符串
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static string GetFormatString(string folder, string fileName, Dictionary<string,object> args)
        {
            string result = String.Empty;
            Template template = GetTemplate(folder, fileName);
            if (template != null)
            {
                using (StringWriter sw = new StringWriter())
                {
                    IContext ctx = new VelocityContext();
                    if (args != null)
                    {
                        foreach (KeyValuePair<string, object> kvp in args)
                        {
                            ctx.Put(kvp.Key, kvp.Value);
                        }
                    }
                    template.Merge(ctx, sw);
                    sw.Flush();
                    result = sw.ToString();
                    sw.Close();
                }
            }
            return result;
        }



    }
}
