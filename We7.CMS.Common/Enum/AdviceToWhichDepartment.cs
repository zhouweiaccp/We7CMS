using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common.Enum
{
    /// <summary>
    /// 转交办理部门枚举
    /// </summary>
    public enum AdviceToWhichDepartment : int
    {
        Samelevel = 0, //同级部门
        LowLevel = 1,  //下级部门
        All = 2  //所有部门
    }
}
