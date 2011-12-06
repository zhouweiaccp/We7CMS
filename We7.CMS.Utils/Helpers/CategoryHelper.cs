using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common;
using We7.Framework;
using Thinkment.Data;
using System.Globalization;
using We7.Framework.Util;
using System.Xml;
using We7.Framework.Cache;

namespace We7.CMS
{
    /// <summary>
    /// 栏目分类
    /// </summary>
    [Serializable]
    [Helper("We7.CategoryHelper")]
    public class CategoryHelper : BaseHelper
    {
        public static CacheRecord CacheRecord;

        static CategoryHelper()
        {
            CacheRecord = CacheRecord.Create(typeof(CategoryHelper));
        }

        /// <summary>
        /// 添加类别
        /// </summary>
        /// <param name="cat"></param>
        public void AddCategory(Category cat)
        {
            if (cat == null)
                throw new Exception("添加类别不能为空");
            if (We7Helper.IsEmptyID(cat.ID))
            {
                cat.ID = We7Helper.CreateNewID();
                cat.CreateDate = DateTime.Now;
            }
            Assistant.Insert(cat);
            CacheRecord.Release();
        }

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="cat"></param>
        public void EditCategory(Category cat)
        {
            if (cat == null)
                throw new Exception("修改内容为空");
            if (We7Helper.IsEmptyID(cat.ID))
                throw new Exception("修改记录ID为为空");
            Assistant.Update(cat);
            CacheRecord.Release();
        }

        public void DeleteCategory(Category cat)
        {
            List<Category> children = GetCategoryByParentID(cat.ID);
            foreach (Category c in children)
            {
                DeleteCategory(c);
            }
            Assistant.Delete(cat);
            CacheRecord.Release();
        }

        public void DeleteCategory(string id)
        {
            Category cat = GetCategory(id);
            if (cat != null)
            {
                DeleteCategory(cat);
            }
            CacheRecord.Release();
        }

        /// <summary>
        /// 取得所有栏目信息
        /// </summary>
        /// <returns></returns>
        public CategoryCollection GetCategorys()
        {
            return CacheRecord.GetInstance<CategoryCollection>("GetCategorys", () =>
           {
               CategoryCollection CatColl = new CategoryCollection();
               List<Category> list = GetCategoryByParentID(We7Helper.EmptyGUID);
               foreach (Category c in list)
               {
                   CatColl.Add(c);
                   AppendChildCategory(c);
               }
               return CatColl;
           });
        }

        public CategoryCollection GetFmtChildren(string parentId)
        {
            return CacheRecord.GetInstance<CategoryCollection>("GetFmtChildren$" + parentId, () =>
           {

               CategoryCollection cats = new CategoryCollection();
               GetFmtChildren(parentId, cats, "");
               return cats;
           });
        }

        private void GetFmtChildren(string parentId, CategoryCollection cats, string prefix)
        {
            List<Category> children = GetCategoryByParentID(parentId);
            for (int i = 0; i < children.Count; i++)
            {
                cats.Add(children[i]);
                if (i == children.Count - 1)
                {
                    children[i].Name = prefix + "└" + children[i].Name;
                    GetFmtChildren(children[i].ID, cats, prefix + "　");
                }
                else
                {
                    children[i].Name = prefix + "├" + children[i].Name;
                    GetFmtChildren(children[i].ID, cats, prefix + "│");
                }
            }
        }

        private void LoadChildren(Category cat)
        {
            if (cat != null)
            {
                List<Category> list = GetCategoryByParentID(cat.ID);
                if (list != null)
                {
                    cat.Children = new CategoryCollection(list);
                    foreach (Category c in cat.Children)
                    {
                        LoadChildren(c);
                    }
                }
            }
        }

        /// <summary>
        /// 按ID取得类别信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Category GetCategory(string id)
        {
            List<Category> list = Assistant.List<Category>(new Criteria(CriteriaType.Equals, "ID", id), new Order[] { new Order("ID", OrderMode.Asc) });
            return list != null && list.Count > 0 ? list[0] : null;
        }

        /// <summary>
        /// 根据关键字取得类别信息
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public Category GetCategoryByKeyword(string keyword)
        {
            List<Category> list = Assistant.List<Category>(new Criteria(CriteriaType.Equals, "KeyWord", keyword), new Order[] { new Order("ID", OrderMode.Asc) });
            return list != null && list.Count > 0 ? list[0] : null;
        }

