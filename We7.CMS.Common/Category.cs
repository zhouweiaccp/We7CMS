using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace We7.CMS.Common
{
    /// <summary>
    /// 类别信息
    /// </summary>
    public class Category
    {
        private CategoryCollection catColl;
        private int intOp=-1;
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        public string ParentID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 配置信息
        /// </summary>
        public string Options { get; set; }

        /// <summary>
        /// 整数的配置信息
        /// </summary>
        public int IntOption
        {
            get
            {
                if (intOp == -1)
                {
                    Int32.TryParse(Options, out intOp);
                }
                return intOp;
            }
        }

        /// <summary>
        /// 索引号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        public CategoryCollection Children
        {
            get
            {
                if (catColl == null)
                {
                    catColl = new CategoryCollection();
                }
                return catColl;
            }
            set { catColl = value; }
        }

        public Category Clone()
        {
            Category c = new Category();
            c.ID = ID;
            c.Description = Description;
            c.KeyWord = KeyWord;
            c.Name = Name;
            c.ParentID = ParentID;
            c.Children = Children.Clone();
            return c;
        }

    }

    public class CategoryCollection : Collection<Category>
    {
        public CategoryCollection()
        {
        }

        public CategoryCollection(IList<Category> cats)
            : base(cats)
        {
        }

        public Category this[string keyword]
        {
            get
            {
                foreach (Category c in this)
                {
                    if (c.KeyWord == keyword)
                        return c;
                }
                return null;
            }
        }

        public CategoryCollection Clone()
        {
            CategoryCollection catcol = new CategoryCollection();
            foreach (Category c in catcol)
            {
                catcol.Add(c.Clone());
            }
            return catcol;
        }
    }
}
