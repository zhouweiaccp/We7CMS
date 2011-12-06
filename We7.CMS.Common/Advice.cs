using System;
using System.Collections.Generic;
using System.Text;
using We7.CMS.Common.Enum;
using System.Xml;
using System.Data;
using System.IO;
using We7.Framework;

namespace We7.CMS.Common
{

    /// <summary>
    /// 反馈信息
    /// </summary>
    [Serializable]
    public class AdviceInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 流水号
        /// </summary>
        public string SN { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Updated { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 反馈类型ID
        /// </summary>
        public string TypeID { get; set; }

        /// <summary>
        /// 是否前台显示
        /// 0：前台不显示,1:前台显示
        /// </summary>
        public int IsShow { get; set; }

        /// <summary>
        /// 显示状态文本
        /// </summary>
        public string IsShowText
        {
            get { return IsShow == 1 ? "显示" : "不显示"; }
        }

        /// <summary>
        /// 处理状态
        /// 0：未处理，1：不处理，2：已受理，处理中，3：已受理，办理中，9：已办结
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 处理状态文字
        /// </summary>
        public string StateText
        {
            get
            {
                switch (State)
                {
                    case 0:
                        return "未处理";
                    case 1:
                        return "不受理";
                    case 2:
                        return "已受理，办理中";
                    case 3:
                        return "已受理，转办中";
                    case 9:
                        return "已办结";
                    default:
                        return "未处理";
                }
            }
        }

        /// <summary>
        /// 办理优先级
        /// 0：可办可不办，1：必办，2：催办
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 优先级状态文字
        /// </summary>
        public string PriorityText
        {
            get
            {
                switch (Priority)
                {
                    case 0:
                        return "普通";
                    case 1:
                        return "必办";
                    case 2:
                        return "催办";
                    default:
                        return "普通";
                }
            }
        }

        /// <summary>
        /// 已读
        /// 0:未读,1:已读
        /// </summary>
        public int IsRead { get; set; }

        /// <summary>
        /// 已读状态文字
        /// </summary>
        public string IsReadText
        {
            get
            {
                return IsRead == 1 ? "已读" : "未读";
            }
        }

        /// <summary>
        /// 公开状态
        /// </summary>
        public int Public { get; set; }

        /// <summary>
        /// 公开状态文字描述
        /// </summary>
        public string PublicText
        {
            get
            {
                switch (Public)
                {
                    case 0:
                        return "不公开";
                    case 1:
                        return "公开";
                    default:
                        return "不公开";
                }
            }
        }

        /// <summary>
        /// 置顶
        /// </summary>
        public int IsTop { get; set; }

        public string IsTopText
        {
            get
            {
                return IsTop == 1 ? "置顶" : "未置顶";
            }
        }


        /// <summary>
        /// 查询密码
        /// </summary>
        public string MyQueryPwd { get; set; }

        /// <summary>
        /// 查询关键字1
        /// </summary>
        public string Display1 { get; set; }

        /// <summary>
        /// 查询关键字2
        /// </summary>
        public string Display2 { get; set; }

        /// <summary>
        /// 查询关键字3
        /// </summary>
        public string Display3 { get; set; }

        /// <summary>
        /// 模型名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 模型数据
        /// </summary>
        public string ModelXml { get; set; }

        /// <summary>
        /// 模型配置
        /// </summary>
        public string ModelConfig { get; set; }

        /// <summary>
        /// 模型定义文件
        /// </summary>
        public string ModelSchema { get; set; }

        #region 针对模型的扩展

        /// <summary>
        /// 获取模型数据
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public object this[string field]
        {
            get
            {
                try
                {
                    return DataSet != null && DataSet.Tables.Count > 0 && DataSet.Tables[0].Rows.Count > 0 && DataSet.Tables[0].Columns.Contains(field) ? DataSet.Tables[0].Rows[0][field] : null;
                }
                catch { }
                return null;
            }
        }
        /// <summary>
        /// 创建内容模型数据集
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        DataSet CreateDataSet(string path)
        {
            DataSet ds = new DataSet();
            ds.ReadXmlSchema(SchemaPath);
            return ds;
        }

        /// <summary>
        /// 创建内容模型数据集
        /// </summary>
        /// <returns></returns>
        DataSet CreateDataSet()
        {
            DataSet ds = new DataSet();
            using (TextReader reader = new StringReader(ModelSchema))
            {
                ds.ReadXmlSchema(reader);
            }
            return ds;
        }

        private string schemapath;
        /// <summary>
        /// Schema路径
        /// </summary>
        public string SchemaPath
        {
            get
            {
                if (String.IsNullOrEmpty(schemapath))
                {
                    throw new Exception("没有设定Schema路径");
                }
                return schemapath;
            }
            set { schemapath = value; }
        }

        /// <summary>
        /// 取得文章的内容模型数据集
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        DataSet GetDataSet()
        {
            DataSet ds = !String.IsNullOrEmpty(ModelSchema) ? CreateDataSet() : CreateDataSet(SchemaPath);
            TextReader reader = new StringReader(ModelXml);
            ds.ReadXml(reader);
            return ds;
        }

        private DataSet dataset;
        public DataSet DataSet
        {
            get
            {
                if (!isdirty)
                {
                    dataset = GetDataSet();
                    isdirty = true;
                }
                return dataset;
            }
        }

        public DataRow DataRow
        {
            get
            {
                return DataSet != null && DataSet.Tables.Count > 0 && DataSet.Tables[0].Rows.Count > 0 ? DataSet.Tables[0].Rows[0] : null;
            }
        }

        private bool isdirty = false;

        #endregion

        #region 针对点击量

        /// <summary>
        /// 日点击量
        /// </summary>
        public int DayClicks { get; set; }

        /// <summary>
        /// 昨日点击量
        /// </summary>
        public int YesterdayClicks { get; set; }

        /// <summary>
        /// 周点击量
        /// </summary>
        public int WeekClicks { get; set; }

        /// <summary>
        /// 月点击量
        /// </summary>
        public int MonthClicks { get; set; }

        /// <summary>
        /// 季点击量
        /// </summary>
        public int QuarterClicks { get; set; }

        /// <summary>
        /// 年点击量
        /// </summary>
        public int YearClicks { get; set; }

        /// <summary>
        /// 点击量
        /// </summary>
        public int Clicks { get; set; }


        #endregion

        /// <summary>
        /// 关联信息编号
        /// </summary>
        public string RelationID { get; set; }

        /// <summary>
        /// 关联内容模型
        /// </summary>
        public string RelationModelName { get; set; }
    }


    /// <summary>
    /// 反馈转移
    /// </summary>
    [Serializable]
    public class AdviceTransfer
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 反馈ID
        /// </summary>
        public string AdviceID { get; set; }

        /// <summary>
        /// 反馈信息
        /// </summary>
        public AdviceInfo Advice { get; set; }

        /// <summary>
        /// 转移来源
        /// </summary>
        public string FromTypeID { get; set; }

        /// <summary>
        /// 转移方向
        /// </summary>
        public string ToTypeID { get; set; }

        /// <summary>
        /// 处理用户ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 处理意见
        /// </summary>
        public string Suggest { get; set; }

        /// <summary>
        /// 处理状态
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created { get; set; }
    }


