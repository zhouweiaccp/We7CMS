using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    public class Point
    {
        public string ID { get; set; }

        public int Value { get; set; }

        public int Action { get; set; }

        public string ObjectID { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        public string AccountID { get; set; }

        public string AccountName { get; set; }

        public string ActionText
        {
            get
            {
                string act="";
                switch ((PointAction)Action)
                {
                    case PointAction.PointIn:
                        act = "收入";
                        break;
                    case PointAction.PointOut:
                        act = "支出";
                        break;
                    case PointAction.ExchangeMoney:
                        act = "积分换币";
                        break;
                    case PointAction.Transfer:
                        act = "转账";
                        break;
                    default:
                        act = "";
                        break;
                }
                return act;
            }
        }

    }

    public enum PointAction
    {
        /// <summary>
        /// 收入积分
        /// </summary>
        PointIn,
        /// <summary>
        /// 积分支出
        /// </summary>
        PointOut,
        /// <summary>
        /// 积分换币
        /// </summary>
        ExchangeMoney,
        /// <summary>
        /// 转账
        /// </summary>
        Transfer
    }
}
