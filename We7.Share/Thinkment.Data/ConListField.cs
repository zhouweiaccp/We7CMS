using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Thinkment.Data
{
    /// <summary>
    /// Select中的字段或表达式（扩展：可对字段进行截取或运算）
    /// </summary>
    [Serializable]
    public class ConListField
    {
        Adorns adorn;
        string fieldName;
        string aliasName;
        int start;
        int length;

        public ConListField()
        {
            adorn = Adorns.None;
        }

        public ConListField(string fn, string an)
        {
            fieldName = fn;
            aliasName = an;
        }

        public ConListField(Adorns ad, string fn, string an)
        {
            adorn = ad;
            fieldName = fn;
            aliasName = an;
        }

        public Adorns Adorn
        {
            get { return adorn; }
            set { adorn = value; }
        }

        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }

        public string AliasName
        {
            get { return aliasName; }
            set { aliasName = value; }
        }

        public int Start
        {
            get { return start; }
            set { start = value; }
        }

        public int Length
        {
            get { return length; }
            set { length = value; }
        }

        private DbType type;
        /// <summary>
        /// 数据类型
        /// </summary>
        public DbType Type
        {
            get { return type; }
            set { type = value; }
        }

        private int size;
        /// <summary>
        /// 字段大小
        /// </summary>
        public int Size
        {
            get { return size; }
            set { size = value; }
        }

    }
}