    /// <summary>
    /// 反馈授权
    /// </summary>
    [Serializable]
    public class AdviceAuth
    {
        public string ID { get; set; }

        /// <summary>
        /// 授权实体ID
        /// </summary>
        public string AuthID { get; set; }

        /// <summary>
        /// 授权类型
        /// 0：用户,1:角色,2:部门
        /// </summary>
        public int AuthType { get; set; }

        /// <summary>
        /// 反馈类型ID
        /// </summary>
        public string AdviceTypeID { get; set; }

        /// <summary>
        /// 功能
        /// 1：查看，2：管理，3：受理，4：办理
        /// </summary>
        public int Function { get; set; }
    }


    #region 以前的代码

    /// <summary>
    /// 反馈信息
    /// </summary>
    [Serializable]
    public class Advice : ProcessObject
    {
        /// <summary>
        ///反馈信息表
        /// </summary>
        private string id;
        private string typeID;
        private string typeTitle;
        private string userID;
        private string userName;
        private string title;
        private string content;
        private DateTime createDate = DateTime.Now;
        private int replyCount;
        private string email;
        private string replyDepID;
        private int isShow;
        private long sn;

        private string replyDept;
        private int state;
        string modelXml;
        DateTime updated = DateTime.Now;
        string myQueryPwd;
        string curProcessState;
        string enumState;