        /// <summary>
        /// 根据关键字取得未格式化的子分类
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public CategoryCollection GetChildrenByKeyword(string keyword)
        {
            return CacheRecord.GetInstance<CategoryCollection>("GetChildrenByKeyword$" + keyword, () =>
           {
               CategoryCollection result = new CategoryCollection();
               List<Category> list = Assistant.List<Category>(new Criteria(CriteriaType.Equals, "KeyWord", keyword), new Order[] { new Order("Index"), new Order("ID", OrderMode.Asc) });
               if (list != null && list.Count > 0)
               {
                   result = GetChildren(list[0].ID);
               }
               return result;
           });
        }

        public CategoryCollection GetChildren(string parentId)
        {
            return CacheRecord.GetInstance<CategoryCollection>("GetChildren$" + parentId, () =>
           {
               CategoryCollection cats = new CategoryCollection();
               GetChildren(parentId, cats);
               return cats;
           });

        }

        private void GetChildren(string parentId, CategoryCollection cats)
        {
            List<Category> children = GetCategoryByParentID(parentId);
            for (int i = 0; i < children.Count; i++)
            {
                cats.Add(children[i]);
                GetChildren(children[i].ID, cats);
            }
        }

        /// <summary>
        /// 取得树状的子类
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public CategoryCollection GetAllChildrenByKeyword(string keyword)
        {
            return CacheRecord.GetInstance<CategoryCollection>("GetAllChildrenByKeyword$" + keyword, () =>
            {
                CategoryCollection result = new CategoryCollection();
                List<Category> list = Assistant.List<Category>(new Criteria(CriteriaType.Equals, "KeyWord", keyword), new Order[] { new Order("Index"), new Order("ID", OrderMode.Asc) });
                if (list != null && list.Count > 0)
                {
                    list = GetCategoryByParentID(list[0].ID);
                    foreach (Category c in list)
                    {
                        result.Add(c);
                        LoadChildren(c);
                    }
                }
                return result;
            });
        }

        public Category GetTopAndSiblingByKeyword(string keyword)
        {
            return CacheRecord.GetInstance<Category>("GetTopAndSiblingByKeyword$" + keyword, () =>
            {
                List<Category> list = Assistant.List<Category>(new Criteria(CriteriaType.Equals, "KeyWord", keyword), new Order[] { new Order("Index"), new Order("ID", OrderMode.Asc) });
                if (list != null && list.Count > 0)
                {
                    return GetCategory(list[0].ParentID);
                }
                return null;
            });
        }

        /// <summary>
        /// 根据关键字取得未格式化的子分类
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public CategoryCollection GetFrmChildrenByKeyword(string keyword)
        {
            return CacheRecord.GetInstance<CategoryCollection>("GetFrmChildrenByKeyword$"+keyword, () =>
            {
                CategoryCollection result = new CategoryCollection();
                List<Category> list = Assistant.List<Category>(new Criteria(CriteriaType.Equals, "KeyWord", keyword), new Order[] { new Order("Index"), new Order("ID", OrderMode.Asc) });
                if (list != null && list.Count > 0)
                {
                    result = GetFmtChildren(list[0].ID);
                }
                return result;
            });

        }

        /// <summary>
        /// 重新加载类别信息
        /// </summary>
        public void ReloadCategorys()
        {
            CacheRecord.Release();
        }

        /// <summary>
        /// 从当前容器中查找指定的记录
        /// </summary>
        /// <param name="cat"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public Category FindCatetory(Category cat, string keyword)
        {
            if (cat.KeyWord == keyword)
                return cat;
            foreach (Category c in cat.Children)
            {
                Category cc = FindCatetory(c, keyword);
                if (cc != null)
                    return cc;
            }
            return null;
        }

        /// <summary>
        /// 根据父ID取得子栏目
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        public List<Category> GetCategoryByParentID(string parentID)
        {
            Criteria c = new Criteria(CriteriaType.Equals, "ParentID", parentID);
            return Assistant.List<Category>(c, new Order[] { new Order("Index"), new Order("CreateDate") });
        }

        /// <summary>
        /// 根据父ID取得子栏目
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        public List<Category> GetChildrenListByKeyword(string keyword)
        {
            Category c = GetCategoryByKeyword(keyword);
            return c != null ? GetCategoryByParentID(c.ID) : null;
        }

        /// <summary>
        /// 根据类别keyword获取其兄弟节点列表
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<Category> GetSiblingListByKeyword(string keyword)
        {
            Category c = GetCategoryByKeyword(keyword);
            return c != null ? GetCategoryByParentID(c.ParentID) : null;
        }


