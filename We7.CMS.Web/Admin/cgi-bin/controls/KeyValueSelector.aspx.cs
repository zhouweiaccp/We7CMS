using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using We7.Model.Core;
using We7.Framework.Util;
using We7.CMS.Common;
using System.Reflection;
using We7.Framework;
using Thinkment.Data;

namespace We7.CMS.Web.Admin.cgi_bin.controls
{
    public partial class KeyValueSelector : System.Web.UI.Page
    {
        #region Helper
        protected static HelperFactory HelperFactory
        {
            get
            {
                return We7.Framework.HelperFactory.Instance;
            }
        }

        /// <summary>
        /// 当前Helper的数据访问对象
        /// </summary>
        protected static ObjectAssistant Assistant
        {
            get
            {
                return HelperFactory.Assistant;
            }
        }
        #endregion

        protected IVoteHelper VoteHelper = VoteFactory.Instance;

        protected void Page_Load(object sender, EventArgs e)
        {
            string cmd = Request["cmd"];

            if (!String.IsNullOrEmpty(cmd))
            {
                switch (cmd.Trim().ToLower())
                {
                    case "thumbnail":
                        ResponseThumbnail();
                        break;
                    case "advice":
                        ResponseAdvice();
                        break;
                    case "modeltype":
                        ResponseModelType();
                        break;
                    case "advicestate":
                        ResponseAdviceState();
                        break;
                    case "accounttype":
                        ResponseModelName(ModelType.ACCOUNT);
                        break;
                    case "advicetype":
                        ResponseModelName(ModelType.ADVICE);
                        break;
                    case "articletype":
                        ResponseModelName(ModelType.ARTICLE);
                        break;
                    case "advicetypeid":
                        ReponseAdviceTypeID();
                        break;
                    case "adtag":
                        ResponseADTag();
                        break;
                    case "vswrapper":
                        ResponseWrapper();
                        break;
                    case "vote":
                        ResponseVote();
                        break;
                    case "questionnaire":
                        ResponseQuestionNaire();
                        break;
                    default:
                        ResponseFile(cmd);
                        break;
                }
            }
            else
            {
                OutPutMsg("命令不能为空");
            }
        }

        /// <summary>
        /// 输出反馈类型
        /// </summary>
        private void ReponseAdviceTypeID()
        {
            List<AdviceType> adviceList = Assistant.List<AdviceType>(null, null);

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("", "请选择");
            foreach (AdviceType at in adviceList)
            {
                if (dic.ContainsKey(at.ID))
                    continue;
                dic.Add(at.ID, at.Title);
            }
            OutPut2(dic);
        }

        private void ResponseQuestionNaire()
        {
            Assembly asm = Assembly.LoadFrom(Path.Combine(Server.MapPath(Request.ApplicationPath), "Bin/We7.Plugin.Questionnaire.dll"));
            Type QstHelper = asm.GetType("We7.Plugin.Questionnaire.Utils.QuestionnaireHelper");

            Object obj = QstHelper.InvokeMember(null,
              BindingFlags.DeclaredOnly |
              BindingFlags.Public | BindingFlags.NonPublic |
              BindingFlags.Instance | BindingFlags.CreateInstance, null, null, null);

            MethodInfo member = QstHelper.GetMethod("GetQuestionnaires", new Type[] { }, null);

            object[] listQst = (object[])member.Invoke(obj, null);

            //List<QstModle> listQst = new QuestionnaireHelper().GetQuestionnaires();

            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (object it in listQst)
            {
                if (dic.ContainsKey(it.GetType().GetProperty("ID").GetValue(it, null).ToString()))
                    continue;
                dic.Add(it.GetType().GetProperty("ID").GetValue(it, null).ToString(), it.GetType().GetProperty("Title").GetValue(it, null).ToString());
            }
            OutPut2(dic);
        }

        /// <summary>
        /// 查询出可用投票列表
        /// </summary>
        private void ResponseVote()
        {
            List<We7.CMS.Common.Vote> listVotes = VoteHelper.GetAvailVotes();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (We7.CMS.Common.Vote vote in listVotes)
            {
                if (dic.ContainsKey(vote.ID))
                    continue;
                dic.Add(vote.ID, vote.Title);
            }
            OutPut2(dic);
        }

