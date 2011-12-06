using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace We7.CMS.Common
{
    [Serializable]
    public class ColumnModule
    {
        public string ID { get; set; }

        public string Title { get; set; }

        public string Desc { get; set; }

        public string ParamIntro { get; set; }

        public string Path { get; set; }
    }

    [Serializable]
    public class ColumnModuleCollection : Collection<ColumnModule>
    {

    }

    public class ChannelModuleMapping
    {
        public string ID { get; set; }

        public string ChannelID { get; set; }

        public string ModuleID { get; set; }

        public string Parameter { get; set; }
    }
}
