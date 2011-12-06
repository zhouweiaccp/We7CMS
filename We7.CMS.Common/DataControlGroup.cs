using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// 控件组信息
    /// </summary>
    [Serializable]
    public class DataControlGroup
    {
        public string Name { get; set; }

        public string Label { get; set; }

        public string Desc { get; set; }

        public string PhysicalPath { get; set; }
    }
}
