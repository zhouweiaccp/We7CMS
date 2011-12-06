using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Text;

namespace We7.CMS.Web.Admin
{
    public partial class AdviceList : BasePage
    {
        //protected override string[] Permissions
        //{
        //    get
        //    {
        //        return new string[] { "Admin.Advices" };
        //    }
        //}

        string AdviceTypeID
        {
            get
            {
                return Request["adviceTypeID"];
                //return "{e9c097c5-be47-4b23-b8cb-6c4bc6cb146d}";
            }
        }
       
        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}