        private string snString;
        private string replyTime;
        private string replyMan;
        string fullTitle;
        string replyState;
        string linkUrl;
        string timeNote;
        int mustHandle;
        DateTime toHandleTime;

        int alertNote;
        string adviceUrl;

        string adviceInfoType;
        string toOtherHandleUserID;

        string phone;
        string fax;
        string address;
        string adviceTag;
        int isRead = 0;

        public Advice()
        {
            ProcessState = ((int)ProcessStates.WaitAccept).ToString();
        }

        /// <summary>
        ///转交办理人
        /// </summary>
        public string ToOtherHandleUserID
        {
            get { return toOtherHandleUserID; }
            set { toOtherHandleUserID = value; }
        }

        /// <summary>
        /// 反馈信息类型
        /// </summary>
        public string AdviceInfoType
        {
            get { return adviceInfoType; }
            set { adviceInfoType = value; }
        }

        public string AdviceUrl
        {
            get { return adviceUrl; }
            set { adviceUrl = value; }
        }

        /// <summary>
        /// 转交办理时间
        /// </summary>
        public DateTime ToHandleTime
        {
            get { return toHandleTime; }
            set { toHandleTime = value; }
        }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }
        /// <summary>
        /// 状态信息
        /// </summary>
        public int State
        {
            get { return state; }
            set { state = value; }
        }

        private string stateText;
        /// <summary>
        /// 反馈状态描述
        /// </summary>
        public string StateText
        {
            get
            {
                AdviceState ast = (AdviceState)System.Enum.Parse(typeof(AdviceState), State.ToString());
                string stateText = "";
                switch (ast)
                {
                    case AdviceState.All:
                        stateText = "全部";
                        break;
                    case AdviceState.WaitAccept:
                        stateText = "待受理";
                        break;
                    case AdviceState.WaitHandle:
                        stateText = "待办理";
                        break;
                    case AdviceState.Checking:
                        stateText = "审核中";
                        break;
                    case AdviceState.Finished:
                        stateText = "办结";
                        break;
                    default:
                        break;
                }

                return stateText;
            }
        }

        private string ownID;

        /// <summary>
        /// 栏目唯一标示符
        /// </summary>
        public string OwnID
        {
            get { return ownID; }
            set { ownID = value; }
        }

        /// <summary>
        /// 编号
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 类别编号
        /// </summary>
        public string TypeID
        {
            get { return typeID; }
            set { typeID = value; }
        }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string TypeTitle
        {
            get { return typeTitle; }
            set { typeTitle = value; }
        }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name
        {
            get { return userName; }
            set { userName = value; }
        }

