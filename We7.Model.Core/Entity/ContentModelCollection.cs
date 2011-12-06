using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace We7.Model.Core
{
    [Serializable]
    public class ContentModelCollection:Collection<ContentModel>,ICloneable
    {
        /// <summary>
        /// 按名称查询值。
        /// </summary>
        /// <param name="key">模型名称</param>
        /// <returns>查询到的值。如果为空，null。</returns>
        public ContentModel this[string name]
        {
            get
            {
                foreach (ContentModel model in this)
                {
                    if (model.Name == name)
                        return model;
                }
                return null;
            }
            set
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].Name==name)
                    {
                        this[i] = value;
                    }
                }
            }
        }
        #region ICloneable 成员

        public object Clone()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
