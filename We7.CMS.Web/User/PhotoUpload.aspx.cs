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
using We7.CMS.Common.PF;
using We7.CMS.Accounts;
using System.IO;
using We7.Framework.Util;

namespace We7.CMS.Web.User
{
    public partial class PhotoUpload : UserBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Account act = AccountFactory.CreateInstance().GetAccount(Security.CurrentAccountID, null);
                if (act != null && !String.IsNullOrEmpty(act.Photo))
                {
                    imgPhoto.ImageUrl = act.Photo;
                }
                else
                {
                    imgPhoto.ImageUrl = "/ModelUI/skin/images/nopic.gif";
                }
            }
            bttnUpload.Click += new EventHandler(bttnUpload_Click);
        }

        void bttnUpload_Click(object sender, EventArgs e)
        {
            string message = string.Empty;
            if (!fuPhoto.HasFile)
            {
                message = "上传图片不能为空";
            }
            else
            {
                string ext = Path.GetExtension(fuPhoto.FileName);
                if (!string.IsNullOrEmpty(ext))
                {
                    ext = ext.ToLower();
                    if (ext == ".jpg" || ext == ".png" || ext == ".gif")
                    {
                        string fileName = GetImageUrl(fuPhoto.FileName);
                        string filePath = Server.MapPath(fileName);
                        FileInfo fi = new FileInfo(filePath);
                        if (!fi.Directory.Exists)
                        {
                            fi.Directory.Create();
                        }
                        fuPhoto.SaveAs(Server.MapPath(fileName));
                        string thumbfileName = GetThumbUrl(fileName);
                        string thumbPath = Server.MapPath(thumbfileName);
                        ImageUtils.MakeThumbnail(filePath, thumbPath, 120, 120, "HW");
                        imgPhoto.ImageUrl = thumbfileName;
                        IAccountHelper helper = AccountFactory.CreateInstance();
                        Account act = helper.GetAccount(Security.CurrentAccountID, null);
                        act.Photo = thumbfileName;
                        helper.UpdateAccount(act, new string[] { "Photo" });

                        message = "上传成功";
                    }
                    else
                    {
                        message = "上传文件格式不对";
                    }
                }
                else
                {
                    message = "上传文件格式不对";
                }
            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "message", "alert('" + message + "')", true);
        }

        protected override bool NeedAnPermission
        {
            get { return false; }
        }

        protected string GetImageUrl(string fileName)
        {
            return String.Format("/_data/{0}/{1}/{2}/{0}{1}{2}{3}{4}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, new Random((int)DateTime.Now.Ticks).Next(), Path.GetExtension(fileName));
        }

        protected string GetThumbUrl(string filePath)
        {
            return filePath.Replace(Path.GetExtension(filePath), "_thumb" + Path.GetExtension(filePath));
        }
    }
}
