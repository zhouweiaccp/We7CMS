using System;
using System.Collections.Generic;
using System.Text;

namespace We7.Framework
{
    /// <summary>
    /// 延迟加载
    /// </summary>
    public static class Lazy
    {
        /// <summary>
        /// 延迟加载创建对象
        /// </summary>
        /// <typeparam name="TResult">对象类型</typeparam>
        /// <param name="creator">创建对象的匿名委托</param>
        /// <returns>创建的对象</returns>
        public static TResult New<TResult>(We7Func<TResult> creator)
        {
            return new Lazy<TResult>(creator).Eval();
        }
    }
   
}
