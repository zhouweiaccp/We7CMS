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
using System.Text;
using System.Xml;
namespace We7.Model.UI.Controls.system.page
{
    public partial class morselect : System.Web.UI.Page
    {
        /// <summary>
        ///JS要用到给父窗口控件赋值
        /// </summary>
        public string Clientid { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            string matchupnode = Request.QueryString["MatchupNode"] == null ? "" : Request.QueryString["MatchupNode"];
            Clientid = Server.UrlDecode(Request.QueryString["ClientID"] == null ? "" : Request.QueryString["ClientID"]);
            string selectvalue = Server.UrlDecode(Request.QueryString["SelectValue"] == null ? "" : Request.QueryString["SelectValue"]);
            string path = Server.MapPath("~/config/SelectContent.xml");
            #region 读XML
            try
            {
                string filename = path;
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(filename);
                //得到顶层节点列表 
                XmlNodeList topM = xmldoc.DocumentElement.ChildNodes;

                foreach (XmlElement element in topM)
                {
                    //得到第二层对应节点列表
                    if (element.Attributes["name"].Value == matchupnode)
                    {
                        //得到第三层节点列表
                        foreach (XmlElement Selectelement in element)
                        {
                            string selectkye = Selectelement.Attributes["kye"].Value;

                            //让前面选中的值，在此默认选中
                            string[] s = selectkye.Split(',');
                            foreach (string ss in s)
                            {
                                string[] v1 = selectvalue.Split(',');
                                ListItem item = new ListItem();
                                foreach (string vv in v1)
                                {
                                    if (vv == ss)
                                    { item.Selected = true; }
                                }
                                item.Value = ss;
                                item.Text = ss;
                                ChbList.Items.Add(item);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //  throw new Exception("没有指定的配置文件");
            }
            #endregion

        }

    }
}