        void ResponseModelType()
        {
            ContentModelCollection cmc = ModelHelper.GetContentModel(ModelType.ADVICE);
            ContentModelCollection cmc2 = ModelHelper.GetContentModel(ModelType.ARTICLE);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("", "请选择");
            foreach (We7.Model.Core.ContentModel c in cmc)
            {
                if (dic.ContainsKey(c.Name))
                    continue;
                dic.Add(c.Name, c.Label);
            }
            foreach (We7.Model.Core.ContentModel c in cmc2)
            {
                if (dic.ContainsKey(c.Name))
                    continue;
                dic.Add(c.Name, c.Label);
            }
            OutPut2(dic);
        }

        void ResponseModelName(ModelType type)
        {
            ContentModelCollection cmc = ModelHelper.GetContentModel(type);

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("", "请选择");
            foreach (We7.Model.Core.ContentModel c in cmc)
            {
                if (dic.ContainsKey(c.Name))
                    continue;
                dic.Add(c.Name, c.Label);
            }
            OutPut2(dic);
        }

        void ResponseFile(string fileName)
        {
            string path = Server.MapPath("~/Config/" + fileName + ".xml");
            if (File.Exists(path))
            {
                ResponseSimpleKvpList(path);
            }
            else
            {
                OutPutMsg("命令不存在:" + fileName);
            }
        }

        void ResponseThumbnail()
        {
            ResponseSimpleKvpList(Server.MapPath("~/Config/thumbnail.xml"));
        }

        void ResponseAdvice()
        {
            ResponseModelName(ModelType.ADVICE);
        }

        void ResponseADTag()
        {
            XmlNodeList items = XmlHelper.GetXmlNodeList(Server.MapPath("~/Config/TagsWord.xml"), "//Value");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (XmlElement xe in items)
            {
                if (!dic.ContainsKey(xe.InnerText.Trim()))
                {
                    dic.Add(xe.InnerText.Trim(), xe.InnerText.Trim());
                }
            }
            OutPut(dic);
        }

        void ResponseWrapper()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Widgets/Wrapper");
            DirectoryInfo di = new DirectoryInfo(dir);
            if (di.Exists)
            {
                FileInfo[] files = di.GetFiles("*.vm");
                if (files != null)
                {
                    foreach (FileInfo f in files)
                    {
                        string name = Path.GetFileNameWithoutExtension(f.Name);
                        dic.Add(name, name);
                    }
                }
            }

            OutPut(dic);
        }

        void ResponseAdviceState()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("全部", "0");
            dic.Add("待受理", "-1");
            dic.Add("待办理", "-3");
            dic.Add("审核中", "-4");
            dic.Add("办结", "99");
            OutPut(dic);
        }

        /// <summary>
        /// 输出简格式的配置文件
        /// </summary>
        /// <param name="path"></param>
        void ResponseSimpleKvpList(string path)
        {
            ResponseSimpleKvpList(path, "name", "value");
        }

        /// <summary>
        /// 输出简格式的配置文件
        /// </summary>
        /// <param name="path"></param>
        void ResponseSimpleKvpList(string path, string key, string value)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlNodeList list = doc.DocumentElement.SelectNodes("item");
            foreach (XmlElement xn in list)
            {
                dic.Add(xn.GetAttribute(key), xn.GetAttribute(value));
            }

            OutPut(dic);
        }

        /// <summary>
        /// 输出数据
        /// </summary>
        /// <param name="KvpList"></param>
        void OutPut(Dictionary<string, string> KvpList)
        {
            string txt = "数组为空";
            if (KvpList != null)
            {
                StringBuilder sb = new StringBuilder("[");
                foreach (KeyValuePair<string, string> kvp in KvpList)
                {
                    sb.Append("{").AppendFormat("k:'{0}',v:'{1}'", kvp.Key, kvp.Value).Append("},");
                }
                if (sb.Length > 1)
                    sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
                txt = sb.ToString();
            }
            OutPutMsg(txt);
        }

        /// <summary>
        /// 按值与键的方式输出字典值
        /// </summary>
        /// <param name="KvpList"></param>
        void OutPut2(Dictionary<string, string> KvpList)
        {
            string txt = "数组为空";
            if (KvpList != null)
            {
                StringBuilder sb = new StringBuilder("[");
                foreach (KeyValuePair<string, string> kvp in KvpList)
                {
                    sb.Append("{").AppendFormat("k:'{0}',v:'{1}'", kvp.Value, kvp.Key).Append("},");
                }
                if (sb.Length > 1)
                    sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
                txt = sb.ToString();
            }
            OutPutMsg(txt);
        }

        void OutPutMsg(string msg)
        {
            Response.Clear();
            Response.Write(msg);
            Response.End();
        }
    }
}
