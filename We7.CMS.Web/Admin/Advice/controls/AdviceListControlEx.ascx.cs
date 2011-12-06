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
using We7.CMS.Common;
using We7.CMS.Accounts;
using System.Collections.Generic;

namespace We7.CMS.Web.Admin
{
    /// <summary>
    /// 反馈提交的表单列表,.aspx包含权限判断
    /// </summary>
    public partial class AdviceListControlEx : BaseUserControl
    {
        #region 属性字段
        private Dictionary<string, string> dicTypeNames = new Dictionary<string, string>();
        IAdviceHelper adviceHelper = AdviceFactory.Create();

        List<string> permissions;
        protected List<string> Permisstions
        {
            get
            {
                if (permissions == null)
                {
                    permissions = adviceHelper.GetPermissions(TypeID, Security.CurrentAccountID);
                }
                return permissions;
            }
        }        

        private int _resultsPageNumber = 1;
        /// <summary>
        /// 当前页
        /// </summary>
        protected int PageNumber
        {
            get
            {
                if (Request.QueryString[Keys.QRYSTR_PAGEINDEX] != null)
                    _resultsPageNumber = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);
                return _resultsPageNumber;
            }
        }

        /// <summary>
        /// 显示状态
        /// </summary>
        public int ShowType { get; set; }
        /// <summary>
        /// 反馈类型
        /// </summary>
        public string TypeID { get; set; }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBind();
            }
        }

        protected void lnkDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (ShowType != 3)
                {
                    foreach (string id in GetSelectedIDs())
                    {
                        adviceHelper.DeleteAdvice(id);
                    }
                }
                else
                {
                    foreach (string id in GetSelectedIDs())
                    {
                        adviceHelper.DeleteTransfer(id);
                    }
                }
                DataBind();
                Messages.ShowMessage("操作成功!");
            }
            catch (Exception ex)
            {
                Messages.ShowError("应用程序错误！错误原因：" + ex.Message);
            }
        }

        protected void lnkSetTop_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (string id in GetSelectedIDs())
                {
                    adviceHelper.SetTop(id);
                }
                DataBind();
                Messages.ShowMessage("操作成功!");
            }
            catch (Exception ex)
            {
                Messages.ShowError("应用程序错误！错误原因：" + ex.Message);
            }
        }

        protected void lnkCancelTop_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (string id in GetSelectedIDs())
                {
                    adviceHelper.CancelTop(id);
                }
                DataBind();
                Messages.ShowMessage("操作成功!");
            }
            catch (Exception ex)
            {
                Messages.ShowError("应用程序错误！错误原因：" + ex.Message);
            }
        }

        protected void lnkShow_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (string id in GetSelectedIDs())
                {
                    adviceHelper.ShowAdvice(id);
                }
                DataBind();
                Messages.ShowMessage("操作成功!");
            }
            catch (Exception ex)
            {
                Messages.ShowError("应用程序错误！错误原因：" + ex.Message);
            }
        }
        protected void lnkHide_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (string id in GetSelectedIDs())
                {
                    adviceHelper.HideAdvice(id);
                }
                DataBind();
                Messages.ShowMessage("操作成功!");
            }
            catch (Exception ex)
            {
                Messages.ShowError("应用程序错误！错误原因：" + ex.Message);
            }
        }
        protected void lnkAccept_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (string id in GetSelectedIDs())
                {
                    adviceHelper.AcceptAdvice(id);
                }
                DataBind();
                Messages.ShowMessage("操作成功!");
            }
            catch (Exception ex)
            {
                Messages.ShowError("应用程序错误！错误原因：" + ex.Message);
            }
        }
        protected void lnkRefuse_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (string id in GetSelectedIDs())
                {
                    adviceHelper.RefuseAdvice(id, String.Empty);
                }
                DataBind();
                Messages.ShowMessage("操作成功!");
            }
            catch (Exception ex)
            {
                Messages.ShowError("应用程序错误！错误原因：" + ex.Message);
            }
        }
        protected void lnkUrge_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (string id in GetSelectedIDs())
                {
                    adviceHelper.SetAdvicePriority(id, 2);
                }
                DataBind();
                Messages.ShowMessage("操作成功!");
            }
            catch (Exception ex)
            {
                Messages.ShowError("应用程序错误！错误原因：" + ex.Message);
            }

        }

        protected void bttnQuery_Click(object sender, EventArgs e)
        {
            DataBind();
        }

        protected void DataBind()
        {
            AdviceUPager.PageIndex = PageNumber;
            AdviceUPager.UrlFormat = We7Helper.AddParamToUrl(Request.RawUrl.Replace("{", "{{").Replace("}", "}}"), Keys.QRYSTR_PAGEINDEX, "{0}");
            AdviceUPager.PrefixText = "共 " + AdviceUPager.MaxPages + "  页 ·   第 " + AdviceUPager.PageIndex + "  页 · ";

            if (ShowType != 3)
            {
                Dictionary<string, object> queryInfo = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(txtQuery.Text.Trim()))
                    queryInfo.Add("title", txtQuery.Text.Trim());
                if (!string.IsNullOrEmpty(Request["RelationID"]))
                {
                    queryInfo.Add("RelationID", Request["RelationID"].Trim());
                }

                AdviceUPager.ItemCount = adviceHelper.QueryAdviceCount(TypeID, ShowType, queryInfo);
                AdviceGridView.Visible = true;
                TransferGridView.Visible = false;
                AdviceGridView.DataSource = adviceHelper.QueryAdvice(TypeID, ShowType, queryInfo, AdviceUPager.Begin - 1, AdviceUPager.Count);
                AdviceGridView.DataBind();
            }
            else
            {
                AdviceUPager.ItemCount = adviceHelper.QueryAdviceCount(TypeID, ShowType);
                AdviceGridView.Visible = false;
                TransferGridView.Visible = true;
                TransferGridView.DataSource = adviceHelper.QueryTransfers(TypeID, AdviceUPager.Begin - 1, AdviceUPager.Count);
                TransferGridView.DataBind();
            }
        }

        protected string GetTitle(AdviceInfo advice)
        {
            if (advice == null)
                return String.Empty;

            string url = String.Empty;
            if (Permisstions.Contains("Advice.Handle") && (advice.State == 2 || advice.State == 3 && advice.TypeID == TypeID))
            {
                url = "AdviceProcessEx.aspx";
            }
            else if (Permisstions.Contains("Advice.Accept") && (advice.State == 0))
            {
                url = "AdviceDistribute.aspx";
            }
            else if (Permisstions.Count > 0)
            {
                url = "AdviceView.aspx";
            }
            return !String.IsNullOrEmpty(url) ? FormatUrl(url, advice) : advice.Title;
        }

        /// <summary>
        /// 取得反馈类型名称
        /// </summary>
        /// <param name="typeID"></param>
        /// <returns></returns>
        protected string GetAdviceTypeName(string typeID)
        {
            if (String.IsNullOrEmpty(typeID))
                return "当前类型不存在";

            if (!dicTypeNames.ContainsKey(typeID))
            {
                AdviceType type = HelperFactory.GetHelper<AdviceTypeHelper>().GetAdviceType(typeID);
                dicTypeNames.Add(typeID, typeID != null ? type.Title : String.Empty);
            }
            return dicTypeNames[typeID];
        }

        protected string FormatUrl(string url, AdviceInfo advice)
        {
            return String.Format("<a href='{0}?id={1}&typeID={3}'>{2}</a>", url, advice.ID, advice.Title, Request["typeID"]);
        }

        protected string FormatDate(object o)
        {
            if (o != null)
            {
                DateTime dt = (DateTime)o;
                return dt.Year == DateTime.Now.Year ? dt.ToString("M月d日 HH时mm分") : dt.ToString("yyyy-MM-dd HH:mm");
            }
            return String.Empty;
        }

        protected List<string> GetSelectedIDs()
        {
            List<string> list = new List<string>();
            string ids = Request["ids"];
            if (!String.IsNullOrEmpty(ids))
            {
                list.AddRange(ids.Split(','));
            }
            return list;
        }
    }
}