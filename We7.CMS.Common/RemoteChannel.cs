using System;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Common
{
    /// <summary>
    /// 远程站点栏目信息
    /// </summary>
    [Serializable]
    public class RemoteChannel
    {
        int nodeLevel;
        /// <summary>
        /// 节点级别
        /// </summary>
        public int NodeLevel
        {
            get { return nodeLevel; }
            set { nodeLevel = value; }
        }

        int parentNodeLevel;
        /// <summary>
        /// 父节点级别
        /// </summary>
        public int ParentNodeLevel
        {
            get { return parentNodeLevel; }
            set { parentNodeLevel = value; }
        }

        string enumState;
        /// <summary>
        /// 状态位
        /// </summary>
        public string EnumState
        {
            get { return enumState; }
            set { enumState = value; }
        }

        DateTime updated;
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Updated
        {
            get { return updated; }
            set { updated = value; }
        }

        string processLayerNO;
        /// <summary>
        /// 审批级别
        /// </summary>
        public string ProcessLayerNO
        {
            get { return processLayerNO; }
            set { processLayerNO = value; }
        }

        string id;
        /// <summary>
        /// 主键ID
        /// </summary>
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        string alias;

        /// <summary>
        /// 别名
        /// </summary>
        public string Alias
        {
            get { return alias; }
            set { alias = value; }
        }

        string parentID;

        /// <summary>
        /// 父级ID
        /// </summary>
        public string ParentID
        {
            get { return parentID; }
            set { parentID = value; }
        }

        string name;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        string parameter;

        /// <summary>
        /// 参数
        /// </summary>
        public string Parameter
        {
            get { return parameter; }
            set { parameter = value; }
        }

        int index;

        /// <summary>
        /// 序列
        /// </summary>
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        string fullPath;
        /// <summary>
        /// 
        /// </summary>
        public string FullPath
        {
            get { return fullPath; }
            set { fullPath = value; }
        }

        string description;

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        string detailTemplate;
        public string DetailTemplate
        {
            get { return detailTemplate; }
            set { detailTemplate = value; }
        }

        string templateName;
        public string TemplateName
        {
            get { return templateName; }
            set { templateName = value; }
        }

        int state;
        public int State
        {
            get { return state; }
            set { state = value; }
        }

        string stateText;
        public string StateText
        {
            get { return stateText; }
            set { stateText = value; }
        }

        int securityLevel;
        public int SecurityLevel
        {
            get { return securityLevel; }
            set { securityLevel = value; }
        }

        string securityLevelText;
        public string SecurityLevelText
        {
            get { return securityLevelText; }
            set { securityLevelText = value; }
        }

        string referenceID;
        public string ReferenceID
        {
            get { return referenceID; }
            set { referenceID = value; }
        }

        DateTime created = DateTime.Now;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        string templateText;
        public string TemplateText
        {
            get { return templateText; }
            set { templateText = value; }
        }

        string detailTemplateText;
        public string DetailTemplateText
        {
            get { return detailTemplateText; }
            set { detailTemplateText = value; }
        }

        RemoteChannel[] remoteChannels;
        public RemoteChannel[] RemoteChannels
        {
            get { return remoteChannels; }
            set { remoteChannels = value; }
        }

        string defaultContentID;
        public string DefaultContentID
        {
            get { return defaultContentID; }
            set { defaultContentID = value; }
        }

        string channelFolder;
        public string ChannelFolder
        {
            get { return channelFolder; }
            set { channelFolder = value; }
        }

        string titleImage;
        public string TitleImage
        {
            get { return titleImage; }
            set { titleImage = value; }
        }

        string process;
        public string Process
        {
            get { return process; }
            set { process = value; }
        }

        string type;
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        string typeText;
        public string TypeText
        {
            get { return typeText; }
            set { typeText = value; }
        }

        string channelName;
        public string ChannelName
        {
            get { return channelName; }
            set { channelName = value; }
        }

        string refAreaID;
        public string RefAreaID
        {
            get { return refAreaID; }
            set { refAreaID = value; }
        }

        int isComment;
        public int IsComment
        {
            get { return isComment; }
            set { isComment = value; }
        }

        string isCommentText;
        public string IsCommentText
        {
            get { return isCommentText; }
            set { isCommentText = value; }
        }

        string fullUrl;
        public string FullUrl
        {
            get { return fullUrl; }
            set { fullUrl = value; }
        }

        string titleImageFileUrl;
        public string TitleImageFileUrl
        {
            get { return titleImageFileUrl; }
            set { titleImageFileUrl = value; }
        }

        string fullFolderPath;
        public string FullFolderPath
        {
            get { return fullFolderPath; }
            set { fullFolderPath = value; }
        }

        string returnUrl;
        public string ReturnUrl
        {
            get { return returnUrl; }
            set { returnUrl = value; }
        }

        //此代码原位于We7.CMS.Common.Utils下的Constants.cs中
        public string ChannelPath = "_data\\Channels";

        //此代码原位于We7.CMS.Common.Utils下的Constants.cs中
        string channelUrlPath;
        public string ChannelUrlPath
        {
            get { return channelUrlPath; }
            set { channelUrlPath = value; }
        }

    }
}
