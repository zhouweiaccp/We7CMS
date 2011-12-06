using System;
using System.Collections.Generic;
using System.Text;

namespace We7.Framework
{
    /// <summary>
    ///不带参数，有返回值 匿名委托
    /// </summary>
    /// <typeparam name="TResult">返回值类型</typeparam>
    /// <returns></returns>
    public delegate TResult We7Func<TResult>();

    /// <summary>
    /// 实现延迟加载
    /// </summary>
    /// <typeparam name="TResult">返回值类型</typeparam>
    public class Lazy<TResult>
    {
        #region Filed

        private We7Func<TResult> creator;
        private bool hasExcuted;
        private TResult result;
        private readonly object sync;

        #endregion

        #region ctor.

        public Lazy(We7Func<TResult> creator)
        {
            this.creator = creator;
            this.hasExcuted = false;
            this.sync = new object();
        }

        #endregion

        #region method

        public TResult Eval()
        {
            if (!this.hasExcuted)
            {
                lock (this.sync)
                {
                    if (!this.hasExcuted)
                    {
                        this.result = this.creator();
                        this.hasExcuted = true;
                    }
                }
            }

            return this.result;
        }

        #endregion
    }
}
