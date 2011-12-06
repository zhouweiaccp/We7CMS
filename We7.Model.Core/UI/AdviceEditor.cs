using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace We7.Model.Core.UI
{
    public class AdviceEditor : AutoEditor
    {
        private string modelName;
        public override string ModelName
        {
            get
            {
                if (String.IsNullOrEmpty(modelName))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(Server.MapPath("~/Config/advicetype.xml"));
                    XmlElement xe=doc.DocumentElement.SelectSingleNode("//item[@value='"+ModelTypeID+"']") as XmlElement;
                    if (xe == null)
                        throw new Exception("不存在不前的反馈模型");
                    modelName = xe.GetAttribute("");
                }
                return modelName;
            }
            set
            {
                modelName = value;
            }
        }

        public string ModelTypeID { get; set; }
    }
}
