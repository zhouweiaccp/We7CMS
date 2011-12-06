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

namespace We7.CMS.Web.User
{
    public partial class index : UserBasePage
    {
        public decimal income = 0;
        public decimal expense = 0;

        public string AccountSafeLevel;
        public int AccountSafeLevelNumber;
        public string AccountSafeLevelClass;
        public string CertClass;
        public string CertText;

        public string InfomationHref = "";
        public int InfomationNumber = 0;
        public string InfomationClass = "";
        public bool IsAdvanceUser;
        public string SafePWDUrl = "";

        private string _userName;
        public decimal currentMoney = 0;

        protected static We7.CMS.Accounts.IAccountHelper AccountHelper
        {
            get { return AccountFactory.CreateInstance(); }
        }

        public string UserName
        {
            get
            {
                if (string.IsNullOrEmpty(_userName))
                {
                    _userName = AccountHelper.GetAccount(Security.CurrentAccountID, null).LoginName;
                }
                return _userName;
            }
        }



        protected void CalculateSafeLevel()
        {
            Account currUser
                = AccountHelper.GetAccount(Security.CurrentAccountID, new string[] { "FirstName", "MiddleName", "LastName", "Photo", "EmailValidate" });
            int level = 0;
            if (currUser.EmailValidate == 1)
            {
                level += 20;
            }
            if (!string.IsNullOrEmpty(currUser.FirstName + currUser.MiddleName + currUser.LastName))
            {
                level += 20;
            }
            if (!string.IsNullOrEmpty(currUser.Photo))
            {
                level += 20;
            }
            string informationUrl = "/User/AccountEdit.aspx";
            //AdvanceUserModel advanceUser = BaseMethod.GetAdvanceUserModelByUserID(Security.CurrentAccountID);
            //if (advanceUser != null && !string.IsNullOrEmpty(advanceUser.CardID))
            //{
            //    level += 20;
            //    if (!string.IsNullOrEmpty(advanceUser.Telphone) && !string.IsNullOrEmpty(advanceUser.Mobile) && !string.IsNullOrEmpty(advanceUser.QQ) && !string.IsNullOrEmpty(advanceUser.MSN))
            //    {
            //        level += 20;
            //    }
            //    else
            //    {
            //        informationUrl = "/Plugins/ShopPlugin/UI/Register_Mall.aspx";
            //    }
            //    IsAdvanceUser = true;
            //    SafePWDUrl = "/Plugins/ShopPlugin/UI/ChangeTradePWD.aspx";
            //}
            //else
            //{
            //    IsAdvanceUser = false;
            //    SafePWDUrl = "/Plugins/ShopPlugin/UI/Register_Mall.aspx";
            //}
            InfomationHref = informationUrl;
            InfomationNumber = level;
            if (level <= 40)
            {
                InfomationClass = "hot";
            }
            else if (level <= 80)
            {
                InfomationClass = "yel";
            }
            else
            {
                InfomationClass = "";
            }

            CertText = (currUser.EmailValidate == 1) ? "已认证" : "未认证";
            CertClass = (currUser.EmailValidate == 1) ? "Green" : "orange1";

            int safeLevel = (currUser.EmailValidate == 1) ? 80 : 60;
            AccountSafeLevelNumber = safeLevel;

            if (safeLevel <= 60)
            {
                AccountSafeLevel = "低";
                AccountSafeLevelClass = "hot";
            }
            else
            {
                AccountSafeLevel = "高";
                AccountSafeLevelClass = "";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Criteria cIncome = new Criteria(CriteriaType.None);
                //cIncome.Add(CriteriaType.Equals, "UserID", Security.CurrentAccountID);
                //cIncome.Add(CriteriaType.LessThan, "TradeType", "10");
                //cIncome.Add(CriteriaType.Equals, "IsSuccess", "1");
                //income = ViewData.GetUserTradeMoneyByCriteria(cIncome);

                //Criteria cExpense = new Criteria(CriteriaType.None);
                //cExpense.Add(CriteriaType.Equals, "UserID", Security.CurrentAccountID);
                //cExpense.Add(CriteriaType.MoreThan, "TradeType", "10");
                //cIncome.Add(CriteriaType.Equals, "IsSuccess", "1");
                //expense = ViewData.GetUserTradeMoneyByCriteria(cExpense);

                CalculateSafeLevel();
            }
        }
        protected override bool NeedAnPermission
        {
            get
            {
                return false;
            }
        }
    
    }
}
