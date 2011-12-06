using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using We7.CMS.Accounts;
using We7.CMS.Common.PF;
using System.Text.RegularExpressions;

namespace We7.CMS.Web.User
{
    public partial class User_Account1 : UserBasePage
    {

        protected Account CurrentAccount
        {
            get { return AccountHelper.GetAccount(Security.CurrentAccountID, null); }
        }

        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
        }

        public string AccountSafeLevel;
        public int AccountSafeLevelNumber;
        public string AccountSafeLevelClass;
        public string CertClass;
        public string CertText;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Init();
            }
        }

        protected void Init()
        {
            if (Security.CurrentAccountID == null)
            {
                Response.Redirect("/user/login.aspx?returnURL=" + HttpUtility.UrlEncode(Request.RawUrl));
            }
            CalculateSafeLevel();
        }

        /// <summary>
        /// 计算安全级别，认证信息
        /// </summary>
        protected void CalculateSafeLevel()
        {
            Account currUser
                = AccountHelper.GetAccount(Security.CurrentAccountID, new string[] { "Password", "EmailValidate" });
            int level = (currUser.EmailValidate == 1) ? 80 : 60;
            CertText = (currUser.EmailValidate == 1) ? "已认证" : "未认证";
            CertClass = (currUser.EmailValidate == 1) ? "" : "notCer";

            AccountSafeLevelNumber = level;
            if (level < 40)
            {
                AccountSafeLevel = "低";
                AccountSafeLevelClass = "";
            }
            else if (level >= 40 && level < 75)
            {
                AccountSafeLevel = "中";
                AccountSafeLevelClass = "yel";
            }
            else
            {
                AccountSafeLevel = "高";
                AccountSafeLevelClass = "gre";
            }

        }

        /// <summary>
        /// 判断密码强度
        /// </summary>
        /// <param name="password">密码</param>
        /// <returns>密码强度（数字越大则强度越强）</returns>
        protected int CheckPassword(string password)
        {
            int strong = 0;
            Regex teShu = new Regex("[~!@#$%_^&*()=+[\\]{}''\";:/?.,><`|！·￥…—（）\\-、；：。，》《]");
            Regex daXie = new Regex("[A-Z]");
            Regex xiaoXie = new Regex("[a-z]");
            Regex shuZi = new Regex("[0-9]");
            if (teShu.IsMatch(password) == true)
                strong++;
            if (daXie.IsMatch(password) == true)
                strong++;
            if (xiaoXie.IsMatch(password) == true)
                strong++;
            if (shuZi.IsMatch(password) == true)
                strong++;
            return strong;
        }

    }
}
