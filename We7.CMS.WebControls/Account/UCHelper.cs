using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework;
using We7.Framework.Cache;
using We7.CMS.Common.PF;
using We7.CMS.Common;
using We7.CMS.Accounts;

namespace We7.CMS.WebControls
{
    public static partial class UCHelper
    {
        /// <summary>
        /// 根据部门ID取得部门名称
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static string GetDepartName(object arg)
        {
            string departId = arg as string;
            return !String.IsNullOrEmpty(departId) ? CacheRecord.Create(typeof(AccountLocalHelper)).GetInstance<string>("Name$" + departId, () =>
            {
                Department depart =  AccountFactory.CreateInstance().GetDepartment(departId, null);
                return depart != null ? depart.Name : String.Empty;
            }) : String.Empty;
        }

        /// <summary>
        /// 取得类别关键字
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static string GetCatName(object arg)
        {
            string keyword = arg as string;
            return !String.IsNullOrEmpty(keyword) ? CacheRecord.Create(typeof(CategoryHelper)).GetInstance<string>("Name$" + keyword, () =>
            {
                Category category = HelperFactory.Instance.GetHelper<CategoryHelper>().GetCategoryByKeyword(keyword);
                return category != null ? category.Name : String.Empty;
            }) : String.Empty;
        }

        public static string GetUrl(string oid, string id)
        {
            return UrlHelper.GetUrl(oid, id);
        }

        public static string GetModelUrl(string modelName, string id)
        {
            return UrlHelper.GetModelUrl(modelName, id);
        }
    }
}
