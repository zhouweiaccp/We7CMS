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
using We7.CMS.Common.Enum;
using We7.CMS.Common;
using System.Xml;
using We7.Framework;
using We7.Model.Core;
using System.IO;

namespace We7.CMS.Web.Admin.Addins
{
    public partial class ArticleList : BasePage
    {
        /// <summary>
        /// 是否判断用户权限
        /// </summary>
        protected override bool NeedAnPermission
        {
            get
            {
                if (AccountHelper.GetAccount(AccountID, new string[] { "UserType" }).UserType == 0)
                {
                    return false;
                }
                return true;
            }
        }

        public string OwnerID
        {
            get
            {
                string oid = Request["oid"];
                return oid;
            }
        }

        protected string Tag
        {
            get
            {
                return Request["tag"];
            }
        }

        protected override MasterPageMode MasterPageIs
        {
            get
            {
                if (Request["notiframe"] != null)
                    return MasterPageMode.FullMenu;
                else
                    return MasterPageMode.NoMenu;
            }
        }

        void DoModel()
        {
            if (!String.IsNullOrEmpty(OwnerID))
            {
                Channel ch = ChannelHelper.GetChannel(OwnerID, null);
                if (ch != null && !String.IsNullOrEmpty(ch.ModelName) && String.Compare(ch.ModelName, "Article", true) != 0)
                {
                    string modelFileName = ModelHelper.GetModelPath(ch.ModelName);
                    if (File.Exists(modelFileName))
                    {
                        Response.Redirect(String.Format("~/Admin/Addins/ModelListNoMenu.aspx?notiframe=0&model={0}&oid={1}", ch.ModelName, ch.ID), true);
                    }
                    else
                    {
                        ch.ModelName = "";
                        ChannelHelper.UpdateChannel(ch);
                    }
                }
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            DoChannelType();
            DoModel();
            if (Request["notiframe"] != null)
            {
                NameLabel.Text = "全部";
                TitleH2.Visible = true;
                if (!We7Helper.IsEmptyID(OwnerID))
                {
                    string chTitle = ChannelHelper.GetChannel(OwnerID, new string[] { "Name" }).Name;
                    NameLabel.Text = string.Format("栏目『{0}』下", chTitle);
                }
                if (!string.IsNullOrEmpty(Tag))
                {
                    NameLabel.Text += string.Format("标签为”{0}“", Tag);
                }
                NameLabel.Text += "信息";

                ListTypeHyperLink.NavigateUrl = "articlelist.aspx" + Request.Url.Query;
                TreeTypeHyperLink.NavigateUrl = "articles.aspx" + Request.Url.Query;
            }
            else
            {
                if (!CheckChannelPermission())
                    Response.Write("您没有权限管理此栏目下信息！");
            }
        }

        void DoChannelType()
        {
            string msg = "";
            Channel ch = ChannelHelper.GetChannel(OwnerID, null);
            if (ch != null)
            {
                switch ((TypeOfChannel)int.Parse(ch.Type))
                {
                    case TypeOfChannel.RssOriginal:
                        msg = "本栏目为RSS源信息展示，不可以发布信息。";
                        break;
                    case TypeOfChannel.BlankChannel:
                        msg = "本栏目为空节点栏目，不可以在此栏目下发布信息。";
                        break;
                    case TypeOfChannel.ReturnChannel:
                        msg = "本栏目已经跳转到另外的地址： " + ch.ReturnUrl;
                        break;
                    default:
                        break;
                }
                if (msg != "")
                {
                    Response.Write(msg);
                    Response.End();
                }
            }
        }

        bool CheckChannelPermission()
        {
            bool canList = AccountID == We7Helper.EmptyGUID;

            if (!canList)
            {
                if (OwnerID != null)
                {
                    List<string> ps = AccountHelper.GetPermissionContents(AccountID, OwnerID);
                    if (ps.Contains("Channel.Article"))
                    {
                        canList = true;
                    }
                }
            }
            return canList;
        }
    }
}
