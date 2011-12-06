using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Collections;

namespace We7.Framework.Common
{
    [Serializable]
    public class ResourceArray:ArrayList
    {
        public string ToJson()
        {
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(this);
            }
            catch 
            {
                return "[]";
            }
        }

        public ResourceArray FromJson(string json)
        {
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Deserialize<ResourceArray>(json)??new ResourceArray();
            }
            catch 
            {
                return new ResourceArray();
            }
        }
    }
}
