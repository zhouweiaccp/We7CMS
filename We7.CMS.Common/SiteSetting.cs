using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    [Obsolete]
    [Serializable]
    public class SiteSetting
    {
        string id;
        string title;
        string value;
        int index;

        public SiteSetting()
        {
        }

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return title; }
            set { title = value; }
        }

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

    }
}
