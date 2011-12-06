using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace We7.CMS.Common
{
    [Serializable]
    public class ThreeValueCollection : Collection<ThreeValue>
    {
        public ThreeValue this[string key]
        {
            get
            {
                foreach (ThreeValue v in this)
                {
                    if (v.Key == key)
                        return v;
                }
                return null;
            }
        }

        public string GetText(string key)
        {
            ThreeValue v=this[key];
            return v == null ? String.Empty : v.Text;
        }

        public string GetValue(string key)
        {
            ThreeValue v = this[key];
            return v == null ? String.Empty : v.Value;
        }
    }

    [Serializable]
    public class ThreeValue
    {
        public string Key{ get; set; }

        public string Value { get; set; }

        public string Text { get; set; }
    }
}
