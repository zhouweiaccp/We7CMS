using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace We7.Framework
{
    /// <summary>
    /// 提供对We7.CMS的版本比较的公共类
    ///     wjz 2010年8月10日
    ///     默认要比当前的版本小 return false
    /// </summary>
    public class VersioncComparison
    {
        /// <summary>
        /// 比较版本大小
        /// </summary>
        /// <param name="verA">比较的版本号</param>
        /// <param name="verB">比较的依据版本号</param>
        /// <param name="agnoreCase">是不是忽略大小写</param>
        /// <returns></returns>
        public static ECompareRelut Compare(string verA, string verB, bool agnoreCase)
        {
            //默认为比较小的版本

            verA = verA.TrimStart("Vv".ToCharArray());
            verB = verB.TrimStart("Vv".ToCharArray());

            ECompareRelut cr = new ECompareRelut() ;
            int r = string.Compare(verA, verB, true);
            if (r > 0 )
            {
                cr = ECompareRelut.Greater;
            }
            else if (r == 0)
            {
                cr = ECompareRelut.Equal;
            }
            else if(r < 0)
            {
                cr = ECompareRelut.Less;
            }
            return cr;
        }
    }
    /// <summary>
    /// 判断大小枚举
    /// </summary>
    public enum ECompareRelut
    {
    
        /// <summary>
        /// 比源数据大
        /// </summary>
        [Description("大于某值")]
        Greater = 0,
        /// <summary>
        /// 俩个数据相等
        /// </summary>
        [Description("等于某值")]
        Equal = 1,
        /// <summary>
        /// 比元数据小
        /// </summary>
        [Description("小于某值")]
        Less= 2,
    }

}
