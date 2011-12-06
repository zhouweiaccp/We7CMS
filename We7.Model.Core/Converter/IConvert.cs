using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using We7.Framework.Util;
using We7.Model.Core.Config;
using We7.Framework;

namespace We7.Model.Core.Converter
{
    public interface IInputConvert
    {
        object Convert(We7DataColumnCollection columns, We7DataColumn column, string[] fields);
    }

    public interface IOutputConvert
    {
        object Convert(We7DataColumn column, DataRow row, string[] fields, object extobj);
    }

    public class ConvertHelper
    {
        private static Regex regExpr = new Regex(@"(?<fun>\w+)\((?<arg>.*)\)", RegexOptions.Compiled);

        public static string GetFun(string expression)
        {
            Match m = regExpr.Match(expression);
            if (!m.Success || !m.Groups["fun"].Success)
                throw new Exception("不存在当前表达示");
            return m.Groups["fun"].Value;
        }

        public static string[] GetArgs(string expression)
        {
            Match m = regExpr.Match(expression);
            if (!m.Success || !m.Groups["arg"].Success)
                throw new Exception("不存在当前表达示");
            return !String.IsNullOrEmpty(m.Groups["arg"].Value) ? m.Groups["arg"].Value.Split(',') : null;
        }

        /// <summary>
        /// 输入转化器
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static IInputConvert GetInputConvert(string expr, out string[] fields)
        {
            string fun = GetFun(expr);
            fields = GetArgs(expr);
            IInputConvert converter = AppCtx.Cache.RetrieveObject<IInputConvert>(GetInputConvertKey(fun));
            if (converter == null)
            {
                string typestr = ModelConfig.Converters[fun];
                converter = Utils.CreateInstance<IInputConvert>(ModelConfig.Converters[fun]);
                if (converter == null)
                {
                    throw new SysException("不存在当前转化器" + fun);
                }
                AppCtx.Cache.AddObjectWithFileChange(GetInputConvertKey(fun), converter, ModelConfig.ConfigFilePath);
            }
            return converter;
        }

        /// <summary>
        /// 输出转化器
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static IOutputConvert GetOutputConvert(string expr, out string[] fields)
        {
            string fun = GetFun(expr);
            fields = GetArgs(expr);
            IOutputConvert converter = AppCtx.Cache.RetrieveObject<IOutputConvert>(GetOutputConvertKey(fun));
            if (converter == null)
            {
                string typestr = ModelConfig.Converters[fun];
                converter = Utils.CreateInstance<IOutputConvert>(ModelConfig.Converters[fun]);
                if (converter == null)
                {
                    throw new SysException("不存在当前转化器" + fun);
                }
                AppCtx.Cache.AddObjectWithFileChange(GetOutputConvertKey(fun), converter, ModelConfig.ConfigFilePath);
            }
            return converter;
        }

        static string GetInputConvertKey(string key)
        {
            return String.Format("{0}_inputConvert", key);
        }

        static string GetOutputConvertKey(string key)
        {
            return String.Format("{0}_outputConvert", key);
        }
    }
}
