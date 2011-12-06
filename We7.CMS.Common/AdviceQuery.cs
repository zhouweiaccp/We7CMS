using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common.Enum;

namespace We7.CMS.Common
{
    /// <summary>
    /// 反馈模型查询类，作为查询时的参数传递
    /// </summary>
    public class AdviceQuery
    {
        public AdviceQuery() { }
        string accountID;
        string adviceTypeID;
        int state;
        string title;
        int isShow;
        long sn;
        string myQueryPwd;
        DateTime startCreated = DateTime.MinValue;
        DateTime endCreated;
        string adviceInfoType;
        int mustHandle;
        string enumState;
        int notEnumState;
        int notState;
        string email;
        string phone;
        string name;
        string fax;
        string content;
        string address;
        string adviceTag;
        //int isRead;

        /// <summary>
        /// 除这个办理状态之外的信息
        /// </summary>
        public int NotState
        {
            get { return notState; }
            set { notState = value; }
        }

        /// <summary>
        /// 除这个状态之外的信息
        /// </summary>
        public int NotEnumState
        {
            get { return notEnumState; }
            set { notEnumState = value; }
        }

        /// <summary>
        /// 反馈信息状态集
        /// </summary>
        public string EnumState
        {
            get { return enumState; }
            set { enumState = value; }
        }

        /// <summary>
        /// 必须办理
        /// </summary>
        public int MustHandle
        {
            get { return mustHandle; }
            set { mustHandle = value; }
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string AccountID
        {
            get { return accountID; }
            set { accountID = value; }
        }

        /// <summary>
        /// 是否显示： 0 、不显示； 1、 显示
        /// </summary>
        public int IsShow
        {
            get { return isShow; }
            set { isShow = value; }
        }

        /// <summary>
        /// 所属模型ID
        /// </summary>
        public string AdviceTypeID
        {
            get { return adviceTypeID; }
            set { adviceTypeID = value; }
        }

        /// <summary>
        /// 反馈状态
        /// </summary>
        public int State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// 反馈标题
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// 反馈流水号
        /// </summary>
        public long SN
        {
            get { return sn; }
            set { sn = value; }
        }

        /// <summary>
        /// 查询密码
        /// </summary>
        public string MyQueryPwd
        {
            get { return myQueryPwd; }
            set { myQueryPwd = value; }
        }

        /// <summary>
        /// 起始创建日期
        /// </summary>
        public DateTime StartCreated
        {
            get { return startCreated; }
            set { startCreated = value; }
        }

        /// <summary>
        /// 终止创建日期
        /// </summary>
        public DateTime EndCreated
        {
            get { return endCreated; }
            set { endCreated = value; }
        }

        /// <summary>
        /// 反馈信息类别
        /// </summary>
        public string AdviceInfoType
        {
            get { return adviceInfoType; }
            set { adviceInfoType = value; }
        }


        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 用户email;
        /// </summary>
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        /// <summary>
        /// 用户电话
        /// </summary>
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        /// <summary>
        /// 用户传真号码
        /// </summary>
        public string Fax
        {
            get { return fax; }
            set { fax = value; }
        }
        /// <summary>
        /// 用户住址
        /// </summary>
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        /// <summary>
        ///反馈内容
        /// </summary>
        public string Content
        {

            get { return content; }
            set { content = value; }
        }

        /// <summary>
        /// 反馈标签
        /// </summary>
        public string AdviceTag
        {
            get { return adviceTag; }
            set { adviceTag = value; }
        }

        /// <summary>
        /// 是否显示未读标记        
        /// </summary>
        //public int IsRead
        //{
        //    get { return isRead; }
        //    set { isRead = value; }
        //}

    }
}
