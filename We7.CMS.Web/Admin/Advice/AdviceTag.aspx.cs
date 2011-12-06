using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using We7.Framework.Util;
using System.Xml;

namespace We7.CMS.Web.Admin
{
    public partial class AdviceTag : BasePage
    {
        private string XPath
        {
            get { return "/AdviceTag"; }
        }

        private string FileName
        {
            get { return Server.MapPath("/Config/AdviceTag.xml"); }
        }
        /// <summary>
        /// 判断该标签名是否存在
        /// </summary>
        private string Exits
        {
            get { return Request.QueryString["haveTag"]; }
        }
        /// <summary>
        /// 要修改的标签名
        /// </summary>
        private string tagName
        {
            get { return Request.QueryString["tagName"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Exits))
                {
                    Response.Clear();
                    Response.Write(IsExitsTag());
                    Response.End();
                }
                else
                {
                    if (!string.IsNullOrEmpty(tagName))
                    {
                        lbTagEdit.Text = "修改标签";
                        btnTagEdit.Text = "修改";
                        tbNewTag.Text = tagName;
                    }
                    DataBindAdviceTagList();
                }
            }
        }

        private void DataBindAdviceTagList()
        {
            XmlNode adviceTagNodes = XmlHelper.GetXmlNode(FileName, XPath);
            List<string> tagList = new List<string>();
            foreach (XmlNode node in adviceTagNodes)
            {
                tagList.Add(node.Attributes["name"].Value);
            }
            PagedDataSource pds = new PagedDataSource();
            pds.AllowPaging = true;
            pds.PageSize = Pager.PageSize;
            pds.CurrentPageIndex = Pager.CurrentPageIndex - 1;
            pds.DataSource = tagList;
            Pager.RecordCount = tagList.Count;


            rptAdviceTag.DataSource = pds;
            rptAdviceTag.DataBind();
        }

        protected void btnTagEditOnClick(object sender, EventArgs e)
        {
            AdviceHelper.InsertAdviceTagToAdviceTagXml(tagName, tbNewTag.Text, FileName, XPath);
            DataBindAdviceTagList();
        }

        protected void DeleteTag(object sender, EventArgs e)
        {
            LinkButton lb = sender as LinkButton;
            XmlHelper.DeleteXmlNode(FileName, XPath + "/tag[@name='" + lb.CommandName + "']");
            DataBindAdviceTagList();
        }

        protected void Pager_PageChanging(object src, Wuqi.Webdiyer.PageChangingEventArgs e)
        {
            Pager.CurrentPageIndex = e.NewPageIndex;
            DataBindAdviceTagList();

        }

        /// <summary>
        /// 判断是否存在该标签
        /// </summary>
        /// <returns></returns>
        public string IsExitsTag()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(FileName);
            XmlNode node = xml.DocumentElement.SelectSingleNode(XPath + "/tag[@name='" + Exits + "']");
            if (node != null)
            {
                return "Exits";
            }
            else
            {
                return "None";
            }

        }
    }
}