        /// <summary>
        /// 回复邮件
        /// </summary>
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }

        /// <summary>
        /// 回复数
        /// </summary>
        public int ReplyCount
        {
            get { return replyCount; }
            set { replyCount = value; }
        }

        /// <summary>
        /// 反馈部门唯一标示符
        /// </summary>
        public string ReplyDepID
        {
            get { return replyDepID; }
            set { replyDepID = value; }
        }

        /// <summary>
        /// 反馈信息是否公开显示
        /// </summary>
        public int IsShow
        {
            get { return isShow; }
            set { isShow = value; }
        }

        /// <summary>
        /// 反馈信息流水号
        /// </summary>
        public long SN
        {
            get { return sn; }
            set { sn = value; }
        }

        /// <summary>
        /// 流水号信息
        /// </summary>
        public string SnString
        {
            get { return snString; }
            set { snString = value; }
        }

        /// <summary>
        /// 链接地址
        /// </summary>
        public string LinkUrl
        {
            get { return linkUrl; }
            set { linkUrl = value; }
        }

        /// <summary>
        /// 时间记录
        /// </summary>
        public string TimeNote
        {
            get { return timeNote; }
            set { timeNote = value; }
        }

        /// <summary>
        /// 回复时间
        /// </summary>
        public string ReplyTime
        {
            get { return replyTime; }
            set { replyTime = value; }
        }

        /// <summary>
        /// 冗余
        /// </summary>
        public string ReplyMan
        {
            get { return replyMan; }
            set { replyMan = value; }
        }

        /// <summary>
        /// 冗余
        /// </summary>
        public string ReplyDept
        {
            get { return replyDept; }
            set { replyDept = value; }
        }

        /// <summary>
        /// 反馈信息完整标题
        /// </summary>
        public string FullTitle
        {
            get { return fullTitle; }
            set { fullTitle = value; }
        }

        /// <summary>
        /// 反馈信息链接地址
        /// </summary>
        public string FullUrl
        {
            get
            {
                return We7Helper.GUIDToFormatString(this.ID) + ".html";
            }
        }

        /// <summary>
        /// 存放扩展信息XML数据
        /// </summary>
        public string ModelXml
        {
            get { return modelXml; }
            set { modelXml = value; }
        }

        /// <summary>
        /// 个性查询密码
        /// </summary>
        public string MyQueryPwd
        {
            get { return myQueryPwd; }
            set { myQueryPwd = value; }
        }

        /// <summary>
        ///  默认“0”为可不办，“1”为必办, “2”为催办。
        ///  如果状态为催办，那必定也是必办的。所以，必办条件为 >=1
        /// </summary>
        public int MustHandle
        {
            get { return mustHandle; }
            set { mustHandle = value; }
        }

        /// <summary>
        /// 办理状态显示图片信息
        /// </summary>
        public string MustHandleText
        {
            get
            {
                switch (MustHandle)
                {
                    case 0:
                        return "";
                        break;

                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        return "<img src='/admin/images/ok.gif' />";
                        break;

                    default:
                        return "";
                        break;
                }
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public string EnumState
        {
            get { return enumState; }
            set { enumState = value; }
        }

        string processText;
        /// <summary>
        /// 反馈信息审核信息
        /// </summary>
        public string ProcessText
        {
            get
            {
                ProcessStates pst = (ProcessStates)System.Enum.Parse(typeof(ProcessStates), ProcessState);
                string processText = "";
                switch (pst)
                {
                    case ProcessStates.Unaudit:
                        processText = "草稿";
                        break;
                    case ProcessStates.FirstAudit:
                        processText = "一审";
                        break;
                    case ProcessStates.SecondAudit:
                        processText = "二审";
                        break;
                    case ProcessStates.ThirdAudit:
                        processText = "三审";
                        break;
                    case ProcessStates.EndAudit:
                        processText = "审结";
                        break;
                    default:
                        break;
                }
                return stateText;
            }
        }

        /// <summary>
        /// 转交办理时间
        /// </summary>
        public string ToHandleTimeText
        {
            get
            {
                if (ToHandleTime != DateTime.MaxValue && ToHandleTime != DateTime.MinValue && State != (int)AdviceState.WaitAccept)
                {
                    TimeSpan c = DateTime.Now.Subtract(ToHandleTime);
                    if (c.Days == 0)
                    {
                        if (c.Hours == 0)
                            return c.Minutes.ToString() + "分钟前";
                        else
                            return c.Hours.ToString() + "小时前";
                    }
                    else
                        return c.Days.ToString() + "天前";
                }
                else
                    return "";
            }
        }

        /// <summary>
        /// 是否提醒标识
        /// </summary>
        public int AlertNote
        {
            get { return alertNote; }
            set { alertNote = value; }
        }

        string display1;
        /// <summary>
        /// 冗余字段1——用于前台列表显示
        /// </summary>
        public string Display1
        {
            get { return display1; }
            set { display1 = value; }
        }

        string display2;
        /// <summary>
        /// 冗余字段1——用于前台列表显示
        /// </summary>
        public string Display2
        {
            get { return display2; }
            set { display2 = value; }
        }

        string display3;
        /// <summary>
        /// 冗余字段1——用于前台列表显示
        /// </summary>
        public string Display3
        {
            get { return display3; }
            set { display3 = value; }
        }

        /// <summary>
        /// 模型名称
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// 模型配置
        /// </summary>
        public string ModelConfig { get; set; }

        /// <summary>
        /// 模型数据架构
        /// </summary>
        public string ModelSchema { get; set; }

        #region 针对模型的扩展

        /// <summary>
        /// 获取模型数据
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public object this[string field]
        {
            get
            {
                return DataSet != null && DataSet.Tables.Count > 0 && DataSet.Tables[0].Rows.Count > 0 && DataSet.Tables[0].Columns.Contains(field) ? DataSet.Tables[0].Rows[0][field] : null;
            }
        }
        /// <summary>
        /// 创建内容模型数据集
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        DataSet CreateDataSet(string path)
        {
            DataSet ds = new DataSet();
            ds.ReadXmlSchema(SchemaPath);
            return ds;
        }

        /// <summary>
        /// 创建内容模型数据集
        /// </summary>
        /// <returns></returns>
        DataSet CreateDataSet()
        {
            DataSet ds = new DataSet();
            using (TextReader reader = new StringReader(ModelSchema))
            {
                ds.ReadXmlSchema(reader);
            }
            return ds;
        }

        private string schemapath;
        /// <summary>
        /// Schema路径
        /// </summary>
        public string SchemaPath
        {
            get
            {
                if (String.IsNullOrEmpty(schemapath))
                {
                    throw new Exception("没有设定Schema路径");
                }
                return schemapath;
            }
            set { schemapath = value; }
        }

        /// <summary>
        /// 取得文章的内容模型数据集
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        DataSet GetDataSet()
        {
            DataSet ds = !String.IsNullOrEmpty(ModelSchema) ? CreateDataSet() : CreateDataSet(SchemaPath);
            TextReader reader = new StringReader(ModelXml);
            ds.ReadXml(reader);
            return ds;
        }

        private DataSet dataset;
        public DataSet DataSet
        {
            get
            {
                if (!isdirty)
                {
                    dataset = GetDataSet();
                    isdirty = true;
                }
                return dataset;
            }
        }

        public DataRow DataRow
        {
            get
            {
                return DataSet != null && DataSet.Tables.Count > 0 && DataSet.Tables[0].Rows.Count > 0 ? DataSet.Tables[0].Rows[0] : null;
            }
        }

        private bool isdirty = false;

        #endregion

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
        public int IsRead
        {
            get { return isRead; }
            set { isRead = value; }
        }

    }

    /// <summary>
    /// 反馈统计
    /// </summary>
    [Serializable]
    public class AdviceRate
    {
        string adviceTypeID;
        string adviceTypeTitle;
        int adviceCount;
        int handleNumber;
        int noHandleNumber;
        int handleCount;
        int noHandleCount;
        string handleRate;
        int noAdminHandleCount;
        string adviceInfoType;
        int notAdminMustHandleCount;

        /// <summary>
        /// 办理人所办理的状态为必须办理的信息数
        /// </summary>
        public int NotAdminMustHandleCount
        {
            get { return notAdminMustHandleCount; }
            set { notAdminMustHandleCount = value; }
        }

        /// <summary>
        /// 反馈模型ID
        /// </summary>
        public string AdviceTypeID
        {
            get { return adviceTypeID; }
            set { adviceTypeID = value; }
        }

        /// <summary>
        /// 反馈模型名称
        /// </summary>
        public string AdviceTypeTitle
        {
            get { return adviceTypeTitle; }
            set { adviceTypeTitle = value; }
        }

        /// <summary>
        ///总件数
        /// </summary>
        public int AdviceCount
        {
            get { return adviceCount; }
            set { adviceCount = value; }
        }

        /// <summary>
        /// 应处理数
        /// </summary>
        public int HandleNumber
        {
            get { return handleNumber; }
            set { handleNumber = value; }
        }


        /// <summary>
        /// 不需处理数
        /// </summary>
        public int NoHandleNumber
        {
            get { return noHandleNumber; }
            set { noHandleNumber = value; }
        }

        /// <summary>
        /// 总处理数
        /// </summary>
        public int HandleCount
        {
            get { return handleCount; }
            set { handleCount = value; }
        }

        /// <summary>
        /// 办理人处理数（非管理员办理的处理数）
        /// </summary>
        public int NoAdminHandleCount
        {
            get { return noAdminHandleCount; }
            set { noAdminHandleCount = value; }
        }
        /// <summary>
        /// 总共未处理数
        /// </summary>
        public int NoHandleCount
        {
            get { return noHandleCount; }
            set { noHandleCount = value; }
        }

        /// <summary>
        /// 解决率，办理率
        /// </summary>
        public string HandleRate
        {
            get { return handleRate; }
            set { handleRate = value; }
        }

        /// <summary>
        /// 反馈信息类型
        /// </summary>
        public string AdviceInfoType
        {
            get { return adviceInfoType; }
            set { adviceInfoType = value; }
        }
    }

    #endregion
}
