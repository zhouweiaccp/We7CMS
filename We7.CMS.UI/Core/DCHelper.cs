using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Reflection;
using We7.CMS.Common;

namespace We7.CMS.WebControls.Core
{
    public class DCHelper
    {
        /// <summary>
        /// 从指定控件中取得配置信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DCInfo PickUp(string path)
        {
            DCInfo info = new DCInfo();
            info.Path = path;

            Page page = new Page();
            if (page != null)
            {
                Control p = page.LoadControl(path);
                PickUp(p, "", "基本信息", info);
            }

            return info;
        }

        public DCInfo PickUp(Control ctr)
        {
            DCInfo info = new DCInfo();
            PickUp(ctr, "", "基本信息", info);
            return info;
        }

        private void PickUp(object o, string prefix, string label, DCInfo info)
        {
            DCPartInfo part = CreateDCPart(info, label);

            foreach (PropertyInfo prop in o.GetType().GetProperties())
            {
                if (Attribute.IsDefined(prop, typeof(IgnoreAttribute)))
                {
                    IgnoreAttribute attr = GetAttribute<IgnoreAttribute>(prop);

                    if (attr.Fields == null || attr.Fields.Length == 0)
                        continue;
                }

                string name = !String.IsNullOrEmpty(prefix) ? String.Format("{0}-{1}", prefix, prop.Name) : prop.Name;

                if (Attribute.IsDefined(prop, typeof(ChildrenAttribute)))
                {
                    DescAttribute desc = GetAttribute<DescAttribute>(prop);
                    PickUp(prop.GetValue(o, null), name,
                        desc != null ? desc.Title : String.Empty, info);
                }
                else if (Attribute.IsDefined(prop, typeof(OptionAttribute)))
                {
                    part.Params.Add(GetDCParam(prop, name));
                }
            }

            SortParam(part);
        }

        private DCPartInfo CreateDCPart(DCInfo info, string label)
        {
            DCPartInfo part = new DCPartInfo();
            part.Name = label;
            info.Parts.Add(part);
            return part;
        }

        private void SortParam(DCPartInfo part)
        {
            part.Params.Sort((a, b) => b.Weight.CompareTo(a.Weight));
        }

        private DataControlParameter GetDCParam(PropertyInfo prop, string name)
        {
            DataControlParameter param = new DataControlParameter();

            param.Name = name;
            param.Required = Attribute.IsDefined(prop, typeof(RequiredAttribute));

            DescAttribute desc = GetAttribute<DescAttribute>(prop);
            if (desc != null)
            {
                param.Title = desc.Title;
                param.Description = desc.Description;

                OptionAttribute option = GetAttribute<OptionAttribute>(prop);
                param.Type = option.Type;
                param.Maximum = option.Maxnum.ToString();
                param.Minium = option.Minnum.ToString();
                param.Length = option.Length;
                param.Data = option.Data;
            }

            WeightAttribute weight = GetAttribute<WeightAttribute>(prop);
            if (weight != null)
            {
                param.Weight = weight != null ? weight.Weight : 0;
            }

            DefaultAttribute defaultVal = GetAttribute<DefaultAttribute>(prop);
            if (defaultVal != null)
            {
                param.DefaultValue=defaultVal.Value.ToString();
            }

            return param;
        }

        private T GetAttribute<T>(MemberInfo member)
            where T : Attribute
        {
            T result = default(T);

            object[] attrs = member.GetCustomAttributes(typeof(T), false);
            if (attrs != null && attrs.Length > 0)
            {
                result = attrs[0] as T;
            }

            return result;
        }
    }

    public class DCInfo : IJsonResult
    {
        private List<DCPartInfo> parts = new List<DCPartInfo>();
        public List<DCPartInfo> Parts
        {
            get { return parts; }
            set { parts = value; }
        }

        public string Path { get; set; }

        public string ToJson()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");
            sb.AppendFormat("path:'{0}',parts:[", Path);

            for (int i = 0; i < Parts.Count; i++)
            {
                sb.Append(Parts[i].ToJson());
                if (i != Parts.Count - 1)
                    sb.Append(",");
            }

            sb.Append("]}");

            return sb.ToString();
        }
    }

    public class DCPartInfo : IJsonResult
    {
        private List<DataControlParameter> _params = new List<DataControlParameter>();

        public DCPartInfo() { }

        public DCPartInfo(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public List<DataControlParameter> Params
        {
            get { return _params; }
            set { _params = value; }
        }

        public string ToJson()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{");
            sb.AppendFormat("name:'{0}',", Name);
            sb.Append("params:[");
            for (int i = 0; i < Params.Count; i++)
            {
                sb.Append(Params[i].ToJson());
                if (i != Params.Count - 1)
                    sb.Append(",");
            }
            sb.Append("]}");

            return sb.ToString();
        }
    }
}
