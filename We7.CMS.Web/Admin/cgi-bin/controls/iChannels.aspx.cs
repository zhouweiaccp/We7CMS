using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Thinkment.Data;
using We7.CMS.Common;

namespace We7.CMS.Web.Admin.cgi_bin.controls
{
    public partial class iChannels : BasePage
    {

        public string CurrentPath="/";
        public string CurrentUrl = "/";
        public string ColumnID
        {
            get
            {
                string id = Request["id"];
                if (id == null)
                {
                    return We7Helper.EmptyGUID;
                }
                return id;
            }
        }
        /// <summary>
        /// 引用栏目ID
        /// </summary>
        public string QuoteOwnerID
        {
            get { return Request["oid"];}
        }

        protected override void Initialize()
        {
            string root;
            if (We7Helper.IsEmptyID(ColumnID))
            {
                GoParentHyperLink.NavigateUrl = "iChannels.aspx";
                root = "";
                CurrentUrl = "/";
            }
            else
            {
                Channel ch =ChannelHelper.GetChannel(ColumnID, new string[] { "ID", "Name", "Description", "ParentID","FullPath","FullUrl" });
                CurrentPath = ch.Description;
                GoParentHyperLink.NavigateUrl = String.Format("iChannels.aspx?id={0}&oid={1}&type=quote", ch.ParentID, QuoteOwnerID);
                root = ch.FullPath;
                CurrentUrl = ch.FullUrl;
            }

            CurrentPath ="根栏目"+ root.Replace("/"," / ");

            List<Channel> data = ChannelHelper.GetChannels(ColumnID);
            if (data==null || data.Count == 0)
            {
                //ShowMessage("没有数据。");
            }
            else
            {
                //ShowMessage(String.Format("总共有 {0}个栏目。", data.Count));
                foreach (Channel ch in data)
                {
                    ch.TemplateText = TemplateHelper.GetTemplateName(ch.TemplateName);
                    ch.FullPath = String.Concat(root, "/", ch.Name);
                }
            }

            DetailGridView.DataSource = data;
            DetailGridView.DataBind();
        }


    }
}