        /// <summary>
        /// 格式化类别树
        /// </summary>
        /// <param name="catcol"></param>
        public void FormatCategorys(CategoryCollection catcol, bool formatRoot)
        {
            for (int i = 0; i < catcol.Count; i++)
            {
                Category pcat = catcol[i];
                if (i == catcol.Count - 1)
                {
                    pcat.Name = (formatRoot ? "├" : "") + pcat.Name;
                    FormatCategorys(pcat, "│");
                }
                else
                {
                    pcat.Name = (formatRoot ? "└" : "") + pcat.Name;
                    FormatCategorys(pcat, "　");
                }
            }
        }

        public void FormatCategorys(Category c, string Pattern)
        {
            for (int i = 0; i < c.Children.Count; i++)
            {
                Category pcat = c.Children[i];
                if (i == c.Children.Count - 1)
                {
                    pcat.Name = Pattern + "├" + pcat.Name;
                    FormatCategorys(pcat, Pattern + "│");
                }
                else
                {
                    pcat.Name = Pattern + "└" + pcat.Name;
                    FormatCategorys(pcat, Pattern + "　");
                }
            }
        }

        /// <summary>
        /// 检测名称是否重复
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool CheckNameRepeat(string name)
        {
            if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(name.Trim()))
                return false;

            Criteria c = new Criteria(CriteriaType.Equals, "Name", name.Trim());
            return Assistant.Count<Category>(c) > 0;

        }

        /// <summary>
        /// 检测关键字是否重复
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public bool CheckKeywordRepeat(string keyword)
        {
            if (String.IsNullOrEmpty(keyword) || String.IsNullOrEmpty(keyword.Trim()))
                return false;

            Criteria c = new Criteria(CriteriaType.Equals, "KeyWord", keyword.Trim());
            return Assistant.Count<Category>(c) > 0;
        }

        /// <summary>
        /// 添加子栏目
        /// </summary>
        /// <param name="c"></param>
        void AppendChildCategory(Category c)
        {
            List<Category> list = GetCategoryByParentID(c.ID);
            if (list != null)
            {
                foreach (Category cc in list)
                {
                    c.Children.Add(cc);
                    AppendChildCategory(cc);
                }
            }
        }

        /// <summary>
        /// 更新分类项
        /// </summary>
        /// <param name="cat"></param>
        public void UpdateCategory(Category cat)
        {
            Assistant.Update(cat, new string[] { "Name", "KeyWord", "Description", "Options", "Index" });
            CacheRecord.Release();
        }
    }

    public class CategoryOption
    {
        private int intValue;

        public string Name { get; set; }

        public string Value { get; set; }

        public int IntValue
        {
            get
            {
                if (intValue == 0)
                {
                    Int32.TryParse(Value, out intValue);
                }
                return intValue;
            }
        }
    }

    public class CategoryOptionHelper
    {
        const string CacheKey = "$CategoryOptionHelper$OptionsType$";
        public static CacheRecord CacheRecord;

        static CategoryOptionHelper()
        {
            CacheRecord = CacheRecord.Create(typeof(CategoryOptionHelper));
        }

        public static bool Check(string opValue, int value)
        {
            int op;
            return Int32.TryParse(opValue, NumberStyles.AllowHexSpecifier, null, out op) && (op == (op & value));
        }

        public static List<CategoryOption> GetOptions(string type)
        {
            return CacheRecord.GetInstance<List<CategoryOption>>("GetOptions", (f) =>
            {
                List<CategoryOption> list = new List<CategoryOption>();
                XmlNodeList nodes = XmlHelper.GetXmlNodeList(f[0], "//type[@name='" + type + "']/option");
                if (nodes != null)
                {
                    foreach (XmlElement xe in nodes)
                    {
                        CategoryOption op = new CategoryOption();
                        op.Name = xe.GetAttribute("name");
                        op.Value = xe.GetAttribute("value");
                        list.Add(op);
                    }
                }
                return list;
            }, We7Utils.GetMapPath("~/Config/CategoryOptions.config"));
        }

        public static List<string> GetOptionTypes()
        {
            return CacheRecord.GetInstance<List<string>>("GetOptionTypes", (f) =>
            {
                List<string> list = new List<string>();
                XmlNodeList nodes = XmlHelper.GetXmlNodeList(f[0], "//type");
                foreach (XmlElement xe in nodes)
                {
                    list.Add(xe.GetAttribute("name"));
                }
                return list;
            }, We7Utils.GetMapPath("~/Config/CategoryOptions.config"));
        }
    }
}
