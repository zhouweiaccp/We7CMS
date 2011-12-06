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
using We7.CMS.Common.Enum;
using We7.CMS.Helpers;
using We7.CMS.Common;
using System.Collections.Generic;
using We7.CMS.Accounts;

namespace We7.CMS.Web.User
{
    public partial class FavoriteEdit : UserBasePage
    {

        private string Url
        {
            get { return FavoriteHelper.GetAbsoluteURl(HttpUtility.UrlDecode(Request["url"])); }

        }
        private string ID
        {
            get
            {
                return Request["id"];

            }
        }
        private string aid
        {
            get
            {
                return Request["aid"];

            }
        }
        private string thumbnail
        {
            get { return HttpUtility.UrlDecode(Request["thumbnail"]); }

        }

        private string title
        {
            get { return HttpUtility.UrlDecode(Request["title"]); }
        }

        private string description
        {

            get
            {
                if (Request.Cookies["aidInfor"] != null)
                {
                    return HttpUtility.UrlDecode(Request.Cookies["aidInfor"].Value);
                }
                else
                {
                    return "";
                }
            }
        }

        private FavoriteHelper FavoriteHelper
        {
            get { return HelperFactory.GetHelper<FavoriteHelper>(); }
        }
        private DataTable dtTagList = new DataTable();
        protected bool definedTag = false;
        protected void Init()
        {
            if (Security.CurrentAccountID == null)
            {
                Response.Redirect("/Signin.aspx?returnURL=" + HttpUtility.UrlEncode(Request.RawUrl));
            }
            else
            {

                DataBindTag();
            }

            if (String.IsNullOrEmpty(ID) && !String.IsNullOrEmpty(Url))
            {
                txtUrl.Text = Url;
                txtThumbnail.Text = thumbnail;
                txtDesc.Text = description;
                if (!string.IsNullOrEmpty(title))
                {
                    txtTitle.Text = title;
                }
                else
                {
                    txtTitle.Text = FavoriteHelper.GetTitle(Url);
                }
                if (definedTag)
                {
                    txtTag.Text = dtTagList.Rows[0][0].ToString();
                }
                else
                {
                    rblSystemTags.SelectedValue = dtTagList.Rows[0][0].ToString();
                }
            }
            else if (!String.IsNullOrEmpty(ID))
            {
                Favorite f = FavoriteHelper.GetFavorite(ID);
                txtUrl.Text = f.Url;
                txtTitle.Text = f.Title;
                txtDesc.Text = f.Description;
                if (definedTag)
                {
                    txtTag.Text = f.Tag;
                }
                else
                {
                    rblSystemTags.SelectedValue = f.Tag;
                }
                txtThumbnail.Text = f.Thumbnail;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Init();
            }
        }

        protected void bttnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(ID))
                {
                    Favorite fav = new Favorite();
                    fav.Url = txtUrl.Text.Trim();
                    fav.Title = txtTitle.Text.Trim();
                    fav.Description = txtDesc.Text.Trim();
                    fav.Created = fav.Updated = DateTime.Now;
                    fav.AccountID = Security.CurrentAccountID;
                    fav.ArticleID = aid;
                    if (definedTag)
                    {
                        fav.Tag = txtTag.Text.Trim();
                    }
                    else
                    {
                        fav.Tag = rblSystemTags.SelectedValue;
                    }
                    fav.Thumbnail = txtThumbnail.Text.Trim();
                    FavoriteHelper.AddFavorite(fav);
                    lblMsg.Text = "添加成功!";
                }
                else
                {
                    Favorite fav = FavoriteHelper.GetFavorite(ID);
                    fav.Url = txtUrl.Text.Trim();
                    fav.Title = txtTitle.Text.Trim();
                    fav.Description = txtDesc.Text.Trim();
                    fav.Created = fav.Updated = DateTime.Now;
                    fav.AccountID = Security.CurrentAccountID;
                    if (definedTag)
                    {
                        fav.Tag = txtTag.Text.Trim();
                    }
                    else
                    {
                        fav.Tag = rblSystemTags.SelectedValue;
                    }
                    fav.Thumbnail = txtThumbnail.Text.Trim();
                    FavoriteHelper.UpdateFavorite(fav);
                    lblMsg.Text = "更新成功!";
                }
                DataBindTag();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "添加失败！详细信息：" + ex.Message;
            }
        }
        protected void bttnReset_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(ID))
            {
                Response.Redirect("FavoriteList.aspx");
            }
            else
            {
                Response.Redirect(Url);
            }
        }

        private void DataBindTag()
        {
            DataTable dtTagDefaultList = FavoriteHelper.ListDefaultTag(ref definedTag);
            if (definedTag)
            {
                DataTable dtTag = FavoriteHelper.ListAllTagByAccount(Security.CurrentAccountID);
                dtTagList.Merge(dtTagDefaultList);
                dtTagList.Merge(dtTag);
                dtTagList = FavoriteHelper.SelectDistinct(dtTagList);
                dlTagList.DataSource = dtTagList;
                dlTagList.DataBind();
            }
            else
            {
                dtTagList = dtTagDefaultList;
                rblSystemTags.DataSource = dtTagList;
                rblSystemTags.DataTextField = "tag";
                rblSystemTags.DataValueField = "tag";
                rblSystemTags.DataBind();
            }
        }


    }
}
