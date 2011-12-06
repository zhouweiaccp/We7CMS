using System;
using System.Collections.Generic;
using System.Text;

namespace We7.Framework
{
    public class ParamPair : Dictionary<string, string>
    {
        public ParamPair Append(string key, string value)
        {
            Add(key, value);
            return this;
        }

        public ParamPair Remove(string key)
        {
            if (ContainsKey(key))
                Remove(key);
            return this;
        }


    }
}
