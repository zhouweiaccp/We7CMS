using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using We7.Model.Core.UI;
using We7.Framework;
using We7.Framework.Config;
using We7.Framework.Util;
using We7.CMS;
using System.IO;
using Thinkment.Data;
using We7.Model.Core;

namespace We7.Model.UI.Controls
{
    public abstract class We7FieldControl : FieldControl
    {
        /// <summary>
        /// 当前文章ID
        /// </summary>
        protected string ArticleID
        {
            get
            {
                string id=Request[We7.Model.Core.UI.Constants.EntityID];
                if (String.IsNullOrEmpty(id)&&Page is ModelHandlerPage)
                {
                    id = ((ModelHandlerPage)Page).RecordID;
                }
                return String.IsNullOrEmpty(id)?We7Helper.CreateNewID():id;
            }
        }
        protected string ImgType
        {
            get
            {
                return Request.QueryString["ImgType"] == null ? "" : Request.QueryString["ImgType"];
            }

        }
        /// <summary>
        /// 业务对象工厂
        /// </summary>
        protected HelperFactory HelperFactory
        {
            get { return (HelperFactory)Application[HelperFactory.ApplicationID]; }
        }

        /// <summary>
        /// 文章业务对象
        /// </summary>
        protected ArticleHelper ArticleHelper
        {
            get { return HelperFactory.GetHelper<ArticleHelper>(); }
        }

        /// <summary>
        /// 数据访问对象
        /// </summary>
        protected ObjectAssistant Assistant
        {
            get { return ArticleHelper.Assistant; }
        }

        /// <summary>
        /// 当前目录存在否,如果没有就创建
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns></returns>
        protected bool IsDirExists(string dirName)
        {
            string dir = Server.MapPath(dirName);
            bool IsEx = true;
            try
            {
                if (System.IO.Directory.Exists(dir))
                {
                    //  throw   new   Exception( "目录已存在 "); 
                    IsEx = false;
                }
                else
                {
                    System.IO.Directory.CreateDirectory(dir);
                }
            }
            catch
            {
                // throw new Exception(ee.Message);
                IsEx = false;
            }
            return IsEx;
        }
        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="dirName"></param>
        /// <returns></returns>
        protected bool DeleteFile(string dirName)
        {
            string dir = Server.MapPath(dirName);
            if (File.Exists(dir))
            {
                File.Delete(dir);
                return true;
            }
            else
            { return false; }

        }

        /// <summary>
        /// 使用Sql语句查询数据
        /// </summary>
        /// <param name="sql">Thinkment.Data格式的SQL语句</param>
        /// <returns>数据值</returns>
        protected DataTable QueryBySql(string sql)
        {
            IDatabase db = Assistant.GetDatabases()["We7.CMS.Common"];
            SqlStatement statement = new SqlStatement();
            statement.SqlClause = sql;
            db.DbDriver.FormatSQL(statement);
            using (IConnection conn = db.CreateConnection())
            {
                return conn.Query(statement);
            }
        }

        /// <summary>
        /// 取得同一界面中的关联值
        /// </summary>
        /// <param name="ctrID"></param>
        /// <returns></returns>
        protected object GetRelationValue(string ctrID)
        {
            UIHelper UIHelper = new UIHelper(this.Page);
            FieldControl ctr = UIHelper.GetControl(ctrID, Page) as FieldControl;
            return ctr != null ? ctr.GetValue() : null;
        }
    }
}
