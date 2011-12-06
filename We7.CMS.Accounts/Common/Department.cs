using System;
using System.Collections.Generic;
using System.Text;
using We7.Framework;
using We7.Framework.Config;

namespace We7.CMS.Common.PF
{
    /// <summary>
    /// 部门实体类
    /// </summary>
    [Serializable]
    public class Department
    {
        /// <summary>
        /// 类属性
        /// </summary>
        public Department()
        {
            Created = DateTime.Now;
            ParentID = We7Helper.EmptyGUID;
            Children = new List<Department>();
            Updated=DateTime.Now;
            FromSiteID = SiteConfigs.GetConfig().SiteID;
        }
        /// <summary>
        /// 主键ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 来源站点ID
        /// </summary>
        public string FromSiteID { get; set; }
        /// <summary>
        /// 单位全称（包括所属父级单位）
        /// </summary>
        public string FullName{ get; set; }
        /// <summary>
        /// 父级单位ID
        /// </summary>
        public string ParentID{ get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name{ get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description{ get; set; }
        /// <summary>
        /// 索引
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Updated { get; set; }
        /// <summary>
        /// 子部门列表
        /// </summary>
        public List<Department> Children{ get; set; }
        /// <summary>
        /// 单位地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        /// 工作邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 部门站点URL
        /// </summary>
        public string SiteUrl { get; set; }
        /// <summary>
        /// 地图位置标记代码
        /// </summary>
        public string MapScript { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 详细职能描述
        /// </summary>
        public string Text { get; set; }

    }
}
